// Global variable for deleting the course
var deleteCourseId;
var departmentCode;
var courseCode;
var sectionNumber;
var sectionId;

// Display a confirmation modal if the user wants to delete a section
$("button#ConfirmSectionDelete").click(function () {
    departmentCode = $(this).attr("data-DepartmentCode");
    courseCode = $(this).attr("data-CourseCode");
    sectionNumber = $(this).attr("data-SectionNumber");
    sectionId = $(this).attr("data-SectionId");
    console.log(departmentCode, courseCode, sectionNumber);
    $("#ConfirmSectionModal").modal("toggle");
})

// Delete a section if the user specifies yes on the confirmation modal
$("button#YesSectionDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmSectionModal").modal("hide");

    var JsonData = JSON.stringify({ DepartmentCode: departmentCode, CourseCode: courseCode, SectionNumber: sectionNumber, SectionId: sectionId });
    
    $.ajax({
        url: "/api/Course/DeleteSection/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {
                $("#errorMessagePlaceHolder").text(result.message)
                $("h4#MessageModal").text("Success!");
                $("div#ErrorSectionModal").modal("toggle");
                reload();
            }
            else {
                // Displays the error message to the user
                $("h4#MessageModal").text("Error!");
                $("#errorMessagePlaceHolder").text(result.message)
                $("div#ErrorSectionModal").modal("toggle");
            }
        }
    })
})


// Resets the new course form modal if the user cancels it
$("button#CancelCreateCourse").click(function () {
    var newCourseForm = $('form#NewCourse');
    newCourseForm.trigger("reset");
    // Reseting span elements
    $("#courseCodeErrorMessage").empty();
    $("#courseNameErrorMessage").empty();
    $("#courseDescriptionErrorMessage").empty();
    $("#courseCreditHoursErrorMessage").empty();
    $("#courseDepartmentErrorMessage").empty();

    // Reseting input elements
    document.getElementById("courseCodeInput").value = "";
    document.getElementById("courseNameInput").value = "";
    document.getElementById("courseDescriptionInput").value = "";
    document.getElementById("courseCreditHoursInput").value = "";
    document.getElementById("courseDepartmentInput").value = "";
});

// Displays a modal form to edit a specific course
$("button#EditCourse").click(function () {
    var courseId = $(this).val();

    $.ajax({
        url: "/api/course/edit/" + courseId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(courseId),
        success: function (result, courseCode){
            document.getElementById("codeInput").value = result.code;
            document.getElementById("nameInput").value = result.name;
            document.getElementById("descriptionInput").value = result.description;
            document.getElementById("creditHourInput").value = result.creditHours;
            document.getElementById("departmentInput").value = result.department;
            document.getElementById("courseId").value = result.id;

            $("#EditCourseModal").modal("show");
        }
    })
});

// Display a confirmation modal if the user wants to delete a course
$("button#ConfirmDelete").click(function () {
    deleteCourseId = $(this).val();
    $("#ConfirmModal").modal("toggle");
})

// Delete a course if the user specifies yes on the confirmation modal
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmModal").modal("hide");

    $.ajax({
        url: "/api/Course/delete/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(deleteCourseId),
        success: function (result) {
            if (result.success) {
                $("#errorMessagePlaceHolder").text(result.message)
                $("h4#MessageModal").text("Success!");
                $("div#ErrorModal").modal("toggle");
            }
            else {
                // Displays the error message to the user
                $("h4#MessageModal").text("Error!");
                $("#errorMessagePlaceHolder").text(result.message)
                $("div#ErrorModal").modal("toggle");
            }
        }
    })
})

// Reloads the page when a Course is successfully deleted
$("#MessageClose").click(function () {
    location.reload();
})

// Allows a course to be edited and saved
$("#SaveCourseChanges").click(function () {
    var newCourseForm = $("form#EditCourseForm");

    // Gets the values of the form, and creates an object to be sent to the server
    var course = {};
    $.each(newCourseForm.serializeArray(), function (i, field) {
        if (field.value == null || field.value == "undefined") {
            course[field.name] = 0;
        }
        else {
            course[field.name] = field.value;
        }
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Course/Save/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(course),
        success: function (result) {
            // Close the modal window
            location.reload();
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);

            // Places validation on the Course Code Field
            if (typeof err.errors.CourseCode === "undefined") {
                $("#EditCourseCodeErrorMessage").empty();
            }
            else {
                $("#EditCourseCodeErrorMessage").html(err.errors.CourseCode[0]);
            }

            // Places validation on the Course Name Field
            if (typeof err.errors.Name === "undefined") {
                $("#EditCourseNameErrorMessage").empty();
            }
            else {
                $("#EditCourseNameErrorMessage").html(err.errors.Name[0]);
            }

            // Places validation on the Course Description Field
            if (typeof err.errors.Description === "undefined") {
                $("#EditCourseDescriptionErrorMessage").empty();
            }
            else {
                $("#EditCourseDescriptionErrorMessage").html(err.errors.Description[0]);
            }

            // Places validation on the Course Credit Hour Field
            if (typeof err.errors.CreditHours === "undefined") {
                $("#EditCourseCreditHoursErrorMessage").empty();
            }
            else {
                $("#EditCourseCreditHoursErrorMessage").html(err.errors.CreditHours[0]);
            }
        }
    })
})

// Submits the form information to the server
$("button#CreateCourse").click(function () {
    var newCourseForm = $("form#NewCourse");

    // Gets the values of the form, and creates an object to be sent to the server
    var course = {};
    $.each(newCourseForm.serializeArray(), function (i, field) {
            if (field.value == null || field.value == "undefined") {
                course[field.name] = 0;
            }
        else {
            course[field.name] = field.value;
        }
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Course/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(course),
        success: function (result) {
            // Close the modal window
            location.reload();
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);

            // Places validation on the Course Code Field
            if (typeof err.errors.CourseCode === "undefined") {
                $("#courseCodeErrorMessage").empty();
            }
            else {
                $("#courseCodeErrorMessage").html(err.errors.CourseCode[0]);
            }

            // Places validation on the Course Name Field
            if (typeof err.errors.Name === "undefined") {
                $("#courseNameErrorMessage").empty();
            }
            else {
                $("#courseNameErrorMessage").html(err.errors.Name[0]);
            }

            // Places validation on the Course Description Field
            if (typeof err.errors.Description === "undefined") {
                $("#courseDescriptionErrorMessage").empty();
            }
            else {
                $("#courseDescriptionErrorMessage").html(err.errors.Description[0]);
            }

            // Places validation on the Course Credit Hour Field
            if (typeof err.errors.CreditHours === "undefined") {
                $("#courseCreditHoursErrorMessage").empty();
            }
            else {
                $("#courseCreditHoursErrorMessage").html(err.errors.CreditHours[0]);
            }
        }
    })
})
