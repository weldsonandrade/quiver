$(document).ready(function () {  
    var opcaoMenu = document.getElementById("menu_plano");
    opcaoMenu.className = opcaoMenu.className + "  active";

    var currentdate = new Date();    
    $('#dataFinal').val(currentdate.toLocaleDateString());
    currentdate.setMonth(currentdate.getMonth() - 1);
    $('#dataInicial').val(currentdate.toLocaleDateString());

    $("#DDLFilterUnidade").chosen().change(function () { });
    $("#DDLFilterUsuario").chosen().change(function () { });

    loadGrids();
});

function filtrar() {
    $("#btnFiltrar").prop("disabled", true);
    $('#card-planos-acao').css('display', 'none');
    $("#icon-filtrar").css('display', 'inline-block');

    var emailResponsavel = $('#plano-input-email').val();
    var unidadesIds = $('#DDLFilterUnidade').chosen().val();
    if (unidadesIds != null) {
        unidadesIds = unidadesIds.toString();
    }
    var usuariosIds = $('#DDLFilterUsuario').chosen().val();
    if (usuariosIds != null) {
        usuariosIds = usuariosIds.toString();
    }

    $('#tabelas-plano-acoes').load('/PlanoAcao/Filtrar', {
        "emailResponsavel": emailResponsavel,
        "IdUnidades": unidadesIds,
        "IdUsuarios": usuariosIds
    }, function () {
        setTimeout(function () {
            $("#card-planos-acao").css('display', 'visible');
            $("#btnFiltrar").prop("disabled", false);
            $("#icon-filtrar").css('display', 'none');
            loadGrids();
        }, 500);
    });
}

function editarPlanoAcao(idPlanoAcao) {
    location.href = "/PlanoAcao/Manipular?IdPlanoAcao=" + idPlanoAcao;
}

function cancelarPlanoAcao(idPlanoAcao) {
    $.getJSON("/PlanoAcao/Cancelar?idPlanoAcao=" + idPlanoAcao, function (data) {
        if (data.ok) {
            var rows = [];
            rows[0] = idPlanoAcao;
            $("#data-table-Aeditar").bootgrid('remove', rows);
        }
    }).always(function () { });
}

function loadGrids() {
    loadGridAEditar();
    loadGridPlanoAcao();
}

