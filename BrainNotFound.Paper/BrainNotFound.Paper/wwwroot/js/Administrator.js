

// Displays the admin form to edit a specific admin
$("button#EditAdmin").click(function () {
    var username = $(this).val();

    $.ajax({
        url: "/api/admin/edit/" + username,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(username),
        success: function (result) {
            document.getElementById("SalutationInput").value = result.salutation;
            document.getElementById("FirstNameInput").value = result.firstName;
            document.getElementById("LastNameInput").value = result.lastName;
            document.getElementById("EmailInput").value = result.email;
            document.getElementById("PhoneInput").value = result.phone;
            document.getElementById("AddressInput").value = result.address;
            document.getElementById("CityInput").value = result.city;
            document.getElementById("StateInput").value = result.state;
            document.getElementById("ZipInput").value = result.zip;
            document.getElementById("DOBInput").value = result.dob;

            document.getElementById("CreateAdmin").value = result.firstName;

            $("#NewAdminModal").modal("show");
        }
    })
});


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
        url: "/api/Admin/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(admin),
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