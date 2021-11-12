window.onload = function () {
}

var enableRequest = true;

$(document).ready(function () {
    var opcaoMenu = document.getElementById("menu_agenda");
    if (opcaoMenu != null) {
        opcaoMenu.className = opcaoMenu.className + "  active";
    }

    $('#card-filtros').css('visibility', 'visible');
    $("#DDLFilterUnidade").chosen().change(function () { filtrarAgenda(); });
    $("#DDLFilterUsuario").chosen().change(function () { filtrarAgenda(); });
    carregarCalendario();
});


$(document).ajaxStop(function () {

});

function filtrarAgenda() {
    $("#calendar").fullCalendar("refetchEvents");
}

function filtrar(start, end, callback) {
    var unidadeID = 0;
    var usuarioID = "";
    if ($("#DDLFilterUnidade").chosen().val() != null) {
        unidadeID = $("#DDLFilterUnidade").chosen().val().toString();
    }
    if ($("#DDLFilterUsuario").chosen().val() != null) {
        usuarioID = $("#DDLFilterUsuario").chosen().val().toString();
    }

    $.ajax({
        url: '/Agenda/ListarEventos',
        dataType: "json",
        contentType: "application/json",
        Type: 'GET',
        data: {
            start: start.format(),
            end: end.format(),
            unidadeId: unidadeID,
            usuarioId: usuarioID
        },
        beforeSend: function (jqXHR) {
            //show your loader here 
            $('.fc-body').append(HTMLLoaderCallendar('Carregando inspeções...'));
            $('.loader-text').css("font-size", "16px");
            $('#addAvaliacao').addClass('disabled');
        },
        success: function (data) {
            console.log('SUCESSO');
            $("div.loader-card-callendar").remove();
            $('#addAvaliacao').removeClass('disabled');
            return callback(data);

        },
        error: function () {
            console.log('errou');
        },
        complete: function (jqXHR) {

        }
    });
}

