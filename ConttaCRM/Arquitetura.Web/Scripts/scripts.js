var initCustomApplication = function () {
    $.datepicker.setDefaults($.datepicker.regional["pt-BR"]);

    $(".cpf").mask("999.999.999-99");
    $(".cnpj").mask("99.999.999/9999-99");
    $(".date").mask("99/99/9999");
    $(".datepicker").mask("99/99/9999");
    $(".telefone").mask("(99) 9999-9999?9");
    $(".cep").mask("99.999-999");
    $(".valor").maskMoney({ decimal: ",", thousands: "", allowZero: true, defaultZero: false });

    $(".conf-delete").click(function (e) {
        e.preventDefault();
        var location = $(this).attr('href');
        /* bootbox.backdrop(false); */
        bootbox.confirm("Confirma exclusão?", "Não", "Sim", function (confirmed) {
            if (confirmed) {
                window.location.replace(location);
            }
        });
    });

    $(".somentenumeros").keydown(function (e) {

        // Allow: backspace, delete, tab, escape, enter and ',' = 188
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 188]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    sinalizeAbasComInsonsistenciaDeValidacao();
    assinaEventoDisabledButtonOnSubmit();
}

var carregarPaginaAjax = function (url) {
    $("#page-content").load(url, function () {
        montaPaginaAoFinalizarCarregamento();
    });
}

var montaPaginaAoFinalizarCarregamento = function () {
    pageSetUp();
    initCustomApplication();
    if (typeof executaScriptsDaPagina == 'function') {
        executaScriptsDaPagina();
    }
}


var setBreadcrumb = function (controller, action) {

    if (action == undefined) {
        $('.breadcrumb').html('<li>' + controller + '</li>');
    }
    else {
        $('.breadcrumb').html('<li>' + controller + '</li><li>' + action + '</li>');
    }
}

var makeFormDisabled = function (formId) {
    $("#" + formId + " :input").attr("readonly", true);
}

/**
 * Carrega arquivos javascript dinamicamente e executa uma funcao de callback.
 * @param {string ou array} scripts 
 * @param {function} callback
 */
var carregarScripts = function (scripts, callback) {

    if (scripts == null) {
        if (callback != undefined && callback != null && $.isFunction(callback)) {
            callback();
        }
        return;
    }

    if (Array.isArray(scripts)) {

        if (scripts.length == 0) {
            callback();
            return;
        }

        var script = scripts[0];
        scripts = scripts.slice(1);
        $.getScript(script, function () { carregarScripts(scripts, callback); });
    }
    else {
        var script = scripts;
        scripts = null;
        $.getScript(script, function () { carregarScripts(scripts, callback); });
    }
}

var truncaDecimal = function (v) {
    v = v.replace(/\D/g, "");
    v = new String(Number(v));
    var len = v.length;
    if (1 == len)
        v = v.replace(/(\d)/, "0.0$1");
    else if (2 == len)
        v = v.replace(/(\d)/, "0.$1");
    else if (len > 2) {
        v = v.replace(/(\d{2})$/, '.$1');
    }
    return v;
}

function formataDecimal(number, decimals, dec_point, thousands_sep) {
    var n = number, c = isNaN(decimals = Math.abs(decimals)) ? 2 : decimals;
    var d = dec_point == undefined ? "," : dec_point;
    var t = thousands_sep == undefined ? "." : thousands_sep, s = n < 0 ? "-" : "";
    var i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
}

var sinalizeAbasComInsonsistenciaDeValidacao = function () {

    var primeiraAbaComInconsistencia = null;

    $('.input-validation-error').each(function () {
        var propId = $(this).closest('.tab-pane').prop('id');
        if (propId != undefined && propId != null && propId != '') {
            if (primeiraAbaComInconsistencia == null) {
                primeiraAbaComInconsistencia = propId;
            }
            $('a[href$="#' + propId + '"]').css('background-color', '#ffeeee').css('color', '#ff0000');
        }
    });

    if (primeiraAbaComInconsistencia != null) {
        $('.nav-tabs a[href="#' + primeiraAbaComInconsistencia + '"]').tab('show');
    }
}


var OnSuccess = function () {
    montaPaginaAoFinalizarCarregamento();
}

var OnFailure = function (response) {
    alert("Falha ao processar a requisição!");
}


var ConfirmaExclusao = function (callbackSim, callbackNao) {
    $.SmartMessageBox({
        title: "<i class='fa fa-trash-o txt-color-orangeDark'></i><span class='txt-color-orangeDark'> <strong>Atenção</strong></span> !",
        content: "Tem certeza que deseja excluir?",
        buttons: '[Não][Sim]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Sim") {
            if (callbackSim != undefined && callbackSim != null) {
                callbackSim();
            }
        }
        if (ButtonPressed === "Não") {
            if (callbackNao != undefined && callbackNao != null) {
                callbackNao();
            }
        }
    });
}

