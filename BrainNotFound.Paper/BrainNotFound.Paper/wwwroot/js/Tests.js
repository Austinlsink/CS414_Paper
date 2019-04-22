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
        url: "/api/Tests/DeleteTest/",
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

//$(".reviewStudentTest").click(function () {
//    var id = $(this).attr("data-studentId");
//    console.log(id);

//    $.ajax({
//        url: "/api/Tests/ReviewStudentTest/",
//        type: "POST",
//        contentType: "application/json",
//        // Data fetched from the form
//        data: id,
//        success: function (result) {
//            console.log("Success");
//        },
//        error: function (xhr, status, error) {
//            console.log("Failed");
//        }
//    })
//})