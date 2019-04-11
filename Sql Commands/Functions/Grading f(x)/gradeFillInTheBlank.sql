CREATE FUNCTION gradeFillInTheBlank(@c_AnswerId BIGINT)
RETURNS BIT
AS
BEGIN
	-- If the student's answer is correct (ASSUME CORRECT)
	DECLARE @correct BIT = 1; 

	-- If the student input as many answers as the question needed
	IF ((SELECT COUNT(*)
		  FROM StudentFillInTheBlankAnswers
		 WHERE AnswerId = @c_AnswerId)
		=
		(SELECT COUNT(*)
		   FROM FillInTheBlankQuestions JOIN Questions ON FillInTheBlankQuestions.QuestionId = Questions.QuestionId
										JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
		  WHERE StudentAnswers.QuestionId = @c_answerId))
	BEGIN
			-- All the expected answers for the question
			DECLARE fillInTheBlankCorrectAnswersCursor CURSOR FOR
					SELECT FillInTheBlankQuestions.FillInTheBlankAnswer, FillInTheBlankQuestions.FillInTheBlankQuestionId
					FROM FillInTheBlankQuestions JOIN Questions ON FillInTheBlankQuestions.QuestionId = Questions.QuestionId
											     JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
					WHERE StudentAnswers.QuestionId = @c_answerId;
			DECLARE @currentCorrectFillInTheBlankAnswer NVARCHAR;
			DECLARE @currentCorrectFillInTheBlankId BIGINT;

			OPEN fillInTheBlankCorrectAnswersCursor
			FETCH NEXT FROM fillInTheBlankCorrectAnswersCursor INTO @currentCorrectFillInTheBlankAnswer, @currentCorrectFillInTheBlankId

			-- Grade answers
			WHILE @@FETCH_STATUS = 0
			BEGIN
				-- If the answer is INCORRECT!!!
				IF ((SELECT AnswerGiven FROM StudentFillInTheBlankAnswers WHERE FillInTheBlankQuestionId = @currentCorrectFillInTheBlankId) != @currentCorrectFillInTheBlankAnswer)
				BEGIN
					SELECT @correct = 0;
				END;

				-- Get next expected answer
				FETCH NEXT FROM fillInTheBlankCorrectAnswersCursor INTO @currentCorrectFillInTheBlankAnswer, @currentCorrectFillInTheBlankId
			END;

			CLOSE fillInTheBlankCorrectAnswersCursor;
			DEALLOCATE fillInTheBlankCorrectAnswersCursor;
	END;

	-- If the student did not put in as many answers as he should have
	ELSE
		SELECT @correct = 0;

	-- Return whether all the answers were correct or not
	RETURN @correct;
END;