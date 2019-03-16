// -- Button Handlers

// Edit test name and course
function EditTestNameAndCourse(TestId)
{
    if ($("#EditNameAndCourse").length == 0) {
        $.ajax({
            url: "/Instructor/Tests/Partials/EditNameAndCourse/" + TestId,
            success: function (result) {
                $("#x_c-infoSection").prepend(result);
            }
        });
    }
}

function NewTestSchedule(TestId) {
    if ($("#NewSchedule").length == 0) {
        $.ajax({
            url: "/Instructor/Tests/Partials/NewSchedule/" + TestId,
            success: function (result) {
                $("#EditSchedulePlaceHolde").append(result);
            }
        });
    }
}

//Disables Time Limit Textbox
function UnlimitedTimeCheckBox() {

    if ($("#UnlimitedTimeCheckBox").is(':checked')) {
        console.log("Checked");
        $("#TimeLimitTextBox").attr("disabled", "disabled");
    }
    else {
        $("#TimeLimitTextBox").removeProp("disabled");
        console.log("UnChecked");
    }
}


// -- General Functions

// Removes Element from DOM if pressed Cancel
function Calcel(ElementId) {

    $(ElementId).remove();
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
