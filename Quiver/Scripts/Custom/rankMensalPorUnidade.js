document.body.onload = function () {
    $('#menu_relatorios').addClass('active');
    $('#rel_rank_por_unidade').addClass('active');
}

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

function pesquisar() {

 
    $('#tabela').load('/Relatorio/CardRankPorUnidade', { "dataInicial": $('#dataInicial').val(), "dataFinal": $('#dataFinal').val() });
}

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
            Efetividade: function (column, row) {
                if (row.Efetividade == "0.0") {
                    column.cssClass = "erro";
                    return 'Sem inspeções para calcular eficiencia.';
                }
                column.cssClass = "ativo";
                return row.Efetividade +"%";

            }

        },
        labels: {
            noResults: 'Nenhuma unidade foi encontrada.',
            search: 'Procurar por unidade',
            all: 'All',
            infos: 'Visualizando de {{ctx.start}} até {{ctx.end}} no total de {{ctx.total}} unidades',
            loading: 'Carregando Ranking de Unidades por efetividade...',
            refresh: 'Refresh',
        }
    });
}

loadBootGrid();