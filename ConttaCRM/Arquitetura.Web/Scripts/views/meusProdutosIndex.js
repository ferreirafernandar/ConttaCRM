var executaScriptsDaPagina = function () {
    setBreadcrumb('Meus Produtos');
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
                carregarPaginaAjax('/MeusProdutos/Index');
            } else {
                MensagemErro(result.message);
            }
        });
    });
}