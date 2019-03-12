/* Event Handlers */

//Add a generic section to the test
var RootURL = "http://" + document.location.host + "/";

$("#EditNameAndCourseBT").click(function () {

    $.ajax({
        url: RootURL + "Instructor/Tests/Partials/EditNameAndCourse/132",
        success: function (result) {
            $("#x_c-infoSection").prepend(result);

        }
    });
});

//Change the generic section to a specified section
//$("#setQuestionType").click(function () {
//    if ("the selected option" == "True/False") {

//    }
//    else if ("the selected option" == "Multple Choice") {

//    }
//    else if ("the selected option" == "Matching") {

//    }
//    else if ("the selected option" == "Fill in the Blank") {

//    }
//    else if ("the selected option" == "Essay") {

//    }
    
//    })




///* DOM Manipulation */

////Delete a specific section from the test
//function DeleteSection(SectionId) {
//    $(SectionId).Remove();
//}


///* Data Access */

//function GetSection(QuestionType) {
    
//}


///* Template Functions */

//function toggle(source, elementName) {
//    checkboxes = document.getElementsByName(elementName);
//    for (var i = 0, n = checkboxes.length; i < n; i++) {
//        checkboxes[i].checked = source.checked;
//    }
//}
