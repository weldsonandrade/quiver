document.body.onload = function () {

}

$(document).ready(function () {
  

    $('#menu_relatorios').addClass('toggled active');
    $('#rel_evolutivo_geral').addClass('active');

    $("#DDLFilterUnidade").chosen().change(function () { });
    $("#DDLFilterUsuario").chosen().change(function () { });
    $("#DDLFilterGrupo").chosen().change(function () { });


    $("#DDLFilterEvolutivoUnidade").chosen().change(function () { });
    $("#DDLFilterEvolutivoUsuario").chosen().change(function () { });
    $("#DDLFilterEvolutivoGrupo").chosen().change(function () { });


    $('#card-filtros').css('visibility', 'visible');
    exibirExcecaoSelecionada();

    $("div.flot-tooltip").remove(); //[Armengue] Remove as 3 divs que não sei como aparecem com a classe flot-tooltip



});

$(document).ajaxStop(function () {
    $('body').addClass('loaded');
});


$("#select-categoria").change(function () {
    exibirExcecaoSelecionada();
    $("#card-filtros").slideUp(300).delay(100).slideDown(300);
});

$('#dataInicial').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$('#dataFinal').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br',
    useCurrent: false
});

linkarDataInicialDataFinal("#dataInicial", "#dataFinal");

function exibirExcecaoSelecionada() {
    exibirExcecaoSelecionadaRelatorio('gráfico', 'DDLFilterEvolutivoUsuario_chosen', 'DDLFilterEvolutivoUnidade_chosen', 'DDLFilterEvolutivoGrupo_chosen');
}

