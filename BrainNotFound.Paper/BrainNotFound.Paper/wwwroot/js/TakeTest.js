// This function reponds to the radios on change event - we're grabbing data!!!!!
$("input[type='radio']").on("change", function () {
    var QuestionId = $(this).attr("data-questionId");
    var Answer = $(this).val();
    var TestScheduleId = $("input#testScheduleId").val();

    var JsonData = JSON.stringify({ QuestionId: QuestionId, Answer: Answer, TestScheduleId: TestScheduleId })
    $.ajax({
        url: "/api/Tests/SaveTrueFalseAnswer/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JsonData,
        success: function (result) {
            // Close the modal window
            console.log(result.message);
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    })
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