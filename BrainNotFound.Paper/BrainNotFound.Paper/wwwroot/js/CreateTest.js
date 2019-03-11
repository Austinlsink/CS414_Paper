/* Event Handlers */


$(document).ready(function () {
    $("#sectionAdd").click(function () {
        $("#genericSection").clone().appendTo("#addSectionButton");
        //alert("Testing Alerts");
    });
});



/* DOM Manipulation */

function DeleteSection(SectionId) {
    $(SectionId).Remove();
}

/* Data Access */