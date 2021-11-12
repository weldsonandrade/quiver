document.body.onload = function () {
    $('#menu_relatorios').addClass('active');
    $('#rel_evolutivo_por_unidade').addClass('active');
}

$(document).ready(function () {
    $('#texto-pre-loading')[0].innerHTML = "Carregando evolutivo por unidade!";
});

$(document).ajaxStop(function () {
    $('body').addClass('loaded');
});

$('#dataInicial').datetimepicker({
    defaultDate: moment().subtract(30, 'days'),
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$('#dataFinal').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br',
    defaultDate: moment()
});

var dataInicial = $('#dataInicial').val();
var dataFinal = $('#dataFinal').val();

function pesquisar() {
    dataInicial = $('#dataInicial').val();
    dataFinal = $('#dataFinal').val();

    $('#chart_card').load('/Relatorio/CardEvolutivoPorUnidade', { "dataInicial": $('#dataInicial').val(), "dataFinal": $('#dataFinal').val() });
}


function getDataSetGrafico() {
    var dataurl = '/Relatorio/GetEvolutivoPorUnidade';
    var displit = dataInicial.split("/");
    var di = new Date(displit[2], displit[1] - 1, displit[0]);
    var dfsplit = dataFinal.split("/");
    var df = new Date(dfsplit[2], dfsplit[1] - 1, dfsplit[0]);
    $.ajax({
        url: dataurl,
        data: { dataInicial: di.toISOString(), dataFinal: df.toISOString() },
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {
                $('#card-sem-grafico').css('display', 'none');
                $('#card-grafico').css('display', 'block');
                $.plot($("#line-chart"), data, options);
            } else {
                $('#card-grafico').css('display', 'none');  
                $('#card-sem-grafico').css('display', 'block');
            }
        }
    });

    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {
            if (item) {
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);
                var DATE_FORMAT = "%d/%m/%y";
                var d = new Date(x);
                $(".flot-tooltip").html(item.series.label + " obteve " + y + "%").css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
            }
            else {
                $(".flot-tooltip").hide();
            }
        });

      //  $("<div class='flot-tooltip' class='chart-tooltip'></div>").appendTo("body");
    }
}
// setup plot
var options = {
    series: {
        shadowSize: 0,
        lines: {
            show: true,
            lineWidth: 2,
        },
    },
    grid: {
        borderWidth: 0,
        labelMargin: 10,
        hoverable: true,
        clickable: true,
        mouseActiveRadius: 6,

    },
    legend: {
        show: true,
        margin: 10,
        backgroundOpacity: 0.9
    },
    points: {
        show: true,
        radius: 3
    },
    lines: {
        show: true
    },
    yaxis: {
        min: 0,
        max: 100,
        tickColor: '#eee',
        font: {
            lineHeight: 13,
            style: "normal",
            color: "#9f9f9f",
        },
        shadowSize: 0
    },
    xaxis: { mode: "time", timeformat: "%d/%m/%y", minTickSize: [1, "day"] }
    //xaxis: { ticks: [[1, "Jan"], [2, "Feb"], [3, "Mar"], [4, "Apr"], [5, "May"], [6, "Jun"], [7, "Jul"], [8, "Aug"], [9, "Sep"], [10, "Oct"], [11, "Nov"], [12, "Dec"]] }
};
function formatter(val, axis) {
    return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}



/* Tooltips for Flot Charts */


