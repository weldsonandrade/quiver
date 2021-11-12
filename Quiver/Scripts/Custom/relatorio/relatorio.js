function exibirExcecaoSelecionadaRelatorio(nmTipo, ddlRelUsuario, ddlRelUnidade, ddlRelGrupo) {
    var categoriaSelecionada = $('#select-categoria').selectpicker('val');
    if (categoriaSelecionada == "Usuários") {
        $("#execao-usuario").css('display', 'block');
        $("#execao-unidade").css('display', 'none');
        $("#execao-grupo").css('display', 'none');
        $("#" + ddlRelUsuario).css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'none');
        $("#avaliacoes-com-grupos").css('display', 'block');
        $("#avaliacoes-com-unidades").css('display', 'block');
        $("#DDLFilterUnidade_chosen").css('width', '100%');
        $("#DDLFilterGrupo_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ' + nmTipo + ' de inspeções finalizadas por usuários com os parâmetros que achar necessário. ');
    }
    else if (categoriaSelecionada == "Unidades") {
        $("#execao-usuario").css('display', 'none');
        $("#execao-unidade").css('display', 'block');
        $("#execao-grupo").css('display', 'none');
        $("#" + ddlRelUnidade).css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'block');
        $("#avaliacoes-com-grupos").css('display', 'block');
        $("#avaliacoes-com-unidades").css('display', 'none');
        $("#DDLFilterUsuario_chosen").css('width', '100%');
        $("#DDLFilterGrupo_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ' + nmTipo +' de inspeções finalizadas por unidades com os parâmetros que achar necessário. ');
    }
    else if (categoriaSelecionada == "Grupos") {
        $("#execao-usuario").css('display', 'none');
        $("#execao-unidade").css('display', 'none');
        $("#execao-grupo").css('display', 'block');
        $("#" + ddlRelGrupo).css('width', '100%');
        $("#avaliacoes-com-usuarios").css('display', 'block');
        $("#avaliacoes-com-grupos").css('display', 'none');
        $("#avaliacoes-com-unidades").css('display', 'block');
        $("#DDLFilterUsuario_chosen").css('width', '100%');
        $("#DDLFilterUnidade_chosen").css('width', '100%');
        $("#sub-title-filtros").text('Caso deseje, gere um ' + nmTipo + ' de inspeções finalizadas por grupos com os parâmetros que achar necessário. ');
    }
}