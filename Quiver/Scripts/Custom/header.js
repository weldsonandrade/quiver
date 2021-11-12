var possuiNotificacoesPendentes = true;
var quantidadeNotificacoesNaoVistas = 0;
var usrid;
var notification;
var invokerClient = false;

//function limparNotificacao(elemento) {
//    var parentDiv = jQuery(elemento).parents('a.lv-item');

//    if (quantidadeNotificacoes() > 0) {
//        setTimeout(function () {
//            $(parentDiv).addClass('animated fadeOutRightBig').delay(250).queue(function () {
//                parentDiv.remove();
//            });
//        }, 250);
//        lerNotificacoes();
//    } 
//}

function quantidadeNotificacoes() {
    return $('#notifications').closest('.listview').find('.lv-item').size();
}

function atualizaQuantidadeNotificacoesNaoVistas() {
    $.ajax({
        url: "/Header/ConsultarQuantidadeNaoLida",
        type: "POST",
        success: function (r) {
            quantidadeNotificacoesNaoVistas = r;
            setQuantidadeNotificacoesNaoVistas(r);
        }
    });
}

function setQuantidadeNotificacoesNaoVistas(quantidade) {
    $("#qtdNaoLida").html(quantidade);
    if (quantidade > 0) {
        $("#qtdNaoLida").css('display', 'block');
        document.title = "(" + quantidade + ") " + document.title;
    }
    else {
        $("#qtdNaoLida").css('display', 'none');
        document.title = document.title.substr(document.title.indexOf(")") + 1)
    }

}

function carregarNotificacoes() {

    if (possuiNotificacoesPendentes) {
        $("#divNotificacaoLoading").css('display', 'block');

        var r = '<div class="listview" id="notifications">' +
                    '<div class="lv-header m-b-0 p-10" style="text-align: center;"><i class="zmdi zmdi-rotate-right zmdi-hc-spin zmdi-hc-2x m-b-10"></i><p>Carregando Notificações</p></div>' +
                '</div>';

        $("#divNotificacoes").html(r);

        //carregar as informações
        $.ajax({
            url: "/Header/ConsultarNotificacoes",
            type: "GET",
            error: function (data) {
                alert("Falha ao tentar buscar notificações.");
            },
            success: function (r) {
                $("#divNotificacoes").html(r);
                $("#divNotificacaoLoading").css('display', 'none');

                quantidadeNotificacoesNaoVistas = 0;
                possuiNotificacoesPendentes = false;

                setQuantidadeNotificacoesNaoVistas(quantidadeNotificacoesNaoVistas);

                if (quantidadeNotificacoesNaoVistas > 5) {
                    $('#lv-body').css('overflow-y', 'scroll');
                }
                else {
                    $('#lv-body').css('overflow-y', 'hidden');
                }

                notification.server.enviarSinalParaUsuario(2, usrid);
                invokerClient = true;
            }
        });
    }
}

$(window).bind("load", function () {
    usrid = $("#hiddenUserId").val();
    atualizaQuantidadeNotificacoesNaoVistas();

    var webApiPath = $('#hiddenUrlWebAPI').val();

    $.getScript(webApiPath + "signalr/hubs", function () {
        $(function () {
            notification = $.connection.notificationHub;
            $.connection.hub.url = webApiPath + "signalr";
            $.connection.hub.qs = { 'userId': usrid };

            notification.client.receberMensagemParaUsuario = function (mensagem) {
                alert("Mensagem para usuario especifico recebida: " + mensagem);
            };

            notification.client.receberMensagemParaTodos = function (mensagem) {
                alert("Mensagem para todos os usuarios recebida: " + mensagem);
            };

            notification.client.receberSinalParaUsuario = function (sinal) {
                if (!invokerClient) {
                    switch (sinal) {
                        case 1:
                            //notificacoes novas chegaram
                            possuiNotificacoesPendentes = true;
                            quantidadeNotificacoesNaoVistas++;
                            setQuantidadeNotificacoesNaoVistas(quantidadeNotificacoesNaoVistas);
                            break;

                        case 2:
                            //notificacoes foram lidas
                            possuiNotificacoesPendentes = true;
                            quantidadeNotificacoesNaoVistas = 0;
                            setQuantidadeNotificacoesNaoVistas(quantidadeNotificacoesNaoVistas);
                            break;
                    }
                }
                invokerClient = false;
            };

            $.connection.hub.start().done(function () {
                //notification.server.enviarMensagemParaTodos('broadcast message content');
                //notification.server.enviarMensagemParaUsuario('notify user content', usrid);
            });
        });
    });
});
