
// Verificando se o formulário possuem questões porém não foi salva.
var myEvent = window.attachEvent || window.addEventListener;
var chkevent = window.attachEvent ? 'onbeforeunload' : 'beforeunload'; /// make IE7, IE8 compatable
var habilitarRefresh; // a mensagem box só vai apaecer caso esteja com o formulário ainda em aberto

myEvent(chkevent, function (e) { // For >=IE7, Chrome, Firefox
    var confirmationMessage = '';  // a space

    if (habilitarRefresh == false)
    {
        (e || window.event).returnValue = confirmationMessage;
    }

    return confirmationMessage;
});



document.body.onload = function () {    
    habilitarRefresh = true;
    var opcaoMenu = document.getElementById("menu_questionarios");
    opcaoMenu.className = opcaoMenu.className + "  active";
}

function onSuccess(data) {
    if (data.ok) {
        habilitarRefresh = true;
        swal({
            title: 'Formulário salvo com sucesso.',
            type: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'ok',
            confirmButtonClass: 'confirm-class',
            cancelButtonClass: 'cancel-class',
            closeOnConfirm: false,
            allowEscapeKey: false
        },
        function (isConfirm) {
            location.replace("/Questionario/Index");
        });
    } else {
        alert("Oops... algo deu errado.");
    }
}


/********************* VALIDACOES *******************************************/

function validarNome() {
    var resposta = validarCampoBranco('formulario-input-nome', '#formulario-nome', '#mensagem-erro-nome');
    return resposta;
}


function validarGrupo() {
   var resposta = validarDropdowSelected('grupoQuestionario', '#formulario-grupo', '#formulario-erro-grupo');
   return resposta;
}


function validarTodosCampos() {
    var grupo = validarDropdowSelected('grupoQuestionario', null, null);
    var nome = validarCampoBranco('formulario-input-nome', null, null);
    var qtdQuestoes = getQuestoesProntaLength();

    if ((nome == true) && (grupo == true) && (qtdQuestoes >= 1)) {
        $('#form-questionario').valid();
        return true;
    }
   
    var htmlInvalido = "";
    if (nome == false) {
        htmlInvalido = htmlInvalido + '<p class="invalido"> Nome em branco </p>';
    }
    if (grupo == false) {
        htmlInvalido = htmlInvalido + '<p class="invalido"> Grupo não selecionado </p>';
    }
    if (qtdQuestoes == 0) {
        htmlInvalido = htmlInvalido + '<p class="invalido"> Adicione ao menos 1 item ao formulário </p>';
    }

    if ((nome == false) || (grupo == false) || (qtdQuestoes == 0)) {
        alertInvalido(htmlInvalido);
        return false;
    }
}

function alertInvalido(invalido) {
    swal({
        title: 'É preciso preencher os seguintes dados',
        text: invalido,
        html: true,
        type: 'error',
        showCancelButton: false,
        confirmButtonText: 'Ok',

    });
}

function limparDescricaoQuestao() {
    resetarCampo('txtDescricao', '#item-Descricao', '#titulo-questao-invalida');
}

function validarDescricaoQuestao() {
    var resposta = validarCampoBranco('txtDescricao', '#item-Descricao', '#titulo-questao-invalida');
    return resposta;
}

// Verifica se alguma mensagem de erro está visivel na tela
function validarQuestao() {
    var titulo = validarCampoBranco('txtDescricao', '', '');

    if (titulo == false) {
        // é invalida
        alertQestaoInvalida()
        return false;
    } else {
        // ainda é valido
        var multipla = document.getElementById("questao-multipla").checked;
        var objetiva = document.getElementById("questao-objetiva").checked;

        if ((multipla == true) || (objetiva == true)) {
            // vamos testar as alternativas
            var qtdAlternativas = $("#alternativas-container").children('.alternativa').length;
            if (qtdAlternativas == 0) {
                // é inválida
                alertQestaoInvalida();
                return false;
            } else {
                // ainda é válido
                var qtdAlternativasInvalidas = 0;
                for (var i = 0; i < $('#alternativasCounter').val(); i++) {

                    if (validarAlternativa(i) == false) {
                        qtdAlternativasInvalidas++;
                    }
                }
                if (qtdAlternativasInvalidas >= 1) {
                    // é invalido
                    alertQestaoInvalida()
                    return false;
                } else {
                    // é valido
                    gerarNovaLinhaQuestao();
                    habilitarRefresh = false; // Se tentar dar refresh vai aparecer a menssage box.
                    return true;
                }

            }
        } else {
            //  Pois ele selecionou a subjetiva
            gerarNovaLinhaQuestao();
            habilitarRefresh = false; // Se tentar dar refresh vai aparecer a menssage box.
            return true;
        }
    }

}


function alertQestaoInvalida() {
    swal({
        text: "Verifique se todos os campos foram digitados corretamente.",
        title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Impossível adicionar item ao formulário!</h1>",
        showCancelButton: false,
        confirmButtonColor: "#9E9E9E",
        confirmButtonText: "Ok",
        closeOnConfirm: true,
        closeOnCancel: true,
        html: true,

    });
}

function validarAlternativa(alternativaID) {
    var resposta = validarCampoBranco('Alternativas[alternativa' + alternativaID + '].Descricao', '#alternativa-Descricaoalternativa' + alternativaID, '#invalidaDescricaoalternativa' + alternativaID);
    return resposta;
}


function validarPesoAlternativa(alternativaID) {
    var txtPeso = document.getElementById("Alternativas[alternativa" + alternativaID + "].Peso");

    // Limpando todos os zeros a esquerda
    if (txtPeso.value.length > 1 && txtPeso.value.indexOf("0") > -1) {
           txtPeso.value = Number(txtPeso.value).toString();
    }

    if (txtPeso.value == "") {
        txtPeso.value = 0;
    }
}

