$(".reviewStudentTest").click(function () {
    var StudentId = $(this).attr("data-studentId");
    var DepartmentCode = $(this).attr("data-DepartmentCode");
    var CourseCode = $(this).attr("data-CourseCode");
    var URLSafeName = $(this).attr("data-URLSafeName");

    $.ajax({
        url: getPath() + "/Instructor/Tests/ReviewStudentTest/" + DepartmentCode  + "/" + CourseCode + "/" + URLSafeName + "/" + StudentId,
        type: "GET",
        contentType: "application/json",
        // Data fetched from the form
        data: DepartmentCode, CourseCode, URLSafeName, StudentId,
        success: function (result) {
            $("#placeholder").html(result);
        },
        error: function (xhr, status, error) {
        }
    })
})