
// ******** RESETAR CAMPO **********************//

function resetarCampo(campo, formGroupID, smallMensagem)
{
    if (document.getElementById(campo) != null) {
        $("#" + campo).val('');
        $(formGroupID + ">div.fg-line").removeClass("fg-toggled");
        $(formGroupID).removeClass('has-success');
        $(formGroupID).removeClass('has-error');
        $(smallMensagem).css('visibility', 'visible');
    }
}

function resetarData(dateID) {
    $(dateID).data("DateTimePicker").destroy();
    $(dateID).datetimepicker({
        format: 'DD/MM/YYYY',
        locale: 'pt-br',
    });
}


//******** VALIDACOES ***********************//


function validarCampoBranco(campo, formGroupID, smallMensagem) {

    if (document.getElementById(campo) != null) {
        var tamanho = document.getElementById(campo).value.trim().length;

        if (tamanho == 0) {
            $(formGroupID).removeClass('has-success');
            $(formGroupID).addClass('has-error');
            $(smallMensagem).css('visibility', 'visible');
            return false;
        }
        $(formGroupID).addClass('has-success');
        $(smallMensagem).css('visibility', 'hidden');
        $(formGroupID).removeClass('has-error');
        return true;
    }
    return null;

}

function validarSenha(campo, formGroupID, smallMensagem) {
    var validacaoSenha = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,32}/;
    if ($('#' + campo).val().match(validacaoSenha)) {
        $(formGroupID).addClass('has-success');
        $(smallMensagem).css('visibility', 'hidden');
        $(formGroupID).removeClass('has-error');
        return true;
    }
    $(smallMensagem).css('visibility', 'visible');
    $(formGroupID).addClass('has-error');
    $(formGroupID).removeClass('has-success');
    return false;
}

function validarDropdowSelected(dpw, formGroupID, smallMensagem) {

    if ($('#' + dpw).val() <= 0) {
        $(smallMensagem).css('visibility', 'visible');
        $(formGroupID).addClass('has-error');
        $(formGroupID).removeClass('has-success');
        return false;
    }
    else {
        $(smallMensagem).css('visibility', 'hidden');
        $(formGroupID).addClass('has-success');
        $(formGroupID).removeClass('has-error');
        return true;
    }
}

function compararDoisItens(campo, formGroupID, smallMensagem, campoParaComparar) {
    campo1 = $('#' + campoParaComparar)[0].value;
    campo2 = $('#' + campo)[0].value;

    if (campo1 != campo2) {
        $(formGroupID).removeClass('has-success');
        $(formGroupID).addClass('has-error');
        $(smallMensagem).text('Senha e confirmação de senha estão diferentes');
        $(smallMensagem).css('visibility', 'visible');
        return false;
    } else {
        $(formGroupID).addClass('has-success');
        $(smallMensagem).css('visibility', 'hidden');
        $(formGroupID).removeClass('has-error');
        return true;
    }

}


function validarTamanhoMinimo(campo, formGroupID, smallMensagem, minimo) {
    var tamanho = document.getElementById(campo).value.trim().length;

    if (tamanho < minimo) {
        $(formGroupID).removeClass('has-success');
        $(formGroupID).addClass('has-error');
        $(smallMensagem).text('A senha tem que ter no mínimo 6 digitos');
        $(smallMensagem).css('visibility', 'visible');
        return false;
    }
    $(formGroupID).addClass('has-success');
    $(smallMensagem).css('visibility', 'hidden');

    $(formGroupID).removeClass('has-error');
    return true;

}





function validarEmail(email, formGroupID, smallMensagem) {
    emailLength = $('#' + email)[0].value.trim().length;


    if (emailLength != 0) {
        var re = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
        // Estudar melhor isso aqui
        if (re.test($('#' + email)[0].value) == false) {
            $(formGroupID).removeClass('has-success');
            $(formGroupID).addClass('has-error');
            $(smallMensagem).text('O email está inválido');
            $(smallMensagem).css('visibility', 'visible');
            return false;
        }
        else {
            $(formGroupID).addClass('has-success');
            $(formGroupID).removeClass('has-error');
            $(smallMensagem).css('visibility', 'hidden');

            return true;
        }
    } else {
        $(formGroupID).removeClass('has-success');
        $(formGroupID).addClass('has-error');
        $(smallMensagem).text('O email é obrigatório');
        $(smallMensagem).css('visibility', 'visible');
        return false;
    }
}


