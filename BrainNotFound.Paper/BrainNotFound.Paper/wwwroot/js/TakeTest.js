﻿// Submits the test
$(".submitTest").click(function () {
    var TestScheduleId = $("input#testScheduleId").val();
    console.log("In the submit test function - test schedule Id = ", TestScheduleId);

    var JsonData = JSON.stringify({ TestScheduleId: TestScheduleId })

    $.ajax({
        url: "/api/Tests/SubmitTest/",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        // Data fetched from the form
        data: JsonData,
        success: function (result) {
            // Close the modal window
            console.log(result.message);
            window.location.href = "/Student/Grades";
            
        },
        error: function (xhr, status, error) {
            console.log(error);
            
        }
    })
})

// This function confirms that when the student submits the test, the pledge and his name match and that all of the questions are answered
$("button#submitTest").click(function () {
    var nameInput = document.getElementById("fullnameInput").value;
    var studentName = $("span#fullname").attr("data-studentName");

    // Error check the student signature
    if (nameInput === "") {
        $("h4#myModalLabel2").text("Warning:");
        $("p#errorMessagePlaceHolder").text("Submitting the test without a pledge will result in a 0. Are you sure you want to proceed?");
        $("#ErrorModal").modal("toggle");
    }
    else if (nameInput.trim() === studentName.trim()) {
        $("#pledgeErrorMessage").addClass("hidden");
        // Now verify that all of the questions have been answered
        var TestScheduleId = $("input#testScheduleId").val();
        $.ajax({
            url: "/api/Tests/ConfirmAllQuestionsAnswered/",
            type: "POST",
            contentType: "application/json",
            // Data fetched from the form
            data: TestScheduleId,
            success: function (result) {
                $("p#successModalPlaceholder").text("You've finished the test. You may now review the results.");
                $("#SuccessModal").modal("toggle");
            },
            error: function (xhr, status, error) {
                $("p#questionModalPlaceholder").text("All questions have not been answered. Are you sure you want to submit the test?");
                $("#QuestionsAnsweredModal").modal("toggle");
            }
        })
    }
    else {
        $("#pledgeErrorMessage").removeClass("hidden");
    }
})

$("a.multipleChoiceOption").click(function () {
    var QuestionId = $(this).attr("data-mcQuestionId");
    var Answer = $(this).text();
    Answer = (Answer.slice(2)).trim();
    var TestScheduleId = $("input#testScheduleId").val();
})

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

// Multiple choice toggle
$(".multipleChoiceOption").click(function () {
    if ($(this).hasClass("btn-default")) {
        $(this).removeClass("btn-default").addClass("btn-primary");
    }
    else {
        $(this).removeClass("btn-primary").addClass("btn-default");
    }
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