function carregarCalendario() {
    var myCalendar = $('#calendar'); //Change the name if you want. I'm also using thsi add button for more actions
    //Generate the Calendar
    myCalendar.fullCalendar({
        header: {
            right: '',
            center: 'prevYear,prev, title, next, nextYear',
            left: 'today',
            lang: 'pt-br'
        },
        buttonText: {
            prevYear: "<<",
            nextYear: ">>"
        },
        theme: true, //Do not remove this as it ruin the design
        selectable: true,
        selectHelper: true,
        editable: true,
        droppable: true,
        dragRevertDuration: 0,
        viewRender: function (view, element) {

        },

        loading: function (bool) {

        },

        ////Add Events
        //events: '/Agenda/ListarEventos'
        events: function (start, end, timezone, callback) {
            var timestamp = moment().format("x");
            $("#timestamp_carregar").val(timestamp);
            setTimeout(function () {
                if ($("#timestamp_carregar").val() == timestamp) {
                    filtrar(start, end, callback)
                }
            }, 1000);
        },
        dayRender: function (date, cell) {
            // Alterando as cores para as datas inferiors a atual
            var currentDate = moment().format("YYYY-MM-DD");
            if (date.format("YYYY-MM-DD") < currentDate)
                $(cell).addClass('disabled');
        },
        eventRender: function (event, element) {
            // Evento finalizado
            if (event.color == "#1B5E20") {
                element.draggable = false;
            }

            var icones = ""; // Usada para incluir os'ícones caso uma inspeção possua itens não conformes ou não seja não agendada 
            if (event.agendada == false) {
                icones = "<i class='zmdi zmdi-smartphone-iphone mdc-text-grey-50' style='padding-right: 2px;'></i> "
            }
            if (event.conforme == false) {
                icones = icones + " <i class='zmdi zmdi-alert-triangle mdc-text-grey-50' style='padding-right: 2px;'></i>"
            }

            element.find('.fc-title').prepend(icones);
        },
        dayClick: function (date, allDay, jsEvent, view) {
            // Só podem
            var currentDate = moment().format("YYYY-MM-DD");
            if (date.format("YYYY-MM-DD") < currentDate) {
                swal({
                    text: "Não é permitido agendar uma inspeção para uma data anterior a hoje'",
                    title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Operação inválida!</h1>",
                    showCancelButton: false,
                    confirmButtonColor: "#9E9E9E",
                    confirmButtonText: "Ok",
                    closeOnConfirm: true,
                    closeOnCancel: true,
                    html: true,

                });
            } else {
                adicionar(date.format("DD/MM/YYYY"));
            }
        },
        droppable: true, // Permite arrastar eventos no calendário
        drop: function (date) {
            $(this).remove();//remove o evento da lista
            var titulo = $(this).html();
            $.ajax({
                type: 'POST',
                url: '/Agenda/Manipular',
                dataType: "json",
                contentType: "application/json",
                success: function () {
                    $("#calendar").fullCalendar('removeEvents');
                    $("#calendar").fullCalendar("refetchEvents");
                }
            });
        },
        eventClick: function (event) {
            var avaliado = "#1B5E20";
            var normal = "#4285F4";
            var atrasado = "#d33";
            if ((event.color == normal) || (event.color == atrasado)) {
                editar(event.id);
                //  telaAvaliado(event.id);                
            }
            else {
                // Chamar modal do evento já avaliado.
                telaAvaliado(event.id);
            }
        },
        editable: true,
        eventDrop: function (event, delta, revertFunc) { //Chama a atualização de evento
            var currentDate = moment().format("YYYY-MM-DD");
            if (event.color == "#1B5E20") {
                swal({
                    text: "Não é permitido alterar a data de uma inspeção já finalizada.",
                    title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Operação inválida!</h1>",
                    showCancelButton: false,
                    confirmButtonColor: "#9E9E9E",
                    confirmButtonText: "Ok",
                    closeOnConfirm: true,
                    closeOnCancel: true,
                    html: true,

                });
                revertFunc();
            }
            else if (event.start.format("YYYY-MM-DD") < currentDate) {
                swal({
                    text: "Não é permitido alterar a data de uma inspeção para uma data anterior a hoje.",
                    title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Operação inválida!</h1>",
                    showCancelButton: false,
                    confirmButtonColor: "#9E9E9E",
                    confirmButtonText: "Ok",
                    closeOnConfirm: true,
                    closeOnCancel: true,
                    html: true,

                });
                revertFunc();
            } else {
                atualizarEvento(event.id, event.start, event.end);
            }
        },
        eventDragStart: function (event) {

            $('#calendarTrash').slideDown("slow", function () {
                $('#calendarTrash').css('display', 'block');
            });

        },
        eventDragStop: function (event, jsEvent) {

            var trashEl = jQuery('#calendarTrash');
            var ofs = trashEl.offset();
            var x1 = ofs.left;
            var x2 = ofs.left + trashEl.outerWidth(true);
            var y1 = ofs.top;
            var y2 = ofs.top + trashEl.outerHeight(true);
            var agendamentoId = event.id;
          
            $('#calendarTrash').css('display', 'none');

            if (jsEvent.pageX >= x1 && jsEvent.pageX <= x2 &&
                jsEvent.pageY >= y1 && jsEvent.pageY <= y2) {

                // Tem que ser pelo estado do agendamento
                var currentDate = moment().format("YYYY-MM-DD");
                if (event.color == '#1B5E20') {
                //if (event.start.format("YYYY-MM-DD") <= currentDate) {
                    swal({
                        text: "Não é permitido remover uma inspeção já finalizada.",
                        title: "<i class='zmdi zmdi-alert-circle animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1  class='titulo-small-alert mdc-text-red-700'>Operação inválida!</h1>",
                        showCancelButton: false,
                        confirmButtonColor: "#9E9E9E",
                        confirmButtonText: "Ok",
                        closeOnConfirm: true,
                        closeOnCancel: true,
                        html: true,

                    });
                }
                else {
                    swal({
                        title: 'Você ter certeza?',
                        text: 'Você não será capaz de recuperar esse arquivo depois!',
                        type: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Sim, remova!',
                        cancelButtonText: 'Não, cancele!',
                        confirmButtonClass: 'confirm-class',
                        cancelButtonClass: 'cancel-class',
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                    function (isConfirm) {
                        if (isConfirm) {
                            $.ajax({
                                type: 'POST',
                                url: '/Agenda/Excluir?agendamentoId=' + agendamentoId,
                                dataType: "json",
                                contentType: "application/json",
                                sucess: function () {
                                    $("#calendar").fullCalendar('removeEvents');
                                    $("#calendar").fullCalendar("refetchEvents");
                                }
                            });

                            $('#calendario').fullCalendar('removeEvents', event.id);
                            $("#calendar").fullCalendar("refetchEvents");
                        }
                    });

                }
            }
        }
    });
    //Create and ddd Action button with dropdown in Calendar header. 
    var actionMenu = '<ul class="actions actions-alt" id="fc-actions">' +
                        '<li class="dropdown">' +
                            '<a href="" data-toggle="dropdown"><i class="zmdi zmdi-more-vert"></i></a>' +
                            '<ul class="dropdown-menu dropdown-menu-right">' +
                                '<li class="active">' +
                                    '<a data-view="month" href="">Visualizar mês</a>' +
                                '</li>' +
                                '<li>' +
                                    '<a data-view="basicWeek" href="">Visualizar semana</a>' +
                                '</li>' +

                                '<li>' +
                                    '<a data-view="basicDay" href="">Visualizar Dia</a>' +
                                '</li>' +

                            '</ul>' +
                        '</div>' +
                    '</li>';
    var diasMes = '<select id="months-tab">' +
                    '<option data-month="0">January</option>' +
                    '<option data-month="1">February</option>' +
                    '<option data-month="2">March</option>' +
                    '<option data-month="3">April</option>' +
                    '<option data-month="4">May</option>' +
                    '<option data-month="5">June</option>' +
                    '<option data-month="6">July</option>' +
                    '<option data-month="7">August</option>' +
                    '<option data-month="8">September</option>' +
                    '<option data-month="9">October</option>' +
                    '<option data-month="10">November</option>' +
                    '<option data-month="11">December</option>' +
                   '</select>';


    myCalendar.find('.fc-toolbar').append(actionMenu);
    //  myCalendar.find('.fc-toolbar').append(diasMes);

    //Calendar views
    $('body').on('click', '#fc-actions [data-view]', function (e) {
        e.preventDefault();
        var dataView = $(this).attr('data-view');

        $('#fc-actions li').removeClass('active');
        $(this).parent().addClass('active');
        myCalendar.fullCalendar('changeView', dataView);
    });

}

$('#months-tab').on('change', function () {
    // get month from the tab. Get the year from the current fullcalendar date
    var month = $(this).find(":selected").attr('data-month'),
        year = $("#calendar").fullCalendar('getDate').format('YYYY');

    var m = moment([year, month, 1]).format('YYYY-MM-DD');

    $('#calendar').fullCalendar('gotoDate', m);
});

function adicionarAvaliacao(dataSelecionada) {

    if (dataSelecionada != undefined) {
        limparCamposModal();
        $("#myAgendamentoModal").modal();
        $('#dataprogramada').prop("value", dataSelecionada);
        $('#dataprogramada').prop("defaultvalue", dataSelecionada);
        $('#DataProgramada').data("DateTimePicker").destroy();
        $('#DataProgramada').datetimepicker({
            format: 'DD/MM/YYYY',
            defaultDate: convertPadraoeData4(dataSelecionada),
            minDate: moment(),
            locale: 'pt-br',
        });
    } else {
        limparCamposModal();
        $('#DataProgramada').val(dataSelecionada);
        $('#DataProgramada').data("DateTimePicker").destroy();
        $('#DataProgramada').datetimepicker({
            format: 'DD/MM/YYYY',
            minDate: moment(),
            locale: 'pt-br',
        });
    }

    $('#myModalLabel').text('Cadastrar nova inspeção');

    getUnidadesAndGruposAndInspetores(null, null, null, function (qtdUnidades, qtdGrupos, qtdUsuarios) {
        if (qtdGrupos != 0 && qtdUnidades != 0 && qtdUsuarios != 0) {
            $('#myAgendamentoModal').modal('show'); // Todas as quantidades estão  inválidas
        } else if (qtdGrupos != 0 && qtdUnidades != 0 && qtdUsuarios == 0) {
            alertAvaliacaoInvalida("usuário inspetor"); // Usuários com quatidades  inválidas
        } else if (qtdGrupos != 0 && qtdUnidades == 0 && qtdUsuarios != 0) {
            alertAvaliacaoInvalida("unidade"); // Unidades com quantidades inválidas
        } else if (qtdGrupos != 0 && qtdUnidades == 0 && qtdUsuarios == 0) {
            alertAvaliacaoInvalida("unidade e usuário inspetor"); // Unidades e Usuários com quantidades inválidas
        } else if (qtdGrupos == 0 && qtdUnidades != 0 && qtdUsuarios != 0) {
            alertAvaliacaoInvalida("grupo"); // Grupos com quantidades inválidas
        } else if (qtdGrupos == 0 && qtdUnidades != 0 && qtdUsuarios == 0) {
            alertAvaliacaoInvalida("grupo e usuário inspetor"); // Grupos e Usuários com quantidades inválidas
        } else if (qtdGrupos == 0 && qtdUnidades == 0 && qtdUsuarios != 0) {
            alertAvaliacaoInvalida("grupo e unidade"); // Grupos e Unidades com quantidades inválidas
        } else {
            alertAvaliacaoInvalida("grupo, unidade e usuário inspetor"); // Todos inválidos com quantidades inválidas
        }

        enableScroll("myAgendamentoModal");
        habilitarSalvar();
    });  
}

function adicionar(dataSelecionada) {
    adicionarAvaliacao(dataSelecionada);
}

function editar(agendamentoId) {
    waitingDialog.show('Carregando inspeção');
    limparCamposModal();
    $.getJSON("/Agenda/Manipular?agendamentoId=" + agendamentoId, function (avaliacao) {
        console.log(avaliacao);
        $('#Id').val(avaliacao.Id);
        $('#avaliaco-input-rotulo').val(avaliacao.RotuloCalendario);
        $("#avaliacao-rotulo > div.fg-line").addClass("fg-toggled");
        $('#DataProgramada').val(moment.utc(avaliacao.DataProgramada).format('DD/MM/YYYY'));

        getUnidadesAndGruposAndInspetores(avaliacao.IdUnidade, avaliacao.IdGrupo, avaliacao.IdUsuario);

        $('#myAgendamentoModal').modal('show');
        $('#myModalLabel').text('Editar inspeção');

    }).always(function () {
        waitingDialog.hide();
    });

}

function atualizarEvento(agendamentoId, dataProgramada, eventEnd) {
    var visualizacaoAtual = $('#calendar').fullCalendar('getView').name;
    var dataRow = {
        'agendamentoID': agendamentoId,
        'dataProgramada': dataProgramada.format('YYYY-MM-DD'),
        'newEventEnd': eventEnd,
        'visualizacaoAtual': visualizacaoAtual
    };
    $.ajax({
        type: 'POST',
        url: "/Agenda/TrocarData",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(dataRow),
        success: function (data) {
            if (data.ok == false) {
                swal("Erro!", data.mensagem, "error");
            }
            $('#calendar').fullCalendar('refetchEvents');
        }
    });
}

function onSuccess(data) {
    if (data.ok) {
        $('#myAgendamentoModal').modal('hide');
        $('#calendar').fullCalendar('refetchEvents');
    }
    else if (data.choque) {
        $('#mensagem-erro-data-choque').css('display', '');
    }
}

// Caso não tenhamos grupos, unidades ou usuários cadastrados é impossível gerar uma inspeção.
function alertAvaliacaoInvalida(texto) {
    waitingDialog.hide();
    swal({
        title: "<i class='zmdi zmdi-alert-octagon animated bounceIn mdc-text-red-700' style='font-size:60px'></i>  <h1 style='margin:0px' class='titulo-small-alert mdc-text-red-700'>Falta algo...</h1>",

        text: "Para criar a inspeção ainda falta cadastrar no mínimo um(a): " +
              "<span class='mdc-text-red'>" + texto + "</span>.",
        showCancelButton: false,
        confirmButtonColor: "#9E9E9E",
        confirmButtonText: "Ok",
        closeOnConfirm: true,
        closeOnCancel: true,
        html: true
    });
    $('#myAgendamentoModal').modal('hide');
}

function calcularPercentualRespondido() {
    var maximo = parseInt($('#maxima')[0].innerText);
    var efetuado = parseInt($('#efetuada')[0].innerText);
    var percentual = (efetuado / maximo) * 100;
    $('#efetuada').text(efetuado + ' (' + percentual.toFixed(2) + '%)')
    return percentual;
}


function habilitarAvaliacoesRepetidas() {
    if ($('#repetir').is(":checked")) {
        // Deixar data inspeção invisível
        $('#avaliacao-data').css('visibility', 'hidden');
        $('#mensagem-erro-data').css('visibility', 'hidden');
        // Deixar card visível
        $("#avaliacoes-recorrentes").removeClass('hidden');
        $('#tooltip-repetir-avaliacoes').css('visibility', 'hidden');
        setTimeout(function () {
            $("#avaliacoes-recorrentes").removeClass('visuallyhidden');
            selecionarModeloRepeticaoAvaliacao();
            habilitarCampoDeTerminoSelecionado();
        }, 20);
    } else {
        // Deixar data inspeção visível
        $('#avaliacao-data').css('visibility', 'visible');
        $('#mensagem-erro-data').css('visibility', 'visible');
        $('#tooltip-repetir-avaliacoes').css('visibility', 'visible');


        // Deixar card invisível
        $("#avaliacoes-recorrentes").addClass('visuallyhidden');
        $("#avaliacoes-recorrentes").one('transitionend', function (e) {
            $("#avaliacoes-recorrentes").addClass('hidden');

        });
    }
    habilitarSalvar();

}


// Modelo de repetições de atividades em modo diário, semanal, mensal e anual
function selecionarModeloRepeticaoAvaliacao() {
    selecionado = $('#select-modelo-repeticao').val();

    // Deixando datas iniciais e div de termino visíveis
    $('#avaliacao-Inicio-data').css('visibility', 'visible');
    $('#div-terminar').css('display', 'block');

    if (selecionado == 'Diária') {
        $('#repeticoes-diaria').css('display', 'block');
        $('#repeticoes-semanal').css('display', 'none');
        $('#repeticoes-mensal').css('display', 'none');
        $('#repeticoes-anual').css('display', 'none');
        $('#repeticoes-customizadas').css('display', 'none');

    } else if (selecionado == 'Semanal') {
        $('#repeticoes-diaria').css('display', 'none');
        $('#repeticoes-semanal').css('display', 'block');
        $('#repeticoes-mensal').css('display', 'none');
        $('#repeticoes-anual').css('display', 'none');
        $('#repeticoes-customizadas').css('display', 'none');
    }
    else if (selecionado == 'Mensal') {
        $('#repeticoes-diaria').css('display', 'none');
        $('#repeticoes-semanal').css('display', 'none');
        $('#repeticoes-mensal').css('display', 'block');
        $('#repeticoes-anual').css('display', 'none');
        $('#repeticoes-customizadas').css('display', 'none');
    }
    else if (selecionado == 'Anual') {
        $('#repeticoes-diaria').css('display', 'none');
        $('#repeticoes-semanal').css('display', 'none');
        $('#repeticoes-mensal').css('display', 'none');
        $('#repeticoes-anual').css('display', 'block');
        $('#repeticoes-customizadas').css('display', 'none');

    } else if (selecionado == 'Customizado') {
        $('#repeticoes-diaria').css('display', 'none');
        $('#repeticoes-semanal').css('display', 'none');
        $('#repeticoes-mensal').css('display', 'none');
        $('#repeticoes-anual').css('display', 'none');
        $('#repeticoes-customizadas').css('display', 'block');
        habilitarDataCustomizada("data-customizada0");

        // Deixando datas iniciais e div de termino invisíveis
        $('#avaliacao-Inicio-data').css('visibility', 'hidden');
        $('#div-terminar').css('display', 'none');
    }
    setdataFimMinima();
    habilitarSalvar();
}


function criarDataCustomizada() {
    var wrapper = $("#campo-de-datas-adicionais");

    var qtd = getQtdDatasCustomizadas();
    var proximoID = getProximoIdDataCustomizada();

    var campoAdicionado = '<div class="input-line-control removeMe col-lg-12 row m-t-10" id="campo-data-customizada' + proximoID + '">' +
                            '<div class="col-md-4">' +

                              '<div class="input-group form-group m-b-0" style="margin-top:-10px">' +
                                 '<small class="p-l-10">Data de programada</small>' +
                                               '<div class="dtp-container fg-line">' +
                                               '<input id="data-customizada' + proximoID + '"  type="text" class="form-control date data-customizada" placeholder="Escolha uma data..." onclick="validarDataCustomizada(' + proximoID + ')" onfocusout="validarDataCustomizada(' + proximoID + ')">' +
                                                '</div>' +
                                 '<small class="help-block" id="mensagem-erro-data-customizada' + proximoID + '" style="padding-left:10px;">Data é obrigatória.</small>' +
                               '</div>' +

                             '</div>' +
                             '<div class="col-md-8">' +
                                '<a class="btn btn-xs btn-danger remove-date m-l-20"  onclick="removerDataCustomizada(' + proximoID + ')"><i class="fa fa-remove"></i>Remover data</a>' +
                              '</div>' +
                           '</div>';

    $(wrapper).append(campoAdicionado); //add input box
    habilitarDataCustomizada("data-customizada" + proximoID);
    habilitarSalvar();
}

function habilitarDataCustomizada(id) {
    $('#' + id).datetimepicker();
    $('#' + id).data("DateTimePicker").destroy();
    $('#' + id).datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment(),
        locale: 'pt-br',
        disabledDates: desabilitarDataSelecionadas()
    });
}

