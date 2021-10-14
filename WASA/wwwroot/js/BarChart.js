var barchart = document.getElementById('bar').getContext('2d');
var mybarchart = new chart(barchart, {
    type: 'bar',
    data: {
        labels: ['001', '002', '003'],
        datasets: [{
            label: 'Bar Chart',
            data: [100, 200, 300],
            backgroundColor: 'rgba(6,128,249)'
        }]
    },
    Options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
})


var piechart = document.getElementById('pie').getContext('2d');
var mypiechart = new chart(piechart, {
    type: 'pie',
    data: {
        labels: ['001', '002', '003'],
        datasets: [{
            label: 'Pie Chart',
            data: [100, 200, 300],
            backgroundColor: 'rgba(6,128,249)'
        }]
    },
    Options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    }
})