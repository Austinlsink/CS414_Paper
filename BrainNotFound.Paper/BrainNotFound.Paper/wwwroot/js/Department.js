// Edit Department code and name
function EditDepartmentCodeAndName(Id) {
    $.ajax({
        url: "/Admin/Department/Edit/" + Id,
        success: function (result) {
            $("#departmentPlaceholder").html(result);
        }
    });
}

$("button#CreateDepartment").click(function () {
    var form = $("form#NewDepartment");
    var d = new Object();
    d.DepartmentName = "Bible";
    d.DepartmentCode = "BI";

    alert(JSON.stringify(d));

    $.ajax({
        url: "/api/Department/New/",
        type: "POST",
        data: JSON.stringify(d),
        success: function (result) {
            alert(result);
        }
    })
})