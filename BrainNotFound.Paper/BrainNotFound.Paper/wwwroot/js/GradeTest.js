﻿/**********************************************************************/
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

    // Delete the template from the DOM
    $("#displayQuestionsTable").remove();

    // Compile display grade question
    var htmlDisplayGradeQuestion = $("#displayGradeQuestion").text();
    template.displayGradeQuestion = Handlebars.compile(htmlDisplayGradeQuestion);

    // Delete the template from the DOM
    $("#displayGradeQuestion").remove();
}

// Initialize page data using the templates
function init_page() {
    // Get the student id of the selected student
    render_question_table();
}

// Renders the Questions Table based on a student
function render_question_table(questionSelectedId = null) {

    var studentId = $("#studentPicker").val();
    var questions = []

    var selectedFlag = true;
    // Formats the template data for each question
    essayQuestions.forEach(function (essayQuestion) {
        var studentAnswer = essayQuestion.studentAnswers.find(sa => sa.studentId === studentId);
        var graded = studentAnswer.pointsEarned >= 0;
        var isSelected;
        if (questionSelectedId == null) {
            isSelected = selectedFlag;
            selectedFlag = false;
        } else {
            isSelected = essayQuestion.questionId == questionSelectedId;
        }

        questions.push({
            questionId: essayQuestion.questionId,
            questionNumber: essayQuestion.questionNumber,
            content: essayQuestion.content,
            selected: isSelected,
            answered: studentAnswer.answered,
            graded: graded
        });
    });

    // Bilds and places rendered table in the DOM
    var templateData = { essayQuestions: questions }
    var renderedQuestionsTable = template.displayQuestionsTable(templateData)
    $("#questionTableContainer").html(renderedQuestionsTable);

    // Hides the loading div
    $("#QuestionsLoading").addClass("hidden");

    // Render the grade question partial
    render_grade_question();
}

// Render grade question
function render_grade_question() {
    // Gather information
    var studentId = $("#studentPicker").val();
    var questionId = $(".questionRow[data-selected='true']").attr("id");


    // Formats the template data for a question
    var currentQuestion = essayQuestions.find(eq => eq.questionId == questionId);
    var studentAnswer = currentQuestion.studentAnswers.find(sa => sa.studentId === studentId);
    var graded = studentAnswer.pointsEarned >= 0;
    currentQuestion.studentAnswer = studentAnswer;
    currentQuestion.graded = graded;
    currentQuestion.totalQuestions = essayQuestions.length;

    var renderedQuestionsTable = template.displayGradeQuestion(currentQuestion)
    $("#gradeQuestionCotainer").html(renderedQuestionsTable);
    $("#GradeQuestionLoading").addClass("hidden");
}
/**********************************************************************/
/*                           Event Handlers                           */
/**********************************************************************/

// Student select change 
$("#studentPicker").change(function () {
    // Displays the loading div
    $("#QuestionsLoading").removeClass("hidden");

    // Fetch selected student and display table
    render_question_table();
})

// Question list select click
$("#questionTableContainer").on("click", ".questionRow", function () {
    $(".questionRow[data-selected='true']").attr("data-selected", false).removeClass("bg-info");
    $(this).attr("data-selected", true).addClass("bg-info");

    render_grade_question();
})

// Error check points earned
$("#gradeQuestionCotainer").on("change", "input#pointsEarned", function () {
    var max = parseInt($(this).attr("max"));
    var min = parseInt($(this).attr("min"));
    var currentVal = parseInt($(this).val());

    // Check if value exceeds max or is below the minimum
    if (currentVal >= max) {
        $(this).val(max);
    } else if (currentVal <= min) {
        $(this).val(min);
    }
})

// Submits grade to server
$("#gradeQuestionCotainer").on("click", "#submitGrade", function () {
    var answerId = $(this).attr("data-answerId");
    var comment = $.trim($("#InstructorComment").val());
    var pointsEarned = $.trim($("#pointsEarned").val());
    var questionId = $(this).attr("data-questionId");
    var studentId = $("#studentPicker").val();
    var hasError = false;

    console.log("answerId: " + answerId);
    console.log("comment: " + comment);
    console.log("pointsEarned: " + pointsEarned);
    console.log("questionId: " + questionId);

    // Error check the earned points
    if (pointsEarned == "") {
        $("#pointsEarnedErrorMessage").removeClass("hidden");
        hasError = true;
    } else {
        $("#pointsEarnedErrorMessage").addClass("hidden");
        hasError = false;
    }

    if (!hasError) {
        // Submits data to server and updates the view
        var JsonData = JSON.stringify({ answerId: answerId, comment: comment, pointsEarned: pointsEarned })
        $.ajax({
            url: "/api/GradeTest/GradeQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    // Updates the local versin of the data
                    essayQuestions
                        .find(eq => eq.questionId == questionId)
                        .studentAnswers.find(sa => sa.studentId === studentId)
                        .comment = comment;
                    essayQuestions
                        .find(eq => eq.questionId == questionId)
                        .studentAnswers.find(sa => sa.studentId === studentId)
                        .comment = comment;

                    essayQuestions
                        .find(eq => eq.questionId == questionId)
                        .studentAnswers.find(sa => sa.studentId === studentId)
                        .pointsEarned = pointsEarned;

                    console.log(essayQuestions);
                    render_question_table(questionId);
                    render_grade_question();


                }
                else {
                    console.log(result);
                }
                // TODO: Display if Sections were already assigned
            }
        })
    }

})