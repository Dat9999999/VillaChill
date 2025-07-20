const chart = new ApexCharts(document.querySelector("#chart"), {
    chart: {
        type: 'bar',
        height: 350
    },
    series: [{
        name: 'Revenue',
        data: [] // sẽ được cập nhật sau
    }],
    xaxis: {
        categories: [] // sẽ được cập nhật sau
    },
    yaxis: {
        labels: {
            formatter: value => value.toLocaleString('vi-VN') + ' ₫'
        }
    },
    tooltip: {
        y: {
            formatter: value => value.toLocaleString('vi-VN') + ' ₫'
        }
    },
    title: {
        text: "Revenue Over Time",
        align: "center"
    }
});

chart.render();
const select = document.getElementById('timeRange');
select.addEventListener('change', () => {
    const range = select.value;

    fetch(`/Dashboard/GetRevenueChartData?range=${range}`)
        .then(res => res.json())
        .then(data => {
            chart.updateOptions({
                xaxis: { categories: data.categories },
                series: [{ name: 'Revenue', data: data.data }]
            });
        });
});
select.value = '1m';
select.dispatchEvent(new Event('change'));