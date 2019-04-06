CREATE FUNCTION GetLetterGradeStats(@inputTestId BIGINT)
RETURNS NVARCHAR
AS
BEGIN
	-- The total number of points for the selected test
	DECLARE @totalTestPoints FLOAT = (SELECT SUM(Questions.PointValue) AS TotalTestPoints
									    FROM Tests JOIN TestSections ON Tests.TestId = TestSections.TestSectionId
											       JOIN Questions ON TestSections.TestSectionId = Questions.TestSectionId
								       WHERE Tests.TestId = @inputTestId);
	-- The number of points each student got on the selected test
	DECLARE studentTestPointsCursor CURSOR FOR 
		SELECT StudentTestAssignments.Grade
		  FROM StudentTestAssignments JOIN TestSchedules ON StudentTestAssignments.TestScheduleId = TestSchedules.TestScheduleId
									  JOIN Tests ON TestSchedules.TestId = Tests.TestId
		 WHERE Tests.TestId = @inputTestId;
	DECLARE @currentStudentTestPoints FLOAT; -- The number of points one student got on the selected test
	DECLARE @currentStudentGradePercentage FLOAT; -- The student's grade as a percentage
	DECLARE @numOfAs INT = 0;
	DECLARE @numOfBs INT = 0;
	DECLARE @numOfCs INT = 0;
	DECLARE @numOfDs INT = 0;
	DECLARE @numOfFs INT = 0;
	DECLARE @A FLOAT = 0.9;
	DECLARE @B FLOAT = 0.8;
	DECLARE @C FLOAT = 0.7;
	DECLARE @D FLOAT = 0.6;


	OPEN studentTestPointsCursor;
	FETCH NEXT FROM studentTestPointsCursor INTO @currentStudentTestPoints;

	-- Go through all the grades for the test
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Get the student's grade as a percentage
		SELECT @currentStudentGradePercentage = @currentStudentTestPoints / @totalTestPoints;

		-- Determine what letter grade the student got and update the corresponding "numOf" variable
		IF (@currentStudentGradePercentage >= @A)
			SELECT @numOfAs += 1;
		ELSE IF (@currentStudentGradePercentage >= @B)
			SELECT @numOfBs += 1;
		ELSE IF (@currentStudentGradePercentage >= @C)
			SELECT @numOfCs += 1;
		ELSE IF (@currentStudentGradePercentage >= @D)
			SELECT @numOfDs += 1;
		ELSE -- If the student got less than a D
			SELECT @numOfFs += 1;

		-- Get the next student grade to check
		FETCH NEXT FROM studentTestPointsCursor INTO @currentStudentTestPoints;
	END;

	CLOSE studentTestPointsCursor;
	DEALLOCATE studentTestPointsCursor;

	-- Return a JSON object containing the number of each letter grade
	RETURN '{"NumberOfAs":"' + @numOfAs + '",' +
		   ' "NumberOfBs":"' + @numOfBs + '",' +
		   ' "NumberOfCs":"' + @numOfCs + '",' +
		   ' "NumberOfDs":"' + @numOfDs + '",' +
		   ' "NumberOfFs":"' + @numOfFs + '"}';
END;