var lineChart;
$(document).ready(function () {
    loadCustomerAndBookingLineChart();
});

function loadCustomerAndBookingLineChart() {
    $(".chart-spinner").show();
    if (lineChart) {
        lineChart.destroy();
    }

    $.ajax({
        url: "/Dashboard/getCustomerAndBookingLineChart",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
      
            loadLineChart("newMembersAndBookingsLineChart", data);

            $(".chart-spinner").hide();
        }
    });
}

function loadLineChart(id, data) {
    var chartColors = getChartColorsArray(id);
    var options = {
        colors: chartColors,
        chart: {
            height: 350,
            //show area later
            type: 'line',
            zoom: {
                type: 'x',
                enabled: true,
                autoScaleYaxis: true
            },
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        series: data.series,
        dataLabels: {
            enabled: false,
        },
        markers: {
            size: 6,
            strokeWidth: 0,
            hover: {
                size: 9
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#fff",
                },
            }
        },
        yaxis: {
            labels: {
                //formatter: function (val) {
                //    return val.toFixed(0);
                //},
                style: {
                    colors: "#fff",
                },
            }
        },
        legend: {
            labels: {
                colors: "#fff",
            },
        },
        tooltip: {
            theme: 'dark'
        }
    };
    lineChart = new ApexCharts(document.querySelector("#" + id), options);
    lineChart.render();
}