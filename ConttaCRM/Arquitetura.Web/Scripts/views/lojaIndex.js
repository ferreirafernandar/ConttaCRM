var executaScriptsDaPagina = function () {
    setBreadcrumb('Loja', "Comprar");

    $('#dialog_simple').dialog({
        appendTo: '#page-content',
        autoOpen: false,
        width: 630,
        resizable: false,
        modal: true,
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Fechar",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
}

var saibaMais = function (elementId) {
    var html = $('#' + elementId).html();
    $('#conteudoSaibaMais').html(html);
    $('#dialog_simple').dialog('open');
}

var comprar = function (produtoId, nome) {
    ConfirmaSimNao('Comprar ' + nome, 'Tem certeza que deseja comprar este produto?', function () {
        $.ajax({
            method: "POST",
            url: "/Loja/Comprar",
            data: { produtoId: produtoId }
        }).done(function (result) {
            if (result.success) {
                MensagemSucesso('Compra realizada com sucesso!');
                carregarPaginaAjax('/Loja/Index');
            } else {
                MensagemErro(result.message);
            }
        });
    });
}

var cancelarCompra = function (produtoId, nome) {
    ConfirmaSimNao('Cancelar compra de ' + nome, 'Tem certeza que deseja cancelar a compra deste produto?', function () {
        $.ajax({
            method: "POST",
            url: "/Loja/CancelarCompra",
            data: { produtoId: produtoId }
        }).done(function (result) {
            if (result.success) {
                MensagemSucesso('Compra cancelada com sucesso!');
                carregarPaginaAjax('/Loja/Index');
            } else {
                MensagemErro(result.message);
            }
        });
    });
}