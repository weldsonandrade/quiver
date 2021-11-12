function loadBootGridInspecao() {
    calcularPorcentagensDasAvaliacoes();
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
                if (row.status == "AVALIADO") {
                    return "<button class=\"btn btn-xs btn-success waves-effect waves-float\" onclick=\"telaAvaliado('" + row.id + "');\">Detalhes</button>&nbsp;";
                }
                return "";
            },
            "programada": function (column, row) {
                if (row.status == "ANDAMENTO") {
                    return "<p style='color:#4285F4;margin-bottom: 0;'>" + row.programada + "</p>";
                }
                if (row.status == "ATRASADA") {
                    return "<p style='color:#d33;margin-bottom: 0;'>" + row.programada + "</p>";
                }
                return "<p style='color:#1B5E20;margin-bottom: 0;'>" + row.programada + "</p>";

            },
            "executada": function (column, row) {
                if (row.status == "ANDAMENTO") {
                    return "<div class='wrapper' style='color:#4285F4;'  onmouseover='tooltipOnHover(" + "avaliacao" + row.id + ");'>Não finalizada. <div class='tooltip' id=" + "avaliacao" + row.id + " >Apenas inspeções finalizadas possuem data de execução.</div></div> ";
                }
                if (row.status == "ATRASADA") {
                    return "<div class='wrapper' style='color:#d33;'  onmouseover='tooltipOnHover(" + "avaliacao" + row.id + ");'>Não finalizada. <div class='tooltip' id=" + "avaliacao" + row.id + " >Apenas inspeções finalizadas possuem data de execução.</div></div> ";
                }
                return "<p style='color:#1B5E20;margin-bottom: 0;'>" + row.executada + "</p>";
            },
            "status": function (column, row) {
                if (row.status == "AVALIADO") {
                    return "<p style='color:#1B5E20;margin-bottom: 0;'>Finalizada</p>"
                }
                else if (row.status == "ANDAMENTO") {
                    return "<p style='color:#4285F4;margin-bottom: 0;'>Em andamento</p>"
                }
                return "<p style='color:#d33;margin-bottom: 0;'>Atrasada</p>"
            },
            "observacoes": function (column, row) {
                if (row.status == "AVALIADO") {
                    // Não agendada e 100% conforme
                    if (row.observacoes == "False True") {
                        return "<i class='zmdi zmdi-smartphone-iphone' style='display:inline;font-size: 17px;color:#1B5E20'>";
                    }
                        // Não agendada e com não conformidades
                    else if (row.observacoes == "False False") {
                        return "<i class='zmdi zmdi-smartphone-iphone' style='display:inline;font-size: 17px;color:#1B5E20'> <i class='zmdi zmdi-alert-triangle  mdc-text-red wrapper' style='display:inline;font-size: 17px;'> "
                    }
                        // Agendada e com não conformidades
                    else if (row.observacoes == "True False") {
                        return "<i class='zmdi zmdi-alert-triangle  mdc-text-red' style='display:inline;font-size: 17px;'>";
                    }
                    //  Aendada e 100% conforme
                    return "<p style='color:#1B5E20;margin-bottom: 0;'>Nenhuma</p>"
                }
                else if (row.status == "ANDAMENTO") {
                    // Caso não esteja finalizada ou seja agendada e 100% conforme
                    return "<p style='color:#4285F4;margin-bottom: 0;'>Nenhuma</p>";
                }
                return "<p style='color:#d33;margin-bottom: 0;'>Nenhuma</p>";

            },
            "efetividade": function (column, row) {
                if ((row.status == "AVALIADO") && (row.Maximo > 0)) {
                    var percentual = calcularPercentualRespondido(row.Maximo, row.Efetuada);
                    return '<p style="color:#1B5E20;margin-bottom: 0;"> <strong>' + percentual + '%  </strong></p>';
                } else if (((row.status == "AVALIADO") && (row.Maximo == 0))) {
                    return '<p style="color:#1B5E20;margin-bottom: 0;"> Sem pontuação</p>';
                }
                var id = "efetividade" + row.id;
                var msgCalcularEfetividadeInspecaoFinalizada = "Para calcular a efetividade é preciso que a inspeção esteja finalizada.";
                var divTooltipMsgCalcularEfetividadeInspecaoFinalizada = "<div class='wrapper' style='color:#4285F4;' onmouseover='tooltipOnHover(" + id + ");'> Não calculada. <div class='tooltip' id=" + id + " >" + msgCalcularEfetividadeInspecaoFinalizada + "</div> </div> ";
                if (row.status == "ANDAMENTO") {
                    return divTooltipMsgCalcularEfetividadeInspecaoFinalizada;
                }
                return divTooltipMsgCalcularEfetividadeInspecaoFinalizada.replace("#4285F4", "#d33");
            }
        },
        labels: {
            noResults: "Não existem inspeções.",
            loading: "Carregando as inspeções.",
            search: "Pesquisar por inspeções",
            infos: "Apresentados {{ctx.start}} de {{ctx.end}} no total de {{ctx.total}} inspeções"
        }
    });
}