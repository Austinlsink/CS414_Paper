// Global variables
var adminId;
var username;


$("button#ViewAdmin").click(function () {
    var username = $(this).val();
    $.ajax({
        url: getPath() + "/Admin/Administrators/" + username,
        success: function (result) {
            $("#ViewAdminPlaceholder").html(result);
        }
    });
})

// Displays the admin form to edit a specific admin
$("button#EditAdmin").click(function () {
    var username = $(this).val();
    $.ajax({
        url: getPath() + "/Admin/Administrators/Edit/" + username,
        success: function (result) {
            $("#EditAdminPlaceholder").html(result);
        }
    });
})

// Saves the changes on the edit form
$("button#EditSaveChanges").click(function () {
    var editAdminForm = $("form#EditAdminForm");
    // Gets the values of the form, and creates an object to be sent to the server
    var admin = {};
    $.each(editAdminForm.serializeArray(), function (i, field) {
        admin[field.name] = field.value;
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: getPath() + "/api/Admin/SaveChanges/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(admin, username),
        success: function (result) {
            // Close the modal window
            location.reload();
        },
        error: function (xhr, status, error) {
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
$("button#CreateAdmin").click(function () {
    var newAdminForm = $("form#NewAdminForm");

    // Gets the values of the form, and creates an object to be sent to the server
    var admin = {};
    $.each(newAdminForm.serializeArray(), function (i, field) {
        admin[field.name] = field.value;
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: getPath() + "/api/Admin/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(admin),
        success: function (result) {
            // Close the modal window
            location.reload();
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);

            // Places validation on the First Name Field
            if (typeof err.errors.FirstName === "undefined") {
                $("#AdminFirstNameErrorMessage").empty();
            }
            else {
                $("#AdminFirstNameErrorMessage").html(err.errors.FirstName[0]);
            }

            // Places validation on the Last Name Field
            if (typeof err.errors.LastName === "undefined") {
                $("#AdminLastNameErrorMessage").empty();
            }
            else {
                $("#AdminLastNameErrorMessage").html(err.errors.LastName[0]);
            }

            // Places validation on the Password Field
            if (typeof err.errors.Password === "undefined") {
                $("#AdminPasswordErrorMessage").empty();
            }
            else {
                $("#AdminPasswordErrorMessage").html(err.errors.Password[0]);
            }

            // Places validation on the Password Field
            if (typeof err.errors.ConfirmPassword === "undefined") {
                $("#AdminConfirmPasswordErrorMessage").empty();
            }
            else {
                $("#AdminConfirmPasswordErrorMessage").html(err.errors.ConfirmPassword[0]);
            }
        }
    })
})

// Resets the new admin form modal if the user cancels it
$("button#CancelCreateAdmin").click(function () {
    var newAdminForm = $('form#NewAdminForm');
    newAdminForm.trigger("reset");
    // Reseting span elements
    $("#AdminFirstNameErrorMessage").empty();
    $("#AdminLastNameErrorMessage").empty();
    $("#AdminPasswordErrorMessage").empty();

    // Reseting input elements
    document.getElementById("FirstNameInput").value = "";
    document.getElementById("LastNameInput").value = "";
    document.getElementById("EmailInput").value = "";
    document.getElementById("PhoneInput").value = "";
    document.getElementById("AddressInput").value = "";
    document.getElementById("CityInput").value = "";
    document.getElementById("StateInput").value = "";
    document.getElementById("ZipInput").value = "";
    document.getElementById("DOBInput").value = "";
    document.getElementById("PasswordInput").value = "";
});

// Display a confirmation modal if the user wants to delete a department
$("button#ConfirmDelete").click(function () {
    adminId = $(this).val();
    $("#ConfirmModal").modal("toggle");
})

// Delete an administrator if the user specifies yes on the confirmation modal
$("button#YesDelete").click(function () {
    // Gets the department Id to be deleted
    $("#ConfirmModal").modal("hide");

    $.ajax({
        url: getPath() + "/api/Admin/Delete/",
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