document.body.onload = function () {
    var opcaoMenu = document.getElementById("menu_grupo");
    opcaoMenu.className = opcaoMenu.className + "  active";
}

$(document).ready(function () {

});

$(document).ajaxStop(function () {

});

function adicionar() {
    $('#Id').val(null);
    resetarCampo('grupo-input-nome', '#grupo-nome', 'mensagem-erro-nome');
    $("#tbodyClassificacao").empty();
    criarClassificacoes();
    formatarDropDownCores();
    $('#myModalLabel').text('Adicionar grupo');
}

function criarClassificacoes() {
    // Depois pode ser simplificado usando um array de objetos e chamando criarLinhaClassificacao para cada um item do array.
    var tamanho = 5;
    var pessimo = { Descricao: 'Péssimo', InicioIntervalo: 0, FimIntervalo: (100 / tamanho), Cor: 'd91c1a' };
    var ruim = { Descricao: 'Ruim', InicioIntervalo: (1 == 0 ? 0 : ((100 / tamanho) * 1) + 1), FimIntervalo: ((100 / tamanho) * (1 + 1)), Cor: 'ff8a15' };
    var razoavel = { Descricao: 'Razoável', InicioIntervalo: (2 == 0 ? 0 : ((100 / tamanho) * 2) + 1), FimIntervalo: ((100 / tamanho) * (2 + 1)), Cor: 'ffcc33' };
    var bom = { Descricao: 'Bom', InicioIntervalo: (3 == 0 ? 0 : ((100 / tamanho) * 3) + 1), FimIntervalo: ((100 / tamanho) * (3 + 1)), Cor: '99cc33' };
    var otimo = { Descricao: 'Ótimo', InicioIntervalo: (4 == 0 ? 0 : ((100 / tamanho) * 4) + 1), FimIntervalo: ((100 / tamanho) * (4 + 1)), Cor: '339933' };

    criarLinhaClassificacao(0, null, pessimo.Descricao, pessimo.InicioIntervalo, pessimo.FimIntervalo, pessimo.Cor);
    criarLinhaClassificacao(1, null, ruim.Descricao, ruim.InicioIntervalo, ruim.FimIntervalo, ruim.Cor);
    criarLinhaClassificacao(2, null, razoavel.Descricao, razoavel.InicioIntervalo, razoavel.FimIntervalo, razoavel.Cor);
    criarLinhaClassificacao(3, null, bom.Descricao, bom.InicioIntervalo, bom.FimIntervalo, bom.Cor);
    criarLinhaClassificacao(4, null, otimo.Descricao, otimo.InicioIntervalo, otimo.FimIntervalo, otimo.Cor);
}

function criarLinhaClassificacao(index, id, descricao, inicioIntervalo, fimIntervalo, corSelecionada) {
    var inputId = '';
    if (id != null)
        inputId = '<input id="ListaClassificacoes[' + index + '].Id" name="ListaClassificacoes[' + index + '].Id" value="' + id + '" type="hidden" />';
    var columnDescricao = '<td><input id="ListaClassificacoes.Index" name="ListaClassificacoes.Index" value="' + index + '" type="hidden" />' + inputId + '<input id="ListaClassificacoes[' + index + '].Descricao" name="ListaClassificacoes[' + index + '].Descricao" value="' + descricao + '" type="text" class="form-control input-sm validar" placeholder="Descrição" size="10" maxlength="50" onblur="validarTodosInputsGrupo();" /></td>';
    var columnInicioIntervalo = '<td><input id="ListaClassificacoes[' + index + '].InicioIntervalo" name="ListaClassificacoes[' + index + '].InicioIntervalo" value="' + inicioIntervalo + '" type = "number" class="form-control input-sm validar" placeholder="0" size="3" min="0" max="100" onblur="validarTodosInputsGrupo();" /></td>';
    var columnFimIntervalo = '<td><input id="ListaClassificacoes[' + index + '].FimIntervalo" name="ListaClassificacoes[' + index + '].FimIntervalo" value="' + fimIntervalo + '" type="number" class="form-control input-sm validar" placeholder="100" size="3" min="0" max = "100" onblur="validarTodosInputsGrupo();" /></td>';
    var columnCores = '<td><select name="ListaClassificacoes[' + index + '].CorIntervaloClassificacao" class="form-control" onchange="formatarDropDownCorSelecionada(this)" id="drop-colorselector-' + index + '" >';
    for (var i = 0; i < todasAsCores.length; i++) {
        var ehParaSelecionar = todasAsCores[i].Value == corSelecionada ? 'selected' : '';
        columnCores += '<option value="' + todasAsCores[i].Value + '" ' + ehParaSelecionar + '></option>';
    }
    columnCores += '</select></td>';
    var columnDeletar = '<td><button type="button" class="btn btn-danger btn-xs" id="btnRemoveClassificacao-' + index + '" name="btnRemoveClassificacao" onclick="removerLinhaClassificacao(this)"> Remover </button></td>';
    $('#tabelaClassificacao tbody').append('<tr>' + columnDescricao + columnInicioIntervalo + columnFimIntervalo + columnCores + columnDeletar + '</tr>');
}

