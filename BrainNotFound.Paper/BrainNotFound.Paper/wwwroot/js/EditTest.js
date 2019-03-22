// Variables for this Page

var TotalPoints = 180;
var NumberOfQuestions = 50;
var NumberOfSections = 4;

// Initialize Page
init_daterangepicker_TestSchedule();
Update_TestStatistics();

// Updates the statistics in the information section of the test
function Update_TestStatistics() {
    $("#TotalPointsStats").text(TotalPoints);
    $("#TotalQuestionsStats").text(NumberOfQuestions);
    $("#TotalSectionsStats").text(NumberOfSections);
}

// -- Event Handlers

// Dropdowns
// Fetched the list of students from the server
$('select#SelectSection').change(function () {
    var data = { "FirstName": "Becky", "LastName": "Mirafuentes", "Id": "bd7afdf5-0a7a-4fa4-8300-e86b7830f294" };
    var TableRowTemplate = $("#StudentsInSectionTableRowTemplate").html();
    Mustache.parse(TableRowTemplate);

    var rendered = Mustache.render(TableRowTemplate, data);

    $("#StudentsInSectionTable > tbody").html(rendered);
    //var sectionId = $("#SelectSection").val();
    //$.ajax({
    //    url: "/api/CreateTest/GetStudentsInSection/" + sectionId,
    //    success: function (result) {
    //        $("#SelectSection > tbody").html(result);
    //    }
    //});
});

// Checkboxes
//Disables Time Limit Textbox
$("#UnlimitedTimeCheckBox").change(function () {

    if ($("#UnlimitedTimeCheckBox").is(':checked')) {
        $("input#TimeLimit").attr("disabled", "disabled");
    }
    else {
        $("input#TimeLimit").removeAttr("disabled");
    }
})

// Buttons
// Shows the edit test name section
$("a#EditTestName").click(function () {
    $("div#EditNameContainer").removeClass("hidden").addClass("show");
})

// Hides the edit test name section
$("a#cancelEditTestName").click(function () {
    $("div#EditNameContainer").removeClass("show").addClass("hidden");
})

// Shows the new sechedule section
$("a#NewSchedule").click(function () {
    $("div#NewScheduleContainer").removeClass("hidden").addClass("show");
    $("a#NewSchedule").addClass("hidden");
})

// Hides the new sechedule section
$("a#CancelNewSchedule").click(function () {
    $("div#NewScheduleContainer").removeClass("show").addClass("hidden");
    $("a#NewSchedule").removeClass("hidden");
})

// Handles all forms submition buttons
$(function () {
    $('.post-using-ajax').each(function () {
        var $frm = $(this);
        $frm.submit(function (e) {
            e.preventDefault();

            $.ajax({
                type: $frm.attr('method'),
                url: $frm.attr('action'),
                data: $frm.serialize(),
                success: function (msg) {
                    alert("Success");
                }
            });
        });
    });
});

// Re-assigns back browser button
window.onpopstate = function () {
    window.location.href = "/Instructor/Tests";
}; history.pushState({}, '');
