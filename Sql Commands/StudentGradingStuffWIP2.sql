CREATE TRIGGER gradeTest ON dbo.StudentTestAssignments
AFTER UPDATE
AS
IF (UPDATE (Submitted)) --If the submitted column has changed
BEGIN
	-- If the test has been submitted
	IF ((SELECT StudentTestAssignments.Submitted FROM StudentTestAssignments JOIN inserted ON StudentTestAssignments.StudentTestAssignmentId = inserted.StudentTestAssignmentId) = 1)
		BEGIN
			-- If the pledge has been signed, grade the test
			IF ((SELECT StudentTestAssignments.Signed FROM StudentTestAssignments JOIN inserted ON inserted.StudentTestAssignmentId = StudentTestAssignments.StudentTestAssignmentId) = 1)
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
													  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId;
													  --JOIN Questions ON Questions.QuestionId = StudentAnswers.QuestionId;
					
					OPEN studentTestCursor;

					FETCH NEXT FROM studentTestCursor INTO @c_AnswerId;

					-- Loop through all student answers
					WHILE @@FETCH_STATUS = 0
						BEGIN
							-- Grade the student answers depending on type

							-- Grade Essay answers
							IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Essay')
								BEGIN
									-- Compare answers
									-- Uhhh... alert teacher?
								END;

							-- Grade FillInTheBlank answers
							IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'FillInTheBlank')
								BEGIN
									-- Compare answers
									IF ((SELECT StudentAnswers.FillInTheBlankAnswerGiven FROM StudentAnswers WHERE StudentAnswers.AnswerId = @c_AnswerId) = (SELECT Questions.FillInTheBlankAnswer FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId))
									BEGIN
										--@c_testGrade = @c_testGrade + (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);
									END;
								END;

							-- Grade Matching answers
							IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Matching')
								BEGIN
									-- Compare answers
									-- STEP 1: Compare student matching answer to the correct answer
									-- STEP 2: If correct, add the question's grade point value to the overall grade
								END;

							-- Grade MultipleChoice answers
							IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'MultipleChoice')
								BEGIN
									-- Compare answers
									-- STEP 1: Compare student multiple choice answer to the correct answer
									-- STEP 2: If correct, add the question's grade point value to the overall grade
								END;

							-- Grade TrueFalse answers
							IF ((SELECT Questions.QuestionType FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'TrueFalse')
								BEGIN
									-- Compare answers
									IF ((SELECT StudentAnswers.TrueFalseAnswerGiven FROM StudentAnswers WHERE StudentAnswers.AnswerId = @c_AnswerId) = (SELECT Questions.TrueFalseAnswer FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId))
									BEGIN
										--@c_testGrade = @c_testGrade + (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);
									END;
								END;

							-- Get next answer to grade
							FETCH NEXT FROM studentTestCursor INTO @c_AnswerId;
						END;
				END;

			-- If the pledge has not been signed, set the grade to 0
			ELSE
				BEGIN
					UPDATE StudentTestAssignments
					   SET Grade = 0
					 WHERE StudentTestAssignments.StudentTestAssignmentId = (SELECT StudentTestAssignments.StudentTestAssignmentId FROM StudentTestAssignments JOIN inserted ON StudentTestAssignments.StudentTestAssignmentId = inserted.StudentTestAssignmentId);
				END;
		END;
END;