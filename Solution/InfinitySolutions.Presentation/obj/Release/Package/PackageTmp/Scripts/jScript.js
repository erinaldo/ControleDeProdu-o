$(document).ready(function () {

    configurarLoading();

    $('.js-telefone').mask('(00) 00000-0000');
    $('.js-cnpj').mask('99.999.999/9999-99');
    $('.js-ie').mask('999.999.999.999');

    $('.js-numero').mask('###################0');
    $('.js-monetaria').mask('00.000.000,00', { reverse: true });
    $('.js-decimal').mask('00.000.000,00', { reverse: true });
    $('.js-decimal-um').mask('00.000.000,0', { reverse: true });
    $('.js-porcentagem').mask('000,00', { reverse: true });
    $('.editable-select').editableSelect();

    (function ($) {
        $.fn.datepicker.dates['pt-BR'] = {
            days: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
            daysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"],
            daysMin: ["Do", "Se", "Te", "Qu", "Qu", "Se", "Sa"],
            months: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
            monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
            today: "Hoje",
            monthsTitle: "Meses",
            clear: "Limpar",
            format: "dd/mm/yyyy"
        };
    }(jQuery));

    $('.js-data').mask('00/00/0000').datepicker({
        language: 'pt-BR',
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true
    });

});

function validarPreenchimento(seletor) {

    var camposErrados = [];

    limparMensagem(seletor);

    $(seletor + ' :input.js-obrigatorio').not('input[type=submit]').not(':disabled').each(function (indice, elemento) {

        if ($(elemento).val() == '') {
            $(elemento).parent().addClass('campoComErro');
            camposErrados.push(elemento);
        }

    });

    if (camposErrados.length > 0) {
        sobeScroll();
        $(seletor + ' .js-escrita-mensagem').text('*Preencha os Campos Obrigatórios.')
        $(seletor + ' .js-mensagem').removeClass('invisivel');
        $(seletor + ' .js-mensagem').addClass('alert-danger');
    }

    return camposErrados.length == 0;
};

function validarQuantidadeMaior($valorMaximo, $valorAtual) {
    if (valorAtual > valorMaximo) {
        sobeScroll();
        $(seletor + ' .js-escrita-mensagem').text('*Valor ' + $valorAtual.parent().find('label').text() + ' não pode ser maior que ' + $valorMaximo.parent().find('label').text() + ' .')
        $(seletor + ' .js-mensagem').removeClass('invisivel');
        $(seletor + ' .js-mensagem').addClass('alert-danger');
    }
}



function limparCampos(idForm) {
    $('form' + idForm + ' :input').not('input[type=submit]').not('input[type=hidden]').each(function (indice, elemento) {
        $(elemento).val('');
    });
}

function limparMensagem(seletor) {
    $(seletor + ' .campoComErro').removeClass('campoComErro');
    $(seletor + ' .js-mensagem').addClass('invisivel');
    $(seletor + ' .js-mensagem').removeClass('alert-danger, campoComErro');
    $(seletor + ' .js-escrita-mensagem').text('')
}

$('.js-itens-menu a').click(function () {
    limparMensagem('');
});



