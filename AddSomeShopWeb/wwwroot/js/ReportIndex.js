document.addEventListener("DOMContentLoaded", function () {
    // Get the chart canvas elements
    var inventoryCtx = document.getElementById('inventoryChart').getContext('2d');

    // Inventory Chart
    // Get data from data attributes
    var instockProducts = document.getElementById('inventoryChart').getAttribute('data-instock-products');
    var lowStockProducts = document.getElementById('inventoryChart').getAttribute('data-low-stock-products');
    var outOfStockProducts = document.getElementById('inventoryChart').getAttribute('data-out-of-stock-products');

    // Create the inventory chart
    var inventoryChart = new Chart(inventoryCtx, {
        type: 'bar',
        data: {
            labels: ['Inventory Levels'],
            datasets: [{
                label: 'In Stock',
                data: [instockProducts],
                backgroundColor: 'rgba(0, 128, 0, 0.7)',   // Green for In Stock
                borderColor: 'rgba(0, 128, 0, 1)',
                borderWidth: 1
            }, {
                label: 'Low Stock',
                data: [lowStockProducts],
                backgroundColor: 'rgba(255, 165, 0, 0.7)', // Orange for Low Stock
                borderColor: 'rgba(255, 165, 0, 1)',
                borderWidth: 1
            }, {
                label: 'Out of Stock',
                data: [outOfStockProducts],
                backgroundColor: 'rgba(255, 0, 0, 0.7)',    // Red for Out of Stock
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