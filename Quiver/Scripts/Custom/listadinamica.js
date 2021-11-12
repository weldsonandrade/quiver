///Cria a nova linha editavel para a classificação de um grupo.
gerarNovaLinhaClassificacao = function () {
    var classificacaoCounter = $('#classificacaoCounter');

    //var indiceLinha = $('#tabelaClassificacao')[0].rows.length;
    var indice = classificacaoCounter.val();//indiceLinha - 1;
    var colunaDescricaoHTML = document.createElement("td");
    var colunaInicioHTML = document.createElement("td");
    var colunaFimHTML = document.createElement("td");
    var colunaCorHTML = document.createElement("td");
    var colunaButtonHTML = document.createElement("td");


    colunaDescricaoHTML.appendChild(criarElementoInput("hidden", "", "", "ListaClassificacoes.Index", "ListaClassificacoes.Index", "10", indice));
    colunaDescricaoHTML.appendChild(criarElementoInput("text", "form-control input-sm validar", "Descrição", "ListaClassificacoes[" + indice + "].Descricao", "", "10", "", "validarTodosInputsGrupo()"));
    colunaInicioHTML.appendChild(criarElementoInputNumber("form-control input-sm validar", "0", "ListaClassificacoes[" + indice + "].InicioIntervalo", "", "3", "", "validarTodosInputsGrupo()", 0, 100));
    colunaFimHTML.appendChild(criarElementoInputNumber("form-control input-sm validar", "0", "ListaClassificacoes[" + indice + "].FimIntervalo", "", "3", "", "validarTodosInputsGrupo()", 0, 100));
    colunaCorHTML.appendChild(criarElementoSelect("form-control", "", "ListaClassificacoes[" + indice + "].CorIntervaloClassificacao", "formatarDropDownCorSelecionada(this);", "00FF00"));
    colunaButtonHTML.appendChild(criarElementoButton("button", "btn btn-danger btn-xs", "", "btnRemoveClassificacao", " Remover ", "removerLinhaClassificacao(this)"));

    var tr = document.createElement("tr");
    tr.appendChild(colunaDescricaoHTML);
    tr.appendChild(colunaInicioHTML);
    tr.appendChild(colunaFimHTML);
    tr.appendChild(colunaCorHTML);
    tr.appendChild(colunaButtonHTML);

    $('#tabelaClassificacao').append(tr);
    validarTodosInputsGrupo();
    formatarDropDownCores();

    classificacaoCounter.val(++indice);
};

criarElementoOption = function (value) {
    var option = document.createElement("option");
    option.value = value;
    option.setAttribute("data-color", "#" + value);
    return option;
}

criarElementoSelect = function (classe, id, nome) {
    var select = document.createElement("select");
    select.className = classe;
    select.id = id;
    select.name = nome;

    for (var i = 0; i < coresArray.length; i++) {
        select.appendChild(
            criarElementoOption(coresArray[i]));
    }

    return select;
};

criarElementoDiv = function (id, classe, style, elemento) {
    var div = document.createElement("div");
    div.id = id;
    div.className = classe;
    div.style = style;
    div.appendChild(elemento);
    return div;
};


criarElementoDiv = function (id, classe, pai) {
    var div = document.createElement("div");
    div.id = id;
    div.className = classe;
    pai.appendChild(div);
    return div;
};


//cria elemento Input para o form de acordo com as informações desejadas.
function criarElementoInput(tipo, classe, placeholder, nome, id, tamanho, value, onBlur) {
    var input = document.createElement("input");
    input.type = tipo;
    input.className = classe;
    input.placeholder = placeholder;
    input.name = nome;
    input.size = tamanho;
    input.id = id;
    input.value = value;


    if (typeof onBlur !== "undefined") {
        input.setAttribute("onblur", onBlur);
    }

    return input;
};

function criarElementoInputNumber(classe, placeholder, nome, id, tamanho, value, onBlur, min, max) {
    var input = document.createElement("input");
    input.type = "number";
    input.className = classe;
    input.placeholder = placeholder;
    input.name = nome;
    input.size = tamanho;
    input.id = id;
    input.value = value;

    if (typeof onBlur !== "undefined") {
        input.setAttribute("onblur", onBlur);
    }

    input.min = min;
    input.max = max;

    return input;
};

