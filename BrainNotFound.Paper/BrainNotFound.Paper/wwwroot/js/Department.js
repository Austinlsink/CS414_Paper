// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
    $.ajax({
        url: "/Admin/Department/Edit/" + Id,
        success: function (result) {
            $("#departmentPlaceholder").html(result);
        }
    });
}

// Event Handlers
$("button#CancelCreateDepartment").click(function () {
    var newDepartmentForm = $('form#NewDepartment');
    newDepartmentForm.trigger("reset");
});

// Submits the form information to the server
$("button#CreateDepartment").click(function () {
    var newDepartmentForm = $('form#NewDepartment');
    // Gets the values of the form, and creates an object to be sent to the server
    var department = {};
    $.each(newDepartmentForm.serializeArray(), function (i, field) {
        department[field.name] = field.value;
    });



    $("#DepartmentNameErrorMessage").text("Your text here");
    // Creates, submits, and responds to Ajax Call
$.ajax({
    url: "/api/Department/New/",
    type: "POST",
    contentType: "application/json",
    // Data fetched from the form
    data: JSON.stringify(department),
    success: function (result) {
        // Close the modal window
        location.reload();
    },
    error: function (xhr, status, error) {
        var err = JSON.parse(xhr.responseText);
        console.log(err.errors.DepartmentCode[0]);
        // Places validation on the Department Code Field
        if (err.errors.DepartmentCode.lenght > 0) {
            $("#DepartmentNameErrorMessage").html(err.errors.DepartmentCode[0]);
        }
        else {
            $("#DepartmentCodeErrorMessage").empty();
        }
        // Places validation on the Department Name Field
        if (err.errors.DepartmentName.lenght > 0) {
            $("#DepartmentNameErrorMessage").html(err.errors.DepartmentName[0]);
        }
        else {
            $("#DepartmentNameErrorMessage").empty();
        }


    }
})
})