-- Compare Student answers (true/false, essay, fill-in-the-blank)
-- TRIGGER when the "Submitted" column in StudentTestAssignments is UPDATEd to 1 (test submitted)
	-- CURSOR for all StudentAnswers related to that test
	-- LOOP to go through all StudentAnswers related to that test

		-- IF "Signed" column = 1 (if the student signed the pledge) THEN

			-- IF the question being graded is FillInTheBlank (see WHERE condition)
				SELECT StudentAnswers.AnswerId, StudentAnswers.QuestionId, StudentAnswers.StudentId, StudentAnswers.TestScheduleId, Questions.QuestionId, AspNetUsers.Id AS StudentId, TestSchedules.TestScheduleId
				  FROM StudentAnswers JOIN Questions ON StudentAnswers.QuestionId = Questions.QuestionId
									  JOIN AspNetUsers ON StudentAnswers.StudentId = AspNetUsers.Id
									  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
									  JOIN StudentTestAssignments ON (AspNetUsers.Id = StudentTestAssignments.StudentTestAssignmentId) AND (StudentTestAssignments.TestId = TestSchedules.TestId)
				 WHERE StudentAnswers.Discriminator = (SELECT QuestionTypeId FROM QuestionTypes WHERE QuestionTypes.Name = 'FillInTheBlank');

			-- IF the question being graded is TrueFalse (see WHERE condition)
				SELECT StudentAnswers.AnswerId, StudentAnswers.QuestionId, StudentAnswers.StudentId, StudentAnswers.TestScheduleId, Questions.QuestionId, AspNetUsers.Id AS StudentId, TestSchedules.TestScheduleId
				  FROM StudentAnswers JOIN Questions ON StudentAnswers.QuestionId = Questions.QuestionId
									  JOIN AspNetUsers ON StudentAnswers.StudentId = AspNetUsers.Id
									  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
									  JOIN StudentTestAssignments ON (AspNetUsers.Id = StudentTestAssignments.StudentTestAssignmentId) AND (StudentTestAssignments.TestId = TestSchedules.TestId)
				 WHERE StudentAnswers.Discriminator = (SELECT QuestionTypes.QuestionTypeId FROM QuestionTypes WHERE QuestionTypes.Name = 'TrueFalse');

			-- IF the question being graded is Essay (see WHERE condition)
				SELECT StudentAnswers.AnswerId, StudentAnswers.QuestionId, StudentAnswers.StudentId, StudentAnswers.TestScheduleId, Questions.QuestionId, AspNetUsers.Id AS StudentId, TestSchedules.TestScheduleId
				  FROM StudentAnswers JOIN Questions ON StudentAnswers.QuestionId = Questions.QuestionId
									  JOIN AspNetUsers ON StudentAnswers.StudentId = AspNetUsers.Id
									  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
									  JOIN StudentTestAssignments ON (AspNetUsers.Id = StudentTestAssignments.StudentTestAssignmentId) AND (StudentTestAssignments.TestId = TestSchedules.TestId)
				 WHERE StudentAnswers.Discriminator = (SELECT QuestionTypes.QuestionTypeId FROM QuestionTypes WHERE QuestionTypes.Name = 'Essay');

		-- ELSE (set grade to 0)

-- Compare student matching answers to the corresponding question
SELECT *
  FROM StudentMatchingAnswers;

-- Compare student multiple choice answers to the corresponding question
SELECT *
  FROM StudentMultipleChoiceAnswers;