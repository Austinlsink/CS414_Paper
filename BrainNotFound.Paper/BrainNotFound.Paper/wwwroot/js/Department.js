// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
        $.ajax({
            url: "/Admin/Department/Edit/" + Id,
            success: function (result) {
                $("#departmentPlaceholder").html(result);

            }
        });
}