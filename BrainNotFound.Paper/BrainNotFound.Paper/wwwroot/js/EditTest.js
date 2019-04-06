//Constants

// Variables for this Page
var TotalPoints = 0;
var NumberOfQuestions = 0;
var NumberOfSections = 0;

// Tracks the assigment
var SectionsAssigned = [];
var IndivisualsAssigned = [];

// DataTable
var studentsDataTable;

// Currently selected Section 
var studentsInSelectedSection;

// Initialize Page
init_daterangepicker_TestSchedule();
init_students_datatable();
Update_TestStatistics();
Update_TestAssignmentTable();
init_testSections();

// Fetches all testSections
function init_testSections() {
    var testId = $("#TestId").val();
    $.ajax({
        url: "/api/CreateTest/GetTestSections/" + testId,
        type: "GET",
        success: function (result) {
            if (result.success) {

                var TestSection = $("#TestSectionTemplate").html();
                var template = Handlebars.compile(TestSection);
                result.testSections.forEach(function (testSection) {
                    //console.log(testSection);
                    $("#TestSections").append(template(testSection));
                    NumberOfSections += 1;

                    // Adds each question to its section

                    switch (testSection.sectionType) {
                        case "TrueFalse":
                            testSection.questions.forEach(function (question) {
                                var rendered = "";
                                var TrueFalseQuestionTemplate = $("#TrueFalseQuestionTemplate").html();
                                var template = Handlebars.compile(TrueFalseQuestionTemplate);

                                rendered += template(question);

                                $("#questionsContainer-" + testSection.sectionId).append(rendered);

                                TotalPoints += question.pointValue;
                                NumberOfQuestions += 1;
                                Update_TestStatistics();
                            });
                            break;
                        case "MultipleChoice":
                            testSection.questions.forEach(function (question) {
                                var rendered = "";
                                var multipleChoiceQuestionTemplate = $("#MultipleChoiceQuestionTemplate").html();
                                var template = Handlebars.compile(multipleChoiceQuestionTemplate);
                                rendered = template(question);

                                $("#questionsContainer-" + question.sectionId).append(rendered);

                                // Adds the question options to the questions
                                rendered = "";
                                question.multipleChoiceAnswers.forEach(function (questionOption) {
                                    var multipleChoiceOptionTemplate = $("#MultipleChoiceOptionTemplate").html();
                                    var template = Handlebars.compile(multipleChoiceOptionTemplate);
                                    rendered += template(questionOption);
                                });

                                $("#multipleChoiceOptionsContainer-" + question.questionId).html(rendered);
                            });
                            break;
                    }
                    
                })

                Update_TestStatistics();
            }
            else {
                console.log(result.errors)
            }
        },
    })

}

// Starts the Datatables for the edit test
function init_students_datatable() {
    studentsDataTable = $('#StudentsInSectionTable').DataTable({
        "columnDefs": [
            {
                targets: 0,
                searchable: false,
                orderable: false,
                className: 'select-checkbox',
                render: function (data, type, full, meta) {
                    return '<input class="selectedStudent" type="checkbox" value="' + $('<div/>').text(data).html() + '">';
                }

            }
        ],
        'order': [[1, 'asc']]
    });
}



// Reset New Schedule Time Picker
function init_daterangepicker_TestSchedule() {

    $('#testScheduleDateTime').daterangepicker({
        timePicker: true,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(11, 'day'),
        locale: {
            format: 'MM/DD/YYYY hh:mm A'
        }
    });

}
// Updates the statistics in the information section of the test
function Update_TestStatistics() {
    $("#TotalPointsStats").text(TotalPoints);
    $("#TotalQuestionsStats").text(NumberOfQuestions);
    $("#TotalSectionsStats").text(NumberOfSections);
}

// Updates the test assignment table
function Update_TestAssignmentTable() {
    var testId = $("#TestId").val();
    $.ajax({
        url: "/api/CreateTest/GetTestSchedules/" + testId,
        type: "GET",
        success: function (result) {
            if (result.success) {

                // There are any test schedules display them
                if (result.schedules != 'none') {
                    var rendered = "";
                    var Row = $("#ScheduleTableRowTemplate").html();
                    var template = Handlebars.compile(Row);

                    result.schedules.forEach(function (schedule) {
                        rendered += template(schedule);
                    })

                    $("#TestAssignmentTable > tbody").html(rendered);
                    $("table#TestAssignmentTable").removeClass("hidden");
                    $("div#NoScheduledTestContainer").addClass("hidden");
                }
                else {
                    $("table#TestAssignmentTable").addClass("hidden");
                    $("div#NoScheduledTestContainer").removeClass("hidden");
                }

            }
            else {
                console.log(result.errors)
            }
        },
    })
}

