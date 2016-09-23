$(document).ready(function () {
    $('#OrigemRecursos').change(function () {
        if ($('#OrigemRecursos').val() && ($('#OrigemRecursos').val() == 1 || $('#OrigemRecursos').val() == 2)) {
            $('#divNomeParlamentar').show();
        }
        else {
            $('#divNomeParlamentar').hide();
            $('#NomeParlamentar').val('');
        }
    });
    $("#Protocolo").mask("####.######.####");
    $("#ValorSolicitado").mask("#.##0,00", { reverse: true, maxlength: false });
    $("#ValorContrapartida").mask("#.##0,00", { reverse: true, maxlength: false });
    $("#ValorTotal").mask("#.##0,00", { reverse: true, maxlength: false });

    $("#ValorSolicitado").blur(function () {
        somar();
    });
    $("#ValorContrapartida").blur(function () {
        somar();
    });

    $('#tabs').tab();
    $('[data-toggle="tooltip"]').tooltip();

    /* -- proponente -- */

    /* Novo -> ParaAnalise */
    $('#ParaAnalise').click(function () {
        confirmaModal("<p>Tem certeza que deseja enviar a solicitação para análise?</p>", function () {
            window.location = '/Workflow/AlterarStatusParaParaAnalise/' + $('#Id').val();
        });
    });

    /* EmDiligencia -> RetornoDeDiligencia */
    $('#RetornoDeDiligencia').click(function () {
        confirmaModal("<p>Tem certeza que deseja enviar a solicitação para análise?</p>", function () {
            window.location = '/Workflow/AlterarStatusParaRetornoDeDiligencia/' + $('#Id').val();
        });
    });

    /* -- concedente -- */

    /* ParaAnalise -> EmAnaliseDeDocumentacao */
    $('#EmAnaliseDeDocumentacao').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em análise de documentação"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmAnaliseDeDocumentacao/' + $('#Id').val();
        });
    });

    /* RetornoDeDiligencia -> EmAnaliseDeDocumentacao */
    $('#RetornoDeDiligenciaParaEmAnaliseDeDocumentacao').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em análise de documentação"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmAnaliseDeDocumentacao/' + $('#Id').val();
        });
    });

    /* EmAnaliseDeDocumentacao -> EmDiligenciaAgetop */
    $('#EmDiligenciaAgetop').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em diligência (AGETOP)"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmDiligenciaAgetop/' + $('#Id').val();
        });
    });

    /* EmDiligenciaAgetop -> favoravel */
    $('#EmDiligenciaAgetopFavoravel').click(function () {
        confirmaModal('<p>Deseja manifestar parecer "favorável"?</p>', function () {
            window.location = '/Workflow/EmDiligenciaAgetopFavoravel/' + $('#Id').val();
        });
    });

    /* EmDiligenciaAgetop -> desfavoravel */
    $('#EmDiligenciaAgetopDesfavoravel').click(function () {
        confirmaModal('<p>Deseja manifestar parecer "desfavorável"?</p><p>Caso necessário anote uma observação antes de continuar.</p>', function () {
            window.location = '/Workflow/EmDiligenciaAgetopDesfavoravel/' + $('#Id').val();
        });
    });

    /* EmAnaliseDeDocumentacao -> EmAnaliseTecnica */
    $('#EmAnaliseTecnica').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em análise técnica"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmAnaliseTecnica/' + $('#Id').val();
        });
    });
    
    /* EmAnaliseTecnica -> ParaAGerenciaDeExecucaoOrcamentaria */
    $('#ParaAGerenciaDeExecucaoOrcamentaria').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À gerência de execução orçamentária"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAGerenciaDeExecucaoOrcamentaria/' + $('#Id').val();
        });
    });

    /* AGerenciaDeExecucaoOrcamentaria -> ParaAutorizacaoLegislativa */
    $('#ParaAutorizacaoLegislativa').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Para autorização legislativa"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaParaAutorizacaoLegislativa/' + $('#Id').val();
        });
    });

    /* ParaAutorizacaoLegislativa, AGerenciaDeExecucaoOrcamentaria -> EmAnaliseJuridica */
    $('#EmAnaliseJuridica').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em análise jurídica"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmAnaliseJuridica/' + $('#Id').val();
        });
    });

    /* EmAnaliseJuridica -> AGerenciaDeConveniosParaAnotacoes */
    $('#AGerenciaDeConveniosParaAnotacoes').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À gerência de convênios para anotações"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAGerenciaDeConveniosParaAnotacoes/' + $('#Id').val();
        });
    });

    /* AGerenciaDeConveniosParaAnotacoes -> AGerenciaDeExecucaoOrcamentariaParaEmissaoEValidacaoDaNotaDeEmpenho */
    $('#AGerenciaDeExecucaoOrcamentariaParaEmissaoEValidacaoDaNotaDeEmpenho').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À gerência de execução orçamentária para emissão e validação da nota de empenho"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAGerenciaDeExecucaoOrcamentariaParaEmissaoEValidacaoDaNotaDeEmpenho/' + $('#Id').val();
        });
    });

    /* AGerenciaDeExecucaoOrcamentariaParaEmissaoEValidacaoDaNotaDeEmpenho -> AGerenciaDeConvenioParaElaboracaoDoTermoDeConvenioEColhimento */
    $('#AGerenciaDeConvenioParaElaboracaoDoTermoDeConvenioEColhimento').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À gerência de convênios para elaboração do termo de convênio e colhimento"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAGerenciaDeConvenioParaElaboracaoDoTermoDeConvenioEColhimento/' + $('#Id').val();
        });
    });

    /* AGerenciaDeConvenioParaElaboracaoDoTermoDeConvenioEColhimento -> AAdvocaciaSetorialParaParecerConclusivo */
    $('#AAdvocaciaSetorialParaParecerConclusivo').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À advocacia setorial para parecer conclusivo"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAAdvocaciaSetorialParaParecerConclusivo/' + $('#Id').val();
        });
    });

    /* AGerenciaDeConvenioParaElaboracaoDoTermoDeConvenioEColhimento -> APgeParaParecerConclusivo */
    $('#APgeParaParecerConclusivo').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À PGE para parecer conclusivo"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAPgeParaParecerConclusivo/' + $('#Id').val();
        });
    });

    /* APgeParaParecerConclusivo -> favoravel */
    $('#APgeParaParecerConclusivoFavoravel').click(function () {
        confirmaModal('<p>Deseja manifestar parecer "favorável"?</p>', function () {
            window.location = '/Workflow/APgeParaParecerConclusivoFavoravel/' + $('#Id').val();
        });
    });

    /* APgeParaParecerConclusivo -> desfavoravel */
    $('#APgeParaParecerConclusivoDesfavoravel').click(function () {
        confirmaModal('<p>Deseja manifestar parecer "desfavorável"?</p><p>Caso necessário anote uma observação antes de continuar.</p>', function () {
            window.location = '/Workflow/APgeParaParecerConclusivoDesfavoravel/' + $('#Id').val();
        });
    });

    /* AAdvocaciaSetorialParaParecerConclusivo -> ConvenioCelebrado */
    $('#ConvenioCelebrado').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Convênio celebrado"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaConvenioCelebrado/' + $('#Id').val();
        });
    });

    /* ConvenioCelebrado -> AGerenciaDeGestaoPlanejamentoEFinancasParaEmissaoDaOrdemDePagamento */
    $('#AGerenciaDeGestaoPlanejamentoEFinancasParaEmissaoDaOrdemDePagamento').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "À gerência de gestão, planejamento e finanças para emissão da ordem de pagamento"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAGerenciaDeGestaoPlanejamentoEFinancasParaEmissaoDaOrdemDePagamento/' + $('#Id').val();
        });
    });

    /* AGerenciaDeGestaoPlanejamentoEFinancasParaEmissaoDaOrdemDePagamento -> AoGestorResponsavelParaPrestacaoDeContas */
    $('#AoGestorResponsavelParaPrestacaoDeContas').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Ao gestor responsável para prestação de contas"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaAoGestorResponsavelParaPrestacaoDeContas/' + $('#Id').val();
        });
    });

    /* * -> EmDiligencia */
    $('#EmDiligencia').click(function () {
        confirmaModal('<p>Deseja alterar o status da solicitação para "Em diligência"?</p>', function () {
            window.location = '/Workflow/AlterarStatusParaEmDiligencia/' + $('#Id').val();
        });
    });

    /* Cancelar */
    $('#CancelarSolicitacao').click(function () {
        confirmaModal("<p>Tem certeza que deseja cancelar a solicitação?</p>", function () {
            window.location = '/Workflow/Cancelar/' + $('#Id').val();
        });
    });

    $(".btn-upload").click(function (obj) {
        var id = $(obj.currentTarget).parent().parent().parent().parent().parent().parent().prop('id');
        $('#' + id + ' .progress .progress-bar').css('width', '0%').attr('aria-valuenow', 0);
    });


});