function editarGrupo(grupoId) {
    waitingDialog.show('Carregando edição de grupo');
    limparCamposModal();
    $.getJSON('/Grupo/Manipular?grupoId=' + grupoId, function (grupo) {
        $('#Id').val(grupo.Id);
        $('#grupo-input-nome').val(grupo.Nome);
        $('.fg-line ').addClass(" fg-toggled");
        
        var classificacoes = grupo.ListaClassificacoes;
        for (var i = 0; i < classificacoes.length; i++) {
            criarLinhaClassificacao(classificacoes[i].Id, classificacoes[i].Id, classificacoes[i].Descricao, classificacoes[i].InicioIntervalo, classificacoes[i].FimIntervalo, classificacoes[i].CorIntervaloClassificacao);
        }

        $('#manipularGrupoModal').modal('show');
        $('#myModalLabel').text('Editar grupo');
        formatarDropDownCores();
        validarTodosInputsGrupo();
    }).always(function () {
        waitingDialog.hide();
    });
}

function excluirGrupo(grupoId) {
    swal({
        title: "Deseja realmente excluir esse usuário?",
        text: "Todos os formulários ligados a esse grupo também serão removidos.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: '/Grupo/Remover?grupoId=' + grupoId,
            cache: false,
            type: 'post',
            complete: function (data) {
                pesquisar();
            }
        });
    });
}


function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Grupo/Tabela', { "termo": termo }, function () { loadBootGrid(); });
}

function onSuccess(data) {
    $('#manipularGrupoModal').modal('hide');
    if (data.ok) {
        pesquisar();
    } else {
        $('#modal').html(data);
        $('#manipularGrupoModal').modal('show');
    }
}


$('#manipularGrupoFormulariosModal').on('hidden.bs.modal', function () {
    $('body').css("overflow-y", 'auto');
})


$('#manipularGrupoFormulariosModal').on('show.bs.modal', function () {
    $('body').css("overflow-y", 'hidden');
})


function onSuccessManipularFormulariosGrupo(data) {
    $('#manipularGrupoFormulariosModal').modal('hide');
    if (data.ok) {
        pesquisar(); // Atualiza a tabela
    } else {
        $('#modal').html(data);
        $('#manipularGrupoFormulariosModal').modal('show');
        $('body').css("overflow-y", 'hidden');
    }
}

function loadBootGrid() {
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
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"editarGrupo(" + row.id + ")\">Editar</button>&nbsp;" +
                "<button class=\"btn btn-xs bgm-teal waves-effect waves-float\" onclick=\"editarGrupoFormularios(" + row.id + ")\">Formulários</button>&nbsp;" +
                      "<button class=\"btn btn-xs btn-danger waves-effect waves-float\" onclick=\"excluirGrupo('" + row.id + "');\">Excluir</button>&nbsp;";
            },
            "formularios": function (column, row) {
                var dados = row.formularios;
                var id;
                var nome;
                var links = "";
                while (dados.length > 0) {
                    id = dados.substr(0, dados.indexOf('/'));
                    dados = dados.substr(id.length + 1);
                    nome = dados.substr(0, dados.indexOf('|'));
                    dados = dados.substr(nome.length + 1);
                    links = links + ' <a target="_blank" href="/Questionario/Manipular?questionarioId=' + id + '" >' + nome + '</a> /'
                }

                if (links == "") {
                    return "Nenhum";
                }
                return links.substr(0, links.length - 1);
            }
        },
        labels: {
            noResults: "Não existem grupos relacionados.",
            loading: "Carregando as grupos cadastradas.",
            search: "Pesquisar por grupos",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} grupo(s)"
        }
    });
}

loadBootGrid();

function limparCamposModal() {
    $('#Id').val(null);
    $('#grupo-input-nome').val('');
    $('#tabelaClassificacao tbody').empty();
}

//****************************** VALIDACOES *********************************************