function validarData(campoId, formGroupID, smallMensagem) {
    if (moment($('#' + campoId)[0].value, "DD/MM/YYYY").isValid() == false) {

        $(formGroupID).removeClass('has-success');
        $(formGroupID).addClass('has-error');
        $(smallMensagem).css('visibility', 'visible');
        return false;
    }
    $(formGroupID).addClass('has-success');
    $(formGroupID).removeClass('has-error');
    $(smallMensagem).css('visibility', 'hidden');
    return true;
}


// Validar que o checklist tenha no mínimo um dia marcado
function qtdMinimoItensMarcados(checkboxClass, mensagemErroID, qtdMinima) {

    var qtdItensMarcados = $('.' + checkboxClass + ':checked').size();

    // Verificar se existe pelo menos um dia marcado
    if (qtdItensMarcados >= parseInt(qtdMinima)) {
        $('#' + mensagemErroID).css('display', 'none');
    }
    else {
        $('#' + mensagemErroID).css('display', 'block');
    }
}


function manterMinimodeItensMarcados(inputCheckboxID, checkboxClass, qtdMinima) {

    var qtdItensMarcados = $('.' + checkboxClass + ':checked').size();
    if (qtdItensMarcados == parseInt(qtdMinima) && $(inputCheckboxID).is(":checked")) {
        $(inputCheckboxID).attr("disabled", true);
    } else {
        $('.' + checkboxClass + ':checked').attr('disabled', false);
    }
}


/* CAMPOS HTML CRIADOS DINAMICAMENTE */
function createFloatingLabel(classesFormGroup, idFormGroup, idInput, nameInput, idErroMensagem, textTitulo, textErro, validateFunctionName) {
    // Exemplo no HTML
    //<div class="form-group fg-float col-md-12 col-lg-7" id="item-nome">
    //    <div class="fg-line">
    //      <input type="text" id="txtDescricao" name="txtDescricao" value=""  class="input-sm form-control fg-input" onkeyup="validarDescricaoQuestao()" onfocusout="validarDescricaoQuestao()" />
    //       <label class="fg-label">Descrição</label>
    //     </div>
    //     <small class="help-block" id="titulo-questao-invalida">O nome da Unidade é obrigatório.</small>
    //</div>

    var divColItem = document.createElement("div");
    divColItem.setAttribute("class", "form-group fg-float " + classesFormGroup);
    divColItem.setAttribute("id", idFormGroup);
    {
        var divFgLine = document.createElement("div");
        divFgLine.setAttribute("class", "fg-line");
        divColItem.appendChild(divFgLine);
        {
            var inputText = document.createElement("input");
            inputText.setAttribute("id", idInput);
            inputText.setAttribute("class", "input-sm form-control fg-input");
            inputText.setAttribute("name", nameInput);
            inputText.setAttribute("type", "text");
            inputText.setAttribute('onkeyup', validateFunctionName);

            var labelTitulo = document.createElement("label");
            labelTitulo.setAttribute("class", "fg-label");
            labelTitulo.textContent = textTitulo;

            divFgLine.appendChild(inputText);
            divFgLine.appendChild(labelTitulo);
        }

        var smallTextErro = document.createElement("small");
        smallTextErro.setAttribute("id", idErroMensagem);
        smallTextErro.setAttribute("class", "help-block");
        smallTextErro.textContent = textErro;
        divColItem.appendChild(smallTextErro);
    }


    return divColItem;

}

/*  FIM CAMPOS HTML CRIADOS DINAMICAMENTE */


//* MAPAS */
function fazMarcacaoNoMapa(lat, long) {
    var latlong = lat + "," + long;

    //colocando na conficuracao necessaria (lat,long) 
    var myLatLgn = new google.maps.LatLng(lat, long);
    //criando variavel tipo google.maps.LatLng e 
    //passando como parametro a latitude e longitude 
    //na configuracao: latitude,longitude. 
    //aproximando o mapa, aumentando o zoom 
    map.setZoom(17);
    //fazendo a marcacao, usando o latitude e longitude da variavel criada acima 
    var marker = new google.maps.Marker({ position: myLatLgn, map: map });
    //aqui a variavel e o comando que faz a marcação 
    map.setCenter(myLatLgn);
    //leva o mapa para a posicao da marcacao
}

