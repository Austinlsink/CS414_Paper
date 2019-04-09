-- Returns the answers a student gave for a particular matching question
SELECT StudentMatchingAnswers.MatchingQuestionSideId, StudentMatchingAnswers.MatchingAnswerSideId
  FROM StudentMatchingAnswers JOIN StudentAnswers ON StudentMatchingAnswers.AnswerId = StudentAnswers.AnswerId;
--WHERE StudentMatchingAnswers.AnswerId = @c_answerId;

-- Get the correct answers for the matching question
SELECT 