function criarValidacaoInputs() {
    var inputs = $('input.validar');

    for (var i = 0; i < inputs.length; i++) {
        var input = inputs[i];
        input.setAttribute('onBlur', 'validarTodosInputsGrupo();');
    }
};

function validarTodosInputsGrupo() {
    var inputs = $('input.validar');
    $('#isValid').prop('value', true);

    removerMsgValidacao();
    for (var i = 0; i < inputs.length; i++) {
        var input = inputs[i];
        validarInputGrupo(input);
    }

    //habilita ou desabilita os botões de salvar
    if ($('#isValid').prop('value') == 'true') {
        document.getElementById("btnSalvarGrupo").disabled = false;
    }
    else {
        document.getElementById("btnSalvarGrupo").disabled = true;
    }
};

//cria o componente small dentro da div para ser atribuido a div que exibe as mensagens de validação.
var atribuirMsgValidacao = function (idDivMensagem, txtMensagem, idDivExibicao) {
    if (document.getElementById(idDivMensagem) == undefined) {
        var divmensagemErro = document.createElement("div");
        divmensagemErro.id = idDivMensagem;
        var smallMensagem = document.createElement("small");
        smallMensagem.class = "help-block";
        smallMensagem.style = "color: #f6675d";
        smallMensagem.innerText = txtMensagem;
        divmensagemErro.appendChild(smallMensagem);
        idDivExibicao.appendChild(divmensagemErro);
    }
};

var removerMsgValidacao = function () {
    for (var i = 0; i <= $('#div-msgvalidacao')[0].childNodes.length && $('#div-msgvalidacao')[0].childNodes.length > 0; i++) {
        $('#div-msgvalidacao')[0].childNodes[0].remove();
    }
};

