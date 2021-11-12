window.onload = function () {

}

$(document).ready(function () {
    $("#DDLFilterUnidade").chosen().change(function () { });
    $("#DDLFilterUsuario").chosen().change(function () { });
    $("#DDLFilterGrupo").chosen().change(function () { });
});


$('#dataInicial').datetimepicker({ format: 'DD/MM/YYYY', locale: 'pt-br' });

$('#dataFinal').datetimepicker({ format: 'DD/MM/YYYY', locale: 'pt-br' });

$('.selectpicker').selectpicker({
});

$(document).ajaxStop(function () {

});

function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela-avaliacoes').load('/Avaliacoes/Tabela', { "termo": termo }, function () { loadBootGrid(); });
}

// Apenas pego os valores selecionados e os salvo em inputs invisíveis no card da tabela, usarei quando for atualizar a tabela depois de editar uma inspeção
function atualizarParametros() {
    // Filtros de data
    var dtInicial = $('#dataInicial').val();
    var dtFinal = $('#dataFinal').val();

    if (($('#dataInicial').val().trim() == "")) {
        dtInicial = moment().subtract(2000, 'year').format('DD/MM/YYYY');
    }
    if (($('#dataFinal').val().trim() == "")) {
        dtFinal = moment().add(7000, 'year').format('DD/MM/YYYY');
    }

    $('#filtroDataInicial').val(dtInicial);
    $('#filtroDataFinal').val(dtFinal);

    // Filtros de inspeções no ranking
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

    $('#filtroUnidades').val(unidadesAvaliacaoID);
    $('#filtroUsuarios').val(usuariosAvaliacaoID);
    $('#filtroGrupos').val(gruposAvaliacaoID);
}

function filtrar(atualizar) {
    if (atualizar == true) {
        atualizarParametros();
    }

    $("#card-lista-avaliacoes").css({ opacity: 1.0, visibility: "hidden" }).animate({ opacity: 0 }, 100);
    $("#btnFiltrar").prop("disabled", true);
    $('#card-lista-avaliacoes').css('display', 'block');
    $("#icon-filtrar").css('display', 'inline-block');

    // Como já tenho os valores atualizados nos inputs hiddens basta usa-los aqui.
    // Filtros de inspeções no ranking    
    var unidadesAvaliacaoID = $('#filtroUnidades').val();
    var usuariosAvaliacaoID = $('#filtroUsuarios').val();
    var gruposAvaliacaoID = $('#filtroGrupos').val();;

    // Filtros de data
    var dtInicial = $('#filtroDataInicial').val();
    var dtFinal = $('#filtroDataFinal').val();


    // Filtros da inspeção 
    var filtroConformidade = null;
    if ($('#select-conformidades').selectpicker('val') == "Apenas inspeções que possuem algum item não conforme") {
        filtroConformidade = false;
    } else if ($('#select-conformidades').selectpicker('val') == "Apenas inspeções que possuem todos os itens conformes") {
        filtroConformidade = true;
    }

    var filtroAgendada = null;
    if ($('#select-agendada').selectpicker('val') == "Apenas as agendadas previamente pelo gerente") {
        filtroAgendada = true;
    } else if ($('#select-agendada').selectpicker('val') == "Apenas as geradas pelo usuário no mobile") {
        filtroAgendada = false;
    }

    $('#avaliacoes-encontradas').load('/Avaliacao/GetAvaliacoes',        {
            "dataInicial": dtInicial, "dataFinal": dtFinal,
            "unidadesAvaliacaoID": unidadesAvaliacaoID, "usuariosAvaliacaoID": usuariosAvaliacaoID,
            "gruposAvaliacaoID": gruposAvaliacaoID, "filtroAgendada": filtroAgendada,
            "filtroConformidade": filtroConformidade
        },
        function () {
            setTimeout(function () {
                $("#card-lista-avaliacoes").css({ opacity: 0, visibility: "visible" }).animate({ opacity: 1.0 }, 100);
                $("#btnFiltrar").prop("disabled", false);
                $("#icon-filtrar").css('display', 'none');
                calcularPorcentagensDasAvaliacoes();
                loadBootGrid();
            }, 500);
        });
}

