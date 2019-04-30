﻿// Global variable for deleting the course
var deleteDepartmentId;

// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
    $.ajax({
        url: getPath() + "/Admin/Department/Edit/" + Id,
        success: function (result) {
            $("#departmentPlaceholder").html(result);
        }
    });
}

// Event Handlers

// Resets Modal if canceled
$("button#CancelCreateDepartment").click(function () {
    var newDepartmentForm = $('form#NewDepartment');
    newDepartmentForm.trigger("reset");
    $("#departmentCodeErrorMessage").empty();
    $("#departmentNameErrorMessage").empty();
    $("#errorMessage").addClass("hidden");
});


// Display a confirmation modal if the user wants to delete a department
$("button#ConfirmDelete").click(function () {
    deleteDepartmentId = $(this).val();
    $("#ConfirmModal").modal("toggle");
})

// Delete a Department if the user specifies yes on the confirmation modal
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmModal").modal("hide");
    
    $.ajax({
        url: getPath() + "/api/department/delete/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(deleteDepartmentId),
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
        },

    })
})

// Reloads the page when a Course is successfully deleted
$("#MessageClose").click(function () {
    location.reload();
})

// Reloads the page when a department is successfully deleted
$("#MessageClose").click(function () {
    location.reload();
})

// Submits the form information to the server
$("button#CreateDepartment").click(function () {
    var newDepartmentForm = $("form#NewDepartment");

    // Gets the values of the form, and creates an object to be sent to the server
    var department = {};
    $.each(newDepartmentForm.serializeArray(), function (i, field) {
        department[field.name] = field.value;
    });
    
    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: getPath() + "/api/Department/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(department),
        success: function (result) {
            // Close the modal window
            console.log(result);
            if (result.sucess) {
                location.reload();
            }
            else {
                $("#departmentCodeErrorMessage").text(result.deparmentCodeError);
                $("#departmentNameErrorMessage").text(result.deparmentNameError);
            }
            
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);
            $("#errorMessage").addClass("hidden");
            // Places validation on the Department Code Field
            if (typeof err.errors.DepartmentCode === "undefined") {
                
            }
            else {
                $("#departmentCodeErrorMessage").html(err.errors.DepartmentCode[0]);
            }

            // Places validation on the Department Name Field
            if (typeof err.errors.DepartmentName === "undefined") {
                $("#departmentNameErrorMessage").empty();
            }
            else {
                $("#departmentNameErrorMessage").html(err.errors.DepartmentName[0]);
            }
        }
    })
})