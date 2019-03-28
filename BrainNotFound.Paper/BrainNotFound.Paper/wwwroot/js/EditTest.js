//Constants

// Variables for this Page
var TotalPoints = 180;
var NumberOfQuestions = 50;
var NumberOfSections = 4;

// Tracks the assigment
var SectionsAssigned = [];
var IndivisualsAssigned = [];

// DataTable
var studentsDataTable;

// Currently selected Section
var studentsInSelectedSection;

// Initialize Page
init_daterangepicker_TestSchedule();
init_students_datatable();
Update_TestStatistics();
Update_TestAssignmentTable();

// Starts the Datatables for the edit test
function init_students_datatable() {
    studentsDataTable = $('#StudentsInSectionTable').DataTable({
        "columnDefs": [
            {
                targets: 0,
                searchable: false,
                orderable: false,
                className: 'select-checkbox',
                render: function (data, type, full, meta) {
                    return '<input class="selectedStudent" type="checkbox" value="' + $('<div/>').text(data).html() + '">';
                }

            }
        ],
        'order': [[1, 'asc']]
    });
}

// Reset New Schedule Time Picker
function init_daterangepicker_TestSchedule() {

    $('#testScheduleDateTime').daterangepicker({
        timePicker: true,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(11, 'day'),
        locale: {
            format: 'MM/DD/YYYY hh:mm A'
        }
    });

}
// Updates the statistics in the information section of the test
function Update_TestStatistics() {
    $("#TotalPointsStats").text(TotalPoints);
    $("#TotalQuestionsStats").text(NumberOfQuestions);
    $("#TotalSectionsStats").text(NumberOfSections);
}

// Updates the test assignment table
function Update_TestAssignmentTable() {
    var testId = $("#TestId").val();
    $.ajax({
        url: "/api/CreateTest/GetTestSchedules/" + testId,
        type: "GET",
        success: function (result) {
            if (result.success) {

                var rendered = "";
                var Row = $("#ScheduleTableRowTemplate").html();
                var template = Handlebars.compile(Row);
                
                result.schedules.forEach(function (schedule) {
                    rendered += template(schedule);
                })
                
                $("#TestAssignmentTable > tbody").html(rendered);
                $("table#TestAssignmentTable").removeClass("hidden");

                
            }
            else {
                console.log(result.errors)
            }
        },
    })
}

// Updates the tables showing witch sections and students are assigned to the test
function UpdateAssignmentTables() {

    // Updates the selected Sections
    if (SectionsAssigned.length == 0) {
        $("#SectionsAssignedTest").removeClass("show").addClass("hidden");
        $("#NoSectionsAssignedTest").removeClass("hidden").addClass("show");
    } else {
        var SectionsAssignedToTableRowTemplate = $("#SectionAssigmentTableRowTemplate").html();
        var template = Handlebars.compile(SectionsAssignedToTableRowTemplate);
        var rendered = "";
        for (var index = 0; index * 2 < SectionsAssigned.length; index++) {

            var templateInfo = { RowNumber: (index + 1), SectionNumbers: SectionsAssigned[(index * 2) + 1], SectionId: SectionsAssigned[(index * 2)] }
            rendered += template(templateInfo);
        }
        $("#SectionsAssignedTest > tbody").html(rendered);

        $("#NoSectionsAssignedTest").removeClass("show").addClass("hidden");
        $("#SectionsAssignedTest").removeClass("hidden");
    }

    // Updates the selected Students
    if (IndivisualsAssigned.length == 0) {
        $("#StudentsAssignedTest").removeClass("show").addClass("hidden");
        $("#NoStudentsAssignedTest").removeClass("hidden").addClass("show");
    } else {
        var StudentAssignmnetRowTemplate = $("#StudentAssignmnetRowTemplate").html();
        var template = Handlebars.compile(StudentAssignmnetRowTemplate);
        var rendered = "";
        for (var index = 0; index * 4 < IndivisualsAssigned.length; index++) {

            var templateInfo = { SectionNumber: IndivisualsAssigned[(index * 4)], StudentId: IndivisualsAssigned[((index * 4) + 1)], FirstName: IndivisualsAssigned[((index * 4) + 2)], LastName: IndivisualsAssigned[((index * 4) + 3)] }
            rendered += template(templateInfo);
        }
        $("#StudentsAssignedTest > tbody").html(rendered);

        $("#NoStudentsAssignedTest").removeClass("show").addClass("hidden");
        $("#StudentsAssignedTest").removeClass("hidden");
    }
}

// Resets the new schedule container
function resetNewSchedule() {
    $("div#NewScheduleContainer").removeClass("show").addClass("hidden");
    $("a#NewSchedule").removeClass("hidden");

    // Resets the dropdown section selection
    $('#SelectSection option').prop('selected', function () {
        return this.defaultSelected;
    });

    // Resets the assigned students and sections
    SectionsAssigned = [];
    IndivisualsAssigned = [];
    UpdateAssignmentTables();

    // Resets the time Picker
    init_daterangepicker_TestSchedule();

    // Reset Unlimited CheckBox
    $('#UnlimitedTimeCheckBox').prop('checked', false);
    $("input#TimeLimit").removeAttr("disabled");

    // Reset Time Limit
    $("#TimeLimit").val("50");

    //hides the students table
    $("div#StudentsInSectionTableContainer").removeClass("show").addClass("hidden");
    $("div#SectionNotSelectedContainer").removeClass("hidden").addClass("show");
}

