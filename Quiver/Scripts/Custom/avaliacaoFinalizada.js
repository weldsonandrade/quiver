window.onload = function () {

}

$(document).ready(function () {
    var grupoId = $('#grupoId').val();
    $('#grupo-classificacoes').load('/Agenda/GetClassificacaoGrupoAvaliado', { "grupoId": grupoId }, function () { });


    //  google.maps.event.addDomListener(window, "load", getMapa);
    var latitude = $('#hiddenLatitude').val();
    var longitude = $('#hiddenLongitude').val();

    $('#card-local').load(getMapa(latitude , longitude ));
    fazMarcacaoNoMapa(latitude.replace(",", "."), longitude.replace(",", "."));

});

$(document).ajaxStop(function () {
    calcularPercentualRespondido();

});


function f() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Usuario/Tabela', { "termo": termo }, function () {  });
}



function calcularPercentualRespondido() {
    if (($('#pontuacao').length > 0))
    {
        var maximo = parseInt($('#maxima')[0].innerText);
        var efetuado = parseInt($('#efetuada')[0].innerText);

        var percentual = ((efetuado * 100) / maximo).toFixed(1);

        if( (percentual.indexOf('.') > -1) && (percentual[percentual.length -1] == "0")){
            percentual = percentual.substring(0, percentual.indexOf('.'));
        }
      //  percentual = Math.round(percentual).toFixed(2);

        var texto = (isNaN(percentual)) ? 'Valor indisponível' : percentual + '%';

        var valor;
        $('.cor-intervalo').each(function (i, obj) {
            valorMaximo = parseInt(obj.id)+0.9;
            valorMinimo = parseInt($('#' + obj.id).attr("name"));

            percentual = (isNaN(percentual)) ? 0 : percentual;


            if (percentual >= valorMinimo && percentual <= valorMaximo) {
                // a efetividade encontra-se nessa faixa 
                $('#efetividade-item').css('background', "#" + $('#' + obj.id).val());

                //        // Pegando o texto da faixa percentual encontrada 
                if (texto != 'Valor indisponível') {
                    var classificacaoTexto = $('#icone-' + obj.id).attr('name');
                    classificacaoTexto = " - " + classificacaoTexto.slice(0, classificacaoTexto.indexOf("("));
                    $('#efetividade').text(texto + classificacaoTexto);
                } else {
                    $('#efetividade').text(texto);
                }
                return percentual;
            }
        });
        return percentual;
    }
}