function habilitarCampoDeTerminoSelecionado() {
    var selectedTerminar = $("input[type='radio'][name='terminar']:checked");
    if (selectedTerminar.val() == "data") {
        if (moment($('#data-fim')[0].value, "DD/MM/YYYY").isValid() == false) {
            $('#mensagem-erro-data-fim').css('visibility', 'visible');
        }
        $('#campo-repetir-numerico').attr("disabled", true);
        $('#data-fim').data("DateTimePicker").enable();
    } else {
        $('#mensagem-erro-data-fim').css('visibility', 'hidden');
        $('#avaliacao-fim-data').removeClass('has-error');
        $('#avaliacao-fim-data').removeClass('has-success');
        $('#campo-repetir-numerico').attr("disabled", false);
        $('#data-fim').data("DateTimePicker").disable();
    }
    habilitarSalvar();
}

// Retornar um array com as datas customizadas já escolhidas para que o próximo não possa repeti-la.
function desabilitarDataSelecionadas() {
    var doc = document.getElementById("campo-de-datas-adicionais");
    var datasDesabilitadas = [];

    if (moment($("#data-customizada0")[0].value, "DD/MM/YYYY").isValid()) {
        datasDesabilitadas.push(convertPadraoeData($("#data-customizada0")[0].value));
    }

    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("removeMe")) {
            var iddataProgramada = $(doc.childNodes[i])[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].id;
            var dataProgramada = $("#" + iddataProgramada)[0].value;
            if (moment(dataProgramada, "DD/MM/YYYY").isValid()) {
                datasDesabilitadas.push(convertPadraoeData(dataProgramada));
            }
        }
    }
    return datasDesabilitadas;
}


