var executaScriptsDaPagina = function () {
    setBreadcrumb('Cliente', $("#Id").val() > 0 ? "Editar" : "Adicionar");
    setTimeout(function () { $('#Nome').focus() }, 100);

    $('#dialog_simple').dialog({
        appendTo: '#page-content',
        autoOpen: false,
        width: 630,
        resizable: false,
        modal: true,
        buttons: [{
            html: "<i class='fa fa-save'></i>&nbsp; Enviar",
            "class": "btn btn-danger",
            click: function () {
                $('#formInformarPagamento').trigger("submit");
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; Cancelar",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dialog_movimentacao').dialog({
        appendTo: '#page-content',
        autoOpen: false,
        width: 630,
        resizable: false,
        modal: true,
        buttons: [{
            html: "<i class='fa fa-save'></i>&nbsp; Salvar",
            "class": "btn btn-danger",
            click: function () {
                $('#formMovimentacao').trigger("submit");
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; Cancelar",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    assineBtnOperacao();
    assineBtnConverter();
}

var cancelarCompra = function (produtoUsuarioId, nome) {
    ConfirmaSimNao('Cancelar compra de ' + nome, 'Tem certeza que deseja cancelar a compra deste produto?', function () {
        $.ajax({
            method: "POST",
            url: "/MeusProdutos/CancelarCompra",
            data: { produtoUsuarioId: produtoUsuarioId }
        }).done(function (result) {
            if (result.success) {
                MensagemSucesso('Compra cancelada com sucesso!');
                carregarPaginaAjax('/Cliente/Editar/' + $('#Id').val());
            } else {
                MensagemErro(result.message);
            }
        });
    });
}

var informarPagamento = function (faturaId, valor) {
    $('#divFormPagamento').find('.alert').remove();
    $('#Data').val('');
    $('#Documento').val('');
    $('#FaturaId').val(faturaId);
    $('#ValorPago').val(valor);
    $('#dialog_simple').dialog('open');
    return false;
}

var pagamentoInformadoSucesso = function (usuarioId) {
    $('#dialog_simple').dialog('close');
    MensagemSucesso('Pagamento informado com sucesso.');
    carregarPaginaAjax('/Cliente/Editar/' + usuarioId);
}

var cancelarPagamento = function (faturaId) {
    ConfirmaSimNao('Atenção!', 'Tem certeza que deseja cancelar o pagamento desta fatura?', function () {
        $.ajax({
            method: "POST",
            url: "/Cliente/CancelarPagamento",
            data: { faturaId: faturaId }
        }).done(function (result) {
            if (result.success) {
                MensagemSucesso('Pagamento cancelado com sucesso!');
                carregarPaginaAjax('/Cliente/Editar/' + $('#Id').val());
            } else {
                MensagemErro(result.message);
            }
        });
    });
}

var assineBtnOperacao = function () {
    $('#btnOperacao').click(function () {
        abrirModalMovimentacao();
    });
}

var assineBtnConverter = function () {
    $('#btnConverter').click(function () {
        ConfirmaSimNao('Atenção!', 'Tem certeza que deseja converter este usuário para afiliado?', function () {
            if ($('#AfiliadoId').val()) {
                ConfirmaSimNao('Atenção!', 'Este usuário está vinculado a um afiliado. Tem certeza que deseja continuar?', function () {
                    executaConverter();
                });
            }
            else {
                executaConverter();
            }
        });
    });
}

var executaConverter = function () {
    var usuarioId = $('#Id').val();
    $.ajax({
        method: "POST",
        url: "/Cliente/ConverterParaAfiliado",
        data: { usuarioId: usuarioId }
    }).done(function (result) {
        if (result.success) {
            MensagemSucesso('Conversão realizada com sucesso!');
            carregarPaginaAjax('/Cliente/Editar/' + $('#Id').val());
        } else {
            MensagemErro(result.message);
        }
    });
}

var abrirModalMovimentacao = function () {
    $('#divFormMovimentacao').find('.alert').remove();
    $('#usuarioId').val($('#Id').val());
    $('#Data').val('');
    $('#Operacao').val('');
    $('#Motivo').val();
    $('#Valor').val();
    $('#dialog_movimentacao').dialog('open');
    return false;
}

var movimentacaoSucesso = function () {
    $('#dialog_movimentacao').dialog('close');
    MensagemSucesso('Operação realizada com sucesso.');
    carregarPaginaAjax('/Cliente/Editar/' + $('#Id').val());
}