function requisicaoAjax(urlMetodo, parametros, funcSucesso, isAsync, method, isSubirScrol) {
    $.ajax({
        url: rootUrl + urlMetodo,
        data: parametros,
        type: method ? method : 'post',
        async: isAsync == undefined || isAsync,
        dataType: 'json',
        cache: false,
        success: function (retorno) {

            if (retorno.IsValido != undefined) {
                if (retorno.IsValido) {
                    funcSucesso(retorno);
                }
                else if (retorno.Mensagem != undefined && retorno.Mensagem != '') {
                    alert(retorno.Mensagem);
                }
                else
                    funcSucesso(retorno);
            }
            else
                funcSucesso(retorno);
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}

function sobeScroll(idCampo) {
    var top = 0;
    if (idCampo != undefined) {
        top = +$("#" + idCampo).position().top;
        if (!top)
            top = 0;
    }
    $("html, body").animate({ scrollTop: top }, "slow");
}

function toFixed(value, precision) {
    var precision = precision || 0,
        power = Math.pow(10, precision),
        absValue = Math.abs(Math.round(value * power)),
        result = (value < 0 ? '-' : '') + String(Math.floor(absValue / power));

    if (precision > 0) {
        var fraction = String(absValue % power),
            padding = new Array(Math.max(precision - fraction.length, 0) + 1).join('0');
        result += '.' + padding + fraction;
    }
    return result;
}

function replaceAll(str, de, para) {
    var pos = str.toString().indexOf(de);
    while (pos > -1) {
        str = str.toString().replace(de, para);
        pos = str.toString().indexOf(de);
    }
    return (str);
}

function formatarMonetaria(valor) {

    if (valor != undefined)
        return valor.toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        });
}

function removerMascara(descricao) {

    while (descricao.indexOf('.') != -1)
        descricao = descricao.replace('.', '');

    while (descricao.indexOf('-') != -1)
        descricao = descricao.replace('-', '');

    while (descricao.indexOf('/') != -1)
        descricao = descricao.replace('/', '');

    while (descricao.indexOf('(') != -1)
        descricao = descricao.replace('(', '');

    while (descricao.indexOf(')') != -1)
        descricao = descricao.replace(')', '');

    return descricao;
}

function removerMascaras() {

    $.each($('.js-monetaria, .js-decimal'), function (index, item) {
        $(item).val($(item).val().replaceAll('.', ''));
    });

}

function formatarData(value) {
    if (value == null)
        return null;
    var dataTexto = value.replace('/', '').replace('/', '').replace('Date', '').replace('(', '').replace(')', '');
    var date = new Date(parseInt(dataTexto));
    return padLeft(date.getDate().toString(), 2, '0') + "/" + padLeft((date.getMonth() + 1).toString(), 2, '0') + "/" + padLeft(date.getFullYear().toString(), 4, '0');
};


function formatarDataHora(value) {
    if (value == null)
        return null;
    var dataTexto = value.replace('/', '').replace('/', '').replace('Date', '').replace('(', '').replace(')', '');
    var date = new Date(parseInt(dataTexto));
    return padLeft(date.getDate().toString(), 2, '0') + "/" + padLeft((date.getMonth() + 1).toString(), 2, '0') + "/" + padLeft(date.getFullYear().toString(), 4, '0') + " " + padLeft(date.getHours().toString(), 2, '0') + ":" + padLeft(date.getMinutes().toString(), 2, '0');
};

function padLeft(texto, quantidadeCaracTotal, caracterPreenchimento) {
    return Array(quantidadeCaracTotal - String(texto).length + 1).join(caracterPreenchimento || '0') + texto;
}

Number.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
}

function removerLinha($elemento, funcaoPosExclusao) {

    var tr = $elemento.closest('tr');
    tr.fadeOut(400, function () {
        tr.remove();
        funcaoPosExclusao();
    });
    return false;

}

function exibirMensagem(idDiv, mensagem) {
    $('#' + idDiv + ' span').text(mensagem);
    $('#' + idDiv + '').removeClass('invisivel');
}



var $document, $window;
var $loadingContainer;
var $loadingImage;
var $loadingBackdrop;

function configurarLoading() {

    $(document).ajaxStart(function () {
        exibirLoading();
    }).ajaxSend(function () {
        exibirLoading();
    }).ajaxStop(function () {
        esconderLoading();
    }).ajaxError(function () {
        esconderLoading();
    });
    $('form').submit(exibirLoading);

    $document = $(document);
    $window = $(window);
    $loadingContainer = $('#loading');
    $loadingImage = $loadingContainer.find('.loading-img img');
    $loadingBackdrop = $loadingContainer.find('.loading-backdrop');
}

function exibirLoading(evt) {

    if ($('.js-loading').length == 0) {

        var div = '<div class="js-loading" id="load"><div id="mask"></div><div id="ajax">';
        $('body').prepend(div + '<img src="' + rootUrl + 'Content/imagens/carregando.gif"/></div></div>');

        $('#ajax').css({
            width: '128px',
            height: '128px',
            position: 'fixed',
            top: '50%',
            left: '50%',
            marginTop: '-64px',
            marginRight: '0px',
            marginBottom: '0px',
            marginLeft: '-64px',
            zIndex: 9999
        });

        $('#mask').css({
            backgroundColor: '#fff',
            opacity: '0.6',
            top: '0',
            left: '0',
            width: '100%',
            height: $(document).height(),
            position: 'fixed',
            zIndex: 9998
        });
    }
}

function esconderLoading() {
    $('.js-loading').remove();
}

function fecharModal(id) {
    $('#' + id + ' .close').click();
    $('#' + id).modal('hide');

    while ($('.modal-backdrop').length > 0)
        $('.modal-backdrop').remove();

    $('.ng-scope').removeClass('modal-open');
}