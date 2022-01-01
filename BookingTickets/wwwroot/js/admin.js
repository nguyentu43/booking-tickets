
jQuery().ready(function () {
    jQuery('.btnDelete').click(function (e) {
        var f = confirm("Delete this?");
        if (!f)
            e.preventDefault();
    });

    let selectSeatTypeColors = {
        "VIP": "bg-danger",
        "NORMAL": "bg-primary",
        "NONE": ""
    }

    jQuery('.selectSeatType').change(function (e) {
        jQuery(this).removeClass().addClass("form-control");
        let val = jQuery(this).find(":selected").text();
        jQuery(this).addClass(selectSeatTypeColors[val]);
    });

    jQuery('[name="StartDate"]').datetimepicker({ timepicker: false, format: 'm/d/Y' });
    jQuery('[name="EndDate"]').datetimepicker({ timepicker: false, format: 'm/d/Y' });

    google.charts.load('current', { 'packages': ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {

        let options = {
            title: 'Revenue for months',
            curveType: 'function',
            legend: { position: 'bottom' },
            hAxis: { title: "Month/Year" },
            vAxis: { title: "Revenue ($)" },
            height: 600,
            bar: { groupWidth: 20 }
        };

        let container = document.getElementById('curve-chart');

        if (container != null) {
            let chart = new google.visualization.ColumnChart(container);

            if (chartData.length < 2) return;

            chart.draw(google.visualization.arrayToDataTable(chartData), options);
        }
    }

    jQuery("#table-paginate").DataTable();
});