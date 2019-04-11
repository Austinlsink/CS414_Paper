CREATE FUNCTION gradeTrueFalse(@c_AnswerId BIGINT)
RETURNS BIT
AS
BEGIN
	-- If the student's answer is correct (ASSUME CORRECT)
	DECLARE @correct BIT = 1; 

	-- If the answer is CORRECT
	IF ((SELECT StudentAnswers.TrueFalseAnswerGiven FROM StudentAnswers WHERE StudentAnswers.AnswerId = @c_AnswerId)
		= (SELECT Questions.TrueFalseAnswer FROM Questions JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId WHERE StudentAnswers.AnswerId = @c_AnswerId))
		SELECT @correct = 1;
	ELSE
		SELECT @correct = 0;

	-- Return whether the student's answer was correct or not
	RETURN @correct;
END;