$('#modal').on('focus', '.data-customizada', function () {
    $(this).on('dp.change', habilitarDataCustomizada(this.id));
});

// Habilitanto a dataMin da data final De acordo com o padrão de repetição escolhido
$('#modal').on('focus', '#data-inicio', function () {
    $("#data-inicio").on("dp.change", function (e) {
        setdataFimMinima();
    });
});

function setdataFimMinima() {
    var minDateFinal = $("#data-inicio")[0].value;

    // Apenas se a dataInicial estiver preenchida no padrão
    if (moment(minDateFinal, "DD/MM/YYYY").isValid()) {
        selecionado = $('#select-modelo-repeticao').val();
        minDateFinal = moment(minDateFinal, "DD/MM/YYYY");

        if (selecionado == "Diária") {
            var minimo = $('#repeticoes-dias')[0].innerHTML;
            minDateFinal.add(minimo, 'd');
        } else if (selecionado == "Semanal") {
            minDateFinal.add(7, 'd');
        } else if (selecionado == "Mensal") {
            var diasPular = moment(minDateFinal).daysInMonth();
            minDateFinal.add(diasPular, 'd');
        } else if (selecionado == "Anual") {
            minDateFinal.add(365, 'd');
        }

        // Apenas se a dataFinal estiver preenchida no padrão
        var dataFim = $('#data-fim')[0].value;
        if (moment(dataFim, "DD/MM/YYYY").isValid()) {
            //Tiro a diferençca da data-fim com a data-fim-minima e caso esteja inferior ou igual a 0 limpo o campo de data-fim
            var datafimMinima = moment(minDateFinal, "DD/MM/YYYY");
            var diferencaDias = datafimMinima.diff(moment(dataFim, "DD/MM/YYYY"), 'days');

            if (diferencaDias >= 0) {
                $('#data-fim')[0].value = "";
                $('#data-fim').data("DateTimePicker").clear();
            }
        }

        $('#data-fim').data("DateTimePicker").minDate(minDateFinal);

    }

}