var inicializaUpload = function (divId) {
    $('#' + divId + ' form').fileupload({
        dataType: 'json',
        url: '/Workflow/UploadFiles',
        autoUpload: true,
        done: function (e, data) {
            $.each(data.result, function (index, value) {
                appendToFiles('#' + divId + ' .append-files', value.name, value.id);
            });
            $('#atualizarHistorico').show();
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#' + divId + ' .progress .progress-bar').css('width', progress + '%').attr('aria-valuenow', progress);
    });
}

var appendToFiles = function (target, fileName, id) {
    $(target).append('<p><div class="btn-group" role="group"><button type="button" class="btn btn-default" style="width:300px;text-align:left"><i class="glyphicon glyphicon-file"></i> ' + fileName + '</button><button onclick="removeFile(this)" data-id="' + id + '" type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="right" title="Remover"><i class="glyphicon glyphicon-trash"></i></button></div></p>');
}

var removeFile = function (obj) {
    confirmaModal("<p>Tem certeza que deseja remover este arquivo?</p>", function () {
        var idToRemove = $(obj).data("id");

        $.ajax({
            type: "POST",
            url: "/Workflow/RemoveFile",
            data: { id: idToRemove },
            cache: false
        }).done(function (result) {
            $('#confirmaModal').modal('hide');
            if (result.sucesso) {
                $(obj).parent().remove();
                $('#atualizarHistorico').show();
            }
            else {
                alertaModal('<p>Ocorreu um erro durante a operação.</p><p>' + result.mensagem + '</p>');
            }
        });
    });
}

var downloadFile = function (obj) {
    var idToDownload = $(obj).data("id");
    window.location = "/Workflow/DownloadFile/" + idToDownload;
}

var somar = function () {
    var n1 = $('#ValorSolicitado').val();
    var n2 = $('#ValorContrapartida').val();
    if (!isNaN(n1)) {
        n1 = 0;
    } else {
        n1 = truncaDecimal(n1);
    }
    if (!isNaN(n2)) {
        n2 = 0;
    } else {
        n2 = truncaDecimal(n2);
    }
    var total = parseFloat(n1) + parseFloat(n2);
    total = formataDecimal(total, 2, ',', '.');
    $('#ValorTotal').val(total);
}
