function validarRotuloAvaliacao() {
    validarCampoBranco('avaliaco-input-rotulo', '#avaliacao-rotulo', '#mensagem-erro-rotulo');
    habilitarSalvar();
}

function validarDataAvaliacao() {
    validarCampoBranco('DataProgramada', '#avaliacao-data', '#mensagem-erro-data');
    habilitarSalvar();
}

function validarGrupo() {
    validarDropdowSelected('grupoAvaliacao', '#avaliacao-grupo', '#avaliacao-erro-grupo');
    habilitarSalvar();
}


function validarUnidade() {
    validarDropdowSelected('unidadeAvaliacao', '#avaliacao-unidade', '#avaliacao-erro-unidade');
    habilitarSalvar();
}

function validarUsuario() {
    validarDropdowSelected('usuarioAvaliacao', '#avaliacao-usuario', '#avaliacao-erro-usuario');
    habilitarSalvar();
}

function validarDataInicial() {
    validarData('data-inicio', '#avaliacao-Inicio-data', '#mensagem-erro-data-inicio');
    habilitarSalvar();
}

function validarDataTermino() {
    // Só precisa ser validado se o Terminar for do tipo data
    var selectedTerminar = $("input[type='radio'][name='terminar']:checked");
    if (selectedTerminar.val() == "data") {
        validarData('data-fim', '#avaliacao-fim-data', '#mensagem-erro-data-fim');
        habilitarSalvar();
    }
}

function validarDataCustomizada(numeroId) {
    validarData('data-customizada' + numeroId, '#campo-data-customizada' + numeroId, '#mensagem-erro-data-customizada'+numeroId);
    habilitarSalvar();
}




function habilitarSalvar() {

    var roteiro = validarCampoBranco('avaliaco-input-rotulo', null, null);
    var data = validarCampoBranco('DataProgramada', null, null);

    var grupo = validarDropdowSelected('grupoAvaliacao', null, null);
    var unidade = validarDropdowSelected('unidadeAvaliacao', null, null);
    var usuario = validarDropdowSelected('usuarioAvaliacao', null, null);

    // Se todos os campos estiverem OK retorna true;

    // Verificar se a avaliãção é recorrente ou de execução única
    if ($('#repetir').is(":checked") && validarAvaliacaoRepetida() == true &&
        (grupo == true) && (unidade == true) && (usuario == true)) {
        $('#btnSalvarAvaliacao').prop("disabled", false);
        return true;
    }

    // Inspeção sem recorrências, ou seja de execução única
    if (($('#repetir').is(":checked") == false) && (roteiro == true) && (data == true) && (grupo == true) && (unidade == true) && (usuario == true)) {
        $('#btnSalvarAvaliacao').prop("disabled", false);
        return true;
    }


    $('#btnSalvarAvaliacao').prop("disabled", true);
    return false;

}


function validarAvaliacaoRepetida() {

    var selecionado = $('#select-modelo-repeticao').val();
    // Somente verifico a faixa de início e fim apenas para modelos de repetição diferentes do customizado
    if (selecionado != "Customizado") {
        //Verifica se existe data Inicial
        if (moment($('#data-inicio')[0].value, "DD/MM/YYYY").isValid() == false) {
            // Está inválido, mantenha desabilitado e retorne falso
            return false;
        }

        var selectedTerminar = $("input[type='radio'][name='terminar']:checked");
        // Verificar se o modelo selecionado de termino foi data 
        if (selectedTerminar.val() == "data" && moment($('#data-fim')[0].value, "DD/MM/YYYY").isValid() == false) {
            return false;
        }

    }

    if (selecionado == 'Semanal' && $('.checkSemanal:checked').size() == 0) {
        return false;
    }
    else if (selecionado == 'Mensal' && $('.checkMes:checked').size() == 0) {
        return false;
    }
    else if (selecionado == 'Customizado') {
        var doc = document.getElementById("campo-de-datas-adicionais");
        if (moment($("#data-customizada0")[0].value, "DD/MM/YYYY").isValid() == false) {
            return false;
        }

        // Procurando alguma data com campo inválido
        for (var i = 0; i < doc.childNodes.length; i++) {
            if ($(doc.childNodes[i]).hasClass("removeMe")) {
                var idDataProgramada = $(doc.childNodes[i])[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].id;
                var dataProgramada = $("#" + idDataProgramada)[0].value;
                if (moment(dataProgramada, "DD/MM/YYYY").isValid() == false) {
                    return false;
                }
            }
        }
    }

    return true;

}



// Caso tenha algum modo onde o usuário conseguiu deixar o botão habilitado mesmo sendo algo inválido 
function validarTodosCampos() {

    if ((validarRotuloAvaliacao() == true) && (validarUnidade() == true) && (validarUsuario() == true) && (validarGrupo() == true) && (validarDataAvaliacao() == true)) {
        $('#form-unidade').valid();
    }
}

function getUnidadesAndGruposAndInspetores(unidadeSelected, grupoSelected, inspetorSelected, callback) {
    $.getJSON("Agenda/GetUnidadesAndGruposAndInspetores", function (data) {
        var unidades = data.unidades;
        var grupos = data.grupos;
        var inspetores = data.inspetores;

        $.each(unidades, function (index, optiondata) {
            $('#unidadeAvaliacao').append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
        });
        if (unidadeSelected != null && unidadeSelected != 'undefined') {
            $('#unidadeAvaliacao').val(unidadeSelected);
            habilitarSalvar();
        }

        $.each(grupos, function (index, optiondata) {
            $('#grupoAvaliacao').append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
        });
        if (grupoSelected != null && grupoSelected != 'undefined') {
            $('#grupoAvaliacao').val(grupoSelected);
            habilitarSalvar();
        }

        $.each(inspetores, function (index, optiondata) {
            $('#usuarioAvaliacao').append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
        });
        if (inspetorSelected != null && inspetorSelected != 'undefined') {
            $('#usuarioAvaliacao').val(inspetorSelected);
            habilitarSalvar();
        }

        if (callback != null){
            callback(unidades.length, grupos.length, inspetores.length);
        }
    });
}