function criarElementoLabel(element_id, classe, texto) {
    var label = document.createElement("label");
    label.className = classe;
    label.for = element_id;
    label.append(texto);
    return label;
};

function criarElemento(elemento,element_id, classe) {
    var label = document.createElement(elemento);
    label.className = classe;
    label.for = element_id;
    return label;
};

function criarElementoSmall(classe, id, style, texto) {
    var small = document.createElement("small");
    small.className = classe;
    small.style = style;
    small.id = id;
    small.append(texto);
    return small;
};

criarElementoButton = function (tipo, classe, id, nome, texto, funcaoClick) {
    var button = document.createElement("button");
    button.type = tipo,
    button.className = classe;
    button.id = id;
    button.name = nome;
    button.textContent = texto;

    if (typeof funcaoClick !== "undefined") {
        button.setAttribute("onclick", funcaoClick);
    }

    return button;
};





criarElementoTexto = function (tag, texto, classe, id, style, elementoPai) {
    var textoTag = document.createElement(tag);
    textoTag.className = classe;
    textoTag.style = style;
    textoTag.id = id;
    textoTag.textContent = texto;
    elementoPai.appendChild(textoTag);
    return textoTag;
};

///verificar quais os parametros defaults do onclick
///remove as linhas da tabela
removerLinhaClassificacao = function (obj) {
    if (obj.target != undefined) {
        obj = obj.target;
    }

    var tr = $(obj).closest('tr');

    tr.remove();

    validarTodosInputsGrupo();
};


function getQuestoesProntaLength() {
    var doc = document.getElementById("div-listaQuestoes");
    var count = 0;

    var questaoTipo;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("questao-pronta") ||
            $(doc.childNodes[i]).hasClass("questao-pronta-HTML")) {
            count++;
        }
    }

    return count;
}


//Retorna o total de pontos de todas as questões
function getPontuacaoTotalQuestoes() {
    var doc = document.getElementById("div-listaQuestoes");
    var pontuacaoMaxima = 0;
    var questaoTipo;
    var listaAlternativasPeso;
    var maiorPeso = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {

        // Caso da questão criada no html
        if ($(doc.childNodes[i]).hasClass("questao-pronta-HTML"))
        {
            questaoTipo = $(doc.childNodes[i].childNodes[13].childNodes[3]);
            if (questaoTipo.hasClass("multipla-escolha") || questaoTipo.hasClass("objetivo"))
            {
                listaAlternativasPeso = $(doc.childNodes[i].childNodes[17]);
                maiorPeso = 0;
                for (var j = 0; j < listaAlternativasPeso[0].childNodes.length; j++)
                {
                    if (questaoTipo.hasClass("multipla-escolha") && $(listaAlternativasPeso[0].childNodes[j].childNodes[1]).hasClass("alternativa-peso-HTML")) {
                        maiorPeso = maiorPeso + parseInt(listaAlternativasPeso[0].childNodes[j].childNodes[1].childNodes[0].value);
                    }
                    else if ( $(listaAlternativasPeso[0].childNodes[j].childNodes[1]).hasClass("alternativa-peso-HTML")
                        && (parseInt(listaAlternativasPeso[0].childNodes[j].childNodes[1].childNodes[0].value) > maiorPeso)
                        )
                    {

                        maiorPeso = parseInt(listaAlternativasPeso[0].childNodes[j].childNodes[1].childNodes[0].value);
                    }
                }
                pontuacaoMaxima = pontuacaoMaxima + maiorPeso;
            }

        } else
            // Caso da questão criada no listadinamica.js
            if ($(doc.childNodes[i]).hasClass("questao-pronta"))
        {
            questaoTipo = $(doc.childNodes[i].childNodes[3].childNodes[1]);
            if (questaoTipo.hasClass("multipla-escolha") || questaoTipo.hasClass("objetivo"))
            {
                listaAlternativasPeso = $(doc.childNodes[i].childNodes[5].childNodes[2]);
                maiorPeso = 0;
                for (var j = 0; j < listaAlternativasPeso[0].childNodes.length; j++)
                {
                    if (questaoTipo.hasClass("multipla-escolha") && $(listaAlternativasPeso[0].childNodes[j]).hasClass("alternativa-peso")) {
                        maiorPeso = maiorPeso + parseInt(listaAlternativasPeso[0].childNodes[j].textContent);
                    }
                    else if ($(listaAlternativasPeso[0].childNodes[j]).hasClass("alternativa-peso")
                        && (parseInt(listaAlternativasPeso[0].childNodes[j].textContent) > maiorPeso))
                    {
                        maiorPeso = parseInt(listaAlternativasPeso[0].childNodes[j].textContent);
                    }
                }
                pontuacaoMaxima = pontuacaoMaxima + maiorPeso;
            }
        }



    }
    return pontuacaoMaxima;
}


