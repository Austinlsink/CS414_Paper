CREATE FUNCTION GetSectionTestAverage(@inputSectionId BIGINT, @inputTestId BIGINT)
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
									  JOIN Courses ON Tests.CourseId = Courses.CourseId
									  JOIN Sections ON Courses.CourseId = Sections.CourseId
		 WHERE Tests.TestId = @inputTestId AND Sections.SectionId = @inputSectionId;
	DECLARE @currentStudentTestPoints FLOAT; -- The number of points one student got on the selected test
	DECLARE @currentStudentGradePercentage FLOAT; -- The student's grade as a percentage
	DECLARE @sumOfStudentGradePercentages FLOAT = 0.0;
	DECLARE @numberOfStudents FLOAT = (SELECT COUNT(*)
									     FROM StudentTestAssignments JOIN TestSchedules ON StudentTestAssignments.TestScheduleId = TestSchedules.TestScheduleId
																     JOIN Tests ON TestSchedules.TestId = Tests.TestId
																     JOIN Courses ON Tests.CourseId = Courses.CourseId
																     JOIN Sections ON Courses.CourseId = Sections.CourseId
									    WHERE Tests.TestId = @inputTestId AND Sections.SectionId = @inputSectionId);


	OPEN studentTestPointsCursor;
	FETCH NEXT FROM studentTestPointsCursor INTO @currentStudentTestPoints;

	-- Go through all the grades for the test
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Get the student's grade as a percentage
		SELECT @currentStudentGradePercentage = @currentStudentTestPoints / @totalTestPoints;

		-- Add that grade to the average
		SELECT @sumOfStudentGradePercentages += @currentStudentGradePercentage;

		-- Get the next student grade to check
		FETCH NEXT FROM studentTestPointsCursor INTO @currentStudentTestPoints;
	END;

	CLOSE studentTestPointsCursor;
	DEALLOCATE studentTestPointsCursor;

	-- Return a JSON object containing the section's average for the selected test
	RETURN '{"SectionAverage":"' + @sumOfStudentGradePercentages / @numberOfStudents + '"}';
END;