// Updates the tables showing witch sections and students are assigned to the test
function UpdateAssignmentTables() {

    // Updates the selected Sections
    if (SectionsAssigned.length == 0) {
        $("#SectionsAssignedTest").removeClass("show").addClass("hidden");
        $("#NoSectionsAssignedTest").removeClass("hidden").addClass("show");
    } else {
        var SectionsAssignedToTableRowTemplate = $("#SectionAssigmentTableRowTemplate").html();
        var template = Handlebars.compile(SectionsAssignedToTableRowTemplate);
        var rendered = "";
        for (var index = 0; index * 2 < SectionsAssigned.length; index++) {

            var templateInfo = { RowNumber: (index + 1), SectionNumbers: SectionsAssigned[(index * 2) + 1], SectionId: SectionsAssigned[(index * 2)] }
            rendered += template(templateInfo);
        }
        $("#SectionsAssignedTest > tbody").html(rendered);

        $("#NoSectionsAssignedTest").removeClass("show").addClass("hidden");
        $("#SectionsAssignedTest").removeClass("hidden");
    }

    // Updates the selected Students
    if (IndivisualsAssigned.length == 0) {
        $("#StudentsAssignedTest").removeClass("show").addClass("hidden");
        $("#NoStudentsAssignedTest").removeClass("hidden").addClass("show");
    } else {
        var StudentAssignmnetRowTemplate = $("#StudentAssignmnetRowTemplate").html();
        var template = Handlebars.compile(StudentAssignmnetRowTemplate);
        var rendered = "";
        for (var index = 0; index * 4 < IndivisualsAssigned.length; index++) {

            var templateInfo = { SectionNumber: IndivisualsAssigned[(index * 4)], StudentId: IndivisualsAssigned[((index * 4) + 1)], FirstName: IndivisualsAssigned[((index * 4) + 2)], LastName: IndivisualsAssigned[((index * 4) + 3)] }
            rendered += template(templateInfo);
        }
        $("#StudentsAssignedTest > tbody").html(rendered);

        $("#NoStudentsAssignedTest").removeClass("show").addClass("hidden");
        $("#StudentsAssignedTest").removeClass("hidden");
    }
}

// Resets the new schedule container
function resetNewSchedule() {
    $("div#NewScheduleContainer").removeClass("show").addClass("hidden");
    $("a#NewSchedule").removeClass("hidden");

    // Resets the dropdown section selection
    $('#SelectSection option').prop('selected', function () {
        return this.defaultSelected;
    });

    // Resets the assigned students and sections
    SectionsAssigned = [];
    IndivisualsAssigned = [];
    UpdateAssignmentTables();

    // Resets the time Picker
    init_daterangepicker_TestSchedule();

    // Reset Unlimited CheckBox
    $('#UnlimitedTimeCheckBox').prop('checked', false);
    $("input#TimeLimit").removeAttr("disabled");

    // Reset Time Limit
    $("#TimeLimit").val("50");

    //hides the students table
    $("div#StudentsInSectionTableContainer").removeClass("show").addClass("hidden");
    $("div#SectionNotSelectedContainer").removeClass("hidden").addClass("show");
}

// -- Event Handlers

// input fiels change event

// Updates the point value in the database for every question
$("#TestSections").on("change", ".pointValue", function () {
    // Gets the infotmetion about the question
    var questionId = $(this).attr("data-questionId");
    var currentPointValue = $(this).attr("data-currentPointValue");
    $(this).attr("data-currentPointValue", pointValue);
    var pointValue = $(this).val();

    var JsonData = JSON.stringify({ questionId: questionId, pointValue: pointValue })
    //console.log(JsonData);
    // send the information to the server
    $.ajax({
        url: "/api/CreateTest/UpdateQuestionPointValue",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {

                TotalPoints -= parseInt(result.oldPointValue);
                TotalPoints += parseInt(pointValue);

                Update_TestStatistics();
            }
            else {
                console.log(result);
            }
            // TODO: Display if Sections were already assigned
        },
    })


})

