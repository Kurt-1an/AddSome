document.addEventListener("DOMContentLoaded", function () {
    // Get the chart canvas elements
    var salesCtx = document.getElementById('salesChart').getContext('2d');

    var salesRevenue = document.getElementById('salesChart').getAttribute('data-sales-revenue');
    var totalCostPrice = document.getElementById('salesChart').getAttribute('data-total-cost-price');

    // Create the sales chart as a bar chart
    var salesChart = new Chart(salesCtx, {
        type: 'bar',
        data: {
            labels: ['Sales & Purchase'],
            datasets: [{
                label: 'Sales Revenue',
                data: [salesRevenue],
                backgroundColor: 'rgba(75, 192, 192, 0.7)',   
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }, {
                label: 'Purchase Cost',
                data: [totalCostPrice],
                backgroundColor: 'rgba(255, 0, 0, 0.7)', 
                borderColor: 'rgba(255, 0, 0, 1)',
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
});