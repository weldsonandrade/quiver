function changePassword() {
    waitingDialog.show('Carregando trocar senha');
    $.ajax({
        url: "/Account/ChangePassword",
        type: "GET",
        cache: false,
        async: true,
        success: function (data) {
            document.getElementById("modalChangePassword").innerHTML = data;
            waitingDialog.hide();
            $('#mChangePassword').modal('show');
        },
        error: function () {
            alert('Ocorreu um erro, tente novamente.');
        }
    });
}

function onChangePasswordSuccess(data) {
    if (data.ok) {
        $('#mChangePassword').modal('hide');
        pesquisar();
    } else {
        $('#mChangePassword').modal('hide');
        $('#modalChangePassword').html(data);
        $('#mChangePassword').modal('show');
    }
}