function validarRepitcaoDias(id) {
    var valorDias = $("#" + id)[0].value;
    var caracteres = $("#" + id)[0].value.trim().length;

    if (valorDias.trim() == "" || parseInt(valorDias) <= 0 || valorDias.substring(0, 2) == "00") {
        $("#" + id)[0].value = 1;
    }

    if (caracteres > 2 && valorDias.substring(0, 2) != "00") {
        $("#" + id)[0].value = valorDias.substring(0, 2);
    }
}



function removerDataCustomizada(idData) {
    $('#campo-data-customizada' + idData).remove();
    habilitarSalvar();

}

function getQtdDatasCustomizadas() {
    var doc = document.getElementById("campo-de-datas-adicionais");
    var count = 0;
    for (var i = 0; i < doc.childNodes.length; i++) {
        if ($(doc.childNodes[i]).hasClass("removeMe")) {
            count++;
        }
    }
    return count;
}

function getProximoIdDataCustomizada() {
    var doc = document.getElementById("campo-de-datas-adicionais");

    var ultimoID = "0";
    for (var i = doc.childNodes.length; i > 0; i--) {
        if ($(doc.childNodes[i]).hasClass("removeMe")) {
            ultimoID = $(doc.childNodes[i])[0].childNodes[0].childNodes[0].childNodes[1].childNodes[0].id;
            break;
        }
    }
    idNumber = ultimoID.replace(/\D/g, '');
    proximoId = parseInt(idNumber) + 1;
    return proximoId;
}

function checkSemanalClick() {
    qtdMinimoItensMarcados('checkSemanal', 'dia-semana-marcado', 1);
    habilitarSalvar();

    var diasSemana = '';

    $('.checkSemanal').each(function (i) {
        var teste = '1';
    });

    $("input[id='Recorrencia.DiasDaSemana']").val();
}

function checkMensalClick() {
    qtdMinimoItensMarcados('checkMes', 'dia-mes-marcado', 1);
    habilitarSalvar();
}


function limparCamposModal() {
    $('#Id').val(null);

    $('#DataProgramada').val('');
    $('#DataProgramada').data("DateTimePicker").destroy();
    $('#DataProgramada').datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment(),
        locale: 'pt-br',
    });

    resetarCampo('avaliaco-input-rotulo', '#avaliacao-rotulo', 'mensagem-erro-rotulo');
    resetarCampo('DataProgramada', '#avaliacao-data', 'mensagem-erro-data');

    $('#unidadeAvaliacao').empty();
    $('#grupoAvaliacao').empty();
    $('#usuarioAvaliacao').empty();
}