// Dropdowns
// Fetched the list of students from the server
$('select#SelectSection').change(function () {
    var sectionId = $("#SelectSection").val();

    $.ajax({
        url: "/api/CreateTest/GetStudentsInSection/" + sectionId,
        type: "GET",
        success: function (students) {
            //Fetches the template and, iterates through students, and renders table rows

            studentsInSelectedSection = students;
            $("#StudentsInSectionTable").DataTable().clear();

            students.forEach(function (student) {
                $('#StudentsInSectionTable').dataTable().fnAddData([
                    student.Id,
                    student.FirstName,
                    student.LastName,
                ]);
            })

            // Hides no section select message
            $("div#SectionNotSelectedContainer").removeClass("show").addClass("hidden");
            $("div#StudentsInSectionTableContainer").removeClass("hidden").addClass("show");


        }
    });
});

// Checkboxes
//Disables Time Limit Textbox
$("#UnlimitedTimeCheckBox").change(function () {

    if ($("#UnlimitedTimeCheckBox").is(':checked')) {
        $("input#TimeLimit").attr("disabled", "disabled");
    }
    else {
        $("input#TimeLimit").removeAttr("disabled");
    }
})

// Buttons
// Shows the edit test name section
$("a#EditTestName").click(function () {
    $("div#EditNameContainer").removeClass("hidden").addClass("show");
})

// Hides the edit test name section
$("a#cancelEditTestName").click(function () {
    $("div#EditNameContainer").removeClass("show").addClass("hidden");
})

// Shows the new sechedule section
$("a#NewSchedule").click(function () {
    $("div#NewScheduleContainer").removeClass("hidden").addClass("show");
    $("a#NewSchedule").addClass("hidden");
})

// Hides and Resets the new sechedule section
$("a#CancelNewSchedule").click(function () {
    resetNewSchedule();
})

// Assign a section to the schedule
$("button#AssignEntireSection").click(function () {
    var sectionId = $("#SelectSection").val();
    var sectionNumber = $("#SelectSection option:selected").text();

    // Checks if section has already been assigned.
    if (sectionId != null && !SectionsAssigned.includes(sectionId, sectionNumber)) {
        SectionsAssigned.push(sectionId, sectionNumber);
        UpdateAssignmentTables();
    }
})

// Assigneds the currently selected section 
$("button#AssignSelectedStudents").click(function () {
    var sectionNumber = $("#SelectSection option:selected").text();
    studentsDataTable.$('.selectedStudent').each(function () {
        if (this.checked) {
            var studentId = $(this).val();

            if (!IndivisualsAssigned.includes(studentId)) {
                var index = studentsInSelectedSection.findIndex(s => s.Id == studentId);
                IndivisualsAssigned.push(
                    sectionNumber.replace("Section ", ""),
                    studentsInSelectedSection[index].Id,
                    studentsInSelectedSection[index].FirstName,
                    studentsInSelectedSection[index].LastName
                )
            }
        }
    })
    UpdateAssignmentTables();
})

// Send the information about a new schedule to the server and updates the view
$("button#SaveNewTestSchedule").click(function () {
    var TestId = $("#TestId").val();
    var StartEndDateTime = $("#testScheduleDateTime").val();
    var IsTimeUnlimited = $("#UnlimitedTimeCheckBox").is(':checked');
    var TimeLimit = $("#TimeLimit").val();
    var Students = [];
    var Sections = [];

    // Get the id for each selected section
    for (var index = 0; index * 2 < SectionsAssigned.length; index++) {
        Sections.push(SectionsAssigned[(index * 2)]);
    }

    // Get the id for each selected student
    for (var index = 0; index * 4 < IndivisualsAssigned.length; index++) {
        Students.push(IndivisualsAssigned[((index * 4) + 1)]);
    }

    var JsonData = JSON.stringify({
        "TestId": TestId,
        "StartEndDateTime": StartEndDateTime,
        "IsTimeUnlimited": IsTimeUnlimited,
        "TimeLimit": TimeLimit,
        "Students": Students,
        "Sections": Sections
    });

    $.ajax({
        url: "/api/CreateTest/NewTestSchedule",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {
                Update_TestAssignmentTable();
                resetNewSchedule();
            }
            else {
                console.log(result);
            }
            // TODO: Display if Sections were already assigned
        },
    })
})

