document.addEventListener('DOMContentLoaded', LoadSentimentDonutChart);
function LoadSentimentDonutChart() {
    const select = document.getElementById("villaSelect");
    const loadChart = (villaId = "") => {
        fetch(`/dashboard/GetSentimentRatio?villaId=${villaId}`)
            .then(res => res.json())
            .then(data => {
                const chartEl = document.querySelector("#sentimentDonutChart");
                chartEl.innerHTML = ""; // clear old chart

                const chart = new ApexCharts(chartEl, {
                    chart: {
                        type: 'donut',
                        height: 250
                    },
                    series: [data.positive, data.negative, data.neutral],
                    labels: ['Positive', 'Negative', 'Neutral'],
                    colors: ['#28a745', '#dc3545', '#ffc107'],
                    legend: {
                        position: 'bottom'
                    }
                });

                chart.render();
            });
    };

    // Initial load (all villas)
    loadChart();

    select.addEventListener('change', () => {
        const villaId = select.value;
        loadChart(villaId);
    });
}
