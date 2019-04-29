// Adds a Generic Section to a Test
$("#AddTestSectionBtn").click(function () {
    var rendered = "";
    var GenericTestSection = $("#GenericTestSectionTemplate").html();
    var template = Handlebars.compile(GenericTestSection);

    rendered += template();

    $("#TestSections").append(rendered);
})

// Display True/false Test Section
//$('#TestSections').on("click", "#setQuestionType", function () {
//    var questionType = $("#questionType option:selected").text();;

//    if (questionType = "True/False") {
//        alert(questionType);
//    }
//    else
//        console.log(questionType);
    
//})

$("select")
    .change(function () {
        var questionType = "";
        $("select option:selected").each(function () {
            questionType += $(this).text() + " ";
        });
        console.log(questionType);
    })
    .change();

// Work in progress
//$("#AddTestSection").click(function () {
//    $("#SectionsAssignedTest").removeClass("show").addClass("hidden");
//})