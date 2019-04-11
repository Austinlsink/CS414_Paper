CREATE FUNCTION gradeMultipleChoice(@c_AnswerId BIGINT)
RETURNS BIT
AS
BEGIN
	-- If the student's answer is correct (ASSUME CORRECT)
	DECLARE @correct BIT = 1; 

	-- If the student put as many right answers as the question had
	IF ((SELECT COUNT(*)
		   FROM StudentMultipleChoiceAnswers JOIN StudentAnswers ON StudentMultipleChoiceAnswers.AnswerId = StudentAnswers.AnswerId
		  WHERE StudentAnswers.QuestionId = @c_answerId) =
		(SELECT COUNT(*)
		   FROM MultipleChoiceAnswers JOIN Questions ON MultipleChoiceAnswers.QuestionId = Questions.QuestionId
			  						  JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
		  WHERE StudentAnswers.QuestionId = @c_answerId AND MultipleChoiceAnswers.IsCorrect = 1 ))
	BEGIN
		-- Get all the answers the student gave for the question
		DECLARE studentMultipleChoiceAnswersCursor CURSOR FOR
			SELECT StudentMultipleChoiceAnswers.MultipleChoiceAnswerId
			  FROM StudentMultipleChoiceAnswers JOIN StudentAnswers ON StudentMultipleChoiceAnswers.AnswerId = StudentAnswers.AnswerId
			 WHERE StudentAnswers.QuestionId = @c_answerId;
		DECLARE @currentStudentMultipleChoiceAnswerId BIGINT;
						
		OPEN studentMultipleChoiceAnswersCursor;
		FETCH NEXT FROM studentMultipleChoiceAnswersCursor INTO @currentStudentMultipleChoiceAnswerId;

		-- Grade answers
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- If the student's answer is NOT in the correct multiple choice answers
			IF (@currentStudentMultipleChoiceAnswerId NOT IN (SELECT MultipleChoiceAnswers.MultipleChoiceAnswerId
																FROM MultipleChoiceAnswers JOIN Questions ON MultipleChoiceAnswers.QuestionId = Questions.QuestionId
																							JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
																WHERE StudentAnswers.QuestionId = @c_answerId AND MultipleChoiceAnswers.IsCorrect = 1))
			BEGIN
				SELECT @correct = 0;
			END;

			-- Get next student answer
			FETCH NEXT FROM studentMultipleChoiceAnswersCursor INTO @currentStudentMultipleChoiceAnswerId;
		END;

		CLOSE studentMultipleChoiceAnswersCursor;
		DEALLOCATE studentMultipleChoiceAnswersCursor;
	END;
	-- If the student did not put as many answers as he was supposed to
	ELSE
		SELECT @correct = 0;

	-- Return whether all the answers were correct or not
	RETURN @correct;
END;