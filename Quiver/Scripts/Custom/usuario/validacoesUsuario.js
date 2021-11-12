/* Validações */

function validarEmailUsuario() {
    var resposta = validarEmail('usuario-input-email', '#usuario-email', '#mensagem-erro-email');
    habilitarSalvarUsuario();
    return resposta;
}

function validarPerfilUsuario() {
    var resposta = validarDropdowSelected('user-perfil', '#usuario-perfil', '#mensagem-erro-perfil');
    habilitarSalvarUsuario();
    return resposta;
}


function validarSenhaUsuario() {
    //var senha = validarTamanhoMinimo('usuario-input-senha', '#usuario-senha', '#mensagem-erro-senha', 8);
    var senha = validarSenha('usuario-input-senha', '#usuario-senha', '#mensagem-erro-senha');

    if (senha == true) {
        // Pois caso ele edite a senha o confirmar senha tem que verificar se eles estão iguais
        compararDoisItens('usuario-input-confirmar-senha', '#usuario-confirmar-senha', '#mensagem-erro-confirmar-senha', 'usuario-input-senha');
    }

    habilitarSalvarUsuario();
    return senha;
}

function validarPerfil() {
    // TODO: Implementação
}

function validarConfirmarSenhaUsuario() {
    var resposta = compararDoisItens('usuario-input-confirmar-senha', '#usuario-confirmar-senha', '#mensagem-erro-confirmar-senha', 'usuario-input-senha');
    habilitarSalvarUsuario();
    return resposta;
}

// Caso tenha algum modo onde o usuário conseguiu deixar o botão habilitado mesmo sendo algo inválido 
function validarTodosCamposUsuario() {
    console.log('validarTodosCamposUsuario -> Está válido?');
    if ((validarEmailUsuario() == true)) {
        console.log('validarTodosCamposUsuario -> Válido');
        $('#form-usuario').valid();
    }
}

function habilitarSalvarUsuario() {
    var email = $("#usuario-email").hasClass("has-success");
    var perfil = $("#usuario-perfil").hasClass("has-success");

    if ((email == true) && (perfil == true)) {
        $('#btnSalvarUsuario').prop("disabled", false);
        return false;
    }
    $('#btnSalvarUsuario').prop("disabled", true);
    return false;
}

