function abrirModalEditar(divQuestao) {
    var itemTipo = "";
    var itemTitulo = "";
    var exigeResposta = ""
    var alternativas = "";
    if ($(divQuestao).hasClass("questao-pronta-HTML")) {
        itemTitulo = divQuestao[0].childNodes[7].childNodes[3].innerText;
        exigeResposta = divQuestao[0].childNodes[11].childNodes[3].childNodes[1].value;
        itemTipo = divQuestao[0].childNodes[13].childNodes[3].className;
        alternativas = divQuestao[0].childNodes[17];
    } else {
        itemTitulo = divQuestao[0].childNodes[0].childNodes[1].innerText;
        exigeResposta = divQuestao[0].childNodes[1].childNodes[1].innerText;
        itemTipo = divQuestao[0].childNodes[2].childNodes[1].className;
        alternativas = divQuestao[0].childNodes[11];
    }

    itemTipo = itemTipo.substring(4); // Para pegar somente a classe com o tipo de pergunta

    var itemID = divQuestao[0].id;



   $('#myModalEditarItem').modal('show');
    jQuery('#myModalBodyEditarItem').html(''); // limpa todo o HTML do body
  
   montarBodyModal(itemID, itemTitulo,itemTipo , exigeResposta);

    validarDescricaoItem();


        // Caso precise vai montar as alternativas
    if( itemTipo == "multipla-escolha" || (itemTipo == "objetivo")){

        myModalBodyEditarItem.append(gerarAlternativas(countAlternativasAtuais(itemID)));
    } 
   
}




   

function montarBodyModal ( idItem, textTituloItem, itemTipo, exigeResposta ){


    var myModalBodyEditarItem = $('#myModalBodyEditarItem');
    // Criando um elemento invisível para capturar o id do Item a ser editado
    $('#myModalBodyEditarItem').append('<input type="hidden" name="myfieldname" id="itemID" value=' + idItem + ' />');

    // Motando campo para editar item
    var divcolunaTitulo = criarElementoDiv("item-Descricao-editar", "form-group fg-float col-md-12 col-lg-9 m-t-10  m-b-10", document.createElement("div"));
    var divFgLine = criarElementoDiv("", "fg-line fg-toggled", divcolunaTitulo);
    var inputDescricao = criarElementoInput("text", "input-sm form-control fg-input", "", "txtEditarTitulo", "txtEditarDescricao", 10, textTituloItem, null);

    inputDescricao.setAttribute("onkeyup", "validarDescricaoItem()");
    inputDescricao.setAttribute("onfocusout", "validarDescricaoItem()");

    var labelTitulo = criarElementoLabel("labelTituloDescicao", "fg-label", "Título do Item");

    divFgLine.appendChild(inputDescricao);
    divFgLine.appendChild(labelTitulo);

    var labelValidarTituloItem = criarElementoSmall("help-block", "titulo-item-modal-validar", "visibility: visible;", "O título do item é obrigatório.");

    divcolunaTitulo.appendChild(labelValidarTituloItem);
    // FIM Motando campo para editar item

    // Montando CheckBox exige resposta

        var divcolunaExigeresposta = criarElementoDiv("item-CheckBox-ExigeResposta", "col-md-3 ", document.createElement("div"));

        var labelValidarTituloItem = criarElementoSmall("", "","visibility: visible;","Opções do item");
        divcolunaExigeresposta.appendChild(labelValidarTituloItem);

        var divRow = criarElementoDiv("","row col-md-12",divcolunaExigeresposta);
        var divCheckBox = criarElementoDiv("","checkbox m-t-5",divRow);
      
        var labelCheckBox = criarElementoLabel("", "", "");
    

        var inputCheckBox = criarElementoInput("checkbox", null,null ,null , "chkModelExigeResposta", 10,null, null);

       
        labelCheckBox.appendChild(inputCheckBox);
        var iCheckbox = criarElemento("i",null,"input-helper");
        iCheckbox.append("Exige Resposta?");
        labelCheckBox.appendChild(iCheckbox);

        divCheckBox.appendChild(labelCheckBox);
    // FIM Montando CheckBox exige resposta


    var labelValidarTituloItem = criarElementoSmall("", "", "visibility: visible;", "Opções do item");
    divcolunaExigeresposta.appendChild(labelValidarTituloItem);

    var divRow = criarElementoDiv("", "row col-md-12", divcolunaExigeresposta);
    var divCheckBox = criarElementoDiv("", "checkbox m-t-5", divRow);

    var labelCheckBox = criarElementoLabel("", "", "");


     myModalBodyEditarItem.append(divcolunaTitulo);
     myModalBodyEditarItem.append(divcolunaExigeresposta); 
 
 //    myModalBodyEditarItem.append(getLayoutTipoItem());


    // Caso precise vai montar as alternativas
//    if( itemTipo == "multipla-escolha" || (itemTipo == "objetivo")){
//
   //     myModalBodyEditarItem.append(gerarAlternativas(countAlternativasAtuais(idItem)));
 //   } 

    labelCheckBox.appendChild(inputCheckBox);
    var iCheckbox = criarElemento("i", null, "input-helper");
    iCheckbox.append("Exige Resposta?");
    labelCheckBox.appendChild(iCheckbox);


     // Preenche o checkbox de pergunta obrigatória 
         if(exigeResposta == "Sim"){
            $("#chkModelExigeResposta").prop('checked', true);
        }else {
            $("#chkModelExigeResposta").prop('checked', false);    
        }   

           // Preenche o radioButton com o tipo de item 
        if( itemTipo == "multipla-escolha")
        {
             $("#item-multipla").prop("checked", true); 
        } 
        else if( itemTipo == "objetivo") 
        {
         $("#item-objetiva").prop("checked", true);
        }
        else if(itemTipo == "subjetivo")
        {
         $("#item-subjetiva").prop("checked", true); 
        } 

        $('input[name=sampleEditar]').attr("disabled",true);


        //Motagem de Alternativas

    myModalBodyEditarItem.append(divcolunaTitulo);
    myModalBodyEditarItem.append(divcolunaExigeresposta);
 
    // Preenche o checkbox de pergunta obrigatória 
    if (exigeResposta == "Sim") {
        $("#chkModelExigeResposta").prop('checked', true);
    } else {
        $("#chkModelExigeResposta").prop('checked', false);
    }
}




