
$(document).ready(function () {
    loadRevenueRadialChart();
});

function loadRevenueRadialChart() {
    $(".chart-spinner").show();
    const card = document.querySelector('[data-owner-id]');
    if (!card) return;

    const ownerId = card.getAttribute('data-owner-id');
    console.log(ownerId);

    $.ajax({
        url: "/Dashboard/GetRevenueRadialChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            const formattedTotal = formatNumberShort(data.totalCount);
            const formattedMonth = formatNumberShort(data.countInCurrentMonth);

            document.querySelector("#spanTotalRevenueCount").innerHTML = formattedTotal;

            var sectionCurrentCount = document.createElement("span");
            if (!data.hasRatioIncreased) {
                sectionCurrentCount.className = "text-success me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-up-right-circle me-1"></i> <span> ' + formattedMonth + '</span>';
            }
            else {
                sectionCurrentCount.className = "text-danger me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle me-1"></i> <span> ' + formattedMonth + '</span>';
            }

            document.querySelector("#sectionRevenueCount").append(sectionCurrentCount);
            document.querySelector("#sectionRevenueCount").append(" since last month");

            loadRadialBarChart("totalRevenueRadialChart", data);

            $(".chart-spinner").hide();
        }

    });
}

