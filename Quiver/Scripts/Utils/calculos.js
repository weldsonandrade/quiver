
function calcularPorcentagensDasAvaliacoes() {
    var maximo = parseInt($('#valor-total')[0].innerHTML);


    var finalizadas = parseInt($('#valor-finalizadas')[0].childNodes[0].nodeValue);
    var atrasadas = parseInt($('#valor-atrasadas')[0].childNodes[0].nodeValue);
    var andamentos = parseInt($('#valor-andamento')[0].childNodes[0].nodeValue);
    var agendadas = parseInt($('#valor-agendadas')[0].childNodes[0].nodeValue);
    var naoAgendadas = parseInt($('#valor-nao-agendadas')[0].childNodes[0].nodeValue);

    // Calcula o percentual e caso tenha valores reais seta para que ele pegue apenas 1 caracter após o ponto
    var percentFinalizadas = fixarTamanhoPosPonto(((finalizadas * 100) / maximo), 1);
    var percentAtrasadas = fixarTamanhoPosPonto(((atrasadas * 100) / maximo), 1);
    var percentAndamentos = fixarTamanhoPosPonto(((andamentos * 100) / maximo), 1);
    var percentAgendadas = fixarTamanhoPosPonto(((agendadas * 100) / maximo), 1);
    var percentNaoAgendadas = fixarTamanhoPosPonto(((naoAgendadas * 100) / maximo), 1);


    $('#percentual-finalizadas')[0].textContent = "(" + percentFinalizadas + "%)";
    $('#percentual-atrasadas')[0].textContent = "(" + percentAtrasadas + "%)";
    $('#percentual-andamento')[0].textContent = "(" + percentAndamentos + "%)"
    $('#percentual-agendadas')[0].textContent = "(" + percentAgendadas + "%)"
    $('#percentual-nao-agendadas')[0].textContent = "(" + percentNaoAgendadas + "%)"
    return null;
}

function fixarTamanhoPosPonto(value, tamanho) {
    if (value.toString().indexOf('.') > -1) {
        return value.toFixed(tamanho);
    }
    else return value;
}
