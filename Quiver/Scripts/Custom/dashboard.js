document.body.onload = function () {
}

$(document).ready(function () {

    var dtfinal = moment().add(7000, 'year').format('DD/MM/YYYY');
    var dtInicial = moment().subtract(2000, 'year').format('DD/MM/YYYY');
    getDataSetGraficoEvolutivoPosicao(dtInicial, dtfinal);

    atualizarSumario();

    // Aplicando o pre-loading 
    $('#ranking-quantitativo-usuarios').append(HTMLLoaderCard('Carregando ranking'));
    $('#ranking-efetividade-usuarios').append(HTMLLoaderCard('Carregando ranking'));
    $('#lista-avaliacoes-atrasadas').append(HTMLLoaderCard('Carregando inspeções atrasadas'));
    $('#lista-avaliacoes-andamentos').append(HTMLLoaderCard('Carregando inspeções atrasadas'));
    $('#dashboard').css('visibility', 'visible');


    $('#ranking-quantitativo-usuarios').load('/Dashboard/rankingQuantidadeUsuarios', function () {
        loadBootGrid();
    });

    $('#ranking-efetividade-usuarios').load('/Dashboard/rankingEfetividadeUsuarios', function () {
        loadBootGrid();
    });

    carregarCalendario();


    $('#menu_agenda').removeClass('active');


});

$(document).ajaxStop(function () {

    tamanhoTabelasAvaliacoesAndamentoseAtrasadas();
});


function atualizarSumario() {

    $('#informativo-inicial').load('/Dashboard/getAvaliacoesDados', function () {
        if (parseInt($('#avaliacoes-atrasadas')[0].innerHTML) > 0) {
            $('#lista-avaliacoes-atrasadas').load('/Dashboard/AvaliacoesPorTipo', { "tipo": "Atrasadas" }, function () {
                loadAvaliacoesBootGrid();
            });
        }
        if (parseInt($('#avaliacoes-andamentos')[0].innerHTML) > 0) {
            $('#lista-avaliacoes-andamentos').load('/Dashboard/AvaliacoesPorTipo', { "tipo": "Andamentos" }, function () {
                loadAvaliacoesBootGrid();
            });
        }
        tamanhoTabelasAvaliacoesAndamentoseAtrasadas();
    });

}


function carregarCalendario() {
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    var myCalendar = $('#calendar'); //Change the name if you want. I'm also using thsi add button for more actions
    //Generate the Calendar
    myCalendar.fullCalendar({
        header: {
            right: '',
            center: 'prev, title, next',
            left: '',
            lang: 'pt-br'
        },
        theme: true, //Do not remove this as it ruin the design
        selectable: false,
        selectHelper: false,
        editable: false,
        droppable: false,
        dragRevertDuration: 0,

        ////Add Events
        events: function (start, end, timezone, callback) { filtrar(start, end, callback); },
        droppable: true, // Permite arrastar eventos no calendário
        editable: false
    });
}



function loadBootGrid() {

    $(".tabela-ranking-dashboard").bootgrid({
        templates: {
            header: "",
            footer: ""
        },
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "efetividade": function (column, row) {
                if (row.Efetividade == "0" || (row.Efetividade == "+Infinito")) {
                    column.cssClass = "erro";
                    return 'Sem valor';
                }
                column.cssClass = "ativo";
                return row.Efetividade + "%";
            },
            "efetividade2": function (column, row) {
                return row.Efetividade + "%";
            },
            "quantidade": function (column, row) {
                if (row.qtdAvaliacoes == "0") {
                    column.cssClass = "erro";
                    return row.qtdAvaliacoes;
                }
                column.cssClass = "ativo";
                return row.qtdAvaliacoes;
            }

        },
        labels: {
            noResults: 'Nenhuma informação foi encontrada.',
            search: 'Procurar',
            all: 'All',
            infos: 'Visualizando de {{ctx.start}} até {{ctx.end}} no total de {{ctx.total}}',
            loading: 'Carregando Ranking de Usuários...',
            refresh: 'Refresh',
        }
    });
}



function loadAvaliacoesBootGrid() {

    $(".tabela-avaliacoes-dashboard").bootgrid({
        templates: {
            header: "",
            footer: ""
        },
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"editarAvaliacao(" + row.id + ")\">Editar</button>&nbsp;";
            },
            "data": function (column, row) {

                return row.dataProgramada.substring(0, 8);
            },
            "Inspetor": function (column, row) {
                return retornarParteDoTexto(row.Inspetor, 10, row.id);
            }
        },
        labels: {
            noResults: 'Nenhuma informação foi encontrada.',
            search: 'Procurar',
            all: 'All',
            infos: 'Visualizando de {{ctx.start}} até {{ctx.end}} no total de {{ctx.total}}',
            loading: 'Carregando Inspeções...',
            refresh: 'Atualizando',
        }

    });
}

