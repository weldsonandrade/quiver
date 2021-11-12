document.body.onload = function () {
}

$(document).ready(function () {
    loadButtons();
    var opcaoMenu = document.getElementById("menu_unidade");
    opcaoMenu.className = opcaoMenu.className + "  active";
});

$(document).ajaxStop(function () {

});

// Carrega a tabela do bootgrid
function loadButtons() {
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
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"editar(" + row.id + ")\">Editar</button>" +
                       "<button class=\"btn btn-xs btn-danger waves-effect waves-float\" onclick=\"excluir(" + row.id + ");\">Excluir</button>&nbsp;";
                //       "<button class=\"btn btn-xs bgm-deeppurple waves-effect waves-float\" onclick=\"perfilUnidade('" + row.id + "');\">Detalhes</button>&nbsp;";
            }
        },
        labels: {
            noResults: "Não existem unidades cadastradas.",
            loading: "Carregando as unidades cadastradas.",
            search: "Pesquisar por unidade",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} unidade(s)"
        }
    });
}

function perfilUnidade(unidadeId) {
    location.href = "/Unidade/Perfil?unidadeId=" + unidadeId;
}

function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('Unidade/Pesquisar', { "termo": termo }, function () {
        loadButtons();
    });
}

function adicionar() {
    $('#Id').val('');
    resetarCampo('unidade-input-nome', '#unidade-nome', 'mensagem-erro-nome');
    $('#myModalLabel').text('Adicionar unidade');
    habilitarSalvar();
}

function editar(unidadeId) {
    waitingDialog.show('Carregando edição de unidade');
    limparCamposModal();
    $.getJSON("Unidade/Manipular?unidadeId=" + unidadeId, function (unidade) {
        $('#Id').val(unidade.Id);
        $('#unidade-input-nome').val(unidade.Nome);
        $('.fg-line ').addClass(" fg-toggled");

        $('#myUnidadeModal').modal('show');
        $('#myModalLabel').text('Editar unidade');
        habilitarSalvar();
    }).always(function () {
        waitingDialog.hide();
    });
}

function excluir(unidadeId) {
    swal({
        title: 'Você ter certeza?',
        text: 'Você irá perder todos os dados ligados a essa unidade!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, delete',
        cancelButtonText: 'Cancele',
        confirmButtonClass: 'confirm-class',
        cancelButtonClass: 'cancel-class',
        closeOnConfirm: true,
        closeOnCancel: true
    },
           function (isConfirm) {
               if (isConfirm) {
                   $.ajax({
                       type: 'POST',
                       url: '/Unidade/Excluir?unidadeID=' + unidadeId,
                       dataType: "json",
                       contentType: "application/json",
                       success: function () {
                           pesquisar();
                       }
                   });
               }
           });

}

function onSuccess(data) {
    if (data.ok) {
        $('#myUnidadeModal').modal('hide');
        pesquisar();
    }
    else if (data.choque) {
        $('#mensagem-erro-data-choque').css('display', '');
    }
}


function limparCamposModal() {
    $('#Id').val(null);
    $('#unidade-input-nome').val('');
    $('.fg-line ').removeClass(" fg-toggled");
}




//********************** VALIDACOES **********************************************


function exibirMsgErroNome() {

    var nome = document.getElementsByName("Nome")[0].value;
    var isValid = false;

    // exibe mensagem para o campo de nome
    if (nome.trim().length < 2) {
        document.getElementById("mensagem-erro-nome").style.display = "inline";
        isValid = false;
    }
    else {
        document.getElementById("mensagem-erro-nome").style.display = "none";
        isValid = true;
    }


    //habilita ou desabilita os botões de salvar
    if (isValid) {
        document.getElementById("salvar-unidade").disabled = false;
    }
    else {
        document.getElementById("salvar-unidade").disabled = true;
    }
}


function validarNome() {
    var resposta = validarCampoBranco('unidade-input-nome', '#unidade-nome', '#mensagem-erro-nome');
    habilitarSalvar();
    return resposta;
}


//********************** VALIDACOES **********************************************//

function habilitarSalvar() {
    descricao = validarCampoBranco('unidade-input-nome', null, null);

    if (descricao == false) {
        $('#salvar-unidade').prop("disabled", true);
        return false;
    }


    $('#salvar-unidade').prop("disabled", false);
    return true;
}

// Caso tenha algum modo onde o usuário conseguiu deixar o botão habilitado mesmo sendo algo inválido 
function validarTodosCampos() {

    if (validarNome() == true) {
        $('#form-unidade').valid();
    }
}



