CREATE TRIGGER gradeTest ON dbo.StudentTestAssignments
AFTER UPDATE
AS
IF (UPDATE (Submitted)) --If the submitted column has changed
BEGIN
	-- Get all tests submitted at time of update
	DECLARE studentTestAssignmentCursor CURSOR FOR (SELECT * FROM inserted);
	DECLARE @studentTestAssignmentId BIGINT,
			@studentId BIGINT,
			@submitted BIT,
			@grade REAL,
			@signed BIT,
			@testScheduleId BIGINT;

	OPEN studentTestAssignmentCursor;

	FETCH NEXT FROM studentTestAssignmentCursor INTO @studentTestAssignmentId, @studentId, @submitted, @grade, @signed, @testScheduleId;

	-- Go through each test
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- If the test has been submitted
		IF (@submitted = 1)
		BEGIN
			-- If the pledge has been signed, grade the test
			IF (@signed = 1)
			BEGIN
				-- The overall grade the student has
				DECLARE @c_testGrade int = 0;
				-- Store the AnswerId you get from the cursor below
				DECLARE @c_AnswerId bigint;
				-- Create a cursor for all the answers a student put on this test
				DECLARE studentTestCursor CURSOR FOR
					SELECT StudentAnswers.AnswerId --, Questions.QuestionId
						FROM StudentTestAssignments JOIN AspNetUsers ON StudentTestAssignments.StudentId = AspNetUsers.Id
													JOIN StudentAnswers ON AspNetUsers.Id = StudentAnswers.StudentId
													JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
						WHERE StudentTestAssignments.StudentTestAssignmentId = @studentTestAssignmentId;
					
				OPEN studentTestCursor;

				FETCH NEXT FROM studentTestCursor INTO @c_AnswerId;

				-- Loop through all student answers
				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- Grade the student answers depending on type

					-- Grade Essay answers (tell teacher that manual grading is required)
					IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Essay')
					BEGIN
						UPDATE StudentTestAssignments
							SET ManualGradingRequired = 1
							WHERE StudentTestAssignments.StudentTestAssignmentId = @studentTestAssignmentId;
					END; -- End "grading" an Essay question

					-- Grade FillInTheBlank answers
					ELSE IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'FillInTheBlank')
					BEGIN
						-- All the expected answers for the question
						DECLARE fillInTheBlankCorrectAnswersCursor CURSOR FOR
								SELECT FillInTheBlankQuestions.FillInTheBlankAnswer
								FROM FillInTheBlankQuestions JOIN Questions ON FillInTheBlankQuestions.QuestionId = Questions.QuestionId
															JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
								WHERE StudentAnswers.QuestionId = @c_answerId;
						DECLARE @currentCorrectFillInTheBlankAnswer NVARCHAR;
						DECLARE @allStudentFillInTheBlankAnswersCorrect BIT = 1; -- Assume that there are no wrong answers

						OPEN fillInTheBlankCorrectAnswersCursor
						FETCH NEXT FROM fillInTheBlankCorrectAnswersCursor INTO @currentCorrectFillInTheBlankAnswer

						-- Grade answers
						WHILE @@FETCH_STATUS = 0
						BEGIN
							-- If the answer is INCORRECT!!!
							IF ((SELECT AnswerGiven FROM StudentFillInTheBlankAnswers WHERE AnswerId = @c_AnswerId) != @currentCorrectFillInTheBlankAnswer)
							BEGIN
								SELECT @allStudentFillInTheBlankAnswersCorrect = 0;
							END;

							-- Get next expected answer
							FETCH NEXT FROM fillInTheBlankCorrectAnswersCursor INTO @currentCorrectFillInTheBlankAnswer
						END;

						CLOSE fillInTheBlankCorrectAnswersCursor;
						DEALLOCATE fillInTheBlankCorrectAnswersCursor;

						-- If all the answers were correct, add the points to the grade
						IF (@allStudentFillInTheBlankAnswersCorrect = 1)
						BEGIN
							SELECT @c_testGrade += (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);
						END;
					END; -- End grading a FillInTheBlank question

					-- Grade Matching answers
					--ELSE IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Matching')
					--BEGIN
						-- Compare answers
						-- STEP 1: Compare student matching answer to the correct answer
						-- STEP 2: If correct, add the question's grade point value to the overall grade
					--END; -- End grading a Matching question

					-- Grade MultipleChoice answers
					ELSE IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'MultipleChoice')
					BEGIN
						-- Get all correct answers for the question
 						DECLARE multipleChoiceCorrectAnswersCursor CURSOR FOR
								SELECT MultipleChoiceAnswers.MultipleChoiceAnswerId
								FROM MultipleChoiceAnswers JOIN Questions ON MultipleChoiceAnswers.QuestionId = Questions.QuestionId
															JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
								WHERE StudentAnswers.QuestionId = @c_answerId AND MultipleChoiceAnswers.IsCorrect = 1;
						DECLARE @currentMultipleChoiceAnswerId BIGINT;
						DECLARE @allStudentMultipleChoiceAnswersCorrect BIT = 1; -- Assume that there are no wrong answers

						OPEN multipleChoiceCorrectAnswersCursor
						FETCH NEXT FROM multipleChoiceCorrectAnswersCursor INTO @currentMultipleChoiceAnswerId

						-- Grade answers
						WHILE @@FETCH_STATUS = 0
						BEGIN
							-- If the answer is INCORRECT!!!
							IF ((SELECT MultipleChoiceAnswerId FROM StudentMultipleChoiceAnswers WHERE AnswerId = @c_AnswerId) != @currentMultipleChoiceAnswerId)
							BEGIN
								SELECT @allStudentMultipleChoiceAnswersCorrect = 0;
							END;

							-- Get next expected answer
							FETCH NEXT FROM multipleChoiceCorrectAnswersCursor INTO @currentMultipleChoiceAnswerId
						END;

						CLOSE multipleChoiceCorrectAnswersCursor;
						DEALLOCATE multipleChoiceCorrectAnswersCursor;

						-- If all the answers were correct, add the points to the grade
						IF (@allStudentMultipleChoiceAnswersCorrect = 1)
						BEGIN
							SELECT @c_testGrade += (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);
						END;
					END; -- End grading a MultipleChoice question

					-- Grade TrueFalse answers
					ELSE IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'TrueFalse')
					BEGIN
						-- Compare answers
						-- If the answer is CORRECT
						IF ((SELECT StudentAnswers.TrueFalseAnswerGiven FROM StudentAnswers WHERE StudentAnswers.AnswerId = @c_AnswerId)
							= (SELECT Questions.TrueFalseAnswer FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId))
						BEGIN
							SELECT @c_testGrade += (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);
						END;
					END; -- End grading a TrueFalse question

					-- Get next answer to grade
					FETCH NEXT FROM studentTestCursor INTO @c_AnswerId;
				END; -- End checking the answers for one test

				CLOSE studentTestCursor;
				DEALLOCATE studentTestCursor;

				-- Set the grade for the test
				UPDATE StudentTestAssignments
					SET Grade = @c_testGrade
					WHERE StudentTestAssignments.StudentTestAssignmentId = @studentTestAssignmentId;
			END; -- End grading when student has signed the pledge

			-- If the pledge has not been signed, set the grade to 0
			ELSE
			BEGIN
				UPDATE StudentTestAssignments
					SET Grade = 0
					WHERE StudentTestAssignments.StudentTestAssignmentId = @studentTestAssignmentId;
			END; -- End grading when student has not signed the pledge
		END; -- End check for pledge

		-- Get the next test to grade
		FETCH NEXT FROM studentTestAssignmentCursor INTO @studentTestAssignmentId, @studentId, @submitted, @grade, @signed, @testScheduleId;
	END; -- End going through each test

	CLOSE studentTestAssignmentCursor;
	DEALLOCATE studentTestAssignmentCursor;
END;