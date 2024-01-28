const salesCtx = document.getElementById('salesChart')
var salesRevenue = parseInt(salesCtx.getAttribute('salesRevenue'));

new Chart(salesCtx, {
    type: 'bar',
    data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [{
            label: 'Sales Revenue',
            data: [salesRevenue],
            backgroundColor: 'rgba(75, 192, 192, 0.7)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

/*
document.addEventListener("DOMContentLoaded", function () {
    // Get the chart canvas elements
    var salesCtx = document.getElementById('salesChart').getContext('2d');

    var salesRevenue = document.getElementById('salesChart').getAttribute('data-sales-revenue');

    var labels = Utils.months({ count: 7 });

    var salesChart = new Chart(salesCtx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Sales Revenue',
                data: [salesRevenue],
                backgroundColor: 'rgba(75, 192, 192, 0.7)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Months'
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Sales Revenue'
                    }
                }
            }
        }
    });
});*/