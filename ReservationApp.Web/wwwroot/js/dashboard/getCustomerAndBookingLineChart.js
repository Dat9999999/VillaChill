var lineChart; // bên ngoài hàm

$(document).ready(function () {
    loadCustomerAndBookingLineChart();
});

function loadCustomerAndBookingLineChart() {
    $(".chart-spinner").show();
    console.log("loadCustomerAndBookingLineChart");
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

    // Nếu đã có biểu đồ cũ thì destroy trước
    if (window.lineChart) {
        window.lineChart.destroy();
    }

    var options = {
        colors: chartColors,
        chart: {
            height: 350,
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

    // Gán vào biến toàn cục để lần sau destroy
    window.lineChart = new ApexCharts(document.querySelector("#" + id), options);
    window.lineChart.render();
}
