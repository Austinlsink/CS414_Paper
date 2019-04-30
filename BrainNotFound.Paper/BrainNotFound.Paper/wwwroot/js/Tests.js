// Global variable for deleting the test
var testId;

// Display a confirmation modal if the user wants to delete a test
$("button.confirmDelete").click(function () {
    testId = $(this).attr("data-testId");

    $("#ConfirmModal").modal("toggle");
})

// Delete a test if the user specifies yes on the confirmation modal
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmModal").modal("hide");

    $.ajax({
        url: getPath() + "/api/Tests/DeleteTest/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: testId,
        success: function (result) {
            if (result.success) {
                $("#errorMessagePlaceHolder").text(result.message);
                $("h4#MessageModal").text("Success!");
                $("div#ErrorModal").modal("toggle");
                console.log(result.messages);
            }
            else {
                // Displays the error message to the user
                $("h4#MessageModal").text("Error!");
                $("#errorMessagePlaceHolder").text(result.error);
                $("div#ErrorModal").modal("toggle");
            }
        },
    })
})

// Reloads the page when a test is successfully deleted
$("#MessageClose").click(function () {
    location.reload();
})
$("#newTest").click(function () {
    $("select#newTestCourse")[0].selectedIndex = 0;
    $("input#newTestName").val("");
    $("#newTestNameErrorMessage").addClass("hidden");
    $("#newTestCourseErrorMessage").addClass("hidden");
    $("#NewTestModal").modal("toggle");
})

//Submits a new Test
$("#submitNewTest").click(function () {
    var pattern = RegExp('^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 _]*$');
    var testName = $.trim($("#newTestName").val());
    var hasError = false;

    // Error check the new test name
    if (testName == "") {
        $("#newTestNameErrorMessage").text("Please enter a test name").removeClass("hidden");
        hasError = true;
    }
    else if (!pattern.test(testName)) {
        $("#newTestNameErrorMessage").text("Name can only contain letters, numbers and spaces").removeClass("hidden");
        hasError = true;
    }
    else {
        $("#newTestNameErrorMessage").addClass("hidden");
    }

    // Error checks the selected course
    if ($("select#newTestCourse")[0].selectedIndex == 0) {
        $("#newTestCourseErrorMessage").removeClass("hidden");
        hasError = true;
    }
    else {
        $("#newTestCourseErrorMessage").addClass("hidden");
    }

    if (!hasError) {
        var courseId = $("#newTestCourse").val();
        var jsonData = JSON.stringify({ courseId: courseId, testName: testName });

        console.log(window.location);

        
        $.ajax({
            url: getPath() + "/api/CreateTest/NewTest/",
            type: "POST",
            contentType: "application/json",
            // Data fetched from the form
            data: jsonData,
            success: function (result) {
                if (result.success) {
                    console.log(result.test);
                    window.location.href = window.location.origin + getPath() + "/Instructor/Tests/Edit/" + result.test.course.department.departmentCode + "/" + result.test.course.courseCode + "/" + result.test.urlSafeName;
                }
                console.log("Success");
            }
        })
    }
})