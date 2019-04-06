-- Get the total number of points a test is worth (returns a single value)
SELECT SUM(Questions.PointValue) AS TotalTestPoints
  FROM Tests JOIN TestSections ON Tests.TestId = TestSections.TestSectionId
			 JOIN Questions ON TestSections.TestSectionId = Questions.TestSectionId;
-- WHERE Tests.TestId = given testId

-- Get the number of points each student got on a particular test (returns multiple values)
SELECT StudentTestAssignments.Grade
  FROM StudentTestAssignments JOIN TestSchedules ON StudentTestAssignments.TestScheduleId = TestSchedules.TestScheduleId
							  JOIN Tests ON TestSchedules.TestId = Tests.TestId;
-- WHERE Tests.TestId = given testId