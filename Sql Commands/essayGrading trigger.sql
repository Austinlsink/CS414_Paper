CREATE TRIGGER addEssayPoints ON StudentAnswers
AFTER UPDATE
AS
IF (UPDATE (PointsEarned))
BEGIN
	-- The AnswerIds of the recently graded essay answers
	DECLARE essayAnswerIdCursor CURSOR FOR
		SELECT AnswerId
		  FROM inserted;
	DECLARE @currentAnswerId BIGINT;

	-- Go through all the recently graded essay answers
	OPEN essayAnswerIdCursor;
	FETCH NEXT FROM essayAnswerIdCursor INTO @currentAnswerId;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF ((SELECT PointsEarned
			  FROM StudentAnswers
			 WHERE StudentAnswers.AnswerId = @currentAnswerId) >= 0)
		BEGIN
			-- Add the points to the corresponding StudentTestAssignment using the StudentAnswers's TestScheduleId in a subquerey
		END;

		FETCH NEXT FROM essayAnswerIdCursor INTO @currentAnswerId;
	END;

	CLOSE essayAnswerIdCursor;
	DEALLOCATE essayAnswerIdCursor;

END;