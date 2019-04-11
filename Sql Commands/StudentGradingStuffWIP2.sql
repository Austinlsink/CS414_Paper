USE [CS414_BrainNotFound]
GO
/****** Object:  Trigger [dbo].[gradeTest]    Script Date: 4/10/2019 9:27:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[gradeTest] ON [dbo].[StudentTestAssignments]
AFTER UPDATE
AS
IF (UPDATE (Submitted)) --If the submitted column has changed
BEGIN
	-- Get all tests submitted at time of update
	DECLARE studentTestAssignmentCursor CURSOR FOR 
		SELECT StudentTestAssignmentId, StudentId, Submitted, Grade, Signed, TestScheduleId
		  FROM inserted;
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
					-- If a question is correct (ASSUMED CORRECT!!!)
					DECLARE @correct BIT = 1; 

					-- Grade the student answers depending on type

					-- Grade Essay answers (tell teacher that manual grading is required)
					IF ((SELECT Questions.QuestionType
						   FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
						  WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Essay')
					BEGIN
						UPDATE StudentTestAssignments
							SET ManualGradingRequired = 1
							WHERE StudentTestAssignments.StudentTestAssignmentId = @studentTestAssignmentId;
						SELECT @correct = 0;
					END; -- End "grading" an Essay question

					-- Grade FillInTheBlank answers
					ELSE IF ((SELECT Questions.QuestionType
								FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
							   WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'FillInTheBlank')
					BEGIN
						SELECT @correct = dbo.gradeFillInTheBlank(@c_AnswerId);
					END; -- End grading a FillInTheBlank question

					-- Grade Matching answers
					ELSE IF ((SELECT Questions.QuestionType
								FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
							   WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'Matching')
					BEGIN
						SELECT @correct = dbo.gradeMatching(@c_AnswerId);
					END; -- End grading a Matching question

					-- Grade MultipleChoice answers
					ELSE IF ((SELECT Questions.QuestionType
							    FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
							   WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'MultipleChoice')
					BEGIN
						SELECT @correct = dbo.gradeMultipleChoice(@c_AnswerId);
					END; -- End grading a MultipleChoice question

					-- Grade TrueFalse answers
					ELSE IF ((SELECT Questions.QuestionType
					            FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
							   WHERE StudentAnswers.AnswerId = @c_AnswerId) = 'TrueFalse')
					BEGIN
						SELECT @correct = dbo.gradeTrueFalse(@c_AnswerId);
					END; -- End grading a TrueFalse question

					-- END GRADING STUDENT ANSWER

					-- Add points to the test if the answer was correct
					IF (@correct = 1)
						SELECT @c_testGrade += (SELECT Questions.PointValue FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId);

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