function loadBootGrid() {
    $("#data-table-command").bootgrid({
        rowCount: [50, 100, -1],
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                if (row.status == "AVALIADO") {
                    return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"telaAvaliado('" + row.id + "');\">Detalhes</button>&nbsp;";
                } else {
                    return "<button class=\"btn btn-xs btn-primary waves-effect waves-float\" onclick=\"editar('" + row.id + "');\">Editar</button>&nbsp;";
                }
            },
            "usuario": function (column, row) {
                var colunaWidth = $(".usuario-coluna")[0].clientWidth;
                return retornarParteDoTexto(row.usuario, colunaWidth / 7, row.id);
            },
            "programada": function (column, row) {
                if (row.status == "ATRASADA") {
                    return "<p style='color:#d33;margin-bottom: 0;'>" + row.programada + "</p>";
                } else if (row.status == "ANDAMENTO") {
                    return "<p style='color:#4285F4;margin-bottom: 0;'>" + row.programada + "</p>";
                }
                return "<p style='color:#1B5E20;margin-bottom: 0;'>" + row.programada + "</p>";
            },
            "executada": function (column, row) {
                var dataExecutada = row.executada;
                if (row.status == "ANDAMENTO") {
                    return "<div class='wrapper' style='color:#4285F4' onmouseover='tooltipOnHover(" + "avaliacao" + row.id + ");'>Não executada<div class='tooltip' id=" + "avaliacao" + row.id + " >Apenas inspeções finalizadas possuem data de execução.</div></div> ";
                } else if (row.status == "ATRASADA") {
                    return "<div class='wrapper' style='color:#d33' onmouseover='tooltipOnHover(" + "avaliacao" + row.id + ");'>Não executada<div class='tooltip' id=" + "avaliacao" + row.id + " >Apenas inspeções finalizadas possuem data de execução.</div></div> ";
                }
                if (row.executada == "01/01/01") {
                    row.executada = "Não executada"
                }
                return "<p style='color:#1B5E20;margin-bottom: 0;'>" + row.executada + "</p>";

            },
            "status": function (column, row) {

                if (row.status == "AVALIADO") {
                    return "<p style='color:#1B5E20;margin-bottom: 0;'>Finalizada</p>"
                }
                else {
                    if (row.status == "ANDAMENTO") {
                        return "<p style='color:#4285F4;margin-bottom: 0;'>Andamento</p>"
                    } else {
                        return "<p style='color:#d33;margin-bottom: 0;'>Atrasada</p>"
                    }
                }
            },
            "efetividade": function (column, row) {
                if ((row.status == "AVALIADO") && (row.maximo > 0)) {
                    var percentual = calcularPercentualRespondido(row.maximo, row.efetuado);
                    return '<p style="color:#1B5E20;margin-bottom: 0;"> <strong>' + percentual + '%  </strong></p>';
                } else if ((row.status == "AVALIADO") && (row.maximo <= 0)) {
                    return '<p style="color:#1B5E20;margin-bottom: 0;"> Sem pontuação.</p>';
                } else {
                    var id = "efetividade" + row.id;
                    if (row.status == "ANDAMENTO") {
                        return " ";
                    }
                    return "";
                }
            }
        },
        labels: {
            noResults: "Não existem inspeções veinculadas a esse usuário.",
            loading: "Carregando as inspeções desse usuário.",
            search: "Pesquisar por inspeções",
            infos: "Apresentados {{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} inspeções"
        }

    });
}

function editar(agendamentoId) {
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

function onSuccess(data) {
    if (data.ok) {
        $('#myAgendamentoModal').modal('hide');
        // Reatualizar a tela
        filtrar(false);
    }
    else if (data.choque) {
        $('#mensagem-erro-data-choque').css('display', '');
    }
}

function telaAvaliado(agendamentoId) {
    var NestId = $(this).data('id');
    var url = "/Agenda/AvaliacaoFinalizada?agendamentoId=" + agendamentoId;
    //  window.location.href = url;
    window.open(url, '_blank'); // abrindo outro tab
}

function calcularPorcentagensDasAvaliacoes() {
    var maximo = parseInt($('#valor-total')[0].innerHTML);

    var finalizadas = parseInt($('#valor-finalizadas')[0].childNodes[0].nodeValue);
    var atrasadas = parseInt($('#valor-atrasadas')[0].childNodes[0].nodeValue);
    var andamentos = parseInt($('#valor-andamento')[0].childNodes[0].nodeValue);
    var agendadas = parseInt($('#valor-agendadas')[0].childNodes[0].nodeValue);
    var naoAgendadas = parseInt($('#valor-nao-agendadas')[0].childNodes[0].nodeValue);

    // Calcula o percentual e caso tenha valores reais seta para que ele pegue apenas 1 caracter após o ponto
    var percentFinalizadas = fixarTamanhoPosPonto(((finalizadas * 100) / maximo), 1);
    var percentAtrasadas = fixarTamanhoPosPonto(((atrasadas * 100) / maximo), 1);
    var percentAndamentos = fixarTamanhoPosPonto(((andamentos * 100) / maximo), 1);
    var percentAgendadas = fixarTamanhoPosPonto(((agendadas * 100) / maximo), 1);
    var percentNaoAgendadas = fixarTamanhoPosPonto(((naoAgendadas * 100) / maximo), 1);


    $('#percentual-finalizadas')[0].textContent = "(" + percentFinalizadas + "%)";
    $('#percentual-atrasadas')[0].textContent = "(" + percentAtrasadas + "%)";
    $('#percentual-andamento')[0].textContent = "(" + percentAndamentos + "%)"
    $('#percentual-agendadas')[0].textContent = "(" + percentAgendadas + "%)"
    $('#percentual-nao-agendadas')[0].textContent = "(" + percentNaoAgendadas + "%)"
    return null;
}

function fixarTamanhoPosPonto(value, tamanho) {
    if (value.toString().indexOf('.') > -1) {
        return value.toFixed(tamanho);
    }
    else return value;
}