// Adds a Generic Section to a Test
$("#AddTestSectionBtn").click(function () {
    var rendered = "";
    var GenericTestSection = $("#GenericTestSectionTemplate").html();
    var template = Handlebars.compile(GenericTestSection);

    rendered += template();

    $("#TestSections").append(rendered);
})



// Removes a section from the assigment table
$("table#SectionsAssignedTest").on("click", ".deleteEntireSectionAssignment", function () {
    var SectionId = $(this).val();

    SectionsAssigned.splice(SectionsAssigned.indexOf(SectionId), 2);
    UpdateAssignmentTables();

})

// Remove a student from the assignment table
$("table#StudentsAssignedTest").on("click", ".deleteStudentAssigment", function () {
    var StudentId = $(this).val();
    IndivisualsAssigned.splice(IndivisualsAssigned.indexOf(StudentId) - 1, 4);

    UpdateAssignmentTables();

})

// Cancels the new section 
$("#TestSections").on("click", "button#cancelQuestionType", function () {
    $(this).parents(".SectionContainer").remove();
})

// Sets the test section to the chosen question type
$("#TestSections").on("click", "button#setQuestionType", function () {
    // Get the Section type chooser container
    var SectionTypeContainer = $(this).parents(".SectionContainer");

    // Get the data
    var QuestionType = $(this).prev().find("#questionType").val();
    var TestId = $("#TestId").val();
    var JsonData = JSON.stringify({ "TestId": TestId, "QuestionType": QuestionType });

    $.ajax({
        url: "/api/CreateTest/CreateTestSection",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JsonData,
        success: function (result) {
            if (result.success) {

                var rendered = "";
                var TestSection = $("#TestSectionTemplate").html();
                var template = Handlebars.compile(TestSection);

                rendered += template({ instructions: result.instructions, sectionId: result.sectionId, sectionType: result.sectionType, header: result.header });

                $(SectionTypeContainer).before(rendered);
                $(SectionTypeContainer).remove();

                NumberOfSections += 1;
                Update_TestStatistics();
            }
            else {
                console.log(result.errors)
            }
        },
    })
})

// Displays section Editable instruction Box
$("#TestSections").on("click", "a.editInstructions", function () {
    var sectionId = $(this).attr("data-sectionId");

    var currentSectionInstructions = $("span#currentInstructions-" + sectionId).text();
    $("input#editSectionInstruction-" + sectionId).val(currentSectionInstructions);
    $("div#editInstructionsContainer-" + sectionId).removeClass("hidden");

})

// Cancels the Edit Section Instruction
$("#TestSections").on("click", "button.cancelEditSectionInstruction", function () {

    var sectionId = $(this).attr("data-sectionId");
    $("div#editInstructionsContainer-" + sectionId).addClass("hidden");
})

// Saves the Edited Section instructions
$("#TestSections").on("click", "button.saveEditedSectionInstruction", function () {

    var sectionId = $(this).attr("data-sectionId");
    var currentSectionInstructions = $("span#currentInstructions-" + sectionId).text();
    var newSectionInstructios = $("input#editSectionInstruction-" + sectionId).val();


    if (currentSectionInstructions != newSectionInstructios) {
        var JsonData = JSON.stringify({ TestSectionId: sectionId, SectionInstructions: newSectionInstructios });
        $.ajax({
            url: "/api/CreateTest/UpdateSectionInstruction",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    $("span#currentInstructions-" + sectionId).text(newSectionInstructios);
                    $("div#editInstructionsContainer-" + sectionId).addClass("hidden");
                }
                else {
                    console.log(result)
                }
            },
        })
    }
})

// Removes new question container
$("#TestSections").on("click", "button.cancelNewQuestion", function () {
    $(this).parents(".newQuestionContainer").remove();
})
// Adds a question to a section
$("#TestSections").on("click", "button.addQuestionToSection", function () {
    var sectionId = $(this).attr("data-sectionId");
    var sectionType = $(this).attr("data-sectionType");
    var rendered = "";
    var templateId = "";

    switch (sectionType) {
        case "TrueFalse":
            templateId = "#NewTrueFalseQuestionTemplate";
            break;
        case "MultipleChoice":
            templateId = "#NewMultipleChoiceQuestionTemplate";
            break;

    }
    var timestamp = new Date().getUTCMilliseconds();
    var newQuestion = $(templateId).html();
    var template = Handlebars.compile(newQuestion);
    rendered += template({ SectionId: sectionId, timeStamp: timestamp, QuestionType: sectionType });

    $("#newQuestionContainer-" + sectionId).append(rendered);

    $('input.flat').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });
})

