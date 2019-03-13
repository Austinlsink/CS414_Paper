///* Event Handlers */

// Button Handlers
// Handles all forms in section as 
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

// Reassigns back browser button
window.onpopstate = function () {
    window.location.href = "/Instructor/Tests";
}; history.pushState({}, '');

////Change the generic section to a specified section
////$("#setQuestionType").click(function () {
////    if ("the selected option" == "True/False") {

////    }
////    else if ("the selected option" == "Multple Choice") {

////    }
////    else if ("the selected option" == "Matching") {

////    }
////    else if ("the selected option" == "Fill in the Blank") {

////    }
////    else if ("the selected option" == "Essay") {

////    }
    
////    })




/////* DOM Manipulation */

//////Delete a specific section from the test
////function DeleteSection(SectionId) {
////    $(SectionId).Remove();
////}


/////* Data Access */

////function GetSection(QuestionType) {
    
////}


/////* Template Functions */

////function toggle(source, elementName) {
////    checkboxes = document.getElementsByName(elementName);
////    for (var i = 0, n = checkboxes.length; i < n; i++) {
////        checkboxes[i].checked = source.checked;
////    }
////}