function getLayoutTipoItem(){
      var divcolunaTipoItem = criarElementoDiv("", "radio col-md-7 opcoes-alternativas m-t-0", document.createElement("div"));

       var labelTipoItem = criarElementoSmall("", "","visibility: visible;","Tipo do Item");
      divcolunaTipoItem.appendChild(labelTipoItem);

      var divcolunaRadioBtnTipo = criarElementoDiv("radioEditar", "area-tipo-questao", divcolunaTipoItem);

    getOpcaoRadioButton(divcolunaRadioBtnTipo, "Subjetivo");
    getOpcaoRadioButton(divcolunaRadioBtnTipo, "Multpla Escolha");
    getOpcaoRadioButton(divcolunaRadioBtnTipo, "Objetivo");


      return divcolunaTipoItem;
}

function getOpcaoRadioButton(divcolunaRadioBtnTipo, iText) {
    var divcolunaOpcao = criarElementoDiv("", "col-md-4 p-t-10 p-l-0", divcolunaRadioBtnTipo);
    var label = criarElementoLabel("", "", "");

    var inputRadioTipo = "";
    if (iText == "Subjetivo") {
        inputRadioTipo = criarElementoInput("radio", null, null, "sampleEditar", "item-subjetiva", 10, "Subjetiva", null);
    } else if (iText == "Multpla Escolha") {
        inputRadioTipo = criarElementoInput("radio", null, null, "sampleEditar", "item-multipla", 10, "ObjetivaMultiplaEscolha", null);
    } else if (iText == "Objetivo") {
        inputRadioTipo = criarElementoInput("radio", null, null, "sampleEditar", "item-objetiva", 10, "ObjetivaUnicaEscolha", null);

      }

      inputRadioTipo.setAttribute("onclick", "showAlternativas(this.id)");
      var iRadio = criarElemento("i",null,"input-helper");
      iRadio.innerText = iText;
      label.appendChild(inputRadioTipo);
      label.appendChild(iRadio);
      return divcolunaOpcao.appendChild(label);
}

