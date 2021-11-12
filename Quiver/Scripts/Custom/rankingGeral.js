document.body.onload = function () {
  
}

$(document).ready(function () {
    

    $('#menu_relatorios').addClass('toggled active');
    $('#rel_rank_geral').addClass('active');

    $("#DDLFilterUnidade").chosen().change(function () { });
    $("#DDLFilterUsuario").chosen().change(function () { });
    $("#DDLFilterGrupo").chosen().change(function () { });


    $("#DDLFilterRankingUnidade").chosen().change(function () { });
    $("#DDLFilterRankingUsuario").chosen().change(function () { });
    $("#DDLFilterRankingGrupo").chosen().change(function () { });

    exibirexecaoSelecionada();
    $('#card-filtros').css('visibility', 'visible');
});

$(document).ajaxStop(function () {
   
});



$("#select-categoria").change(function () {
    exibirexecaoSelecionada();
    $("#card-filtros").slideUp(200).delay(100).slideDown(200);
});


function exibirexecaoSelecionada() {
    var categoriaSelecionada = $('#select-categoria').selectpicker('val');
    if (categoriaSelecionada == "Usuários") {
        $("#execao-usuario").css('display', 'block');
        $("#execao-unidade").css('display', 'none');
        $("#execao-grupo").css('display', 'none');
        $("#DDLFilterRankingUsuario_chosen").css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'none');
        $("#avaliacoes-com-grupos").css('display', 'block');
        $("#avaliacoes-com-unidades").css('display', 'block');
        $("#DDLFilterUnidade_chosen").css('width', '100%');
        $("#DDLFilterGrupo_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ranking de avaliações finalizadas por usuários com os parâmetros que achar necessário. ');
    }
    else if (categoriaSelecionada == "Unidades") {
        $("#execao-usuario").css('display', 'none');
        $("#execao-unidade").css('display', 'block');
        $("#execao-grupo").css('display', 'none');
        $("#DDLFilterRankingUnidade_chosen").css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'block');
        $("#avaliacoes-com-grupos").css('display', 'block');
        $("#avaliacoes-com-unidades").css('display', 'none');
        $("#DDLFilterUsuario_chosen").css('width', '100%');
        $("#DDLFilterGrupo_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ranking de avaliações finalizadas por unidades com os parâmetros que achar necessário. ');

    }
    else if (categoriaSelecionada == "Grupos") {
        $("#execao-usuario").css('display', 'none');
        $("#execao-unidade").css('display', 'none');
        $("#execao-grupo").css('display', 'block');
        $("#DDLFilterRankingGrupo_chosen").css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'block');
        $("#avaliacoes-com-grupos").css('display', 'none');
        $("#avaliacoes-com-unidades").css('display', 'block');
        $("#DDLFilterUsuario_chosen").css('width', '100%');
        $("#DDLFilterUnidade_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ranking de avaliações finalizadas por grupos com os parâmetros que achar necessário. ');
    }

}

$('#dataInicial').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$('#dataFinal').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br',
});

linkarDataInicialDataFinal("#dataInicial", "#dataFinal");

function loadBootGrid() {
    $("#data-table-command").bootgrid({
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "efetividade": function (column, row) {
                var efetividade = row.efetividade;
                if (efetividade == 0.0) {
                    column.cssClass = "erro";
                    return 'Sem avaliações finalizadas para calcular eficiência.';
                }
                column.cssClass = "ativo";
                
                if (efetividade.substr(efetividade.indexOf('.')) == ".00") {
                    return efetividade.substr(0, efetividade.indexOf('.'));
                }
                return efetividade + "%";
            },
            "efetividade2": function (column, row) {
                var efetividade = row.efetividade;
                if (efetividade == 0.0) {
                    return 'Sem avaliações finalizadas para calcular eficiência.';
                }
                if (efetividade.substr(efetividade.indexOf('.')) == ".00") {
                    return efetividade.substr(0, efetividade.indexOf('.'));
                }
                return efetividade + "%";
            },
            "quantidade":  function (column, row) {
                var qtd = row.qtdAvaliacoes;
                if (qtd == 0) {
                    column.cssClass = "erro";
                    return '0';
                }
                column.cssClass = "ativo";
                return qtd;
            },
            "opcoes": function (column, row) {
                if (efetividade != 0.0) {
                    return "<button class='btn btn-xs btn-success waves-effect waves-float' onclick=\"detalhesPosicao('" + row.id + "', '" + row.posicao + "', '" + row.qtdAvaliacoes + "', '" + row.efetividade + "', '" + row.tipoRanking + "')\"> Detalhes</button>"
                }
                return '';
            }

        },
        labels: {
            noResults: 'Nenhuma informação foi encontrada.',
            search: 'Procurar',
            all: 'All',
            infos: 'Visualizando de {{ctx.start}} até {{ctx.end}} no total de {{ctx.total}}',
            loading: 'Carregando Ranking...',
            refresh: 'Refresh',
        }

    });
}


