//document.body.onload = function () {
//    var opcaoMenu = document.getElementById("menu_usuarios");
//    opcaoMenu.className = opcaoMenu.className + "  active";
//}

var modalId = '#criarEmpresaTesteModal';

function criarTesteEmpresa() {
    waitingDialog.show('Carregando cadastro de empresa teste');
    $.ajax({
        url: "/Admin/CriarEmpresaTeste",
        type: "GET",
        cache: false,
        async: true,
        success: function (data) {
            document.getElementById("modal").innerHTML = data;
            waitingDialog.hide();
            $(modalId).modal('show');
        },
        error: function () {
            alert('Ocorreu um erro, tente novamente.');
        }
    });
}