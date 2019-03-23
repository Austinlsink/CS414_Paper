// Adds a Generic Section to a Test
$("#AddTestSection").click(function () {
    var rendered = "";
    var GenericTestSection = $("#GenericTestSectionTemplate").html();
    var template = Handlebars.compile(GenericTestSection);

    rendered += template();

    $("#TestSections").append(rendered);
})

// 