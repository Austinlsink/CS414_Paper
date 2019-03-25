var deleteCourseId;

// Resets Modal if canceled
$("button#CancelCreateCourse").click(function () {
    var newDepartmentForm = $('form#NewCourse');
    newDepartmentForm.trigger("reset");
    $("#courseCodeErrorMessage").empty();
    $("#courseNameErrorMessage").empty();
    $("#courseDescriptionErrorMessage").empty();
    $("#courseCreditHoursErrorMessage").empty();
    $("#courseDepartmentErrorMessage").empty();

    document.getElementById("courseCodeInput").value = "";
    document.getElementById("courseNameInput").value = "";
    document.getElementById("courseDescriptionInput").value = "";
    document.getElementById("courseCreditHoursInput").value = "";
    document.getElementById("courseDepartmentInput").value = "";
});

// Edit a course
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

// Confirm deleting a course
$("button#ConfirmDelete").click(function () {
    deleteCourseId = $(this).val();

    console.log("Delete course ID: " + deleteCourseId);
    $("#ConfirmModal").modal("toggle");
})

// Delete a course
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    console.log(deleteCourseId);
    $("#ConfirmModal").modal("hide");

    $.ajax({
        url: "/api/Course/delete/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(deleteCourseId),
        success: function (result) {
            if (result.success) {
                $("#errorMessagePlaceHolder").text(result.message)
                $("div#ErrorModal").modal("toggle");
            }
            else {
                // Displays the error message to the user
                $("#errorMessagePlaceHolder").text(result.message)
                $("div#ErrorModal").modal("toggle");
            }
        }
    })
})

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
            console.log(xhr);
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
            console.log(xhr);
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
