CREATE FUNCTION GetOverallSignedStats()
RETURNS NVARCHAR
AS
BEGIN
	-- Whether or not the students signed the pledge
	DECLARE studentSignedCursor CURSOR FOR 
		SELECT StudentTestAssignments.Signed
		  FROM StudentTestAssignments;
	DECLARE @currentSigned REAL; -- If the current test has been signed
	DECLARE @numSigned INT = 0;
	DECLARE @numUnsigned INT = 0;

	OPEN studentSignedCursor;
	FETCH NEXT FROM studentSignedCursor INTO @currentSigned;

	-- Go through all the tests
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- See if the student has signed the pledge and update the corresponding "num" variables
		IF (@currentSigned = 1) -- If the pledge was signed
			SELECT @numSigned += 1;
		ELSE -- If the pledge wasn't signed
			SELECT @numUnsigned +=1;

		-- Get the next student grade to check
		FETCH NEXT FROM studentSignedCursor INTO @currentSigned;
	END;

	CLOSE studentSignedCursor;
	DEALLOCATE studentSignedCursor;

	-- Return a JSON object containing the signed stats
	RETURN '{"NumberSigned":"' + @numSigned + '",' +
		   ' "NumberUnsigned":"' + @numUnsigned + '"}';
END;