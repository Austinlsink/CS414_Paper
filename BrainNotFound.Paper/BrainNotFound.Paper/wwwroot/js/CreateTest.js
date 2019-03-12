/* Event Handlers */

//Add a generic section to the test
var RootURL = "http://" + document.location.host;

$(document).ready(function () {
    $("#sectionAdd").click(function () {
        $.ajax({
            url: RootURL + "api/GetSection", success: function (result) {
                $("#contentBeforeButton").append(result);
            }
        });
        
        //$("#contentBeforeButton").Append(GetSection("Generic"));
        //alert("Testing Alerts");
    });
});

//Change the generic section to a specified section
$("#setQuestionType").click(function () {
    if ("the selected option" == "True/False") {

    }
    else if ("the selected option" == "Multple Choice") {

    }
    else if ("the selected option" == "Matching") {

    }
    else if ("the selected option" == "Fill in the Blank") {

    }
    else if ("the selected option" == "Essay") {

    }
    
}); 



/* DOM Manipulation */

//Delete a specific section from the test
function DeleteSection(SectionId) {
    $(SectionId).Remove();
}


/* Data Access */

function GetSection(QuestionType) {
    $.ajax({
        url: RootURL + "api/GetSection",
        type: "GET",
        data: { QuestionType: + "\"" + QuestionType + "\"" },
        contentType: "application/json",
        success: function (data) {
            return data;
        }
    });
}


/* Template Functions */

function HtmlForNewSection(SectionId) {


}
