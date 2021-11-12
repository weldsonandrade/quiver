
function gerarAlternativas(countAlternativasAtuais){
       var divGridAlternativas = criarElementoDiv("editar-multipla-ou-objetiva", "col-md-12 p-b-10 p-t-10 m-t-20", document.createElement("div"));
       var label = criarElementoSmall("", "", "","Lista de Alternativas");
       divGridAlternativas.appendChild(label);

      var divalternativaContainer = criarElementoDiv("editar-alternativas-container", "col-md-12 ", divGridAlternativas );



     for(i = 0; i < countAlternativasAtuais; i++) { 
      gerarNovaLinhaEditarAlternativa("#alternativasEditarCounter", "#editar-alternativas-container");
    }

      return divGridAlternativas;
}



function countAlternativasAtuais(itemID){
 var doc = document.getElementById("listasAlternativas"+itemID);
    var count = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("alternativaDescricao")) {
            count++;
        }
    }
    return count;
}


// Gera as alternativas lá no card de Adicionar questão
function gerarNovaLinhaEditarAlternativa( idCounter, idContainerAlternativas ) {

    // Primeiro vamos contar quantas alternativas já existem
     if (getAlternativaLength(idContainerAlternativas) == 0) {
        $(idCounter).val(0);
     }

    var alternativasCounter = $(idCounter);

    var alternativaContainer = $(idContainerAlternativas);

    var indice = alternativasCounter.val();

    var alternativaId = "alternativa" + indice;

    var divAlternativa = document.createElement("div");
    divAlternativa.setAttribute("id", alternativaId);
    divAlternativa.setAttribute("class", "alternativa col-md-12 m-t-5 draggable");


    alternativaContainer.append(divAlternativa);
    alternativasCounter.val(++indice);
}



// Usado para adicionar õ item objetivo ou múltiplo escolha no formulário
function getAlternativaLength(idContainerAlternativas) {

    var doc = document.getElementById("editar-alternativas-container");
  //  var doc = $("#editar-alternativas-container");
  //  doc = doc.context;

    var count = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("alternativa")) {
            count++;
        }
    }
    return count;
  
}



 /* {
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
    */


