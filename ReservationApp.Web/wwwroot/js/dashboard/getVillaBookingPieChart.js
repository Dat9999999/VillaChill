$(document).ready(function () {
    loadVillaBookingPieChart();
});

function loadVillaBookingPieChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: '/Dashboard/GetVillaBookingPieChart',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            data = [
                { "villaName": "Villa A", "bookingCount": 10 },
                { "villaName": "Villa B", "bookingCount": 20 },
                { "villaName": "Villa C", "bookingCount": 5 }
            ]
            const labels = data.map(x => x.villaName);
            const values = data.map(x => x.bookingCount);

            var options = {
                chart: {
                    type: 'pie',
                    height: 350
                },
                labels: labels,
                series: values,
                // fixed color
                colors: ['#F0006B', '#00E396', '#FEB019', '#775DD0', '#008FFB'],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: { width: 300 },
                        legend: { position: 'bottom' }
                    }
                }]
            };

            var chart = new ApexCharts(document.querySelector("#loadVillaBookingPieChart"), options);
            chart.render();

            $(".chart-spinner").hide();
        }
    });
}
