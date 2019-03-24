-- Get enrollments with student names
SELECT Enrollments.EnrollmentId, AspNetUsers.FirstName + AspNetUsers.LastName AS Student, Enrollments.SectionId
  FROM Enrollments JOIN AspNetUsers ON Enrollments.StudentId = AspNetUsers.Id
ORDER BY Student, SectionId;

-- Get course of a specific section
SELECT Courses.Name
FROM Courses Join Sections ON Courses.CourseId = Sections.CourseId
WHERE SectionId = 16;

-- Get course related to above section
SELECT Courses.Name, Sections.SectionId
FROM Courses JOIN Sections ON Courses.CourseId = Sections.CourseId
WHERE Courses.Name = 'Origins';

-- Get the count of sections related to each course with sections
SELECT Courses.CourseId, Courses.Name, COUNT(*)
  FROM Courses JOIN Sections ON Courses.CourseId = Sections.CourseId
GROUP BY  Courses.CourseId, Courses.Name
ORDER BY Courses.CourseId;