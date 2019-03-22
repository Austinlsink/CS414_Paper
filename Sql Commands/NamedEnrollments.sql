-- Get enrollments with student names
SELECT Enrollments.EnrollmentId, AspNetUsers.FirstName + AspNetUsers.LastName AS Student, Enrollments.SectionId
  FROM Enrollments JOIN AspNetUsers ON Enrollments.StudentId = AspNetUsers.Id
ORDER BY Student, SectionId;
