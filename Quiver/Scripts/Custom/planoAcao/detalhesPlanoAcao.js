$(document).ready(function () {
    var opcaoMenu = document.getElementById("menu_plano");
    opcaoMenu.className = opcaoMenu.className + "  active";

    trocarSelectedBackground();

    $('#pa-input-quando').datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: '01/01/2000',
        locale: 'pt-br'
    });
});

function onBegin() {
    $("#btnSalvarPA").prop("disabled", true);
    $("#icon-carregar").css('display', 'inline-block');
}

function onComplete() {
    $("#btnSalvarPA").prop("disabled", false);
    $("#icon-carregar").css('display', 'none');
}

function onSuccess(data) {
    if (data.ok) {
        swal({
            title: 'Plano de ação salvo com sucesso.',
            type: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'ok',
            confirmButtonClass: 'confirm-class',
            cancelButtonClass: 'cancel-class',
            closeOnConfirm: false,
            allowEscapeKey: false
        },
        function (isConfirm) {
            location.replace("/PlanoAcao/Index");
        });
    } else {
        alert("Oops... algo deu errado.");
    }
}


/********************* VALIDACOES *******************************************/

function validarOque() {
    var resposta = validarCampoBranco('pa-input-oque', '#pa-oque', '#mensagem-erro-oque');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarPorque() {
    var resposta = validarCampoBranco('pa-input-porque', '#pa-porque', '#mensagem-erro-porque');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarComo() {
    var resposta = validarCampoBranco('pa-input-como', '#pa-como', '#mensagem-erro-como');
    return resposta;
}

function validarQuem() {
    var resposta = validarCampoBranco('pa-input-quem', '#pa-quem', '#mensagem-erro-quem');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarResponsavel() {
    var resposta = validarEmail('pa-input-responsavel', '#pa-responsavel', '#mensagem-erro-responsavel');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarOnde() {
    var resposta = validarCampoBranco('pa-input-onde', '#pa-onde', '#mensagem-erro-onde');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarQuanto() {
    var resposta = validarCampoBranco('pa-input-quanto', '#pa-quanto', '#mensagem-erro-quanto');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarData() {
    var resposta = validarCampoBranco('pa-input-quando', '#pa-quando', '#mensagem-erro-data');
    habilitarSalvarPlanoAcao();
    return resposta;
}

function validarTudo() {
    validarOque();
    validarPorque();
    validarComo();
    validarQuem();
    validarResponsavel();
    validarOnde();
    validarQuanto();
    validarData();
}

function habilitarSalvarPlanoAcao() {
    var data = $("#pa-quando").hasClass("has-success");
    var responsavel = $("#pa-responsavel").hasClass("has-success");
    var quem = $("#pa-quem").hasClass("has-success");
    var oque = $("#pa-oque").hasClass("has-success");
    var porque = $("#pa-porque").hasClass("has-success");
    var como = $("#pa-como").hasClass("has-success");
    var quanto = $("#pa-quanto").hasClass("has-success");
    var onde = $("#pa-onde").hasClass("has-success");

    if (data && responsavel && quem && oque && porque && como && quanto && onde) {
        $('#btnSalvarPA').prop("disabled", false);
        return false;
    } else {
        $('#btnSalvarPA').prop("disabled", true);
        return false;
    }
}

$('#select-pa-status').on('hidden.bs.select', function (e) {
    trocarSelectedBackground();
});

function trocarSelectedBackground() {
    var statusSelecionado = $('#select-pa-status').selectpicker('val');

    $('#select-pa-status').selectpicker('setStyle', 'selected-Aeditar', 'remove');
    $('#select-pa-status').selectpicker('setStyle', 'selected-Atrasado', 'remove');
    $('#select-pa-status').selectpicker('setStyle', 'selected-Aberto', 'remove');
    $('#select-pa-status').selectpicker('setStyle', 'selected-Cancelado', 'remove');
    $('#select-pa-status').selectpicker('setStyle', 'selected-Resolvido', 'remove');
    $('#select-pa-status').selectpicker('setStyle', 'selected-Finalizado', 'remove');

    if (statusSelecionado == "A editar") {
        $('#select-pa-status').selectpicker('setStyle', 'selected-Aeditar');
    } else if (statusSelecionado == "Em aberto") {
        $('#select-pa-status').selectpicker('setStyle', 'selected-Aberto');
    } else if (statusSelecionado == "Cancelado") {
        $('#select-pa-status').selectpicker('setStyle', 'selected-Cancelado');
    } else if (statusSelecionado == "Finalizado") {
        $('#select-pa-status').selectpicker('setStyle', 'selected-Finalizado');
    } else if (statusSelecionado == "Resolvido") {
        $('#select-pa-status').selectpicker('setStyle', 'selected-Resolvido');
    }
}