//function validarFormClassificacoes(input)
function validarInputGrupo(input) {
    var isValid = true;
    var trContainer = input.parentElement.parentElement;
    var nomeCase = input.name.substring(input.name.indexOf(".") + 1);

    switch (nomeCase) {
        case "Nome":
            //Valida Nome
            var resposta = validarCampoBranco('grupo-input-nome', '#grupo-nome', '#mensagem-erro-nome');

            if (false == resposta) {
                isValid = false;
            }
            break;
        case "Descricao":
            //Valida Descricao
            var idLinha = $('[name="ListaClassificacoes.Index"]', trContainer).prop('value');
            var nome = document.getElementsByName("ListaClassificacoes[" + idLinha + "].Descricao")[0].value;
            if (nome.length < 2) {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].Descricao")[0].style.border = "1px solid #F40C0C";
                atribuirMsgValidacao("msgErroDescricao", "A descrição da classificação não pode ter menos de dois caracteres.", $('#div-msgvalidacao')[0]);
                isValid = false;
            }
            else {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].Descricao")[0].style.border = "1px solid #FFF";
            }
            break;
        case "InicioIntervalo":
            //Valida Intervalos
            var idLinha = $('[name="ListaClassificacoes.Index"]', trContainer).prop('value');
            var inicioIntervalo = $('[name="ListaClassificacoes[' + idLinha + '].InicioIntervalo"]', trContainer).prop('value');
            var fimIntervalo = $('[name="ListaClassificacoes[' + idLinha + '].FimIntervalo"]', trContainer).prop('value');
            var mensagem = "";
            if (trContainer.previousElementSibling == null) {
                if (parseInt(inicioIntervalo) != 0) {
                    mensagem = "O primeiro valor do intervalo deve ser 0.";
                    atribuirMsgValidacao("msgErroValor0", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
            }
            else {
                var idLinhaAnterior = $('[name="ListaClassificacoes.Index"]', trContainer.previousElementSibling).prop('value');
                var fimIntervaloAnterior = $('[name="ListaClassificacoes[' + idLinhaAnterior + '].FimIntervalo"]', trContainer.previousElementSibling).prop('value');

                if (inicioIntervalo <= parseInt(fimIntervaloAnterior)) {
                    mensagem = "O valor inicial do intervalo não pode ser menor que o anterior final.";
                    atribuirMsgValidacao("msgErroValorMenorAnterior", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
                else if (parseInt(inicioIntervalo) > parseInt(fimIntervalo)) {
                    mensagem = "O valor inicial do intervalo não pode ser menor que o final.";
                    atribuirMsgValidacao("msgErroMenorFinal", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
            }
            if (!isValid) {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].InicioIntervalo")[0].style.border = "1px solid #F40C0C";
            }
            else {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].InicioIntervalo")[0].style.border = "1px solid #FFF";
            }
            //tabelaClassificacao
            break;
        case "FimIntervalo":
            //Valida Intervalos
            var idLinha = $('[name="ListaClassificacoes.Index"]', trContainer).prop('value');
            var fimIntervalo = $('[name="ListaClassificacoes[' + idLinha + '].FimIntervalo"]', trContainer).prop('value');
            var inicioIntervalo = $('[name="ListaClassificacoes[' + idLinha + '].InicioIntervalo"]', trContainer).prop('value');
            var mensagem = "";
            if (trContainer.nextElementSibling == null) {
                if (parseInt(fimIntervalo) != 100) {
                    mensagem = "O último valor do intervalo deve ser 100.";
                    atribuirMsgValidacao("msgErroUltimoValor", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
            }
            else {
                var idLinhaSeguinte = $('[name="ListaClassificacoes.Index"]', trContainer.nextElementSibling).prop('value');
                var inicioProximoIntervalo = $('[name="ListaClassificacoes[' + idLinhaSeguinte + '].InicioIntervalo"]', trContainer.nextElementSibling).prop('value');

                if (parseInt(fimIntervalo) >= parseInt(inicioProximoIntervalo)) {
                    mensagem = "O valor final do intervalo não pode ser maior que o proximo inicial.";
                    atribuirMsgValidacao("msgErroMaiorProximo", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
                else if (parseInt(fimIntervalo) < parseInt(inicioIntervalo)) {
                    mensagem = "O valor final do intervalo não pode ser maior que o inicial.";
                    atribuirMsgValidacao("msgErroMaiorInicial", mensagem, $('#div-msgvalidacao')[0]);
                    isValid = false;
                }
            }
            if (!isValid) {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].FimIntervalo")[0].style.border = "1px solid #F40C0C";
            }
            else {
                document.getElementsByName("ListaClassificacoes[" + idLinha + "].FimIntervalo")[0].style.border = "1px solid #FFF";
            }
            //tabelaClassificacao
            break;
        default: break;
    }
    if (!isValid) {
        $('#isValid').prop('value', false);
    }
}

function formatarDropDownCores() {
    $('[name$=CorIntervaloClassificacao] option').each(
        function () {
            $(this).attr("data-color", "#" + $(this).prop("value"));
        }
    );

    $('[name$=CorIntervaloClassificacao]').each(
        function () {
            if ($(this).val() != '') {
                $(this).colorselector();
            }
        }
    );
}



// FORMULARIOS LIGADOS AOS GRUPOS
function editarGrupoFormularios(grupoId) {
    waitingDialog.show('Carregando lista de formulários ligados ao grupo');

    $.getJSON('/Grupo/ManipularFormulariosGrupo?grupoId=' + grupoId, function (grupo) {
        $('#GrupoId').val(grupo.Id);
        $('#nome-grupo').text( "Grupo: " + grupo.Nome);
        $('#manipularGrupoFormulariosModal').modal('show');
        $('#myModalLabel').text('Editar formulários pertencente ao grupo');

        $('#lista-formularios').empty(".checkbox");

        $.getJSON('/Questionario/GetTodosPertencentesAoGrupo?grupoId=' + grupoId, function (formularios)
        {
            for (i = 0; i < formularios.length; i++)
            {
                $('#lista-formularios').append(criarCheckBox(
                    formularios[i].Nome,
                    formularios[i].Id,
                    formularios[i].Marcado));
            }
        });

    }).always(function () {
        waitingDialog.hide();
    });

}

function criarCheckBox(nomeFormulario, id, marcado)
{
    var divCheckbox = document.createElement("div");
    divCheckbox.className = "checkbox m-b-15";

    var label = document.createElement("label");

    var input = document.createElement("input");
    input.type = "checkbox";
    input.id = "Formularios[" + id + "].Selected";
    input.name = "Formularios[" + id + "].Selected";
    //var input = $("<input type='checkbox' name='Formularios[" + id + "].Selected' id='Formularios[" + id + "].Selected' ></input>");

    var inputHiddenIndex = $('<input type="hidden" name="Formularios.Index" value="' + id + '" />');
    var inputHiddenValue = $("<input type='hidden' name='Formularios[" + id + "].Id' value=" + id + "></input>");

    var i = document.createElement("i");
    i.className = "input-helper";

    input.checked = marcado;

    label.append(input);
    label.append(i)
    label.append(nomeFormulario);

    divCheckbox.append(label);
    $('#lista-formularios').append(inputHiddenIndex);
    $('#lista-formularios').append(inputHiddenValue);

    return  divCheckbox;
}