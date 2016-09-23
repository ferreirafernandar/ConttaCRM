var executaScriptsDaPagina = function () {
    setBreadcrumb('Configuração', "Editar");
    setTimeout(function () { $('#Conta').focus() }, 100);

    $('#btnTesteEnvioEmail').click(function () {
        testarEnvioEmail();
    });
}

var testarEnvioEmail = function () {

    var email = $("#destinatarioTesteEnvioEmail").val();

    $("#testando").text('Aguarde, testando...');

    $.ajax({
        type: "POST",
        url: "/Configuracao/TestaEnvioEmail",
        data: { email: email },
        cache: false
    }).done(function (result) {
        if (result.sucesso) {
            alertaModal('<p>E-mail de teste enviado com sucesso!</p>');
        }
        else {
            alertaModal('<p>Ocorreu um erro durante a operação.</p><p>' + result.mensagem + '</p>');
        }

        $("#testando").text('');
    });
}

