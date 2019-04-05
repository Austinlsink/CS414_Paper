// This function reponds to the radios on change event - we're grabbing data!!!!!
$("input[type='radio']").on("change", function () {
    var QuestionId = $(this).attr("data-questionId");
    var Answer = $(this).val();
    var StudentId = $("input#studentId").val();
    var TestScheduleId = $("input#testScheduleId").val();

    console.log(questionId, answer, studentId, testScheduleId);
})



// True false toggle
$("label[data-questionType='trueFalse']").click(function () {
    var newClass = $(this).attr("data-toogled-class");
    $(this).removeClass("btn-primary").removeClass("btn-danger").addClass(newClass);
    $(this).siblings().removeClass("btn-primary").removeClass("btn-danger").addClass("btn-default");
})

// stripes every other row
$(document).ready(function () {
    var hasBackground = false;
    $(".questionRow").each(function () {
        if (hasBackground) {
            $(this).addClass("bg-light");
            hasBackground = false;
        }
        else {
            hasBackground = true;
        }
    });
})