// Saves a newly created true false question
$("#TestSections").on("click", "button.saveNewTrueFalseQuestion", function () {
    var sectionId = $(this).attr("data-sectionId");
    var uuid = $(this).attr("data-uuid");

    // Getting question data from the view
    var questionContent = $("#TrueFalseContent-" + uuid).val();
    var pointValue = $("#TrueFalsePointValue-" + uuid).val();
    var questionAnswer = $("input[name='TrueFalseRadioButton-" + uuid + "']:checked").val();

    // Error checking on question content
    if (questionContent.length == 0) {
        $("#TFContentError-" + uuid).removeClass("hidden");
    }
    else {
        // Submits the data to the server
        var JsonData = JSON.stringify({ testSectionId: sectionId, content: questionContent, pointValue: pointValue, answer: questionAnswer });
        var newQuestionContainer = $(this).parents(".newQuestionContainer");

        console.log("questionAnswer: " + questionAnswer);
        console.log("JasonData: " + JsonData);
        $.ajax({
            url: "/api/CreateTest/NewTrueFalseQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    var rendered = "";
                    var TrueFalseQuestionTemplate = $("#TrueFalseQuestionTemplate").html();
                    var template = Handlebars.compile(TrueFalseQuestionTemplate);

                    rendered += template(result.question);

                    $("#questionsContainer-" + sectionId).append(rendered);
                    $(newQuestionContainer).remove();
                    TotalPoints += result.question.pointValue;
                    NumberOfQuestions += 1;
                    Update_TestStatistics();
                }
                else {
                    console.log(result)
                }
            },
        })
    }

})

// Saves a newly created Multiple choice question
$("#TestSections").on("click", ".saveNewMultipleChoiceQuestion", function () {
    var testSectionId = $(this).attr("data-sectionId");
    var uuid = $(this).attr("data-uuid");
    var questionContent = $.trim($("#MultipleChoiceContent-" + uuid).val());
    var pointValue = $("#MultipleChoicePointValue-" + uuid).val();
    var newQuestionContainer = $(this).parents(".newQuestionContainer");
    var hasError = false;

    // Checks if there is at least two options
    if ($(".newMultipleChoiceOption-" + uuid).length < 2) {
        hasError = true;
        $("li#MinimumNumberOfOptionErrorMessage-" + uuid).removeClass("hidden");
    }
    else {
        $("li#MinimumNumberOfOptionErrorMessage-" + uuid).addClass("hidden");
    }



    // gets the multiple choices options
    var multipleChoiceAnswers = [];
    var hasCorretOption = false;
    $(".newMultipleChoiceOption-" + uuid).each(function () {

        var isCorrect = $(this).find(".isCorrect").is(':checked');
        var optionContent = $.trim($(this).find(".optionContent").val());

        // Error check if options are empty
        if (optionContent == "") {
            hasError = true;
            $(this).find(".optionErrorMessage").removeClass("hidden");
        }
        else {
            $(this).find(".optionErrorMessage").addClass("hidden");
        }

        // Error Checking for correct 
        if (isCorrect) {
            hasCorretOption = true;
        }

        multipleChoiceAnswers.push({ isCorrect: isCorrect, optionContent: optionContent });
    });

    // Error Checking for correct 
    if (!hasCorretOption) {
        hasError = true;
        $("li#SelectedCorrectOptionErrorMessage-" + uuid).removeClass("hidden");
    } else {
        $("li#SelectedCorrectOptionErrorMessage-" + uuid).addClass("hidden");
    }

    // Error check if content is empty
    if (questionContent == "") {
        hasError = true;
        $("#multipleChoiceContentError-" + uuid).removeClass("hidden");
    }
    else {
        $("#multipleChoiceContentError-" + uuid).addClass("hidden");
    }

    if (!hasError) {
        var JsonData = JSON.stringify({ testSectionId: testSectionId, questionContent: questionContent, pointValue: pointValue, multipleChoiceAnswers: multipleChoiceAnswers });

        $.ajax({
            url: "/api/CreateTest/NewMultipleChoiceQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    // Adds question to section
                    var rendered = "";
                    var multipleChoiceQuestionTemplate = $("#MultipleChoiceQuestionTemplate").html();
                    var template = Handlebars.compile(multipleChoiceQuestionTemplate);
                    rendered = template(result.question);

                    $("#questionsContainer-" + result.question.sectionId).append(rendered);

                    // Adds the question options to the questions
                    rendered = "";
                    result.question.multipleChoiceAnswers.forEach(function (questionOption) {
                        var multipleChoiceOptionTemplate = $("#MultipleChoiceOptionTemplate").html();
                        var template = Handlebars.compile(multipleChoiceOptionTemplate);
                        rendered += template(questionOption);
                    });

                    $("#multipleChoiceOptionsContainer-" + result.question.questionId).html(rendered);

                    $(newQuestionContainer).remove();
                    console.log(result);
                    // ADD-NOTIFICATION
                }
                else {
                    console.log(result)
                }
            }
        })
    }
})

