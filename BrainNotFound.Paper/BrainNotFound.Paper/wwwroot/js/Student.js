// Global variables
var studentId;
var username;

// Displays the admin form to edit a specific admin
$("button#EditStudent").click(function () {
    var username = $(this).val();
    $.ajax({
        url: "/Admin/Students/Edit/" + username,
        success: function (result) {
            $("#EditStudentPlaceholder").html(result);
        }
    });
});

// Saves the changes on the edit form
$("button#EditSaveChanges").click(function () {
    var editStudentForm = $("form#EditStudentForm");
    // Gets the values of the form, and creates an object to be sent to the server
    var student = {};
    $.each(editStudentForm.serializeArray(), function (i, field) {
        student[field.name] = field.value;
        console.log(admin[field.name]);
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Student/SaveChanges/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(student, username),
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
                $("#EditStudentFirstNameErrorMessage").empty();
            }
            else {
                $("#EditStudentFirstNameErrorMessage").html(err.errors.FirstName[0]);
            }

            // Places validation on the Last Name Field
            if (typeof err.errors.LastName === "undefined") {
                $("#EditStudentLastNameErrorMessage").empty();
            }
            else {
                $("#EditStudentLastNameErrorMessage").html(err.errors.LastName[0]);
            }
        }
    })
})

// Submits the form information to the server
$("button#CreateStudent").click(function () {
    var newStudentForm = $("form#NewStudentForm");

    // Gets the values of the form, and creates an object to be sent to the server
    var student = {};
    $.each(newStudentForm.serializeArray(), function (i, field) {
        student[field.name] = field.value;
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Student/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(student),
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
                $("#StudentFirstNameErrorMessage").empty();
            }
            else {
                $("#StudentFirstNameErrorMessage").html(err.errors.FirstName[0]);
            }

            // Places validation on the Last Name Field
            if (typeof err.errors.LastName === "undefined") {
                $("#StudentLastNameErrorMessage").empty();
            }
            else {
                $("#StudentLastNameErrorMessage").html(err.errors.LastName[0]);
            }

            // Places validation on the Password Field
            if (typeof err.errors.Password === "undefined") {
                $("#StudentPasswordErrorMessage").empty();
            }
            else {
                $("#StudentPasswordErrorMessage").html(err.errors.Password[0]);
            }
        }
    })
})

// Resets the new admin form modal if the user cancels it
$("button#CancelCreateStudent").click(function () {
    var newAdminForm = $('form#NewStudentForm');
    newAdminForm.trigger("reset");
    // Reseting span elements
    $("#StudentFirstNameErrorMessage").empty();
    $("#StudentLastNameErrorMessage").empty();
    $("#StudentPasswordErrorMessage").empty();

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
        url: "/api/Student/Delete/",
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