// Usado para adicionar õ item objetivo ou múltiplo escolha no formulário
function getAlternativasLength() {
    var doc = document.getElementById("alternativas-container");
    var count = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("alternativa")) {
            count++;
        }
    }
    return count;

}

// Usado para adicionar õ item objetivo ou múltiplo escolha no formulário
function getItem( idItem) {




}



gerarNovaLinhaQuestao = function () {
    var questoesCounter = $('#questoesCounter');

    var questoesContainer = $('#div-listaQuestoes');
    var indice = parseInt(questoesCounter.val()) + 1;
    var questaoID = "questao" + indice;

    var divLinha = document.createElement("div");
    divLinha.setAttribute("class", "row questao-pronta  draggable");
    divLinha.setAttribute("id", questaoID);

    var valueInput = "linha" + indice;
    var input = criarElementoInput("hidden", "", "", "Questoes.Index", "", 10, valueInput);

    //Ordem
    var inputOrdem = criarElementoInput("hidden", "", "", "Questoes[" + valueInput + "].Ordem", "Questoes[" + valueInput + "].Ordem", 10, valueInput);

    var inputDescricao = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Descricao", "", 10, $('#txtDescricao')[0].value);
    inputDescricao.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");

    var inputExigeResposta = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].ExigeResposta", "", 10, $('#chkExigeResposta')[0].checked);
    inputExigeResposta.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");
    var inputTipo = "";


    if ($('#questao-subjetiva')[0].checked) {

        inputTipo = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Tipo", "Questoes_" + valueInput + "__Tipo", 10, $('#questao-subjetiva')[0].value);
        inputTipo.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");
    }

    if ($('#questao-multipla')[0].checked) {
        inputTipo = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Tipo", "Questoes_" + valueInput + "__Tipo", 10, $('#questao-multipla')[0].value);
        inputTipo.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");
    }

    if ($('#questao-objetiva')[0].checked) {
        inputTipo = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Tipo", "Questoes_" + valueInput + "__Tipo", 10, $('#questao-objetiva')[0].value);
        inputTipo.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");
    }

    var divAlternativas = document.createElement("div");
    divAlternativas.setAttribute("id", "divAlternativasQuestao" + valueInput);

    var alternativasContainer = $('#alternativas-container');
    for (var i = 0; i < alternativasContainer.children('.alternativa').length; i++) {

        //Alternativa Ordem
        var inputAlternativaOrdem = criarElementoInput("hidden", "", "", "Questoes[" + valueInput + "].Alternativas[" + i + "].Ordem", "Questoes[" + valueInput + "].Alternativas[" + i + "].Ordem", 10, i);

        var divAlternativa = document.createElement("div");
        divAlternativa.setAttribute("id", "divAlternativasQuestao" + valueInput + "_" + i);

        var alternativaDescricaoInput = $('[name=alternativa-descricao]', alternativasContainer.children('.alternativa')[i]);//$(divAlternativa).find('[name=alternativa-descricao]');
        var inputAlternativaDescricao = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Alternativas[" + i + "].Descricao", "", 10, alternativaDescricaoInput.val());
        inputAlternativaDescricao.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");

        var alternativaPesoInput = $('[name=alternativa-peso]', alternativasContainer.children('.alternativa')[i]);
        var inputAlternativaPeso = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Alternativas[" + i + "].Peso", "", 10, alternativaPesoInput.val());
        inputAlternativaPeso.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");

        var alternativaNaoConformidadeInput = $('[name=alternativa-naoconformidade]', alternativasContainer.children('.alternativa')[i]);
        var inputAlternativaNaoConformidade = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Alternativas[" + i + "].NaoConformidade", "", 10, alternativaNaoConformidadeInput.is(':checked'));
        inputAlternativaNaoConformidade.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");

        var alternativaExigeJustificativaInput = $('[name=alternativa-exigejustificativa]', alternativasContainer.children('.alternativa')[i]);
        var inputAlternativaExigeJustificativa = criarElementoInput("text", "", "", "Questoes[" + valueInput + "].Alternativas[" + i + "].ExigeJustificativa", "", 10, alternativaExigeJustificativaInput.is(':checked'));
        inputAlternativaExigeJustificativa.setAttribute("style", "border: none; background:#F3F3F3; font-size: 12px; font-family: inherit; color:#000;");

        divAlternativa.appendChild(inputAlternativaOrdem);
        divAlternativa.appendChild(inputAlternativaDescricao);
        divAlternativa.appendChild(inputAlternativaPeso);
        divAlternativa.appendChild(inputAlternativaNaoConformidade);
        divAlternativa.appendChild(inputAlternativaExigeJustificativa);
        divAlternativas.appendChild(divAlternativa);
    }

    //  Montando a linha com layout estilizado 
    {
        var colunaTitulo = criarElementoDiv("tituloColuna", "col-lg-6 col-md-6 col-xs-12 ", divLinha);
        {
            // Descição do titulo com seu small
            criarElementoTexto("small", "Titulo", null,null, "color:black", colunaTitulo);
            criarElementoTexto("p", inputDescricao.value, "m-0 label-questao","descricaoItem"+questaoID, null, colunaTitulo);
        }

        // Descição da Resposta com seu small
        var colunaExigeResposta = criarElementoDiv("tituloResposta", "col-lg-2 col-md-3 col-xs-4 ", divLinha);
        {
            colunaExigeResposta.appendChild(criarElementoTexto("small", "Exige resposta?", null, null,"color:black", colunaExigeResposta));
            if ($('#chkExigeResposta')[0].checked) {
                criarElementoTexto("p", "Sim", "m-0 label-questao", null,null, colunaExigeResposta);
            } else {
                criarElementoTexto("p", "Não", "m-0 label-questao",  null,null, colunaExigeResposta);
            }
        }


        // Tipo de  ITEM com small
        var colunaTipo = criarElementoDiv("tituloTipo", "col-lg-2 col-md-3 col-xs-4 ", divLinha);
        {
            colunaTipo.appendChild(criarElementoTexto("small", "Tipo de item", null, null, "color:black", colunaTipo));
            if ($('#questao-subjetiva')[0].checked) {
                criarElementoTexto("p", "Subjetivo", "m-0 subjetivo",  null,"color:#2196f3", colunaTipo);

            } else if ($('#questao-objetiva')[0].checked) {
                criarElementoTexto("p", "Objetivo", "m-0 objetivo",  null,"color:#1B5E20 ", colunaTipo);
            } else if ($('#questao-multipla')[0].checked) {
                criarElementoTexto("p", "Múltiplas escolhas ", "m-0 multipla-escolha",  null,"color:#FF9800", colunaTipo);

            }
        }


        //  Coluna de Excluir botão
        var colunaExcluirEditarItem = criarElementoDiv("excluiritem", "col-lg-2 col-md-12 col-xs-2 text-right", divLinha);
        colunaExcluirEditarItem.appendChild(criarElementoButton("button", "btn btn-success btn-xs text-right m-0 m-r-5", questaoID, null, "Editar", "editarQuestao('" + questaoID + "');"));
        colunaExcluirEditarItem.appendChild(criarElementoButton("button", "btn btn-danger btn-xs text-right m-0 ", questaoID, null, "Excluir", "excluirQuestao('" + questaoID + "');"));

        if (($('#questao-multipla')[0].checked) || ($('#questao-objetiva')[0].checked)) {

            var colunaAlternativa = criarElementoDiv("listasAlternativas" + questaoID, "col-xs-12 ", divLinha);
            {
                // Descição do titulo com seu small
                criarElementoTexto("h6", "Lista de Alternativas", "m-b-0", null, "color:black", colunaAlternativa);

                //Criando as colunas das alternativas
                var colunaAlternativaDescricao = criarElementoDiv("AlternativaDescricao", "col-lg-6 col-xs-6 p-l-0 alternativaDescricao", colunaAlternativa);
                var colunaAlternativaPeso = criarElementoDiv("AlternativaPeso", "col-lg-2 col-xs-2", colunaAlternativa);
                var colunaAlternativaNaoConforme = criarElementoDiv("AlternativaNaoConforme", "col-lg-2 col-xs-4", colunaAlternativa);
                var colunaAlternativaExigeJustificativa = criarElementoDiv("AlternativaNaoConforme", "col-lg-2 col-xs-4", colunaAlternativa);

                // Adicionando os small Titulo
                criarElementoTexto("small", "Não conforme?", null, null, "color:black", colunaAlternativaNaoConforme);
                criarElementoTexto("small", "Exigir Justificar?", null,  null,"color:black", colunaAlternativaExigeJustificativa);
                criarElementoTexto("small", "Peso", null, null, "color:black", colunaAlternativaPeso);
                criarElementoTexto("small", "Descricao", null, null, "color:black", colunaAlternativaDescricao);

                // Varrendo as alternativas
                var alternativasContainer = $('#alternativas-container');
                for (var i = 0; i < alternativasContainer.children('.alternativa').length; i++) {
                    // Exige Conformidade
                    var alternativaNaoConformidadeInput = $('[name=alternativa-naoconformidade]', alternativasContainer.children('.alternativa')[i]);
                    if (alternativaNaoConformidadeInput[0].checked) {
                        criarElementoTexto("p", "Sim", "m-0 label-questao", null, null, colunaAlternativaNaoConforme);
                    } else {
                        criarElementoTexto("p", "Não", "m-0 label-questao", null, null, colunaAlternativaNaoConforme);
                    }

                    //Exige Justificativa
                    var alternativaExigeJustificativaInput = $('[name=alternativa-exigejustificativa]', alternativasContainer.children('.alternativa')[i]);
                    if (alternativaExigeJustificativaInput[0].checked) {
                        criarElementoTexto("p", "Sim", "m-0 label-questao", null, null, colunaAlternativaExigeJustificativa);
                    } else {
                        criarElementoTexto("p", "Não", "m-0 label-questao", null, null, colunaAlternativaExigeJustificativa);
                    }

                    // Peso da alternativa
                    criarElementoTexto("p", $('[name=alternativa-peso]', alternativasContainer.children('.alternativa')[i]).val(), "m-0 alternativa-peso label-questao",  null,null, colunaAlternativaPeso);

                    // Descricao da alternativa
                    criarElementoTexto("p", (i + 1) + ". " + $('[name=alternativa-descricao]', alternativasContainer.children('.alternativa')[i]).val(), "m-0 label-questao", null, null, colunaAlternativaDescricao);
                }

            }


        }
    }

    //Deixando os inputs com display:none
    inputDescricao.setAttribute("class", "questao-input");
    inputExigeResposta.setAttribute("class", "questao-input");
    input.setAttribute("class", "questao-input");
    inputTipo.setAttribute("class", "questao-input");
    divAlternativas.setAttribute("class", "questao-input");

    divLinha.appendChild(input);
    divLinha.appendChild(inputOrdem);
    divLinha.appendChild(inputDescricao);
    divLinha.appendChild(inputExigeResposta);
    divLinha.appendChild(inputTipo);
    divLinha.appendChild(divAlternativas);

    questoesContainer.append(divLinha);
    questoesCounter.val(++indice);
    limparQuestionario();

    $("#total-questoes").text(getQuestoesProntaLength());
    $("#total-subjetivos").text($('.subjetivo').length);
    $("#total-objetivos").text($('.objetivo').length);
    $("#total-multiplas").text($('.multipla-escolha').length);
    $("#total-pontuacao").text(getPontuacaoTotalQuestoes());
    $("#cartao-itens").css("display", "block");
};