// Add a multiple choice option to a new multiple choice question
$("#TestSections").on("click", ".addMultipleChoiceOptionNewQuestion", function () {
    var uuid = $(this).attr("data-uuid");

    rendered = "";
    var MultipleChoiceOptionTextBoxTemplate = $("#MultipleChoiceOptionTextBoxTemplate").html();
    var template = Handlebars.compile(MultipleChoiceOptionTextBoxTemplate);

    rendered += template({ "timeStamp": uuid });

    $("#MultipleChoiceOptionsContainer-" + uuid).append(rendered);
})

// Add a multiple choice option to a new multiple choice question
$("#TestSections").on("click", ".addMultipleChoiceOptionEditQuestion", function () {
    
    var questionId = $(this).attr("data-questionId");
    console.log("questionId: " + questionId);
    var rendered = "";
    var EditableMultipleChoiceOptionTextBoxTemplate = $("#EditableMultipleChoiceOptionTextBoxTemplate").html();
    var template = Handlebars.compile(EditableMultipleChoiceOptionTextBoxTemplate);

    rendered += template({questionId: questionId});
    $("#EditMultipleChoiceOptionsContainer-" + questionId).append(rendered);

    rendered += template({ questionId: questionId });
})

// Delets a multiple choice from Multiple choice new multiple choic
$("#TestSections").on("click", ".deleteMultipleChoiceOptionNewQuestion", function () {
    var uuid = $(this).attr("data-uuid");
    var questionId = $(this).attr("data-questionId");

    $(this).parents(".newMultipleChoiceOption-" + uuid).remove();
    $(this).parents(".EditMultipleChoiceOption-" + questionId).remove();
    
})

// Deletes a Question from a section
$("#TestSections").on("click", ".deleteQuestion", function () {
    var questionId = $(this).attr("data-questionId");
    var JsonData = JSON.stringify({ questionId: questionId });

    $('#confirm-deletion-modal .modal-body').html("<p>Are you sure you want to delete this question?</p>");
    $("#confirmDeletion").data("questionId", questionId);
    $("#confirmDeletion").removeData("sectionId");

    $('#confirm-deletion-modal').modal('toggle');

})

// Deletes a test Section
$("#TestSections").on("click", ".deleteSection", function () {
    var testSectionId = $(this).attr("data-testSectionId");

    $('#confirm-deletion-modal .modal-body').html("<p>Are you sure you want to delete this section?</p>");
    $("#confirmDeletion").data("sectionId", testSectionId);

    $('#confirm-deletion-modal').modal('toggle');
})

// Confirmes the deletion of a section or question
$("#confirmDeletion").click(function () {
    if (typeof $("#confirmDeletion").data('sectionId') !== 'undefined') {
        var sectionId = $(this).data("sectionId");
        var JsonData = JSON.stringify({ sectionId: sectionId });

        $.ajax({
            url: "/api/CreateTest/DeleteTestSection",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    $(".sectionContainer-" + sectionId).remove();
                    $('#confirm-deletion-modal').modal('toggle');
                    // ADD-NOTIFICATION
                }
                else {
                    console.log(result)
                }
            }
        })
    }
    else {
        var questionId = $(this).data("questionId");
        var JsonData = JSON.stringify({ questionId: questionId });
        $.ajax({
            url: "/api/CreateTest/DeleleQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    $("#questionContainer-" + questionId).remove();

                    $('#confirm-deletion-modal').modal('toggle');

                    // ADD-NOTIFICATION
                }
                else {
                    console.log(result)
                }
            }
        })
    }
})

