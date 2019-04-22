/**********************************************************************/
/*                           Initialize Page                          */
/**********************************************************************/
$(document).ready(function () {
    compile_template();
    init_page();
})

/**********************************************************************/
/*                             Variables                              */
/**********************************************************************/

var template = {}; // Compiled templaes
var essayQuestions = []; // Essay questions

/**********************************************************************/
/*                             Functions                              */
/**********************************************************************/
// Compile templates
function compile_template() {

    // Compile display questions table
    var htmlDisplayQuestionsTable = $("#displayQuestionsTable").text();
    template.displayQuestionsTable = Handlebars.compile(htmlDisplayQuestionsTable);
    $("#displayQuestionsTable").remove();

}

// Initialize page data using the templates
function init_page() {
    console.log(essayQuestions);

    var selectedStudentId = $("#studentPicker").val();
    console.log(selectedStudentId);
}

// Renders the Questions Table based on the selected student
function RenderQuestionsTable(studentId) {
    var templateData = { essayQuestions: essayQuestions }
    var renderedQuestionsTable = template.displayQuestionsTable(templateData)
    $("#questionTableContainer").html(renderedQuestionsTable);

    $("#QuestionsLoading").addClass("hidden");
}


