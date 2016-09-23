var executaScriptsDaPagina = function () {
    setBreadcrumb('Usuário', $("#Id").val() > 0 ? "Editar" : "Adicionar");
    setTimeout(function () { $('#NomeUsuario').focus() }, 100);
}