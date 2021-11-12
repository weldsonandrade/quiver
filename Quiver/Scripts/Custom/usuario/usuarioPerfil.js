document.body.onload = function () {
    var opcaoMenu = document.getElementById("menu_usuarios");
    opcaoMenu.className = opcaoMenu.className + "  active";

}

$('#dataInicial').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$('#dataFinal').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$(document).ready(function () {
    filtrar();
});

$(document).ajaxStop(function () {

});


function filtrar() {
    // Aplicando o pre-loading 
    $('#tabela-avaliacoes').css('display', 'block');
    $('#tabela-avaliacoes').append(HTMLLoaderCard('Carregando tabela de inspeções.'));
    $('#grafico-body').append(HTMLLoaderCard('Carregando gráfico de eficiência.'));
    $('#grafico-body-qtd').append(HTMLLoaderCard('Carregando gráfico quantitativo.'));
  
    var dataInicial = $('#dataInicial').val();
    var dataFinal = $('#dataFinal').val();

    $('#tabela-avaliacoes').load('/Usuario/PerfilAvaliacoes', { "usuarioId": $('#Id').val(), "dataInicial": $('#dataInicial').val(), "dataFinal": $('#dataFinal').val() }, function () {
        if (parseInt($('#valor-total')[0].innerHTML) > 0) {
            loadBootGridInspecao();
            $('#tabela-sem-avaliacoes').css('display', 'none');
            $('#tabela-avaliacoes').css('display', 'block');

            getDataSetGraficoEvolutivoPerfil($('#Id').val(), dataInicial, dataFinal);
            getDataSetGraficoQuantitativoPerfil($('#Id').val(), dataInicial, dataFinal);
        } else {
            $('#tabela-sem-avaliacoes').css('display', 'block');
            $('#tabela-avaliacoes').css('display', 'none');
            $('#card-grafico').css('display', 'none');
            $('#card-grafico-qtd').css('display', 'none');
        }
    });
}

function pesquisar() {f
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Usuario/Tabela', { "termo": termo }, function () { loadBootGridInspecao(); });
}

function telaAvaliado(agendamentoId) {
    var NestId = $(this).data('id');
    var url = "/Agenda/AvaliacaoFinalizada?agendamentoId=" + agendamentoId;
    //  window.location.href = url;
    window.open(url, '_blank'); // abrindo outro tab
}

function fixarTamanhoPosPonto(value, tamanho) {
    if (value.toString().indexOf('.') > -1) {
        return value.toFixed(tamanho);
    }
    else return value;
}


/* GRÁFICO EVOLUTIVO*/
function getDataSetGraficoEvolutivoPerfil(usuarioId, dataInicial, dataFinal) {
 
    dataInicial = convertPadraoeData5(dataInicial);
    dataFinal = convertPadraoeData5(dataFinal);
   

    $.ajax({
        url: '/Usuario/GetEvolutivo',
        data: { usuarioId: usuarioId.toString(), dataInicial: dataInicial, dataFinal: dataFinal },
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data != 0) {
                $('#card-sem-grafico').css('display', 'none');
                $('#card-grafico').css('display', 'block');
                $("div.loader-card").remove();

                var plot = $.plot($("#line-chart-efetividade-perfil"), data, options); // Desenho o gráfico 
                // Mantem responsivo
                window.onresize = function (event) {
                    plot = $.plot($("#line-chart-efetividade-perfil"), data, options);
                    plot.draw();
                }

            } else {
                $('#card-grafico').css('display', 'none');
                $('#card-sem-grafico').css('display', 'block');

            }
        }
    });

    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {
            if (item) {
                // Defino as coordenadas do ponto X e Y
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);
                if(y > 0 ){
                // Efetividade
                var dataexecutada = " Data de execução: " + x + '</br>';
                var qtdAvaliacoesNoPonto = " Quantidade de inspeções: " + item.series.avaliacoesPonto[item.dataIndex][0].length + '</br>';

                if  (y.substr(y.indexOf('.')) == ".00")  {
                    y =  y.substr(0, y.indexOf('.'));
                }

              
                var efetividadeGeral = "Efetividade Geral: " + y + '% </br>';
                var dataexecutada = " Data executada: " + item.series.avaliacoesPonto[item.dataIndex][0][0].DataExecutada.substring(0, 10) + '</br>';

                var rotulo = "";
                var unidade = "";
                var avaliacao = "";
                if (item.series.avaliacoesPonto[item.dataIndex][0].length > 1) {
                    efetividadeGeral = "Efetividade Geral: " + y + '% </br>';

                    var avaliacoes = [];
                    var titulo = ""
                    var efetividade = "";
                    for (i = 0; i < item.series.avaliacoesPonto[item.dataIndex][0].length; i++) {
                        titulo = "<strong>" + (i + 1) + "º inspeção" + " </strong>";
                        unidade = "Unidade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].nomeUnidade;
                        if (item.series.avaliacoesPonto[item.dataIndex][0][i].efetividade == 0) {
                            efetividade = "Efetividade: Nenhum ponto."      
                        } else {
                            efetividade = "Efetividade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].efetividade + "%";
                        }

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
            } else{
                    $(".flot-tooltip").html("Nenhuma inspeção finalizada nessa data.");
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
        axisLabel: 'Datas das inspeções',
    }],
    yaxes: [{
        position: 'left',
        axisLabel: '(%) Efetividade',
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
        fillOpacity: 1.0,
        show: true,
        fill: true,
        lineWidth:1
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
};

function formatter(val, axis) {
    return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
/* FIM GRÁFICO EVOLUTIVO*/


/* GRÁFICO QUANTITATIVO*/
function getDataSetGraficoQuantitativoPerfil(usuarioId, dataInicial, dataFinal) {

    dataInicial = dataInicial;
    dataFinal = dataFinal;

    $.ajax({
        url: '/Usuario/GetQuantitativo',
        data: { usuarioId: usuarioId.toString(), dataInicial: dataInicial, dataFinal: dataFinal },
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data != 0) {
                $('#card-sem-grafico-qtd').css('display', 'none');
                $('#card-grafico-qtd').css('display', 'block');
                $("div.loader-card").remove();


                optionsQtd.yaxis.max = data[0].eixoY;

                var plot = $.plot($("#line-chart-quantidade-perfil"), data, optionsQtd); // Desenho o gráfico 
                // Mantem responsivo
                window.onresize = function (event) {
                    plot = $.plot($("#line-chart-quantidade-perfil"), data, optionsQtd);
                    plot.draw();
                }
            } else {
                $('#card-grafico-qtd').css('display', 'none');
                $('#card-sem-grafico-qtd').css('display', 'block');

            }
        }
    });

    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {

            if (item) {
                // Defino as coordenadas do ponto X e Y
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);
            }
        });

        $("<div class='flot-tooltip' class='chart-tooltip'></div>").appendTo("body");
    }
}



// setup plot quantitativo
var optionsQtd = {
    axisLabels: {
        show: true
    },
    xaxes: [{
        axisLabel: 'Datas das inspeções',
    }],
    yaxes: [{
        position: 'left',
        axisLabel: 'Quantidade',
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
        fillOpacity: 1.0,
        show: true,
        fill: true,
        lineWidth: 1
    },
    yaxis: {
        min: 0,
        max: 0,
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