function filtrar() {
    $("#card-Ranking").css({ opacity: 1.0, display: "none" }).animate({ opacity: 0 }, 200);
    $("#btnFiltrar").prop("disabled", true);
    $("#icon-filtrar").css('display', 'inline-block');

    
    var tipoRanking = "";
    var selected = $("input[type='radio'][name='tipoRanking']:checked");
    if (selected.length > 0) {
        tipoRanking = selected.val();
    }

    var categoriaSelecionada = $('#select-categoria').selectpicker('val');


    // Filtros de avaliações no ranking
    var unidadesAvaliacaoID = "0";
    var usuariosAvaliacaoID = "0";
    var gruposAvaliacaoID = "0";


    if ($("#DDLFilterUnidade").chosen().val() != null) {
        unidadesAvaliacaoID = $("#DDLFilterUnidade").chosen().val().toString();

    }
    if ($("#DDLFilterUsuario").chosen().val() != null) {
        usuariosAvaliacaoID = $("#DDLFilterUsuario").chosen().val().toString();
    }
    if ($("#DDLFilterGrupo").chosen().val() != null) {
        gruposAvaliacaoID = $("#DDLFilterGrupo").chosen().val().toString();

    }


    var conjuntoDeFiltrados = "0"; // São os elementos selecionados para filtro no card de categoria do Raking
    if (categoriaSelecionada == "Usuários") {
        if ($("#DDLFilterRankingUsuario").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterRankingUsuario").chosen().val().toString();
        }
        usuariosAvaliacaoID = "0";

    } else if (categoriaSelecionada == "Unidades") {
        if ($("#DDLFilterRankingUnidade").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterRankingUnidade").chosen().val().toString();
        }
        unidadesAvaliacaoID = "0";

    } else if (categoriaSelecionada == "Grupos") {
        if ($("#DDLFilterRankingGrupo").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterRankingGrupo").chosen().val().toString();
        }
        gruposAvaliacaoID = "0";
    }




    // Filtros da avaliação 
    var filtroConformidade;
    if ($('#select-conformidades').selectpicker('val') == "Todas") {
        filtroConformidade = 'todos';
    } else if ($('#select-conformidades').selectpicker('val') == "Apenas avaliações que possuem algum item não conforme") {
        filtroConformidade = 'NaoConformes';
    } else {
        filtroConformidade = 'SomenteConformes';
    }


    var filtroAgendada;
    if ($('#select-agendada').selectpicker('val') == "Todas") {
        filtroAgendada = 'todos';
    } else if ($('#select-agendada').selectpicker('val') == "Apenas as agendadas previamente pelo gerente") {
        filtroAgendada = 'Agendada';
    } else {
        filtroAgendada = 'Mobile';
    }


    // Filtros de data
    var dtInicial = $('#dataInicial').val();
    var dtfinal = $('#dataFinal').val();
    $('#titulo-Ranking').html("Ranking de <strong id='categoria-Ranking-ID'>" + categoriaSelecionada + "</strong>  por  <strong id='tipo-Ranking-ID'>" + tipoRanking + "</strong> das avaliações finalizadas entre <strong>" + dtInicial + "</strong> e <strong>" + dtfinal + "</strong> <small id='sub-titulo-Ranking'>.</small>");

    if (($('#dataInicial').val().trim() == "") && ($('#dataFinal').val().trim() == "")) {
        dtfinal = moment().add(7000, 'year').format('DD/MM/YYYY');
        dtInicial = moment().subtract(500, 'year').format('DD/MM/YYYY');
        // Escreve o titulo
        $('#titulo-Ranking').html("Ranking de  <strong id='categoria-Ranking-ID'>" + categoriaSelecionada + "</strong> por <strong id='tipo-Ranking-ID'>" + tipoRanking + "</strong> das avaliações finalizadas durante todos os tempos. <small id='sub-titulo-Ranking'>.</small> ");
    }
    else if (($('#dataInicial').val().trim() != "") && ($('#dataFinal').val().trim() == "")) {
        dtfinal = moment().add(7000, 'year').format('DD/MM/YYYY');
        $('#titulo-Ranking').html("Ranking de  <strong id='categoria-Ranking-ID'>" + categoriaSelecionada + "</strong> por <strong id='tipo-Ranking-ID'>" + tipoRanking + "</strong> das avaliações finalizadas que se iniciarem em: <strong>" + dtInicial + "</strong>  <small id='sub-titulo-Ranking'>.</small>");

    } else if (($('#dataInicial').val().trim() == "") && ($('#dataFinal').val().trim() != "")) {
        dtInicial = moment().subtract(500, 'year').format('DD/MM/YYYY');
        $('#titulo-Ranking').html("Ranking de  <strong id='categoria-Ranking-ID'>" + categoriaSelecionada + "</strong> por <strong id='tipo-Ranking-ID'>" + tipoRanking + "</strong> das avaliações até: <strong>" + dtfinal + "</strong>  <small id='sub-titulo-Ranking'>.</small>");
    }

    // Deixo display none pois quero pegar esses valores no método de gerar o modal do ranking detalhado
    $('#sub-titulo-Ranking').html("<strong style='display:none' id='unidades-Filtro-ID'> " + unidadesAvaliacaoID +
        "</strong> <strong style='display:none' id='grupos-Filtro-ID'> " + gruposAvaliacaoID +
        "</strong> <strong style='display:none' id='usuarios-Filtro-ID'> " + usuariosAvaliacaoID +
        "</strong>  <strong id='conjunto-Selecionado-ID' style='display:none'>" + conjuntoDeFiltrados +
        "</strong>  <strong id='filtro-Conformidade-ID' style='display:none'>" + filtroConformidade +
        "</strong>  <strong id='filtro-Agendada-ID' style='display:none'>" + filtroAgendada +
        "</strong>  <strong id='dtInicial-Ranking-ID' style='display:none'>" + dtInicial +
        "</strong>  <strong id='dtFinal-Ranking-ID' style='display:none'>" + dtfinal +"</strong>");



    $('#body-ranking').load('/Relatorio/CardRankingGeral', {
        "dataInicial": dtInicial, "dataFinal": dtfinal,
        "categoriaSelecionada": categoriaSelecionada, "tipoRanking": tipoRanking, "conjuntoFiltrados": conjuntoDeFiltrados,
        "filtroConformidade": filtroConformidade, "filtroAgendada": filtroAgendada,
        "unidadesAvaliacaoID": unidadesAvaliacaoID, "usuariosAvaliacaoID": usuariosAvaliacaoID, "gruposAvaliacaoID": gruposAvaliacaoID
    },
        function () {
            loadBootGrid();
           $("#card-Ranking").css({ opacity: 0, display: "block" }).animate({ opacity: 1.0 }, 200);

            $("#btnFiltrar").prop("disabled", false);
            $("#icon-filtrar").css('display', 'none');
        });
}