var ConfirmaSimNao = function (titulo, texto, callbackSim, callbackNao) {
    $.SmartMessageBox({
        title: titulo,
        content: texto,
        buttons: '[Não][Sim]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Sim") {
            if (callbackSim != undefined && callbackSim != null) {
                callbackSim();
            }
        }
        if (ButtonPressed === "Não") {
            if (callbackNao != undefined && callbackNao != null) {
                callbackNao();
            }
        }
    });
}

var MensagemSucesso = function (mensagem) {
    $.smallBox({
        title: "Sucesso!",
        content: "<i>" + mensagem + "</i>",
        color: "#659265",
        iconSmall: "fa fa-check fa-2x fadeInRight animated",
        timeout: 4000
    });
}

var MensagemErro = function (mensagem) {
    $.smallBox({
        title: "Erro!",
        content: "<i>" + mensagem + "</i>",
        color: "#C46A69",
        iconSmall: "fa fa-times fa-2x fadeInRight animated",
        timeout: 4000
    });
}

var ConfirmaLogout = function () {
    $.SmartMessageBox({
        title: "<i class='fa fa-sign-out txt-color-orangeDark'></i> Sair <span class='txt-color-orangeDark'><strong>" + $("#show-shortcut").text() + "</strong></span> ?",
        content: "Você pode melhorar sua segurança depois de sair fechando a janela do navegador",
        buttons: '[Não][Sim]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Sim") {
            $.root_.addClass("animated fadeOutUp");
            setTimeout(function () {
                window.location = "/Home/Sair";
            }, 1e3);
        }
    });
}

var iniciaSearchBar = function () {
    $('#select_search_pessoas').select2({
        width: '300px',
        placeholder: "Buscar pessoas...",
        allowClear: true,
        ajax: {
            url: "/Home/GetPessoas",
            dataType: 'json',
            quietMillis: 250,
            data: function (term, page) {
                return {
                    q: term,
                    page: page
                };
            },
            results: function (data, page) {
                var more = (page * 30) < data.total_count;
                return { results: data.items, more: more };
            },
            cache: false
        }
    });

    $('#btn-header-search').click(function (e) {
        e.preventDefault();
    });

    $('#s2id_select_search_pessoas').find('.select2-arrow').find('b').remove();

    $('#select_search_pessoas').on("select2-selecting", function (e) {
        $.ajax({
            type: "GET",
            url: "/Home/AbrirPessoa",
            data: { id: e.val }
        }).done(function (returnUrl) {
            if (returnUrl == "false") {
                alert("Não foi possível completar a operação.")
            } else {
                carregarPaginaAjax(returnUrl);
            }
        });
    });
}


var onGridLoadCallBack = function (id) {
    $("#" + id).unbind("click");
    $("#" + id).off("click");
    $("#" + id).on('click', 'tbody tr', function (e) {
        var url = $(this).find('td:not(:empty):first').find(':hidden').val();
        if (url != undefined && url != null && url != '') {
            carregarPaginaAjax(url);
        }
    });
}

var assinaEventoDisabledButtonOnSubmit = function () {
    $("[data-loading-text]").unbind("click");
    $("[data-loading-text]").click(function (e) {
        var _this = this;
        e.preventDefault();
        var novoTexto = $(this).data('loading-text');
        var textoAnterior = $(this).html();
        $(_this).prop("disabled", true);
        $(_this).html(novoTexto);
        setTimeout(function () {
            $(_this).html(textoAnterior)
            $(_this).prop("disabled", false);
        }, 10000);
        $(_this).trigger('submit');
        return true;
    });
}