// Displays the edit box for a true false question
$("#TestSections").on("click", ".editTrueFalseQuestion", function () {

    var questionId = $(this).attr("data-questionId");
    var pointValue = $("#pointValue-" + questionId).val();
    var answer = $("#TrueFalseAnswer-" + questionId).attr("data-answer") == "true";
    var content = $("#content-" + questionId).text();
    // Fetch the template
    var rendered = "";
    var editQuestionTemplate = $("#EditTrueFalseQuestionTemplate").html();
    var template = Handlebars.compile(editQuestionTemplate);

    //Apply template
    rendered += template({ questionId: questionId, pointValue: pointValue, answer: answer, content: content });
    console.log(answer);
    $("#questionContainer-" + questionId).before(rendered);
    $("#questionContainer-" + questionId).addClass("hidden");

    // Initialize rdio buttons
    $('input.flat').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });

})
// Displays the edit box for a multiple choice question
$("#TestSections").on("click", ".editMultipleChoiceQuestion", function () {
    // Get the question Information
    var questionId = $(this).attr("data-questionId");
    var pointValue = $("#pointValue-" + questionId).val();
    var content = $("#content-" + questionId).text();

    // Get Editable question Template
    var rendered = "";
    var EditMultipleChoiceQuestionTemplate = $("#EditMultipleChoiceQuestionTemplate").html();
    var template = Handlebars.compile(EditMultipleChoiceQuestionTemplate);

    rendered = template({ questionId: questionId, content: content, pointValue: pointValue})

    // Place editable question in DOM 
    $("#questionContainer-" + questionId).before(rendered).addClass("hidden");
    rendered = "";
    // Get all the question options
    $(this).parents("#questionContainer-" + questionId)
        .find(".multipleChoiceOption")
        .each(function () {
            var isCorrect = $(this).children("p").attr("data-isCorrect") == "true";
            var optionContent = $(this).children("p").text();


            var EditableMultipleChoiceOptionTextBoxTemplate = $("#EditableMultipleChoiceOptionTextBoxTemplate").html();
            var template2 = Handlebars.compile(EditableMultipleChoiceOptionTextBoxTemplate);
            rendered += template2({ questionId: questionId, optionContent: optionContent, isCorrect: isCorrect})
        });
    $("#EditMultipleChoiceOptionsContainer-" + questionId).html(rendered);

})
// Closes the edit question state
$("#TestSections").on("click", ".cancelEditQuestion", function () {
    var questionId = $(this).attr("data-questionId");
    $(this).parents(".editQuestionContainer").remove();
    $("#questionContainer-" + questionId).removeClass("hidden");
})

// Saves the edited multiple choice on the database
$("#TestSections").on("click", ".saveEdittedMultipleChoiceQuestion", function () {
    var questionId = $(this).attr("data-questionId");
    var questionContent = $.trim($("#MultipleChoiceContent-" + questionId).val());
    var pointValue = $("#MultipleChoicePointValue-" + questionId).val();
    var editQuestionContainer = $(this).parents(".editQuestionContainer");
    var hasError = false;

    // Checks if there is at least two options
    if ($(".EditMultipleChoiceOption-" + questionId).length < 2) {
        hasError = true;
        $("li#MinimumNumberOfOptionErrorMessage-" + questionId).removeClass("hidden");
    }
    else {
        $("li#MinimumNumberOfOptionErrorMessage-" + questionId).addClass("hidden");
    }
    
    // gets the multiple choices options
    var multipleChoiceAnswers = [];
    var hasCorretOption = false;
    $(".EditMultipleChoiceOption-" + questionId).each(function () {

        var isCorrect = $(this).find(".isCorrect").is(':checked');
        var optionContent = $.trim($(this).find(".optionContent").val());

        // Error check if options are empty
        if (optionContent == "") {
            hasError = true;
            $(this).find(".optionErrorMessage").removeClass("hidden");
        }
        else {
            $(this).find(".optionErrorMessage").addClass("hidden");
        }

        // Error Checking for correct 
        if (isCorrect) {
            hasCorretOption = true;
        }

        multipleChoiceAnswers.push({ isCorrect: isCorrect, optionContent: optionContent });
    });

    // Error Checking for correct 
    if (!hasCorretOption) {
        hasError = true;
        $("li#SelectedCorrectOptionErrorMessage-" + questionId).removeClass("hidden");
    } else {
        $("li#SelectedCorrectOptionErrorMessage-" + questionId).addClass("hidden");
    }

    // Error check if content is empty
    if (questionContent == "") {
        hasError = true;
        $("#multipleChoiceContentError-" + questionId).removeClass("hidden");
    }
    else {
        $("#multipleChoiceContentError-" + questionId).addClass("hidden");
    }

    if (!hasError) {
        var JsonData = JSON.stringify({questionId: questionId, questionContent: questionContent, pointValue: pointValue, multipleChoiceAnswers: multipleChoiceAnswers });

        $.ajax({
            url: "/api/CreateTest/UpdateMultipleChoiceQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {

                    $(editQuestionContainer).remove();
                    // Adds question to section
                    // Adds question to section
                    var rendered = "";
                    var multipleChoiceQuestionTemplate = $("#MultipleChoiceQuestionTemplate").html();
                    var template = Handlebars.compile(multipleChoiceQuestionTemplate);
                    rendered = template(result.question);

                    $("#questionContainer-" + result.question.questionId).before(rendered).remove();

                    // Adds the question options to the questions
                    rendered = "";
                    result.question.multipleChoiceAnswers.forEach(function (questionOption) {
                        var multipleChoiceOptionTemplate = $("#MultipleChoiceOptionTemplate").html();
                        var template = Handlebars.compile(multipleChoiceOptionTemplate);
                        rendered += template(questionOption);
                    });

                    $("#multipleChoiceOptionsContainer-" + result.question.questionId).html(rendered);
                    
                    console.log(result);
                    // ADD-NOTIFICATION
                }
                else {
                    console.log(result)
                }
            }
        })
    }


    // TODO: UpdateTrueFalseQuestion -> QuestionId, Point value, content, bool ansewer
})

