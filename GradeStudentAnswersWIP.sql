-- WIP code for grading student answers

DECLARE
	@specificStudentAnswewrType bigint;
	-- Set this equal to the "Discriminator" of each student answer
	-- Then, use that in a CASE statement to determine how to grade the answer
BEGIN
	SELECT StudentAnswers.StudentId, TestSchedules.TestScheduleId, Tests.TestId, TestSections.TestSectionId, Questions.QuestionId, StudentAnswers.QuestionId
	  FROM StudentAnswers
	  JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
	  JOIN Tests ON TestSchedules.TestId = Tests.TestId
	  JOIN TestSections ON Tests.TestId = TestSections.TestId
	  JOIN Questions ON TestSections.TestSectionId = Questions.TestSectionId AND Questions.QuestionId = StudentAnswers.QuestionId;
	IF ((SELECT StudentAnswers.Discriminator FROM (StudentAnswers
											   JOIN TestSchedules ON StudentAnswers.TestScheduleId = TestSchedules.TestScheduleId
											   JOIN Tests ON TestSchedules.TestId = Tests.TestId
											   JOIN TestSections ON Tests.TestId = TestSections.TestId
											   JOIN Questions ON TestSections.TestSectionId = Questions.TestSectionId AND Questions.QuestionId = StudentAnswers.QuestionId)
										   WHERE Questions.QuestionTypeId = StudentAnswers.Discriminator) = SELECT QuestionTypeId FROM QuestionTypes)
		(CASE
			WHEN 
		);
END;