//variavel para que seja criado o mapa Google Maps
var map = null;

//Essa e a funcao que criara o mapa GoogleMaps
function getMapa(latitude, longitude) {

    //aqui vamos definir as coordenadas de latitude e longitude no qual sera exibido o  mapa
    var latlng = new google.maps.LatLng(latitude, longitude);

    //aqui vamos configurar o mapa, como o zoom, o centro do mapa, etc
    var myOptions = {
        zoom: 17,//utilizaremos o zoom 15
        center: latlng,
        scrollwheel: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    //criando o mapa dentro da div com o id="map_canvas" que ja criamos
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    //DEFINE AS COORDENADAS do ponto exato - CENTRALIZAÇÃO DO MAPA
    map.setCenter(new google.maps.LatLng(latitude, longitude));
}



//* FIM DE MAPAS */

/* GALERIA DE IMAGENS NO MODAL*/

// Vou usar isso aqui para recolocar o modal na posicao Y referente quando ele foi clicado pois sem ele vai para o topo


// DESABILITAR SCROOL NA IMAGEM

// left: 37, up: 38, right: 39, down: 40,
// spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36
var keys = { 37: 1, 38: 1, 39: 1, 40: 1 };

function preventDefault(e) {
    e = e || window.event;
    if (e.preventDefault)
        e.preventDefault();
    e.returnValue = false;
}

function preventDefaultForScrollKeys(e) {
    if (keys[e.keyCode]) {
        preventDefault(e);
        return false;
    }
}


function disableScroll(modalID) {
    if (window.addEventListener) // older FF
        window.addEventListener('DOMMouseScroll', preventDefault, false);
    window.onwheel = preventDefault; // modern standard
    window.onmousewheel = document.onmousewheel = preventDefault; // older browsers, IE
    window.ontouchmove = preventDefault; // mobile
    document.onkeydown = preventDefaultForScrollKeys;
    $("#" + modalID).css("overflow", "hidden");
}

function enableScroll(modalID) {
    if (window.removeEventListener)
        window.removeEventListener('DOMMouseScroll', preventDefault, false);
    window.onmousewheel = document.onmousewheel = null;
    window.onwheel = null;
    window.ontouchmove = null;
    document.onkeydown = null;
    $("#" + modalID).css("overflow", "auto");
}


/* FIM GALERIA DE IMAGENS NO MODAL*/


function tooltipOnHover(elemento) {
    id = elemento.id;
    //if (element.id == 'observacao') {
    //    element = 'observacao';
    //} 
    var qtdCaracteres = $('#' + id)[0].innerHTML.length;
    if (qtdCaracteres > 20) {
        var n = qtdCaracteres / 2;
    } else {
        var n = qtdCaracteres;
    }
    $('#' + id).css('width', n + 'ch');
}


// Converte a data DD/MM/YYYY para o formato YYYY-MM-DD
function convertPadraoeData(dataString) {
    var ano = dataString.substring(6, 10);
    var mes = dataString.substring(3, 5);
    var dia = dataString.substring(0, 2);

    var dataConvertida = ano + '-' + mes + '-' + dia;
    return dataConvertida;

}


// Converte a data DD/MM/YY para o formato YYYY-MM-DD
function convertPadraoeData2(dataString) {
    var ano = dataString.substring(6, 8);
    var mes = dataString.substring(3, 5);
    var dia = dataString.substring(0, 2);

    var dataConvertida =  '20'+ano + '-' + mes + '-' + dia;
    return dataConvertida;

}

// Converte a data DD/MM/YY 00:00:00 para o formato DD/MM/YYYY
function convertPadraoeData3(dataString) {
    var ano = dataString.substring(6, 8);
    var mes = dataString.substring(3, 5);
    var dia = dataString.substring(0, 2);
    return dia + '/' + mes + '/20' + ano;
}

// Converte a data DD/MM/YYYY para o formato MM/DD/YYYY
function convertPadraoeData4(dataString) {
    var ano = dataString.substring(6, 10);
    var mes = dataString.substring(3, 5);
    var dia = dataString.substring(0, 2);

    var dataConvertida =    mes + '/' + dia + '/' + ano ;
    return dataConvertida;
}

// Converte a data DD/MM/YYYY para o formato MM/DD/YY
function convertPadraoeData5(dataString) {
    var ano = dataString.substring(8, 10);
    var mes = dataString.substring(3, 5);
    var dia = dataString.substring(0, 2);

    var dataConvertida = mes + '/' + dia + '/' + ano;
    return dataConvertida;
}



function linkarDataInicialDataFinal( inicialId, finalId ) {

    $(inicialId).on("dp.change", function (e) {
        if (e.date != null) {
            $(finalId).data("DateTimePicker").minDate(e.date);
        } else {
            $(finalId).data("DateTimePicker").minDate(new Date("0001/01/01"));
        }
    });


    $(finalId).on("dp.change", function (e) {
        if (e.date != null) {
            $(inicialId).data("DateTimePicker").maxDate(e.date);
        } else {
            $(inicialId).data("DateTimePicker").maxDate(new Date("2999/12/29"));
        }
    });


}

// Tooltip de do tipo email nas tabelas
function retornarParteDoTexto(texto, qtd, id) {
    var tamanho = texto.length;
    if (tamanho > qtd) {
        return "<div class='wrapper' >" + texto.substring(0, qtd) + '...' +
            "<div class='tooltip' id=" + "usuario" + id + " >" + texto + "</div></div> ";
    }
    return texto;
}


var UID = {
    _current: 0,
    getNew: function () {
        this._current++;
        return this._current;
    }
};

HTMLElement.prototype.pseudoStyle = function (element, prop, value) {
    var _this = this;
    var _sheetId = "pseudoStyles";
    var _head = document.head || document.getElementsByTagName('head')[0];
    var _sheet = document.getElementById(_sheetId) || document.createElement('style');
    _sheet.id = _sheetId;
    var className = "pseudoStyle" + UID.getNew();
    _this.className += " " + className;
    _sheet.innerHTML += " ." + className + ":" + element + "{" + prop + ":" + value + "}";
    _head.appendChild(_sheet);
    return this;
};




/* HEIGHT AND WIDTH NAVIGATOR */
function getHeightNavigator() {
    var myHeight = 0;
    if (typeof (window.innerHeight) == 'number') {
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientHeight)) {
        //IE 4 compatible
        myHeight = document.body.clientHeight;
    }
    return  myHeight;
}