function detalhesPosicao(id, posicao, qtdAvaliacoes, efetividade, infoDaPosicao) {

    var categoriaSelecionada = $('#categoria-Ranking-ID')[0].textContent.trim();

    waitingDialog.show('Carregando detalhes da ' + posicao + ' posição');

    var dtInicial = $('#dtInicial-Ranking-ID')[0].textContent.trim();
    var dtFinal = $('#dtFinal-Ranking-ID')[0].textContent.trim();

    var filtroConformidade = $('#filtro-Conformidade-ID')[0].textContent.trim();
    var filtroAgendada = $('#filtro-Agendada-ID')[0].textContent.trim();

    var unidadesAvaliacaoID = $('#unidades-Filtro-ID')[0].textContent.trim();
    var usuariosAvaliacaoID = $('#usuarios-Filtro-ID')[0].textContent.trim();
    var gruposAvaliacaoID = $('#grupos-Filtro-ID')[0].textContent.trim();
    var conjuntoDeFiltrados = $('#conjunto-Selecionado-ID')[0].textContent.trim();

    $.ajax({
        url: "/Relatorio/ModalDetalhesPosicao",
        type: "POST",
        data: {
            "dataInicial": dtInicial, "dataFinal": dtFinal,
            "categoriaSelecionada": categoriaSelecionada, "idSelecionado": id, "conjuntoFiltrados": conjuntoDeFiltrados,
            "filtroConformidade": filtroConformidade, "filtroAgendada": filtroAgendada,
            "unidadesAvaliacaoID": unidadesAvaliacaoID, "usuariosAvaliacaoID": usuariosAvaliacaoID, "gruposAvaliacaoID": gruposAvaliacaoID
        },
        cache: false,
        async: true,
        success: function (data) {
            document.getElementById("modal").innerHTML = data;
            waitingDialog.hide();
            $('#myDetalhesRankignModal').modal('show');
           enableScroll("myDetalhesRankignModal");
            $('#valor-total').html(qtdAvaliacoes +" <span class='badge'>(100%)</span>");

            $('#valor-efetividade').html(efetividade +"%");

            var agendadas = $('#valor-agendadas')[0].childNodes[0].nodeValue.trim();
            var naoAgendadas = qtdAvaliacoes - agendadas;
            $('#valor-nao-agendadas').html(naoAgendadas + " <span class='badge' id='percentual-agendadas'>(" + calcularPercentualRespondido(qtdAvaliacoes, naoAgendadas) + "%)</span>");
            // Agendadas é diferente pois já tenho o valor vindo do controller
            $('#percentual-agendadas').text("(" + calcularPercentualRespondido(qtdAvaliacoes, agendadas) + "%)");

            var naoConformes = $('#valor-nao-conformes')[0].childNodes[0].nodeValue.trim();
            var conformes = qtdAvaliacoes - naoConformes;
            $('#valor-conformes').html(conformes + " <span class='badge' id='percentual-conformes'>(" + calcularPercentualRespondido(qtdAvaliacoes, conformes) + "%)</span>");
            // Não conformes é diferente pois já tenho o valor vindo do controller
            $('#percentual-nao-conformes').text("(" + calcularPercentualRespondido(qtdAvaliacoes, naoConformes) + "%)");

            $('#myModaLabel').html("Detalhes de <strong style='color:#250ee6'>" + infoDaPosicao + "</strong> na <strong style='color:#250ee6'>" + posicao + "</strong> posição do ranking.");

            getDataSetGraficoEvolutivoPosicao(categoriaSelecionada, id, dtInicial, dtFinal);
            getDataSetGraficoQuantidadePosicao(categoriaSelecionada, id, dtInicial, dtFinal);

        },
        error: function () {
            alert('Ocorreu um erro, tente novamente.');
        }
    });
}


