CREATE FUNCTION GetStudentAverageGradePerSection(@inputStudentId BIGINT, @inputSectionId BIGINT)
RETURNS NVARCHAR
AS
BEGIN
	-- All the tests a student has taken in a specific section
	DECLARE studentTestAssignmentsCursor CURSOR FOR SELECT StudentTestAssignments.StudentTestAssignmentId, Tests.TestId
										    FROM StudentTestAssignments JOIN TestSchedules ON StudentTestAssignments.TestScheduleId = TestSchedules.TestScheduleId
																	    JOIN Tests ON TestSchedules.TestId = Tests.TestId
												  					    JOIN Courses ON Tests.CourseId = Courses.CourseId
																	    JOIN Sections ON Courses.CourseId = Sections.SectionId
										   WHERE StudentTestAssignments.StudentId = @inputStudentId AND Sections.SectionId = @inputSectionId;
	DECLARE @currentStudentTestAssignmentId BIGINT;
	DECLARE @currentTestId BIGINT;

	-- The number of tests a student has taken in a specific section
	DECLARE @numOfTests FLOAT = (SELECT COUNT(*)
								  FROM StudentTestAssignments JOIN TestSchedules ON StudentTestAssignments.TestScheduleId = TestSchedules.TestScheduleId
															  JOIN Tests ON TestSchedules.TestId = Tests.TestId
															  JOIN Courses ON Tests.CourseId = Courses.CourseId
															  JOIN Sections ON Courses.CourseId = Sections.SectionId
								 WHERE StudentTestAssignments.StudentId = @inputStudentId AND Sections.SectionId = @inputSectionId);
	DECLARE @sumOfStudentGradePercentages FLOAT = 0.0;

	OPEN studentTestAssignmentsCursor;
	FETCH NEXT FROM studentTestAssignmentsCursor into @currentStudentTestAssignmentId, @currentTestId;

	-- Go through all of the student's tests in this section
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- The total number of points for the selected test
		DECLARE @totalTestPoints FLOAT = (SELECT SUM(Questions.PointValue) AS TotalTestPoints
											FROM Tests JOIN TestSections ON Tests.TestId = TestSections.TestSectionId
													   JOIN Questions ON TestSections.TestSectionId = Questions.TestSectionId
										   WHERE Tests.TestId = @currentTestId);

		-- The number of points each student got on the selected test
		DECLARE @studentTestPoints FLOAT = (SELECT StudentTestAssignments.Grade
											  FROM StudentTestAssignments
											 WHERE StudentTestAssignments.StudentTestAssignmentId = @currentStudentTestAssignmentId);
		-- The student's grade as a percentage
		DECLARE @studentGradePercentage FLOAT;

		-- Get the student's grade as a percentage
		SELECT @studentGradePercentage = @studentTestPoints / @totalTestPoints;

		-- Add that grade to the average
		SELECT @sumOfStudentGradePercentages += @studentGradePercentage;

		FETCH NEXT FROM studentTestAssignmentsCursor into @currentStudentTestAssignmentId, @currentTestId;
	END;

	CLOSE studentTestAssignmentsCursor;
	DEALLOCATE studentTestAssignmentsCursor;

	-- Return a JSON object containing the section's average for the selected test
	RETURN '{"StudentSectionAverage":"' + @sumOfStudentGradePercentages / @numOfTests + '"}';
END;