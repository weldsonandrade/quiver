document.body.onload = function () {
}

$(document).ready(function () {
    loadButtons();
    var opcaoMenu = document.getElementById("menu_empresas");
    opcaoMenu.className = opcaoMenu.className + "  active";
});

$(document).ajaxStop(function () {

});

function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('Empresa/Pesquisar', { "termo": termo }, function () {
        loadButtons();
    });
}

function adicionar() {
    resetarCampos();
    $('#myModalLabel').text('Adicionar empresa');
    habilitarSalvar();
}

function editar(empresaId) {
    waitingDialog.show('Carregando edição da empresa');
    resetarCampos();
    $.getJSON("/Empresa/Manipular?empresaId=" + empresaId, function (empresa) {
        $('#Id').val(empresa.Id);
        $('#empresa-input-cnpj').val(empresa.Cnpj);
        $('#empresa-input-nome').val(empresa.Nome);
        $('#empresa-input-situacao').prop('checked', empresa.Situacao);
        $('#empresa-input-limite-licencas').val(empresa.LimiteLicencas);
        $('.fg-line').addClass("fg-toggled");

        $('#myEmpresaModal').modal('show');
        $('#myModalLabel').text('Editar empresa');
        habilitarSalvar();
    }).always(function () {
        waitingDialog.hide();
    }).error(function (request, status, error) {
    });;
}

//********* Usuarios ************//

function adicionarUsuario(empresaId) { 
    // Limpar campos.
    $('#user-perfil').empty();
    resetarCampo('Nome', '#usuario-nome', '');
    resetarCampo('usuario-input-email', '#usuario-email', 'mensagem-erro-email');
    resetarCampo('usuario-input-senha', '#usuario-senha', 'mensagem-erro-senha');
    resetarCampo('usuario-input-confirmar-senha', '#usuario-confirmar-senha', 'mensagem-erro-confirmar-senha');
    getPerfis();
    $('#IdEmpresa').val(empresaId);
    $('#manipularUsuarioModal').modal('show');
}

function getPerfis() {
    $.getJSON('/Usuario/GetRoles', function (roles) {
        $.each(roles, function (index, optiondata) {
            $('#user-perfil').append("<option value='" + optiondata.Id + "'>" + optiondata.Nome + "</option>");
        });
    });
}

//*******************************//

function resetarCampos() {
    $('#Id').val('');
    resetarCampo('empresa-input-cnpj', '#empresa-cnpj', 'mensagem-erro-cnpj');
    resetarCampo('empresa-input-nome', '#empresa-nome', 'mensagem-erro-nome');
    resetarCampo('empresa-input-limite-licencas', '#empresa-limite-licencas', 'mensagem-erro-limite-licencas');
}

function onSuccess(data) {
    if (data.ok) {
        $('#myEmpresaModal').modal('hide');
        $('#manipularUsuarioModal').modal('hide');
        pesquisar();
    }
}

//********************** VALIDACOES **********************************************//

function exibirMsgErroNome() {
    var nome = $("#Nome").val();
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
        document.getElementById("salvar-empresa").disabled = false;
    }
    else {
        document.getElementById("salvar-empresa").disabled = true;
    }
}

function validarCnpj() {
    var resposta = validarCampoBranco('empresa-input-cnpj', '#empresa-cnpj', '#mensagem-erro-cnpj');
    habilitarSalvar();
    return resposta;
}

function validarNome() {
    var resposta = validarCampoBranco('empresa-input-nome', '#empresa-nome', '#mensagem-erro-nome');
    habilitarSalvar();
    return resposta;
}

//********************** VALIDACOES **********************************************//


function habilitarSalvar() {
    var cnpj = validarCampoBranco('empresa-input-cnpj', null, null);
    var nome = validarCampoBranco('empresa-input-nome', null, null);

    if (cnpj == false || nome == false) {
        $('#salvar-empresa').prop("disabled", true);
        return false;
    }

    $('#salvar-empresa').prop("disabled", false);
    return true;
}

// Caso tenha algum modo onde o usuário conseguiu deixar o botão habilitado mesmo sendo algo inválido 
function validarTodosCampos() {
    if (validarNome() == true) {
        $('#form-empresa').valid();
    }
}

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
                       "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"adicionarUsuario(" + row.id + ");\">Adicionar Usuário</button>&nbsp;";
            }
        },
        labels: {
            noResults: "Não existem empresas cadastradas no Quiver.",
            loading: "Carregando as empresas cadastradas.",
            search: "Pesquisar por empresas",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} empresa(s)"
        }
    });
}