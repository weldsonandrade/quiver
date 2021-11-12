var modalId = '#manipularUsuarioModal';
// Inputs
var id = $('#id');
var nome = $('#Nome');
var email = $('#user-email');
var perfil = $('#user-perfil');
var senha = $('#usuario-input-senha');
var confirmarSenha = $('#usuario-input-confirmar-senha');

function adicionar() {
    resetarCampo('Nome', '#usuario-nome', '');
    resetarCampo('usuario-input-email', '#usuario-email', 'mensagem-erro-email');
    $('#user-perfil').empty();
    resetarCampo('usuario-input-senha', '#usuario-senha', 'mensagem-erro-senha');
    resetarCampo('usuario-input-confirmar-senha', '#usuario-confirmar-senha', 'mensagem-erro-confirmar-senha');

    getPerfis();
    $('#myModalLabel').text('Adicionar usuário');
    habilitarSalvarUsuario();
}

function getPerfis() {
    $.getJSON("Usuario/GetRoles", function (roles) {
        $.each(roles, function (index, optiondata) {
            perfil.append("<option value='" + optiondata.Id + "'>" + optiondata.Nome + "</option>");
        });
    });
}

function perfilUsuario(usuarioId) {
    location.href = "/Usuario/Perfil?usuarioId=" + usuarioId;
}

function excluirUsuario(usuarioId) {
    swal({
        title: "Deseja realmente excluir esse usuário?",
        text: "Após a remoção o usuário perderá acesso ao sistema.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: '/Usuario/Remover?usuarioId=' + usuarioId,
            cache: false,
            type: 'get',
            complete: function (data) {
                pesquisar();
            }
        });
    });
}

function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Usuario/Pesquisar', { "termo": termo }, function () { loadBootGrid(); });
}

function onBeginManipularUsuario(data) {
    $("#icon-filtrar").css('display', 'inline-block');
    $('#btnSalvarUsuario').prop("disabled", true);
}

function onManipularUsuarioSuccess(data) {    
    console.log('onManipularUsuarioSuccess -> Start')
    if (data.ok) {
        console.log('onManipularUsuarioSuccess -> Sucess')
        $(modalId).modal('hide');
        pesquisar();
        swal({
            type: 'success',
            title: "Usuário cadastrado com sucesso, foi enviado um e-mail com as instruções para ativação da conta.",
            showCancelButton: false,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'ok',
            closeOnConfirm: true
        });
    } else {
        console.log('onManipularUsuarioSuccess -> Failure')
        $('#usuario-erro').addClass('has-error');
        $('#mensagem-erro').text(data.errorMessage);
        $('#mensagem-erro').css('visibility', 'visible');
        $(modalId).modal('show');
    }

    $("#btnSalvarUsuario").prop("disabled", false);
    $("#icon-filtrar").css('display', 'none');
    console.log('onManipularUsuarioSuccess -> Finish')
}

function limparCamposModal() {
    id.val(null);
    nome.val('');
    email.val('');
    email.prop('disabled', false);
    perfil.empty();
    senha.prop('hidden', false);
    confirmarSenha.prop('hidden', false);
}