function gerarAlternativas() {
    var divGridAlternativas = criarElementoDiv("editar-multipla-ou-objetiva", "col-md-12 p-b-10 p-t-10 m-t-20", document.createElement("div"));
    var label = criarElementoSmall("", "", "", "Lista de Alternativas");
    divGridAlternativas.appendChild(label);

    var divalternativaContainer = criarElementoDiv("editar-alternativas-container", "col-md-12 ", divGridAlternativas);


    gerarNovaLinhaEditarAlternativa("#alternativasEditarCounter", "#editar-alternativas-container");
    gerarNovaLinhaEditarAlternativa("#alternativasEditarCounter", "#editar-alternativas-container");

    return divGridAlternativas;
}

/* VALICAÇÕES DO MODAL */

function validarDescricaoItem() {
    var resposta = validarCampoBranco('txtEditarDescricao', '#item-Descricao-editar', '#titulo-item-modal-validar');

    if (resposta == true) {
        $('#salvarItemFormulario').prop('disabled', false);
    } else {
        $('#salvarItemFormulario').prop('disabled', true);
    }
    return resposta;
}

function validarQuestao() {

}


/* SALVAR  MOFICIAÇÕES*/
function salvarEdicoes() {
    var idItem = $('#itemID').val();
    var idNumber = idItem.match(/\d+/);
    var questaoItem = $('#' + idItem);

    var inputHiddenDescricao = criarElementoInput("hidden", "", "", "Questoes[linha" + idNumber + "].Descricao", "Questoes_linha" + idNumber + "__Descricao", 10, $('#txtEditarDescricao').val(), "");

    if ($(questaoItem).hasClass("questao-pronta-HTML")) {
        // Título reescrito
        $(questaoItem[0].childNodes[7].childNodes[3]).text($('#txtEditarDescricao').val());



        var idInputCheckbox = "#Questoes_linha" + idNumber[0] + "__ExigeResposta";
        if ($('#chkModelExigeResposta')[0].checked) {
            $(questaoItem[0].childNodes[11].childNodes[3].childNodes[1]).val("Sim");
            // Adicionando o valor do chekbox no input invisível que passa o valor ao controller
            $(idInputCheckbox).val("True");
        } else {
            $(questaoItem[0].childNodes[11].childNodes[3].childNodes[1]).val("Não");
            $(idInputCheckbox).val("False");
        }

    } else {
        $(questaoItem[0].childNodes[0].childNodes[1]).text($('#txtEditarDescricao').val());
        $(questaoItem[0].childNodes[0].childNodes[1]).appendChild(inputHiddenDescricao);

        if ($('#chkModelExigeResposta')[0].checked) {
            $(questaoItem[0].childNodes[1].childNodes[1]).text("Sim");
            $(idInputCheckbox).val("True");
        } else {
            $(questaoItem[0].childNodes[1].childNodes[1]).text("Não");
            $(idInputCheckbox).val("False");
        }
    }

    document.getElementById("descricaoItem-questao" + idNumber).appendChild(inputHiddenDescricao);


    $('#myModalEditarItem').modal('hide');

    return true;
}

// Deixar invisível Alternativas caso o item seja subjetivo
function showAlternativas(idTipo) {
    if ((idTipo == 'item-objetiva') || (idTipo == 'item-multipla')) {
        $('#editar-multipla-ou-objetiva').show();
        //  gerarAlternativasIniciais();
    } else {
        $('#editar-multipla-ou-objetiva').hide();
    }
}


// Gera as alternativas lá no card de Adicionar questão
function gerarNovaLinhaEditarAlternativa(idCounter, idContainerAlternativas) {

    if (getAlternativasLength(idContainerAlternativas) == 0) {
        $(idCounter).val(0);
    }

    var alternativasCounter = $(idCounter);

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
}

// Usado para adicionar õ item objetivo ou múltiplo escolha no formulário
function getAlternativasLength(idContainer) {
    var doc = document.getElementById(idContainer);
    var count = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("alternativa")) {
            count++;
        }
    }
    return count;
}
