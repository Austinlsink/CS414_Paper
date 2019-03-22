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

// Resets Modal if canceled
$("button#CancelCreateDepartment").click(function () {
    var newDepartmentForm = $('form#NewDepartment');
    newDepartmentForm.trigger("reset");
    $("#departmentCodeErrorMessage").empty();
    $("#departmentNameErrorMessage").empty();
});

// Submits the form information to the server
$("button#CreateDepartment").click(function () {
    var newDepartmentForm = $('form#NewDepartment');

    // Gets the values of the form, and creates an object to be sent to the server
    var department = {};
    $.each(newDepartmentForm.serializeArray(), function (i, field) {
        department[field.name] = field.value;
    });
    
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
            console.log(xhr.responseText);
            // Places validation on the Department Code Field
            if (!(typeof err.errors.DepartmentCode === 'undefined')) {
                $("#departmentCodeErrorMessage").html(err.errors.DepartmentCode[0]);
            }
            else {
                $("#departmentCodeErrorMessage").empty();
            }

            // Places validation on the Department Name Field
            if (err.errors.DepartmentName.length > 0) {
                $("#departmentNameErrorMessage").html(err.errors.DepartmentName[0]);
            }
            else {
               $("#departmentNameErrorMessage").empty();
            }

        }
    })
})