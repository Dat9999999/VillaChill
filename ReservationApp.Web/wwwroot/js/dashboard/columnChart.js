const chartOptions = {
    chart: {
        type: 'bar',
        height: 350
    },
    series: [{
        name: 'Revenue',
        data: [120000, 150000, 170000, 200000]
    }],
    xaxis: {
        categories: ['Week 1', 'Week 2', 'Week 3', 'Week 4']
    },
    yaxis: {
        labels: {
            formatter: function (value) {
                return value.toLocaleString('vi-VN') + ' ₫';  // hoặc '$' nếu cần
            }
        }
    },
    tooltip: {
        y: {
            formatter: function (value) {
                return value.toLocaleString('vi-VN') + ' ₫';  // định dạng số + tiền
            }
        }
    },
    title: {
        text: "Revenue Over Time",
        align: "center"
    }
};


const chart = new ApexCharts(document.querySelector("#chart"), chartOptions);
chart.render();

const timeRanges = {
    '1m': {
        categories: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
        data: [120, 150, 170, 200]
    },
    '3m': {
        categories: ['Jan', 'Feb', 'Mar'],
        data: [400, 500, 600]
    },
    '6m': {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
        data: [450, 470, 520, 490, 530, 580]
    },
    '12m': {
        categories: ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'],
        data: [300, 320, 310, 400, 380, 420, 460, 480, 500, 520, 540, 600]
    }
};

const select = document.getElementById('timeRange');

select.addEventListener('change', function () {
    const range = this.value;
    const { categories, data } = timeRanges[range];

    chart.updateOptions({
        xaxis: { categories },
        series: [{ name: 'Revenue', data }]
    });
});

// Default: show 1 month
select.value = '1m';
select.dispatchEvent(new Event('change'));
