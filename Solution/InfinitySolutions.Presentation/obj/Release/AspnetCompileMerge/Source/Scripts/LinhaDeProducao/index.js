$('#btnLinhaDeProducaoPesquisar').click(function () {
    $('#formLinhaDeProducao').prop('action', rootUrl + 'LinhaDeProducao/Index');
    $('#formLinhaDeProducao').submit();
});

$('td').on('click', '.js-ConfirmarProducao', function () {

    if (confirm("Deseja Confirmar Produção?")) {

        requisicaoAjax('LinhaDeProducao/ConfirmarProducao?codigoLinhaProducao=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-TerceirizarProducao', function () {

    var tds = $(this).parent().parent().find('td');
    var codigoLinhaProducaoCodigoPedidoProduto = $(this).prop('id').split('|');

    $(tds[2]).html('<input class="form-control js-obrigatorio js-data" id="txtDataPrevisaoInicio" name="linhaProducaoTerceirizado.DataPrevisaoInicio" type="text" maxlength="10">' +
        '<input type="hidden" name="linhaProducaoTerceirizado.Codigo" value=' + +codigoLinhaProducaoCodigoPedidoProduto[0] + '>' +
        '<input type="hidden" name="linhaProducaoTerceirizado.Produto.CodigoPedidoProduto" value=' + +codigoLinhaProducaoCodigoPedidoProduto[1] + '>');

    $(tds[3]).html('<input class="form-control js-obrigatorio js-data" id="txtDataPrevisaoFim" name="linhaProducaoTerceirizado.DataPrevisaoFim" type="text" maxlength="10">');

    var options = '';
    requisicaoAjax('Pedido/ListarTerceirizados', null, function (retorno) {
        if (retorno.IsValido) {
            var terceirizados = retorno.Entity;
            $.each(terceirizados, function (index, terceirizado) {
                options += '<option value="' + terceirizado.Codigo + '">' + terceirizado.Nome.toUpperCase() + '</option>';
            }, false);
        }
    }, false);

    $(tds[4]).html(
        '<select class="form-control js-obrigatorio js-LinhaProducaoTerceirizados" id="ddlLinhaProducaoTerceirizados" name="linhaProducaoTerceirizado.Terceirizado.Codigo">'
        + options +
        '</select > '
    );

    $(this).parent().parent().find('.js-TerceirizarProducao, .js-ConfirmarProducao').addClass('invisivel');
    $(this).parent().parent().find('.js-ConfirmarTerceirizarProducao').removeClass('invisivel');

});

$('td').on('click', '.js-ConfirmarTerceirizarProducao', function () {

    if (confirm("Deseja Terceirizar?")) {

        requisicaoAjax('LinhaDeProducao/Terceirizar', $('#formLinhaDeProducao').serializeArray(), function (retorno) {
            if (retorno.IsValido) {
                window.location.href = window.location.href;
            }
        });

    }
});