﻿function init_morris_charts() {

    if ($('#graph_donut').length) {

        Morris.Donut({
            element: 'graph_donut',
            data: [
                { label: 'z', value: 25 },
                { label: 'B', value: 35 },
                { label: 'C', value: 20 },
                { label: 'D', value: 9 },
                { label: 'F', value: 2 }
            ],
            colors: ['darkgreen', 'yellowgreen', 'gold', 'orangered', 'red'],
            formatter: function (y) {
                return y + " students";
            },
            resize: false
        });
    }
};

// Save Matching choice questions
$(".TestChartSelection").change(function () {
    $.ajax({
        url: "/Instructor/Chart",
        success: function (result) {
            $("#graph_donut").html(result);
        }
    });
})