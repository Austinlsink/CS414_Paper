/**********************************************************************/
/*                           Initialize Page                          */
/**********************************************************************/
$(document).ready(function () {
    compile_template();
    initialize_page();
})

/**********************************************************************/
/*                             Variables                              */
/**********************************************************************/

var template = {}; // Compiled templaes
var essayQuestions = {}; // Essay questions

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
function initialize_page() {
    var templateData = {
        essayQuestions: [
            {
                questionId: 101,
                questionNumber: 1,
                content: "Talk about the beloved country that Bima is from",
                expectedAnswer: "Something that says that Brazil is cool ans awesome!",
                pointValue: 23,
                selected: true,
                studentAnswers: [{
                    studentId: "AbmaelSilva",
                    studentFullName : "Abmael Fernandes da Silva Jr",
                    answer: "Brazil is simply cool!",
                    comment: "You forgot the Awesome Part",
                    pointsEarned: 20
                }]
            }]
    }
    //$("#questionTableContainer").html(template.displayQuestionsTable(templateData));

    //$("#QuestionsLoading").addClass("hidden");

    console.log(essayQuestions);
}