// Gera as alternativas lá no card de Adicionar questão
gerarNovaLinhaAlternativa = function () {
    if (getAlternativasLength() == 0) {
        $('#alternativasCounter').val(0);
    }

    var alternativasCounter = $('#alternativasCounter');

    var alternativaContainer = $('#alternativas-container');

    var indice = alternativasCounter.val();

    var alternativaId = "alternativa" + indice;

    var divAlternativa = document.createElement("div");
    divAlternativa.setAttribute("id", alternativaId);
    divAlternativa.setAttribute("class", "alternativa col-md-12 m-t-5 draggable ");
    {
        var input = criarElementoInput("hidden", "", "", "Alternativas.Index", "", 10, alternativaId);
        divAlternativa.appendChild(input);

        // Adicionando a descrição da alternativa
        divAlternativa.appendChild(createFloatingLabel("col-md-12 col-lg-6 ", "alternativa-Descricao" + alternativaId, "Alternativas[" + alternativaId + "].Descricao", "alternativa-descricao", "invalidaDescricao" + alternativaId, "Descrição", "Descrição obrigatória", "validarAlternativa(" + indice + ")"));

        var divColPeso = document.createElement("div");
        divColPeso.setAttribute("class", "col-md-2");
        divAlternativa.appendChild(divColPeso);
        {
            var divPesoAlternativaGroup = document.createElement("div");
            divPesoAlternativaGroup.setAttribute("class", "input-group form-group alternativa-group");
            divColPeso.appendChild(divPesoAlternativaGroup);
            {
                var spanPeso = document.createElement("span");
                spanPeso.setAttribute("class", "input-group-addon");
                spanPeso.textContent = "Peso";
                divPesoAlternativaGroup.appendChild(spanPeso);

                var divPeso = document.createElement("div");
                divPeso.setAttribute("class", "fg-line");
                divPesoAlternativaGroup.appendChild(divPeso);
                {
                    var textPeso = document.createElement("input");
                    textPeso.setAttribute("type", "number");
                    textPeso.setAttribute("min", "0");
                    textPeso.value = "0";
                    textPeso.setAttribute("id", "Alternativas[" + alternativaId + "].Peso");
                    //textPeso.setAttribute("name", "Alternativas[" + alternativaId + "].Peso");
                    textPeso.setAttribute("name", "alternativa-peso");
                    textPeso.setAttribute("placeholder", "Peso");
                    textPeso.setAttribute("class", "form-control");
                    textPeso.setAttribute("onfocusout", "validarPesoAlternativa(" + indice + ")");
                    textPeso.setAttribute("onkeyup", "validarPesoAlternativa(" + indice + ")")
                    divPeso.appendChild(textPeso);
                }
            }
        }

        var divColCheck = document.createElement("div");
        divColCheck.setAttribute("class", "col-md-1 col-lg-3");
        divAlternativa.appendChild(divColCheck);
        {
            var divCheck = document.createElement("div");
            divCheck.setAttribute("class", "checkbox");
            divColCheck.appendChild(divCheck);
            {
                var labelConformidade = document.createElement("label");
                labelConformidade.setAttribute("id", "chkNaoConformidade");
                labelConformidade.textContent = "Não Conformidade";
                divCheck.appendChild(labelConformidade);
                {
                    var checkConformidade = document.createElement("input");
                    checkConformidade.setAttribute("type", "checkbox");
                    checkConformidade.setAttribute("value", "");
                    checkConformidade.setAttribute("id", "Alternativas[" + alternativaId + "].NaoConformidade");
                    checkConformidade.setAttribute("name", "alternativa-naoconformidade");
                    labelConformidade.appendChild(checkConformidade);

                    var iConformidade = document.createElement("i");
                    iConformidade.setAttribute("class", "input-helper")
                    labelConformidade.appendChild(iConformidade);
                }
            }
        }

        // Coluna de Justificativa
        var divColJustificativa = document.createElement("div");
        divColJustificativa.setAttribute("class", "col-md-1 col-lg-1");
        divAlternativa.appendChild(divColJustificativa);
        {
            var divCheck = document.createElement("div");
            divCheck.setAttribute("class", "checkbox");
            divColCheck.appendChild(divCheck);
            {
                var labelConformidade = document.createElement("label");
                labelConformidade.setAttribute("id", "chkExigeJustificativa");
                labelConformidade.textContent = "Justificar?";
                divCheck.appendChild(labelConformidade);
                {
                    var checkConformidade = document.createElement("input");
                    checkConformidade.setAttribute("type", "checkbox");
                    checkConformidade.setAttribute("value", "");
                    checkConformidade.setAttribute("id", "Alternativas[" + alternativaId + "].ExigeJustificativa");
                    checkConformidade.setAttribute("name", "alternativa-exigejustificativa");
                    labelConformidade.appendChild(checkConformidade);

                    var iExigeJustificativa = document.createElement("i");
                    iExigeJustificativa.setAttribute("class", "input-helper")
                    labelConformidade.appendChild(iExigeJustificativa);
                }
            }
        }

        var divCol2 = document.createElement("div");
        divCol2.setAttribute("class", "col-md-2 col-lg-1");
        divAlternativa.appendChild(divCol2);
        {
            var buttonExcluirAlternativa = document.createElement("button");
            buttonExcluirAlternativa.setAttribute("type", "button");
            buttonExcluirAlternativa.setAttribute("class", "btn btn-danger btn-xs  m-l-0");
            buttonExcluirAlternativa.setAttribute("id", "btn" + alternativaId);
            buttonExcluirAlternativa.setAttribute("name", "btnRemoveAlternativa");
            buttonExcluirAlternativa.setAttribute("data-toggle", "modal");
            buttonExcluirAlternativa.setAttribute("onclick", "excluirAlternativa('" + alternativaId + "');");
            buttonExcluirAlternativa.textContent = "Excluir";
            divCol2.appendChild(buttonExcluirAlternativa);
        }
    }

    $(divAlternativa).fadeIn(300);
    alternativaContainer.append(divAlternativa);
    alternativasCounter.val(++indice);
};