function calcularPercentualRespondido(maximo, efetuado) {
    var percentual = (efetuado * 100) / maximo;
    if (isNaN(percentual)) {
        return 0;
    }
    return percentual.toFixed(1);
}




/* GRÁFICO EVOLUTIVO*/
function getDataSetGraficoEvolutivoPosicao(categoriaSelecionada , escolhidoId, dataInicial, dataFinal) {
    $.ajax({
        url: '/Relatorio/GraficoModalDetalhes',
        data: {
            categoriaSelecionada: categoriaSelecionada, tipo: "efetividade", Id: escolhidoId.toString(),
            dataInicial: dataInicial, dataFinal: dataFinal
        },
        type: 'post',
        dataType: 'json',
        success: function (data) {
            if (data != 0) {
                $("#nao-grafico-ranking").css('display', 'none');
                $("#grafico-quantidade").css('display', 'block');
                $("#grafico-efetividade").css('display', 'block');
                $("#avaliacoes-dados").css('display', 'block');
                var plot = $.plot($("#line-chart-efetividade-ranking"), data, options); // Desenho o gráfico

                   // Mantem responsivo
                window.onresize = function (event) {
                    plot = $.plot($("#line-chart-efetividade-ranking"), data, optionsQuantidade);
                    plot.draw();
                }

            } else {
                $("#nao-grafico-ranking").css('display', 'block');
                $("#grafico-quantidade").css('display', 'none');
                $("#grafico-efetividade").css('display', 'none');
                $("#avaliacoes-dados").css('display', 'none');
            }
        }
    });

    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {
            if (item) {
                // Defino as coordenadas do ponto X e Y
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                // Efetividade
                var dataexecutada = " Data de execução: " + x + '</br>';
                var qtdAvaliacoesNoPonto = " Quantidade de Avaliações: " + item.series.avaliacoesPonto[item.dataIndex][0].length + '</br>';

                if (y.substr(y.indexOf('.')) == ".00") {
                    y = y.substr(0, y.indexOf('.'));
                }

                var efetividadeGeral = "Efetividade Geral: " + y + '% </br>';
              //  var dataexecutada = " Data executada: " + convertPadraoeData3(item.series.avaliacoesPonto[item.dataIndex][0][0].DataExecutada.substring(0, 8)) + '</br>';

                var rotulo = "";
                var unidade = "";
                var avaliacao = "";
                if (item.series.avaliacoesPonto[item.dataIndex][0].length > 1) {
                    efetividadeGeral = "Efetividade Geral: " + y + '% </br>';

                    var avaliacoes = [];
                    var titulo = ""
                    var efetividade = "";
                    for (i = 0; i < item.series.avaliacoesPonto[item.dataIndex][0].length; i++) {
                        titulo = "<strong>" + (i + 1) + "º avaliação" + " </strong>";
                        unidade = "Unidade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].nomeUnidade;
                        efetividade = "Efetividade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].efetividade + "%";


                        if (item.series.avaliacoesPonto[item.dataIndex][0][i].rotulo != null) {
                            rotulo = "Rotulo: " + item.series.avaliacoesPonto[item.dataIndex][0][i].rotulo;
                            avaliacao = titulo + "</br>" + rotulo + '</br>' + unidade + '</br>' + efetividade + '</br>';
                        } else {
                            avaliacao = titulo + "</br>" + unidade + '</br>' + efetividade + '</br>';
                        }

                        avaliacoes.push(avaliacao);
                    }
                    $(".flot-tooltip").html(efetividadeGeral + dataexecutada + qtdAvaliacoesNoPonto + ' <hr class="m-t-5 m-b-5">' + avaliacoes);
                    
                } else {
                    efetividadeGeral = "Efetividade: " + y + '% </br>';
                    unidade = "Unidade: " + item.series.avaliacoesPonto[item.dataIndex][0][0].nomeUnidade;

                    if (item.series.avaliacoesPonto[item.dataIndex][0][0].rotulo != null) {
                        rotulo = "Rotulo: " + item.series.avaliacoesPonto[item.dataIndex][0][0].rotulo;
                        avaliacao = rotulo + '</br>' + unidade;
                    } else {
                        avaliacao = unidade;
                    }

                    $(".flot-tooltip").html(efetividadeGeral + dataexecutada + qtdAvaliacoesNoPonto + avaliacao);
        
                } 
            }
            else {
                $(".flot-tooltip").hide();
            }
        });

        $("<div class='flot-tooltip' class='chart-tooltip'></div>").appendTo(".modal-body");
        var text = 5;
    }
}