// -- Event Handlers

// Dropdowns
// Fetched the list of students from the server
$('select#SelectSection').change(function () {
    var sectionId = $("#SelectSection").val();

    $.ajax({
        url: "/api/CreateTest/GetStudentsInSection/" + sectionId,
        type: "GET",
        success: function (students) {
            //Fetches the template and, iterates through students, and renders table rows

            studentsInSelectedSection = students;
            $("#StudentsInSectionTable").DataTable().clear();

            students.forEach(function (student) {
                $('#StudentsInSectionTable').dataTable().fnAddData([
                    student.Id,
                    student.FirstName,
                    student.LastName,
                ]);
            })

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
    resetNewSchedule();
})

// Assign a section to the schedule
$("button#AssignEntireSection").click(function () {
    var sectionId = $("#SelectSection").val();
    var sectionNumber = $("#SelectSection option:selected").text();

    // Checks if section has already been assigned.
    if (sectionId != null && !SectionsAssigned.includes(sectionId, sectionNumber)) {
        SectionsAssigned.push(sectionId, sectionNumber);
        UpdateAssignmentTables();
    }
})


$("button#AssignSelectedStudents").click(function () {
    var sectionNumber = $("#SelectSection option:selected").text();
    studentsDataTable.$('.selectedStudent').each(function () {
        if (this.checked) {
            var studentId = $(this).val();

            if (!IndivisualsAssigned.includes(studentId)) {
                var index = studentsInSelectedSection.findIndex(s => s.Id == studentId);
                IndivisualsAssigned.push(
                    sectionNumber.replace("Section ", ""),
                    studentsInSelectedSection[index].Id,
                    studentsInSelectedSection[index].FirstName,
                    studentsInSelectedSection[index].LastName
                )
            }
        }
    })
    UpdateAssignmentTables();
})

// Send the information about a new schedule to the server and updates the view
$("button#SaveNewTestSchedule").click(function () {
    var TestId = $("#TestId").val();
    var StartEndDateTime = $("#testScheduleDateTime").val();
    var IsTimeUnlimited = $("#UnlimitedTimeCheckBox").is(':checked');
    var TimeLimit = $("#TimeLimit").val();
    var Students = [];
    var Sections = [];

    // Get the id for each selected section
    for (var index = 0; index * 2 < SectionsAssigned.length; index++) {
        Sections.push(SectionsAssigned[(index * 2)]);
    }

    // Get the id for each selected student
    for (var index = 0; index * 4 < IndivisualsAssigned.length; index++) {
        Students.push(IndivisualsAssigned[((index * 4) + 1)]);
    }

    var JsonData = JSON.stringify({
        "TestId": TestId,
        "StartEndDateTime": StartEndDateTime,
        "IsTimeUnlimited": IsTimeUnlimited,
        "TimeLimit": TimeLimit,
        "Students": Students,
        "Sections": Sections
    });

    $.ajax({
        url: "/api/CreateTest/NewTestSchedule",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {
                Update_TestAssignmentTable();
                resetNewSchedule();
            }
            // TODO: Display if Sections were already assigned
        },
    })
})

// Adds a Generic Section to a Test
$("#AddTestSectionBtn").click(function () {
    var rendered = "";
    var GenericTestSection = $("#GenericTestSectionTemplate").html();
    var template = Handlebars.compile(GenericTestSection);

    rendered += template();

    $("#TestSections").append(rendered);
})



// Removes a section from the assigment table
$("table#SectionsAssignedTest").on("click", ".deleteEntireSectionAssignment", function () {
    var SectionId = $(this).val();

    SectionsAssigned.splice(SectionsAssigned.indexOf(SectionId), 2);
    UpdateAssignmentTables();

})

// Remove a student from the assignment table
$("table#StudentsAssignedTest").on("click", ".deleteStudentAssigment", function () {
    var StudentId = $(this).val();
    IndivisualsAssigned.splice(IndivisualsAssigned.indexOf(StudentId) - 1, 4);

    UpdateAssignmentTables();

})

// Cancels the new section 
$("#TestSections").on("click", "button#cancelQuestionType", function () {
    $(this).parents(".SectionContainer").remove();
})

// Sets the test section to the chosen question type
$("#TestSections").on("click", "button#setQuestionType", function () {
    // Get the Section type chooser container
    var SectionTypeContainer = $(this).parents(".SectionContainer");

    // Get the data
    var QuestionType = $(this).prev().find("#questionType").val();
    var TestId = $("#TestId").val();
    var JsonData = JSON.stringify({ "TestId": TestId, "QuestionType": QuestionType });

    $.ajax({
        url: "/api/CreateTest/CreateTestSection",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {

                var rendered = "";
                var TestSection = $("#TestSectionTemplate").html();
                var template = Handlebars.compile(TestSection);

                rendered += template({ Instructions: result.instructions, SectionId : result.sectionId, SectionType : result.sectionType, Header : result.header });
                
                $(SectionTypeContainer).before(rendered);
                $(SectionTypeContainer).remove();

            }
            else {
                console.log(result.errors)
            }
        },
    })
})

// Displays section Editable instruction Box
$("#TestSections").on("click", "a.editInstructions", function () {
    var sectionId = $(this).val();
    console.log("The Section id is: " + sectionId);

    var currentInstructions = $("span.instructions-" + sectionId).text();
    console.log("Current Instructions: " + currentInstructions);
    //$(".editInstructionsContainer[value='" + sectionId +"']").removeClass("hidden");
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