function qtdAlternativas() {
    qtd = $("#alternativas-container").children('.alternativa').length;
    return qtd;
}

excluirAlternativa = function (alternativaId) {
    if (qtdAlternativas() >= 2) {
        var divAlternativa = $('#' + alternativaId);
        removerAnimado(divAlternativa, 300);

        return true;
    }
    swal({
        text: "Esse tipo de questão precisa de pelo menos 1 alternativa",
        title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Operação inválida!</h1>",
        showCancelButton: false,
        confirmButtonColor: "#9E9E9E",
        confirmButtonText: "Ok",
        closeOnConfirm: true,
        closeOnCancel: true,
        html: true,

    });
}

excluirQuestao = function (questaoID) {

    var divQuestao = $('#' + questaoID);
    var questaoTipo = $(divQuestao[0].childNodes[3].childNodes[1]);
   
    removerAnimado(divQuestao, 300);
    var qtdQuestoesProntas = getQuestoesProntaLength() - 1;  // O menos 1 é por que é executado antes da animação de remover

    $("#total-questoes").text(qtdQuestoesProntas);
    if (qtdQuestoesProntas == 0) {
        $("#cartao-itens").css("display", "none");
    }        
   
}


editarQuestao = function(questaoID){
    var divQuestao = $('#' + questaoID);
    abrirModalEditar(divQuestao);

}


limparQuestionario = function () {
    limparDescricaoQuestao();

    $("#chkExigeJustificativa").prop('checked', false);
    $("#chkExigeResposta").prop('checked', false);

    $("#questao-subjetiva").prop('checked', true);
    $("#questao-subjetiva").val('Subjetiva');
    $('#multipla-ou-objetiva').hide();

    $('div[id^=alternativa]', $("#alternativas-container")).remove();
};


// Função feita para remover um elmento de forma animada 
function removerAnimado(elemento, tempo) {
    elemento.animate({
        padding: "0px",
        'height': '0px',
        'opacty': '0px',
        'border': '0px',
        'box-shadow': 'none'

    }, tempo, function () {


        elemento.remove();
        $("#total-subjetivos").text($('.subjetivo').length);
        $("#total-objetivos").text($('.objetivo').length);
        $("#total-multiplas").text($('.multipla-escolha').length);
        $("#total-pontuacao").text(getPontuacaoTotalQuestoes());
    });
}




