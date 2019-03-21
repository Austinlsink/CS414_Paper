// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
    $.ajax({
        url: "/Admin/Department/Edit/" + Id,
        success: function (result) {
            $("#departmentPlaceholder").html(result);
        }
    });
}

// Add a new department
function AddNewDepartment() {
    $.ajax({
        url: "/Admin/Departments/New",
        succes: function (result) {
            $("newDepartmentPlaceholder").html(result);
        }
    });
}