// Saves the edited true false information on the database
$("#TestSections").on("click", ".saveEdittedTrueFalseQuestion", function () {
    var questionId = $(this).attr("data-questionId");
    var pointValue = $("#pointValue-" + questionId).val();
    var content = $("#TrueFalseContent-" + questionId).val();
    var saveButton = this;
    var answer = $("input[name='TrueFalseRadioButton-" + questionId + "']:checked").val();

    // Error checks for empty Question content
    if (content.length > 0) {

        var JsonData = JSON.stringify({ questionId: questionId, content: content, pointValue: pointValue, answer: answer });

        console.log(JsonData);
        $.ajax({
            url: "/api/CreateTest/UpdateTrueFalseQuestion",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JsonData,
            success: function (result) {
                if (result.success) {
                    $(saveButton).parents(".editQuestionContainer").remove();

                    // Updates the question
                    $("#questionContainer-" + questionId).removeClass("hidden");
                    $("#pointValue-" + questionId).val(pointValue);
                    $("#content-" + questionId).text(content);
                    $("#TrueFalseAnswer-" + questionId).attr("data-answer", answer);
                    if (answer == "true") {
                        $("#TrueFalseAnswer-" + questionId).removeClass("label-danger").addClass("label-success").text("True");
                    } else {
                        $("#TrueFalseAnswer-" + questionId).removeClass("label-success").addClass("label-danger").text("False");
                    }

                    // ADD-NOTIFICATION
                }
                else {
                    console.log(result)
                }
            }
        })

    } else {
        $("TFContentError-" + questionId).removeClass("hidden");
    }


    // TODO: UpdateTrueFalseQuestion -> QuestionId, Point value, content, bool ansewer
})


// Handles all forms submition buttons
$(function () {
    $('.post-using-ajax').each(function () {
        var $frm = $(this);
        $frm.submit(function (e) {
            e.preventDefault();

            $.ajax({
                type: $frm.attr('method'),
                url: $frm.attr('action'),
                data: $frm.serialize(),
                success: function (msg) {
                    alert("Success");
                }
            });
        });
    });
});

// Re-assigns back browser button
window.onpopstate = function () {
    window.location.href = "/Instructor/Tests";
}; history.pushState({}, '');


$("table#TestAssignmentTable").on("click", ".DeleteSectionSchedule", function () {
    var sectionScheduleId = $(this).attr("data-testScheduleId");
    $.ajax({
        url: "/api/CreateTest/DeleteSectionSchedule",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: sectionScheduleId,
        success: function (result) {
            if (result.success) {
                Update_TestAssignmentTable();
            }
            else {
                console.log(result.error);
            }
        }
    })
})