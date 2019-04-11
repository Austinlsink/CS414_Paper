USE [CS414_BrainNotFound]
GO
/****** Object:  UserDefinedFunction [dbo].[gradeMatching]    Script Date: 4/10/2019 9:18:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[gradeMatching](@c_AnswerId BIGINT)
RETURNS BIT
AS
BEGIN
	-- If the student's answer is correct (ASSUME CORRECT)
	DECLARE @correct BIT = 1; 


	-- If the student put as many correct answers as he was supposed to
		IF ((SELECT COUNT(*)
			   FROM StudentMatchingAnswers
			  WHERE AnswerId = @c_AnswerId)
		=
		(SELECT COUNT(*)
		   FROM MatchingQuestionSides JOIN Questions ON MatchingQuestionSides.QuestionId = Questions.QuestionId
								      JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
		  WHERE StudentAnswers.QuestionId = @c_answerId))
		BEGIN
			-- Get all the correct answers
			DECLARE matchingCorrectAnswersCursor CURSOR FOR
				SELECT MatchingQuestionSides.MatchingAnswerSideId, MatchingQuestionSides.MatchingQuestionSideId
				  FROM MatchingQuestionSides JOIN Questions ON MatchingQuestionSides.QuestionId = Questions.QuestionId
										     JOIN StudentAnswers ON Questions.QuestionId = StudentAnswers.QuestionId
				 WHERE StudentAnswers.AnswerId = @c_AnswerId;
			DECLARE @currentMatchingAnswerSideId BIGINT;
			DECLARE @currentMatchingQuestionSideId BIGINT;

			OPEN matchingCorrectAnswersCursor;
			FETCH NEXT FROM matchingCorrectAnswersCursor INTO @currentMatchingAnswerSideId, @currentMatchingQuestionSideId;

			-- Go through each of the correct matching answers
			WHILE @@FETCH_STATUS = 0
			BEGIN
				-- If the student's given answer is INCORRECT
				IF ((SELECT MatchingAnswerSideId FROM StudentMatchingAnswers WHERE MatchingQuestionSideId != @currentMatchingQuestionSideId)
					= @currentMatchingAnswerSideId)
					SELECT @correct = 0;

				FETCH NEXT FROM matchingCorrectAnswersCursor INTO @currentMatchingAnswerSideId, @currentMatchingQuestionSideId;
			END;

			CLOSE matchingCorrectAnswersCursor;
			DEALLOCATE matchingCorrectAnswersCursor;
		END;

		-- If the student did not put as many answers as he was supposed to
		ELSE
			SELECT @correct = 0;

	-- Return whether all the answers were correct or not
	RETURN @correct;
END;