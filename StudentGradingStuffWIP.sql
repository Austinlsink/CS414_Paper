-- Compare Student answers (true/false, essay, fill-in-the-blank)
SELECT StudentAnswers.AnswerId, StudentAnswers.QuestionId, StudentAnswers.StudentId, StudentAnswers.TestScheduleId, Questions.QuestionId, AspNetUsers.Id AS StudentId, TestSchedules.TestScheduleId
  FROM StudentAnswers JOIN Questions ON StudentAnswers.QuestionId = Questions.QuestionId
				      JOIN AspNetUsers ON StudentAnswers.StudentId = AspNetUsers.Id
					  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId;
-- WHERE StudentAnswers.Discriminator = [Insert type of question you're grading];

-- Compare student matching answers to the corresponding question
SELECT *
  FROM StudentMatchingAnswers;

-- Compare student multiple choice answers to the corresponding question
SELECT *
  FROM StudentMultipleChoiceAnswers;