function filtrar() {
    $("#card-Evolutivo").css({ opacity: 1.0, visibility: "hidden" }).animate({ opacity: 0 }, 200);
    $("#btnFiltrar").prop("disabled", true);
    $("#icon-filtrar").css('display', 'inline-block');


    var tipoEvolutivo = "";
    var selected = $("input[type='radio'][name='tipoGrafico']:checked");
    if (selected.length > 0) {
        tipoEvolutivo = selected.val();
    }

    $('#tipoSelecionadoInput').val(tipoEvolutivo);

    var categoriaSelecionada = $('#select-categoria').selectpicker('val');


    // Filtros de inspeções no gráfico
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

    var conjuntoDeFiltrados = "0"; // São os elementos selecionados para filtro no card de categoria do Gráfico
    if (categoriaSelecionada == "Usuários") {
        if ($("#DDLFilterEvolutivoUsuario").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterEvolutivoUsuario").chosen().val().toString();
        }
        usuariosAvaliacaoID = "0";

    } else if (categoriaSelecionada == "Unidades") {
        if ($("#DDLFilterEvolutivoUnidade").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterEvolutivoUnidade").chosen().val().toString();
        }
        unidadesAvaliacaoID = "0";

    } else if (categoriaSelecionada == "Grupos") {
        if ($("#DDLFilterEvolutivoGrupo").chosen().val() != null) {
            conjuntoDeFiltrados = $("#DDLFilterEvolutivoGrupo").chosen().val().toString();
        }
        gruposAvaliacaoID = "0";
    }


    // Filtros da inspeção 
    var filtroConformidade;
    if ($('#select-conformidades').selectpicker('val') == "Todas") {
        filtroConformidade = 'todos';
    } else if ($('#select-conformidades').selectpicker('val') == "Apenas inspeções que possuem algum item não conforme") {
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
    $('#titulo-Evolutivo').html("Gráfico de <strong id='categoria-Evolutivo-ID'>" + categoriaSelecionada + "</strong> com inspeções finalizadas entre <strong>" + dtInicial + "</strong> e <strong>" + dtfinal + "</strong> <small id='sub-titulo-Evolutivo'>.</small>");

    if (($('#dataInicial').val().trim() == "") && ($('#dataFinal').val().trim() == "")) {
        dtfinal = moment().add(7000, 'year').format('DD/MM/YYYY');
        dtInicial = moment().subtract(2000, 'year').format('DD/MM/YYYY');
        // Escreve o titulo
        $('#titulo-Evolutivo').html("Gráfico de  <strong id='categoria-Evolutivo-ID'>" + categoriaSelecionada + "</strong> com inspeções finalizadas durante todos os tempos. <small id='sub-titulo-Evolutivo'>.</small> ");
    }
    else if (($('#dataInicial').val().trim() != "") && ($('#dataFinal').val().trim() == "")) {
        dtfinal = moment().add(7000, 'year').format('DD/MM/YYYY');
        $('#titulo-Evolutivo').html("Gráfico de  <strong id='categoria-Evolutivo-ID'>" + categoriaSelecionada + " </strong> com inspeções finalizadas que se iniciarem em: <strong>" + dtInicial + "</strong>  <small id='sub-titulo-Evolutivo'>.</small>");

    } else if (($('#dataInicial').val().trim() == "") && ($('#dataFinal').val().trim() != "")) {
        dtInicial = moment().subtract(2000, 'year').format('DD/MM/YYYY');
        $('#titulo-Evolutivo').html("Gráfico de  <strong id='categoria-Evolutivo-ID'>" + categoriaSelecionada + "</strong> com inspeções até: <strong>" + dtfinal + "</strong>  <small id='sub-titulo-Evolutivo'>.</small>");
    }

    // Deixo display none pois quero pegar esses valores no método de gerar o modal do evolutivo detalhado
    $('#sub-titulo-Ranking').html("<strong style='display:none' id='unidades-Filtro-ID'> " + unidadesAvaliacaoID +
        "</strong> <strong style='display:none' id='grupos-Filtro-ID'> " + gruposAvaliacaoID +
        "</strong> <strong style='display:none' id='usuarios-Filtro-ID'> " + usuariosAvaliacaoID +
        "</strong>  <strong id='conjunto-Selecionado-ID' style='display:none'>" + conjuntoDeFiltrados +
        "</strong>  <strong id='filtro-Conformidade-ID' style='display:none'>" + filtroConformidade +
        "</strong>  <strong id='filtro-Agendada-ID' style='display:none'>" + filtroAgendada +
        "</strong>  <strong id='dtInicial-Ranking-ID' style='display:none'>" + dtInicial +
        "</strong>  <strong id='dtFinal-Ranking-ID' style='display:none'>" + dtfinal + "</strong>");



    getDataSetGraficoEvolutivoPosicao(categoriaSelecionada, dtInicial, dtfinal, tipoEvolutivo, conjuntoDeFiltrados,
     filtroConformidade, filtroAgendada, unidadesAvaliacaoID, usuariosAvaliacaoID, gruposAvaliacaoID)


}




/* GRÁFICO EVOLUTIVO*/
function getDataSetGraficoEvolutivoPosicao(categoriaSelecionada, dataInicial, dataFinal,
    tipoEvolutivo, conjuntoDeFiltrados, filtroConformidade, filtroAgendada,
    unidadesAvaliacaoID, usuariosAvaliacaoID, gruposAvaliacaoID) {

    $.ajax({
        url: '/Relatorio/CardEvolutivoGeral',
        data: {
            categoriaSelecionada: categoriaSelecionada, dataInicial: dataInicial, dataFinal: dataFinal, conjuntoFiltrados: conjuntoDeFiltrados,
            filtroConformidade: filtroConformidade, filtroAgendada: filtroAgendada, tipoEvolutivo: tipoEvolutivo,
            unidadesAvaliacaoID: unidadesAvaliacaoID, usuariosAvaliacaoID: usuariosAvaliacaoID, gruposAvaliacaoID: gruposAvaliacaoID
        },
        type: 'post',
        dataType: 'json',
        success: function (data) {
            $("#card-Evolutivo").css('display', 'block');
            $("#card-Evolutivo").css({ opacity: 0, visibility: "visible" }).animate({ opacity: 1.0 }, 200);
            $("#btnFiltrar").prop("disabled", false);
            $("#icon-filtrar").css('display', 'none');

            if (data != 0) {

                $("").detach();
                $("#body-grafico").css('display', 'block');
                $("#body-grafico-vazio").css('display', 'none');
                options.yaxis.max = data[0].eixoY + 1;
                options.yaxes[0].axisLabel = getYaxisLabel(),


                somePlot = $.plot($("#line-chart-evolutivo"), data, options); // Desenho o gráfico

                // Mantem responsivo
                window.onresize = function (event) {
                    somePlot = $.plot($("#line-chart-evolutivo"), data, options);
                }

                gerarTogglingControls(data, categoriaSelecionada);
            } else {
                $("#body-grafico").css('display', 'none');
                $("#body-grafico-vazio").css('display', 'block');
            }
        }
    });


    if ($(".flot-chart")[0]) {
        $(".flot-chart").bind("plothover", function (event, pos, item) {
            if (item) {
                // Defino as coordenadas do ponto X e Y
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);
                if (y > 0) { 

                if (y.substr(y.indexOf('.')) == ".00") {
                    y = y.substr(0, y.indexOf('.'));
                }

                var dataexecutada = " Data de execução: " +  x + '</br>';
                var qtdAvaliacoesNoPonto = " Quantidade de Inspeções: " + item.series.avaliacoesPonto[item.dataIndex][0].length + '</br>';

                var efetividadeGeral;

                var efetividadeGeral = "Efetividade Geral: " + y+ '% </br>';
                // var DataExecutada = ConverteTimestampToData(x);
                var dataexecutada = " Data executada: " + convertPadraoeData3( item.series.avaliacoesPonto[item.dataIndex][0][0].DataExecutada.substring(0,8)) + '</br>';

                var rotulo = "";
                var unidade = "";
                var avaliacao = "";
                if (item.series.avaliacoesPonto[item.dataIndex][0].length > 1) {
                    efetividadeGeral = "Efetividade Geral: " + y + '% </br>';

                    var avaliacoes = [];
                    var titulo = ""
                    var efetividade = "";
                    for (i = 0; i < item.series.avaliacoesPonto[item.dataIndex][0].length; i++) {
                        titulo = "<strong>"+ (i + 1) + "º inspeção" + " </strong>";
                        rotulo = "Rotulo: " + item.series.avaliacoesPonto[item.dataIndex][0][i].rotulo;
                        unidade = "Unidade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].nomeUnidade;
                        efetividade = "Efetividade: " + item.series.avaliacoesPonto[item.dataIndex][0][i].efetividade +"%";
                     
                        avaliacao = titulo + "</br>" +rotulo + '</br>' + unidade + '</br>' + efetividade + '</br>';
                        avaliacoes.push(avaliacao);
                    }
                    $(".flot-tooltip").html(efetividadeGeral + dataexecutada + qtdAvaliacoesNoPonto + ' <hr class="m-t-5 m-b-5">' + avaliacoes);

                } else {

                    efetividadeGeral = "Efetividade: " + y + '% </br>';
                    rotulo = "Rotulo: " + item.series.avaliacoesPonto[item.dataIndex][0][0].rotulo;
                    unidade = "Unidade: " + item.series.avaliacoesPonto[item.dataIndex][0][0].nomeUnidade;
                    
                    if (item.series.avaliacoesPonto[item.dataIndex][0][0].rotulo != null) {
                        avaliacao = rotulo + '</br>' + unidade;
                    } else { avaliacao = unidade}

                    $(".flot-tooltip").html(efetividadeGeral + dataexecutada + qtdAvaliacoesNoPonto + avaliacao);

                }
            }else{
                $(".flot-tooltip").html("Nenhuma inspeção finalizada");

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
        axisLabel: 's',
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


somePlot = null;




function linhaOnOff(idx) {
    var someData = somePlot.getData();
    someData[idx].lines.show = !someData[idx].lines.show;
    someData[idx].points.show = !someData[idx].points.show;
    somePlot.setData(someData);
    somePlot.draw();

    var div = document.getElementById('helper' + idx);
    var cor = document.getElementById("helper"+idx).style.background;


    $('#helper' + idx)[0].className = $('#helper' + idx)[0].className.replace(/\bpseudoStyle.*?\b/g, '');
    if (someData[idx].lines.show == true) {
        div.pseudoStyle("before", "background", cor + " !important");
    } else {
        div.pseudoStyle("before", "background", "white !important");
    }

   

}

// Retorna a string que vai ficar no eixoY
function getYaxisLabel() {
    if ($('#tipoSelecionadoInput').val() == 'efetividade') {
        return '(%) ' + $('#tipoSelecionadoInput').val();
    }
    return $('#tipoSelecionadoInput').val();
}

function getMaxYaxis() {
    if ($('#tipoSelecionadoInput').val() == 'efetividade') {
        return 100;
    }
    return 10;s
}

function gerarTogglingControls(data, categoriaSelecionada) {
    //// <div id="opcoes-grafico-evolutivo" class="row m-b-20">
    //     <p class="col-md-12">Deixe as linhas visíveis ou invisíveis:</p>

    //     <div class="toggle-switch col-lg-3">
    //         <label for="ts1" class="ts-label">Toggle Swith Default</label>
    //         <input id="ts1" type="checkbox" hidden="hidden">
    //         <label for="ts1" class="ts-helper"></label>
    //     </div>
    //</div>


    $('#opcoes-grafico-evolutivo').empty();
    var divOpcoesEvolutivo = document.createElement("div");
    divOpcoesEvolutivo.setAttribute("class", "row m-b-20");

    var pTituloOpcoes = document.createElement("p");
    pTituloOpcoes.setAttribute("class", "col-md-12");
    pTituloOpcoes.innerHTML = "Controle a visibilidade das linhas de <strong style='text-transform: lowercase;'>" + categoriaSelecionada + "</strong> presentes no gráfico.";

    $('#opcoes-grafico-evolutivo').append(pTituloOpcoes);


    var toggle;
    var labelNomeSelecionado;
    var inputCheckBoxNome;
    var labelHelperNome;
    for (i = 0; i < data.length; i++) {

        toggle = document.createElement("div");
        toggle.setAttribute("class", "toggle-switch col-lg-3");
        toggle.setAttribute('id', i);
        toggle.setAttribute('onchange', "linhaOnOff(" + data[i].idx + ")");


        labelNomeSelecionado = document.createElement("label");
        labelNomeSelecionado.setAttribute('class', 'ts-label');
        labelNomeSelecionado.setAttribute('for', data[i].label);
        labelNomeSelecionado.textContent = data[i].label;

        inputCheckBoxNome = document.createElement("input");
        inputCheckBoxNome.setAttribute('type', 'checkbox');

        inputCheckBoxNome.setAttribute('hidden', 'hidden');
        inputCheckBoxNome.checked = true;
        inputCheckBoxNome.setAttribute('id', 'input' + data[i].idx);
        inputCheckBoxNome.setAttribute('id', data[i].label);

        labelHelperNome = document.createElement("label");
        labelHelperNome.setAttribute('class', 'ts-helper');
        labelHelperNome.setAttribute('id', 'helper' + data[i].idx);
        labelHelperNome.setAttribute('for', data[i].label);
        labelHelperNome.style.background = data[i].color;
        labelHelperNome.pseudoStyle("before", "background", data[i].color + " !important");

        toggle.appendChild(labelNomeSelecionado);
        toggle.appendChild(inputCheckBoxNome);
        toggle.appendChild(labelHelperNome);

        $('#opcoes-grafico-evolutivo').append(toggle);

        labelNomeSelecionado = null;
        inputCheckBoxNome = null;
        labelHelperNome = null;
        toggle = null;



    }



}