function getWidthNavigator() {
    var myWidth = 0;
    if (typeof (window.innerWidth) == 'number') {
        myWidth = window.innerWidth;
    } else if (document.documentElement && (document.documentElement.clientWidth)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
    } else if (document.body && (document.body.clientWidth)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
    }
    return myWidth;
}


//*************** HTML Carregando CARD ****************************//
function HTMLLoaderCard(textoCard) {
    var divLoaderCard = document.createElement("div");
    divLoaderCard.setAttribute('class', 'loader-card');
    divLoaderCard.innerHTML = '<div class="loader-text" > ' + textoCard + ' <i class="zmdi-hc-li zmdi zmdi-refresh zmdi-hc-spin"></i></div>';
    return divLoaderCard;
}

function HTMLLoaderCallendar(textoCard) {
    var divLoaderCard = document.createElement("div");
    divLoaderCard.setAttribute('class', 'loader-card-callendar');
    divLoaderCard.innerHTML = '<div class="loader-text" style="color:white"> ' + textoCard + ' <i class="zmdi-hc-li zmdi zmdi-refresh zmdi-hc-spin"></i></div>';
    return divLoaderCard;
}

/******************** CHAMA INSPEÇÂO  FINALIZADA ********************************/
function telaAvaliado(agendamentoId) {
    var NestId = $(this).data('id');
    var url = "/Agenda/AvaliacaoFinalizada?agendamentoId=" + agendamentoId;
    //  window.location.href = url;
    window.open(url, '_blank'); // abrindo outro tab
}


function calcularPercentualRespondido(maximo, efetuado) {
    var percentual = (efetuado / maximo) * 100;
    percentual = percentual.toFixed(2);
    if (percentual.substr(percentual.indexOf('.')) == ".00") {
        return percentual.substr(0, percentual.indexOf('.'));
    }
    return percentual;
}


function limparFilhos(idPai) {
    var myNode = document.getElementById(idPai);
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }
}




function telaAvaliado(agendamentoId) {

    var NestId = $(this).data('id');
    var url = "/Agenda/AvaliacaoFinalizada?agendamentoId=" + agendamentoId;
    //  window.location.href = url;
    window.open(url, '_blank'); // abrindo outro tab
}
