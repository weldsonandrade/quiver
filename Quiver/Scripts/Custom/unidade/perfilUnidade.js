document.body.onload = function () {
    var opcaoMenu = document.getElementById("menu_unidade");
    opcaoMenu.className = opcaoMenu.className + "  active";

}

$('#dataInicial').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$('#dataFinal').datetimepicker({
    format: 'DD/MM/YYYY',
    locale: 'pt-br'
});

$(document).ready(function () {
     filtrar();
});


function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Unidade/Tabela', { "termo": termo }, function () { loadBootGridInspecao(); });
}


function filtrar() {
    // Aplicando o pre-loading 
    $('#tabela-avaliacoes').css('display', 'block');
    $('#tabela-avaliacoes').append(HTMLLoaderCard('Carregando tabela de inspeções.'));
 //   $('#grafico-body').append(HTMLLoaderCard('Carregando gráfico de eficiência.'));
  //  $('#grafico-body-qtd').append(HTMLLoaderCard('Carregando gráfico quantitativo.'));

    var dataInicial = $('#dataInicial').val();
    var dataFinal = $('#dataFinal').val();

    $('#tabela-avaliacoes').load('/Unidade/PerfilAvaliacoes', { "unidadeId": $('#Id').val(), "dataInicial": $('#dataInicial').val(), "dataFinal": $('#dataFinal').val() }, function () {
        if (parseInt($('#valor-total')[0].innerHTML) > 0) {
            loadBootGridInspecao();
            $('#tabela-sem-avaliacoes').css('display', 'none');
            $('#tabela-avaliacoes').css('display', 'block');

//            getDataSetGraficoEvolutivoPerfil($('#Id').val(), dataInicial, dataFinal);
 //           getDataSetGraficoQuantitativoPerfil($('#Id').val(), dataInicial, dataFinal);
        } else {
            $('#tabela-sem-avaliacoes').css('display', 'block');
            $('#tabela-avaliacoes').css('display', 'none');
        //    $('#card-grafico').css('display', 'none');
         //   $('#card-grafico-qtd').css('display', 'none');
        }
    });
}

function telaAvaliado(agendamentoId) {
    var NestId = $(this).data('id');
    var url = "/Agenda/AvaliacaoFinalizada?agendamentoId=" + agendamentoId;
    //  window.location.href = url;
    window.open(url, '_blank'); // abrindo outro tab
}
