// Line Chart
document.addEventListener("DOMContentLoaded", function () {
    var salesCtx = document.getElementById('linechart').getContext('2d');
    var pieCtx = document.getElementById('piechart').getContext('2d');

    var salesRev = document.getElementById('linechart').getAttribute('salesrevenue');
    var totalcostprice = document.getElementById('linechart').getAttribute('costpricetotal');

    //hardcoded data for piechart
    var salesByChannelData = [1000, 1500, 800, 1200];

    function generateLabels() {
        var labels = [];
        var currentDate = new Date();
        for (var i = 6; i >= 0; i--) {
            labels.push(currentDate.toLocaleDateString('en-US', { weekday: 'long' }));
            currentDate.setDate(currentDate.getDate() - 1);
        }
        return labels.reverse();
    }

    var linechart = new Chart(salesCtx, {
        type: 'line',
        data: {
            labels: generateLabels(),
            datasets: [{
                label: 'Sales Revenue',
                data: [salesRev], // Parse the attribute value as JSON array
                backgroundColor: 'rgba(75, 192, 192, 0.7)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }, {
                label: 'Purchase Cost',
                data: [totalcostprice], // Parse the attribute value as JSON array
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

    var piechart = new Chart(pieCtx, {
        type: 'pie',
        data: {
            labels: ['Walk-in', 'On Call', 'Chat-based', 'Website'],
            datasets: [{
                data: salesByChannelData,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.7)',
                    'rgba(54, 162, 235, 0.7)',
                    'rgba(255, 206, 86, 0.7)',
                    'rgba(75, 192, 192, 0.7)',
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                ],
                borderWidth: 1
            }]
        }
    });
});