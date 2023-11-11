// Line Chart
document.addEventListener('DOMContentLoaded', function () {
    getOrderTotalData();
    createPieChart();
});

function getOrderTotalData() {
    $.ajax({
        url: "/Admin/AdPage/GetOrderTotalData",
        type: "GET",
        success: function (data) {
            createLineChart(data);
        },
        error: function (error) {
            console.log("Error fetching data:", error);
        }
    });
}

function createLineChart(data) {
    var ctx = document.getElementById('linechart').getContext('2d');

    function generateLabels() {
        var labels = [];
        var currentDate = new Date();
        for (var i = 6; i >= 0; i--) {
            labels.push(currentDate.toLocaleDateString('en-US', { weekday: 'long' }));
            currentDate.setDate(currentDate.getDate() - 1);
        }
        return labels.reverse();
    }

    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: generateLabels(),
            datasets: [
                {
                    label: "Sales",
                    backgroundColor: 'transparent',
                    borderColor: "#0000FF",
                    borderWidth: 2,
                    pointBorderColor: "#0000FF",
                    data: data,
                    fill: true,
                    lineTension: 0.5,
                    showLine: true,
                },
                {
                    label: "Purchase Cost",
                    backgroundColor: 'transparent',
                    borderColor: "#0D0D0D",
                    borderWidth: 2,
                    pointBorderColor: "#0D0D0D",
                    data: data,
                    fill: true,
                    lineTension: 0.5,
                    showLine: true,
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    });
}

// Pie Chart hard-coded na data
function createPieChart() {
    const salesChannelsData = {
        labels: ['Walk-in', 'On-call', 'Chat-based', 'Website'],
        datasets: [{
            data: [5000, 3000, 2000, 3000],
            backgroundColor: ['rgb(255, 99, 132)', 'rgb(54, 162, 235)', 'rgb(75, 192, 192)', 'rgb(255,255,153)'],
        }]
    };

    const ctx = document.getElementById('piechart').getContext('2d');

    const salesChannelsChart = new Chart(ctx, {
        type: 'pie',
        data: salesChannelsData,
        options: {
            legend: {
                display: true,
                position: 'left',
            },
        },
    });
}
