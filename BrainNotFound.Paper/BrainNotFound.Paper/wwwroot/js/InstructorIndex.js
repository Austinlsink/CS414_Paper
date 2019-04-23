function init_morris_charts() {

    if ($('#graph_donut').length) {

        Morris.Donut({
            element: 'graph_donut',
            data: [
                { label: 'JJ', value: 25 },
                { label: 'B', value: 35 },
                { label: 'C', value: 20 },
                { label: 'D', value: 9 },
                { label: 'F', value: 2 }
            ],
            colors: ['darkgreen', 'yellowgreen', 'gold', 'orangered', 'red'],
            formatter: function (y) {
                return y + " students";
            },
            resize: true
        });

    }

};

// Save Matching choice questions
$(".TestChartSelection").change(function () {


    $.ajax({
        url: "/api/Tests/SaveMatchingChoiceAnswer/",
        type: "POST",
        contentType: "application/json",
        // Data fetched from the form
        data: JsonData,
        success: function (result) {
            console.log("Success");
        },
        error: function (xhr, status, error) {
            console.log("Failed");
        }
    })
})