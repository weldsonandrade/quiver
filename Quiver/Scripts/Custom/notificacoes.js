$(document).ready(function () {

});

document.body.onload = function () {

}

$(document).ajaxStop(function () {
    $('body').addClass('loaded');
});


function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Header/TabelaNotificacoes', { "termo": termo }, function () { loadBootGrid(); });
}



function loadBootGrid() {
    $("#data-table-command").bootgrid({
        rowCount: [ 50, 100, -1],
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "data": function (column, row) {
                return convertPadraoeData3(row.data);
            },
            "avaliacao": function (column, row) {
                var dados = row.avaliacao;
                var nome = dados.substr(0, dados.indexOf('/'));
                dados = dados.substr(nome.length + 1); // Agora dados tem apenas o ID da inspeção
                if (row.status == "AVALIADO")
                {
                    return '<a target="_blank" href="/Agenda/AvaliacaoFinalizada?agendamentoId=' + dados + '" >' + nome + '</a> '
                }
                return nome;
                },
            "status": function (column, row) {
                if (row.status == "AVALIADO") {
                    return "<p style='color:#1B5E20;margin-bottom: 0;'>Finalizada</p>"
                }
                else if (row.status == "NAO_AVALIADO") {
                    return "<p style='color:#d33;margin-bottom: 0;'>Atrasada</p>"
                }
            },
            "observacoes": function (column, row) {
      
                if (row.observacoes == "True False") // Agendada e Não Conforme
                {
                    return "<i class='zmdi zmdi-alert-triangle  mdc-text-red' style='display:inline;font-size: 17px;'>";
                }
                else if (row.observacoes == "False True") // Não agendada e Conforme
                {
                    return "<i class='zmdi zmdi-smartphone-iphone' style='display:inline;font-size: 17px;color:#1B5E20'>";
                }
                else if (row.observacoes == "False False") // Não agendada e Não Conforme
                {
                    return "<i class='zmdi zmdi-smartphone-iphone' style='display:inline;font-size: 17px;color:#1B5E20'> <i class='zmdi zmdi-alert-triangle  mdc-text-red wrapper' style='display:inline;font-size: 17px;'> "
                }
                return 'Nenhuma';
            }
        },
        labels: {
            noResults: "Não existem notificações.",
            loading: "Carregando as notificações.",
            search: "Pesquisar por notificações",
            infos: "Entre {{ctx.start}} até {{ctx.end}} de {{ctx.total}} notificações geradas"
        }
    });
}

loadBootGrid();