// Submits the test
$("button#submit").click(function () {
    var newInstructorForm = $("form#NewInstructorForm");

    // Gets the values of the form, and creates an object to be sent to the server
    var instructor = {};
    $.each(newInstructorForm.serializeArray(), function (i, field) {
        instructor[field.name] = field.value;
    });

    // Creates, submits, and responds to Ajax Call
    $.ajax({
        url: "/api/Instructor/New/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JSON.stringify(instructor),
        success: function (result) {
            // Close the modal window
            console.log(result.message);
            location.reload();
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            var err = JSON.parse(xhr.responseText);
        }
    })
})




// This function confirms that when the student submits the test, the pledge and his name match and that all of the questions are answered
$("button#submitTest").click(function () {
    var nameInput = document.getElementById("fullnameInput").value;
    var studentName = $("span#fullname").attr("data-studentName");

    if (nameInput === "") {
        $("h4#myModalLabel2").text("Warning:");
        $("p#errorMessagePlaceHolder").text("Submitting the test without a pledge will result in a 0. Are you sure you want to proceed?");
        $("#ErrorModal").modal("toggle");
    }
    else if (nameInput.trim() === studentName.trim()) {
        // Now verify that all of the questions have been answered
        var TestScheduleId = $("input#testScheduleId").val();
        $.ajax({
            url: "/api/Tests/ConfirmAllQuestionsAnswered/",
            type: "POST",
            contentType: "application/json",
            // Data fetched from the form
            data: TestScheduleId,
            success: function (result) {
                $("h4#myModalLabel2").text("Success!");
                $("p#errorMessagePlaceHolder").text("You've finished the test. You may now review the results.");
                $("#SuccessModal").modal("toggle");
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
                var err = JSON.parse(xhr.responseText);
            }
        })

        
    }
    else {
        $("h4#myModalLabel2").text("Error!");
        $("p#errorMessagePlaceHolder").text("The pledge does not match. Please re-enter your name.");
        document.getElementById("MessageNo").style.display = "none";
        document.getElementById("MessageYes").innerText = "Ok";
        $("#ErrorModal").modal("toggle");
    }
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