// setup plot
var options = {
    axisLabels: { show: true },
    xaxes: [{ axisLabel: 'Datas das avaliações',}],
    yaxes: [{ position: 'left', axisLabel: '(%) Efetividade', }],
    series: { shadowSize: 0,
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
        radius: 4
    },
    lines: {
        fillOpacity: 1.0,
        show: true,
        fill: true,
        lineWidth: 1
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



/* GRÁFICO QUANTITATIVO AVALIACOES*/
function getDataSetGraficoQuantidadePosicao(categoriaSelecionada, escolhidoId, dataInicial, dataFinal) {
    $.ajax({
        url: '/Relatorio/GraficoModalDetalhes',
        data: {
            categoriaSelecionada: categoriaSelecionada, tipo: "quantidade", Id: escolhidoId.toString(),
            dataInicial: dataInicial, dataFinal: dataFinal
        },
        type: 'post',
        dataType: 'json',
        success: function (data) {
            if (data != 0) {
                var plot = $.plot($("#line-chart-quantidade-ranking"), data, optionsQuantidade); // Desenho o gráfico
                // Mantem responsivo
                window.onresize = function (event) {
                    plot = $.plot($("#line-chart-quantidade-ranking"), data, optionsQuantidade);
                    plot.draw();
                }
            }
        }
    });


}


// setup plot
var optionsQuantidade = {
    axisLabels: {
        show: true
    },
    xaxes: [{
        axisLabel: 'Datas de fianlização das avaliações',
    }],
    yaxes: [{
        position: 'left',
        axisLabel: 'Quantidade de Avaliações',
    }],

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
        radius: 4
    },
    lines: {
        show: true
    },
    yaxis: {
        min: 0,
        max: $('#alturaEixoY').val(),
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
