// -- Button Handlers
var StudentsSelected = [];

var StudentsAssigned = [];
var SectionsAssigned = [];

// Edit test name and course
function EditTestNameAndCourse(TestId) {
    if ($("#EditNameAndCourse").length == 0) {
        $.ajax({
            url: "/Instructor/Tests/Partials/EditNameAndCourse/" + TestId,
            success: function (result) {
                $("#x_c-infoSection").prepend(result);
            }
        });
    }
}

// Places the new Schechedule well in the page
function NewTestSchedule(TestId) {
    if ($("#NewSchedule").length == 0) {
        $.ajax({
            url: "/Instructor/Tests/Partials/NewSchedule/" + TestId,
            success: function (result) {
                $("#EditSchedulePlaceHolde").html(result);
            }
        });
    }
}

//
function UpdateAssigmentTables() {
    $.ajax({
        url: "/Instructor/Tests/Partials/ViewSectionAndStudentsAssigned/",
        type: "POST",
        data: { "SectionIds": SectionsAssigned, "StudentIds": StudentsAssigned },
        success: function (result) {
            //alert(result);
            $("#AssignedTablePlaceHolder").html(result);
        }
    });
}

//Disables Time Limit Textbox
function UnlimitedTimeCheckBox() {

    if ($("#UnlimitedTimeCheckBox").is(':checked')) {
        $("#TimeLimitTextBox").attr("disabled", "disabled");
    }
    else {
        $("#TimeLimitTextBox").removeAttr("disabled");
    }
}

// Adds or removes selected students from list
function StudentCheckBox(StudentId) {

    if ($("#CB-" + StudentId).is(':checked')) {

        StudentsSelected.push(StudentId);
    } else {
        for (var i = 0; i < StudentsSelected.length; i++) {
            if (StudentsSelected[i] === StudentId) {
                StudentsSelected.splice(i, 1);
            }
        }
    }
    console.log("-------------------------------------------------");
    for (var i = 0; i < StudentsSelected.length; i++) {
        console.log(StudentsSelected[i]);
    }
}

// Removes a specific Section from the list of assigments
function removeSectionFromAssigmentList(sectionId) {

    for (var i = 0; i < SectionsAssigned.length; i++) {
        if (SectionsAssigned[i] == sectionId) {
            SectionsAssigned.splice(i, 1);
        }
    }
    UpdateAssigmentTables();
}

// Assignes the selected section to the list of assigments
function AssignEntireSection() {
    var sectionId = $("#SelectSection").val();

    if (!SectionsAssigned.includes(sectionId)) {
        SectionsAssigned.push(sectionId);
        UpdateAssigmentTables();
    }
}

function AssignToSelectedStudents() {
    StudentsAssigned = StudentsSelected;
    UpdateAssigmentTables();
}

// -- Event Handlers
$('#EditSchedulePlaceHolde').on('change', '#SelectSection', function () {
    $("table#StudentsInSection").remove();
    var sectionId = $("#SelectSection").val();
    $.ajax({
        url: "/Instructor/Tests/Partials/StudentInSectionTable/" + sectionId,
        success: function (result) {
            $("#StudentTablePlaceHolder").html(result);
        }
    });
});

// -- General Functions

// Removes Element from DOM if pressed Cancel
function Cancel(ElementId, caller) {
    $(ElementId).remove();
    if (caller.id == "NewScheduleCancelButton") {
        StudentsSelected = [];

        StudentsAssigned = [];
        SectionsAssigned = [];
    }
}

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