function editarAvaliacao(agendamentoId) {
    waitingDialog.show('Carregando edição da inspeção');

    $('#unidadeAvaliacao').empty();
    $('#grupoAvaliacao').empty();
    $('#usuarioAvaliacao').empty();

    $.getJSON("/Agenda/Manipular?agendamentoId=" + agendamentoId, function (avaliacao) {

        $('#Id').val(avaliacao.Id);
        $('#avaliaco-input-rotulo').val(avaliacao.RotuloCalendario);
        $("#avaliacao-rotulo > div.fg-line").addClass("fg-toggled");

        var dataProgramada = new Date(parseInt(avaliacao.DataProgramada.substr(6)));
        // TODO: Colocar a data em formato correto.
        $('#DataProgramada').val(moment(dataProgramada).format('DD/MM/YYYY'));

        getUnidadesAndGruposAndInspetores(avaliacao.IdUnidade, avaliacao.IdGrupo, avaliacao.IdUsuario);

        $('#myAgendamentoModal').modal('show');
        $('#myModalLabel').text('Editar inspeção');

    }).always(function () {
        waitingDialog.hide();
    });

 
}


// Do tamanho das tabelas de avaliado e andamento 
function tamanhoTabelasAvaliacoesAndamentoseAtrasadas() {
    var qtdAndamento = parseInt($('#avaliacoes-andamentos')[0].innerHTML);
    var qtdAtrasadas = parseInt($('#avaliacoes-atrasadas')[0].innerHTML);


    if ((qtdAtrasadas == 0) && (qtdAndamento == 0)) {
        $('#lista-andamentos').css('display', 'none');
        $('#lista-atrasadas').css('display', 'none');
    }
    else if ((qtdAtrasadas > 0) && (qtdAndamento == 0)) {
        $('#lista-andamentos').css('display', 'none');
        $('#lista-atrasadas').addClass("col-xs-12");
    } else if ((qtdAtrasadas == 0) && (qtdAndamento > 0)) {
        $('#lista-atrasadas').css('display', 'none');
        $('#lista-andamentos').addClass("col-xs-12");
    } else {
        $('#lista-andamentos').addClass("col-lg-6 col-md-12");
        $('#lista-atrasadas').addClass("col-lg-6 col-md-12");
    }
}


function getDataSetGraficoEvolutivoPosicao(dataInicial, dataFinal) {
    $.ajax({
        url: '/Dashboard/graficoQuantitativo',
        data: {
            dataInicial: dataInicial, dataFinal: dataFinal
        },
        type: 'post',
        dataType: 'json',
        success: function (data) {
            if (data != 0) {
                options.yaxis.max = data[0].eixoY + 1;
                somePlot = $.plot($("#line-chart-quantitativo"), data, options); // Desenho o gráfico

                if ($(window).width() > 992) {
                    $('#line-chart-quantitativo').css('height', $('#agenda-dashboard').height() - 63);
                }


                // Mantem responsivo
                window.onresize = function (event) {
                    somePlot = $.plot($("#line-chart-quantitativo"), data, options);
                }


            }
        }
    });

    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {
            if (item) {
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1];

                var DATE_FORMAT = "%d/%m/%y";
                if (event.currentTarget.id == "line-chart-quantitativo") {
                    var d = convertPadraoeData3(item.series.dataAvaliacao + "");
                    var htmlTexto = "<br>Via: <strong>" + item.series.label + ' </strong> <br/> Em: <strong>' + d + "</strong>";
                    if (y == 0) {
                        $(".flot-tooltip").html("Nenhuma inspeção finalizada").css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
                    } else if (y == 1) {
                        $(".flot-tooltip").html("<strong>" + y + "</strong> inspeção finalizada" + htmlTexto).css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
                    } else {
                        $(".flot-tooltip").html("<strong>" + y + "</strong> inspeções finalizadas" + htmlTexto).css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
                    }
                } else {
                    var titulo = item.series.data[item.dataIndex][2];
                    var d = new Date(x);
                    $(".flot-tooltip").html(titulo + " inspeções").css({ top: item.pageY + 5, left: item.pageX + 5 }).show();
                }

            }
            else {
                $(".flot-tooltip").hide();
            }
        });

        $("<div class='flot-tooltip' class='chart-tooltip'></div>").appendTo("body");
    }

}



// setup plot
var options = {
    axisLabels: {
        show: true
    },

    xaxes: [{
        axisLabel: 'Datas das inspeções finalizadas',
    }],
    yaxes: [{
        position: 'left',
        axisLabel: 'Quantidade de Inspeções finalizadas',
    }],

    series: {
        shadowSize: 0,
        lines: {
            show: true,
            lineWidth: 1,
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
        backgroundOpacity: 0.9,
    },
    points: {
        show: true,
        radius: 2
    },
    lines: {
        show: true
    },
    yaxis: {
        min: 0,
        max: 10,
        tickColor: '#eee',
        font: {
            lineHeight: 13,
            style: "normal",
            color: "#9f9f9f",
        },
        shadowSize: 0
    },
    xaxis: { mode: "time", timeformat: "%d/%m/%y", minTickSize: [1, "day"] }
};