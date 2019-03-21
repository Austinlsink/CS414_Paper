// Add a new department
function AddNewDepartment() {

    $.ajax({
        url: "/Admin/Departments/New/",
        success: function (result) {
            console.log("Bima Got Here");
            $("#newDepartmentPlaceholder").html(result);
        }
    });
}

function ValidateNewDepartment() {
    $.ajax({
        url: "/Admin/Departments/New/",
        type: "POST",
        success: function (result) {
            console.log("Bima Got Here");
            $("#newDepartmentPlaceholder").html(result);
        }
    });
}

// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
    $.ajax({
        url: "/Admin/Department/Edit/" + Id,
        success: function (result) {
            $("#departmentPlaceholder").html(result);
        }
    });
}

