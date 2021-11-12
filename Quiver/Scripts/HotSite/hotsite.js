
$(document).ready(function () {

    $("#video-landing").css('height', getHeightNavigator() + "px");

    $("#funcionalidades-background").css('min-height', (getHeightNavigator() - 70) + "px");

    $("#vantagens").css('min-height', (getHeightNavigator() - 70) + "px");
    $("#vantagens-img").css('max-height', (getHeightNavigator() - 70) + "px");
    $("#funcionalidades-texto").css('min-height', (getHeightNavigator() - 70) + "px");
    $("#customizado-img").css('max-height', (getHeightNavigator() +50) + "px");
    $("#funcionalidades-texto").css('padding-top', (getHeightNavigator()/20) + "px");

    $(".intro-text").css('padding-top', (getHeightNavigator() / 2) + "px");
    $(".arrow").css('margin-top',   "-50px");

    leftPositionPicturesTreinamento();
 
    scrollY = window.pageYOffset || document.documentElement.scrollTop
    if (scrollY >= 100) {

        document.getElementById("menu-logo").className = "menu-logo";
        document.getElementById("icon-texto").className = "icon-texto";
        document.getElementById("bs-example-navbar-collapse-1").style.marginTop = "0";
        document.getElementById("nav-bar").className = "navbar navbar-default navbar-fixed-top animated slideInDown navbar-shrink";

    }
    else {
      
  
        document.getElementById("menu-logo").className = "menu-logo-grande";
        document.getElementById("icon-texto").className = "icon-texto-grande";
        document.getElementById("bs-example-navbar-collapse-1").style.marginTop = "50px";
        document.getElementById("nav-bar").className = "navbar navbar-default navbar-fixed-top animated slideInDown";
    }

});

window.onresize = function (event) {
    $("#video-landing").css('height', getHeightNavigator() + "px");
    $(".intro-text").css('padding-top', (getHeightNavigator() / 2) + "px");
    $("#vantagens-img").css('max-height', (getHeightNavigator() - 70) + "px");
    $("#customizado-img").css('max-height', (getHeightNavigator()+50 ) + "px");
    $("#vantagens").css('min-height', (getHeightNavigator() - 70) + "px");
   
    $("#funcionalidades-background").css('min-height', (getHeightNavigator() - 70) + "px");
    $(".arrow").css('margin-top', "-50px");
   
}
   



// PARALAXX
$(function () {
    $('.parallax').each(function () {
        var $obj = $(this);
        $(window).scroll(function () {
            var offset = $obj.offset();
            var yPos = -($(window).scrollTop() - offset.top) / $obj.data('speed');
            var bgpos = '50% ' + yPos + 'px';
            $obj.css('background-position', bgpos);
        });
    });
});





function validateEmail(email) {
    emailLength = email.length;

    if (emailLength != 0) {
        var re = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
        return re.test(email);
    } else {
        return true;
    }
}


function validarEmailContato() {
    var email = document.getElementById("email").value;
    if ((validateEmail(email) == false) || (email.length == 0)) {
        document.getElementById("mensagem-erro-email").style.display = "block";
        $('#icon-email').addClass('zmdi-close-circle c-red')
        $('#icon-email').removeClass('zmdi-check-circle c-green');
        document.getElementById("btnEnviarEmail").disabled = true;
        return false;
    } else {
        document.getElementById("mensagem-erro-email").style.display = "block";
        $('#icon-email').removeClass('zmdi-close-circle c-red')
        $('#icon-email').addClass('zmdi-check-circle c-green');
        validarEnvioEmail();
        return true;
    }
}

function validarNomeContato() {
    var nome = document.getElementById("nome").value;
    if (nome.trim().length == 0) {
        document.getElementById("mensagem-erro-nome").style.display = "block";
        $('#icon-nome').addClass('zmdi-close-circle c-red')
        $('#icon-nome').removeClass('zmdi-check-circle c-green');
        document.getElementById("btnEnviarEmail").disabled = true;
        return false;
    } else {
        document.getElementById("mensagem-erro-nome").style.display = "block";
        $('#icon-nome').removeClass('zmdi-close-circle c-red')
        $('#icon-nome').addClass('zmdi-check-circle c-green');
        validarEnvioEmail();
        return true;
    }
}


function validarTextoContato() {
    var mensagem = document.getElementById("msg").value;
    if (mensagem.trim().length == 0) {
        document.getElementById("mensagem-erro-msg").style.display = "block";
        $('#icon-mensagem').addClass('zmdi-close-circle c-red')
        $('#icon-mensagem').removeClass('zmdi-check-circle c-green');
        document.getElementById("btnEnviarEmail").disabled = true;
        return false;
    } else {
        document.getElementById("mensagem-erro-msg").style.display = "block";
        $('#icon-mensagem').removeClass('zmdi-close-circle c-red')
        $('#icon-mensagem').addClass('zmdi-check-circle c-green');
        validarEnvioEmail();
        return true;
    }
}





function validarEnvioEmail() {
    // Habilita o button Salvar
    var mensagem = document.getElementById("msg").value;
    var nome = document.getElementById("nome").value;
    var email = document.getElementById("email").value;

    var valido = false;

    if ((mensagem.trim().length == 0) ||
        (nome.trim().length == 0) ||
        (validateEmail(email) == false) ||
        (email.length == 0)) {
        document.getElementById("btnEnviarEmail").disabled = true;
        return false;
    }
    document.getElementById("btnEnviarEmail").disabled = false;
}

function limparcampos() {
    document.getElementById("email").value = "";
    document.getElementById("nome").value = "";
}


function onContatoSuccess(result) {
    if (result.success) {
        alert('Enviado com sucesso.');

        swal({

            text: "Muito obrigado pelo interesse e em breve nossa equipe irá entrar contato contigo.",

            title: "<i class='zmdi zmdi-email animated bounceIn mdc-text-green-700' style='font-size:90px'></i>  <h1 style='margin:0px' class='mdc-text-green-700'>Email enviado</h1>",
            confirmButtonColor: "#1976D2",
            confirmButtonText: "Confirmar",
            html: true
        }, function () {
            window.location.href = 'http://quiveer.com/';

        });

    } else {
        swal({

            text: "Algo estranho aconteceu e o email não foi enviado, refaça a operação ou se preferir entre em contato com o nosso time pelo telefone.",

            title: "<i class='zmdi zmdi-email animated bounceIn mdc-text-red-700' style='font-size:90px'></i>  <h1 style='margin:0px' class='mdc-text-red-700'>Email não enviado</h1>",
            confirmButtonColor: "#1976D2",
            confirmButtonText: "Confirmar",
            html: true
        }, function () {
           

        });
    }
}

function onContatoFailure(result) {
    altert('ERRO');
}


function leftPositionPicturesTreinamento() {

    left = 200;
    for (var i = 1; i < 6; i++) {
        $('.phone' + i).css('left', left + 'px');
        left = left + 200;
    }
    
}


