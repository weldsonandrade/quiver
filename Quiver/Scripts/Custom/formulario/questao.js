function showTipo(idTipo)
{
    if ((idTipo == 'questao-objetiva') || (idTipo == 'questao-multipla'))
    {
        $('#multipla-ou-objetiva').show();
        gerarAlternativasIniciais();
    } else
    {
        $('#multipla-ou-objetiva').hide();
    }
};

 function gerarAlternativasIniciais()
 {
     var qtdAlternativas = $("#alternativas-container").children('.alternativa').length;
     if (qtdAlternativas == 0)
     {
         gerarNovaLinhaAlternativa();
         gerarNovaLinhaAlternativa();
     }
 }




