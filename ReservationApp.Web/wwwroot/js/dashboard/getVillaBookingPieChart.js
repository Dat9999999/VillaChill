$(document).ready(function () {
    loadVillaBookingPieChart();
});

function loadVillaBookingPieChart() {
    const card = document.querySelector('[data-owner-id]');
    if (!card) return;

    const ownerId = card.getAttribute('data-owner-id');
    console.log("call villa distribution pie chart");
    $(".chart-spinner").show();

    $.ajax({
        url: `/Dashboard/GetVillaBookingPieChart/?ownerEmail=${ownerId}`,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            

            var options = {
                chart: {
                    type: 'pie',
                    height: 350
                },
                labels: data.labels,
                series: data.series,
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
