document.body.onload = function () {
    var opcaoMenu = document.getElementById("menu_usuarios");
    opcaoMenu.className = opcaoMenu.className + "  active";
}

$(document).ready(function () {
    $('#usuario-input-confirmar-senha').bind('cut copy paste', function (event) {
        event.preventDefault();
    });
});

$(document).ajaxStop(function () {

});

function loadBootGrid() {
    console.log('loadBootGrid -> Start')
    $("#data-table-command").bootgrid({
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                var actionExcluir = "<button class=\"btn btn-xs btn-danger waves-effect waves-float\" onclick=\"excluirUsuario('" + row.id + "');\">Excluir</button>&nbsp;";
                var actionDetalhes = "<button class=\"btn btn-xs bgm-deeppurple waves-effect waves-float\" onclick=\"perfilUsuario('" + row.id + "');\">Detalhes</button>&nbsp;";
                if (row.Logado == 'True') {
                    return actionDetalhes;
                } else {
                    return actionExcluir + actionDetalhes;
                }
            }
        },
        labels: {
            noResults: "Não existem usuários cadastrados.",
            loading: "Carregando os usuários.",
            search: "Pesquisar usuário",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} usuário(s)"
        }
    });
    console.log('loadBootGrid -> Finish')
}

loadBootGrid();