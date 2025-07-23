
$(document).ready(function () {
    loadBalanaceRadialChart();
});

function loadBalanaceRadialChart() {
    const card = document.querySelector('[data-owner-id]');
    if (!card) return;

    const ownerId = card.getAttribute('data-owner-id');
    
    $(".chart-spinner").show();

    $.ajax({
        url: `/Dashboard/GetCurrentBalanceRadialChartData/?ownerEmail=${ownerId}`,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            document.querySelector("#spanCurrentBalanceCount").innerHTML = formatNumberShort(data.currentBalance);

            $(".chart-spinner").hide();
        }
    });
}

