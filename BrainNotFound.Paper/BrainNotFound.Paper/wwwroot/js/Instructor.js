﻿// Global variables
var adminId;

// Displays the instructor form to edit a specific admin
$("button#EditInstructor").click(function () {
    var username = $(this).val();

    $.ajax({
        url: "/api/admin/Edit/" + username,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(username),
        success: function (result) {
            document.getElementById("EditSalutationInput").value = result.salutation;
            document.getElementById("EditFirstNameInput").value = result.firstName;
            document.getElementById("EditLastNameInput").value = result.lastName;
            document.getElementById("EditEmailInput").value = result.email;
            document.getElementById("EditPhoneInput").value = result.phone;
            document.getElementById("EditAddressInput").value = result.address;
            document.getElementById("EditCityInput").value = result.city;
            document.getElementById("EditStateInput").value = result.state;
            document.getElementById("EditZipInput").value = result.zip;
            document.getElementById("EditDOBInput").value = result.dob;


            $("#EditAdminModal").modal("show");
        }
    })
});

// Saves the changes on the edit form
$("button#EditSaveChanges").click(function () {
    var editAdminForm = $("form#EditAdminForm");
    var username = $(this).val();
    // Gets the values of the form, and creates an object to be sent to the server
    var admin = {};
    $.each(editAdminForm.serializeArray(), function (i, field) {
        admin[field.name] = field.value;
        console.log(admin[field.name]);
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Admin/SaveChanges/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(admin, username),
        success: function (result) {
            // Close the modal window
            console.log(result.message);
            location.reload();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            var err = JSON.parse(xhr.responseText);

            // Places validation on the First Name Field
            if (typeof err.errors.FirstName === "undefined") {
                $("#EditAdminFirstNameErrorMessage").empty();
            }
            else {
                $("#EditAdminFirstNameErrorMessage").html(err.errors.FirstName[0]);
            }

            // Places validation on the Last Name Field
            if (typeof err.errors.LastName === "undefined") {
                $("#EditAdminLastNameErrorMessage").empty();
            }
            else {
                $("#EditAdminLastNameErrorMessage").html(err.errors.LastName[0]);
            }
        }
    })
})

// Submits the form information to the server
$("button#CreateInstructor").click(function () {
    var newInstructorForm = $("form#NewInstructorForm");

    // Gets the values of the form, and creates an object to be sent to the server
    var instructor = {};
    $.each(newInstructorForm.serializeArray(), function (i, field) {
        instructor[field.name] = field.value;
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Instructor/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(instructor),
        success: function (result) {
            // Close the modal window
            console.log(result.message);
            location.reload();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            var err = JSON.parse(xhr.responseText);

            // Places validation on the First Name Field
            if (typeof err.errors.FirstName === "undefined") {
                $("#InstructorFirstNameErrorMessage").empty();
            }
            else {
                $("#InstructorFirstNameErrorMessage").html(err.errors.FirstName[0]);
            }

            // Places validation on the Last Name Field
            if (typeof err.errors.LastName === "undefined") {
                $("#InstructorLastNameErrorMessage").empty();
            }
            else {
                $("#InstructorLastNameErrorMessage").html(err.errors.LastName[0]);
            }

            // Places validation on the Password Field
            if (typeof err.errors.Password === "undefined") {
                $("#InstructorPasswordErrorMessage").empty();
            }
            else {
                $("#InstructorPasswordErrorMessage").html(err.errors.Password[0]);
            }
        }
    })
})

// Resets the new admin form modal if the user cancels it
$("button#CancelCreateInstructor").click(function () {
    var newAdminForm = $('form#NewInstructorForm');
    newAdminForm.trigger("reset");
    // Reseting span elements
    $("#InstructorFirstNameErrorMessage").empty();
    $("#InstructorLastNameErrorMessage").empty();
    $("#InstructorPasswordErrorMessage").empty();

    // Reseting input elements
    document.getElementById("InstructorFirstNameInput").value = "";
    document.getElementById("InstructorLastNameInput").value = "";
    document.getElementById("InstructorEmailInput").value = "";
    document.getElementById("InstructorPhoneInput").value = "";
    document.getElementById("InstructorAddressInput").value = "";
    document.getElementById("InstructorCityInput").value = "";
    document.getElementById("InstructorStateInput").value = "";
    document.getElementById("InstructorZipInput").value = "";
    document.getElementById("InstructorDOBInput").value = "";
    document.getElementById("InstructorPasswordInput").value = "";
});

// Display a confirmation modal if the user wants to delete a department
$("button#ConfirmDelete").click(function () {
    adminId = $(this).val();
    $("#ConfirmModal").modal("toggle");
})

// Delete a Department if the user specifies yes on the confirmation modal
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmModal").modal("hide");

    $.ajax({
        url: "/api/Admin/Delete/",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(adminId),
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