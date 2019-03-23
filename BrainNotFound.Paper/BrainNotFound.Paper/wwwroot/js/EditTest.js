// Variables for this Page

var TotalPoints = 180;
var NumberOfQuestions = 50;
var NumberOfSections = 4;

// Tracks the assigment
var SectionsAssigned = [];
var IndivisualsAssigned = [];

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
    var sectionId = $("#SelectSection").val();

    $.ajax({
        url: "/api/CreateTest/GetStudentsInSection/" + sectionId,
        success: function (result) {

            // Fetches the template and, iterates through students, and renders table rows
            var rendered = "";
            var StudentTableRowTemplate = $("#StudentsInSectionTableRowTemplate").html();
            var template = Handlebars.compile(StudentTableRowTemplate);

            result.forEach(function (student) {
                rendered += template(student);
            })

            $("#StudentsInSectionTable > tbody").html(rendered);

            // Hides no section select message
            $("div#SectionNotSelectedContainer").removeClass("show").addClass("hidden");
            $("div#StudentsInSectionTableContainer").removeClass("hidden").addClass("show");
        }
    });
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

// Hides and Resets the new sechedule section
$("a#CancelNewSchedule").click(function () {
    $("div#NewScheduleContainer").removeClass("show").addClass("hidden");
    $("a#NewSchedule").removeClass("hidden");

    // Resets the dropdown section selection
    $('#SelectSection option').prop('selected', function () {
            return this.defaultSelected;
    });

    //hides the students table
    $("div#StudentsInSectionTableContainer").removeClass("show").addClass("hidden");
    $("div#SectionNotSelectedContainer").removeClass("hidden").addClass("show");

})

// Assign a section to the schedule
$("button#AssignEntireSection").click(function () {
    var sectionId = $("SelectSection").val();
    if (SectionsAssigned.indexOf(sectionId)) {
        SectionsAssigned.push(sectionId);
    }

    console.log(SectionsAssigned[0]);
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
