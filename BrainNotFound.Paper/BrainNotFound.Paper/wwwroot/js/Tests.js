﻿// Global variable for deleting the test
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
                $("#errorMessagePlaceHolder").text(result.messages);
                $("h4#MessageModal").text("Success!");
                $("div#ErrorModal").modal("toggle");
            }
            else {
                // Displays the error message to the user
                $("h4#MessageModal").text("Error!");
                $("#errorMessagePlaceHolder").text(result.messages);
                $("div#ErrorModal").modal("toggle");
                console.log(result.ErrorMessage);
            }
        },
    })
})
