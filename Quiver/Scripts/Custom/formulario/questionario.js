document.body.onload = function () {
}

$(document).ready(function () {
    $("#total-questoes").text(getQuestoesProntaLength());
    $("#total-subjetivos").text($('.subjetivo').length);
    $("#total-objetivos").text($('.objetivo').length);
    $("#total-multiplas").text($('.multipla-escolha').length);
    $("#total-pontuacao").text(getPontuacaoTotalQuestoes());

    var opcaoMenu = document.getElementById("menu_questionarios");
    //var opcaoSubMenu = document.getElementById("submenu_lista_questionario");

    opcaoMenu.className = opcaoMenu.className + "  active";
    //opcaoSubMenu.className = opcaoSubMenu.className + "active";

    cardQuestoesVisibility();
});

$(document).ajaxStop(function () {
});

function adicionarQuestionario() {
    $.ajax({
        url: "/Grupo/ExisteGrupo",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        success: function (response) {
            if (response == "True") {
                // Existem grupos cadastrados
                location.href = "/Questionario/Manipular";
            } else {
                // Não existem grupos cadastrados
                swal({
                    title: "<i class='zmdi zmdi-alert-octagon animated bounceIn mdc-text-red' style='font-size:90px'></i>  <h1 style='margin:0px' class='mdc-text-red'>Atenção...</h1>",

                    text: "Para cadastrar formulários é preciso ter ao menos " +
                          "<span class='mdc-text-red'>um grupo</span> cadastrado.",

                    html: true
                });
            }
        }
    });
}

function editarQuestionario(questionarioId) {
    location.href = "/Questionario/Manipular?questionarioId=" + questionarioId;
}

function excluirQuestionario(questionarioID) {
    swal({
        title: "Deseja realmente excluir esse formulário?",
        text: "Todos os formulários ligados a esse grupo também serão removidos.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085D6',
        cancelButtonColor: '#d333',
        confirmButtonText: "Confirmar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: '/Questionario/Excluir?questionarioID=' + questionarioID,
            cache: false,
            type: 'post',
            complete: function (data) {
                pesquisar();
            }
        });
    });
}

function pesquisar() {
    var termo = $('#pesquisar').val();
    $('#tabela').load('/Questionario/Tabela', { "termo": termo }, function () { loadBootGrid(); });
}


function loadBootGrid() {
    $("#data-table-command").bootgrid({
        css: {
            icon: 'zmdi icon',
            iconColumns: 'zmdi-view-module',
            iconDown: 'zmdi-expand-more',
            iconRefresh: 'zmdi-refresh',
            iconUp: 'zmdi-expand-less'
        },
        formatters: {
            "commands": function (column, row) {
                return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"editarQuestionario(" + row.id + ")\">Editar</button>&nbsp;" +
                   "<button class=\"btn btn-xs btn-danger waves-effect waves-float\" onclick=\"excluirQuestionario(" + row.id + ")\">Excluir</button>";
            }
        },
        labels: {
            noResults: "Não existem formulários cadastrados.",
            loading: "Carregando os formulários.",
            search: "Pesquisar formulário",
            infos: "{{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} formulário(s)"
        }
    });
}

function onBegin() {
    $("#btnSalvarQuestionario").prop("disabled", true);
    $("#icon-salvar").css('display', 'inline-block');
}

function onComplete() {
    $("#btnSalvarQuestionario").prop("disabled", false);
    $("#icon-salvar").css('display', 'none');
}

loadBootGrid();

function cardQuestoesVisibility() {
    var qtdQuestoes = getQuestoesProntaLength();

    if (qtdQuestoes != 0) {
        $('#cartao-itens').css('display', 'block');
    }
}

$(document).ready(function () {
    $(".droppable").sortable({
        update: function (event, ui) {
            Dropped();
        }
    });
});

function Dropped(event, ui) {
    $(".draggable").each(function () {
        //var p = $(this).position();
        $(this).removeClass("animated");
    });
    refresh();
}

