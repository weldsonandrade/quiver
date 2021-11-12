
// Carrega a tabela do bootgrid com os planos que podem ser criados
function loadGridAEditar() {
    $("#data-table-Aeditar").bootgrid({
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\"  onclick=\"editarPlanoAcao(" + row.id + ");\">Criar</button>" +
                       "<button class=\"btn btn-xs btn-danger waves-effect waves-float\" onclick=\"cancelarPlanoAcao(" + row.id + ");\">Cancelar</button>&nbsp;";
            }
        },
        labels: {
            noResults: "Não existem NÂO CONFORMIDADES necessitando de um plano de ação.",
            loading: "Carregando as não conformidades sem plano de ação.",
            search: "Pesquisar por não conformidade",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} não conformidades sem plano de ação"
        }
    });
}

// Carrega a tabela com planos de acoes em aberto
function loadGridPlanoAcao() {
    $(".table").bootgrid({
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"editarPlanoAcao(" + row.id + ");\">Editar</button>&nbsp;";
            }
        },
        labels: {
            noResults: "Não existem NÂO CONFORMIDADES necessitando de um plano de ação.",
            loading: "Carregando as não conformidades sem plano de ação.",
            search: "Pesquisar por não conformidade",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} não conformidades sem plano de ação"
        }
    });
}


