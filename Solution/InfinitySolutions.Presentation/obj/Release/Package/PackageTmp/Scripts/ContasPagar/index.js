$('#ddlContaPagarTipoContaPagar').change(function () {

    requisicaoAjax('ContasPagar/ConsultarFormaPagamento?codigoFormaPagamento=' + $('#ddlContaPagarTipoContaPagar').val(), null, function (retorno) {

        $('#txtContaPagarQuantidadeParcelas').prop('disabled', !retorno.Entity.ContemParcelas);
        if (!retorno.Entity.ContemParcelas)
            $('#txtContaPagarQuantidadeParcelas').val('');

        if (!retorno.Entity.ContemParcelas) {
            $('#divContaPagarParcela').addClass("invisivel");
        }
        else {
            $('#divContaPagarParcela').removeClass("invisivel");
        }

    }, false);

});

$('#txtContaPagarQuantidadeParcelas').blur(function () {

    var quantidadeParcelas = +$('#txtContaPagarQuantidadeParcelas').val();

    $('#tblContaPagarParcelas tbody').html('');
    $('#divContaPagarParcela').addClass('invisivel');

    if (quantidadeParcelas > 0) {

        var valor = +$('#txtContaPagarValor').val().replaceAll('.', '').replace(',', '.') / quantidadeParcelas;
        var dataVencimento = $('#txtContaPagarDataVencimento').val();
        var dataArray = dataVencimento.split('/');
        var dia = dataArray[0];
        var mes = +dataArray[1];
        var ano = dataArray[2];

        var dataParcela = new Date(ano, mes - 1, dia);
        $('#divContaPagarParcela').removeClass('invisivel');

        for (var i = 0; i < quantidadeParcelas; i++) {

            var html = '';

            html += '<tr>';
            html += '    <td scope="col">' + (i + 1).toString() + '</td> <input name="ContaPagar.Parcelas[' + i.toString() + '].Numero" type="hidden" value=' + (i + 1).toString() + ' >';
            html += '    <td scope="col"><input class="form-control js-data js-dadosParcela" name="ContaPagar.Parcelas[' + i.toString() + '].DataVencimento" type="text" value=' + dataParcela.toLocaleDateString('pt-BR') + ' maxlength="10"></td>';
            html += '    <td scope="col"><input class="form-control js-monetaria js-dadosParcela"  type="text" value=' + toFixed(valor, 2) + ' maxlength="13">';
            html += '    <input name="ContaPagar.Parcelas[' + i.toString() + '].Valor" type="hidden" value=' + toFixed(valor, 2).replace('.', ',') + ' ></td>';
            html += '</tr>';

            $('#tblContaPagarParcelas tbody').append(html);

            dataParcela.setMonth(dataParcela.getMonth() + 1);
        }
    }
});

$('td').on('click', '.js-ExcluirContaPagar', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('ContasPagar/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });
    }
});

$('td').on('click', '.js-AlterarContaPagar', function (e) {
    e.preventDefault();

    if (confirm("Deseja Alterar?")) {
        carregarContaPagar($(this).prop('id'));
    }
});

$('td').on('click', '.js-DarBaixaPagamentoContaPagar', function () {

    if (confirm("Deseja dar baixa no Pagamento?")) {

        var tds = $(this).parent().parent().find('td');

        $('#hdnPagamentoContaPagarCodigo').val($(this).prop('id'));

        $('#txtContaPagarPagamentoValor').val($(tds[5]).text().replace('R$ ', ''));
        $('#txtContaPagarPagamentoJuros').val();
        $('#txtContaPagarPagamentoDataPagamento').val();
        $('#txtContaPagarPagamentoDataVencimento,#hdnDataVencimento').val($(tds[3]).text());

        $('#modalRegistrarPagamento').modal("show");
    }

});


$('#btnSalvarContaPagar').click(function (e) {
    e.preventDefault();

    if (validarPreenchimento('#formContasPagar')) {
        $('#txtContaPagarValor').val($('#txtContaPagarValor').val().replaceAll('.', ''));
        $('#formContasPagar').prop('action', rootUrl + 'ContasPagar/Salvar');
        $('#formContasPagar').submit();
    }

});

$('#btnBaixaPagamento').click(function (e) {
    e.preventDefault();

    if (validarPreenchimento('#formDarBaixaPagamento')) {
        $('#txtContaPagarPagamentoValor').val($('#txtContaPagarPagamentoValor').val().replaceAll('.', ''));
        $('#txtContaPagarPagamentoJuros').val($('#txtContaPagarPagamentoJuros').val().replaceAll('.', ''));
        $('#formDarBaixaPagamento').submit();
    }

});

$('#btnContasPagarPesquisar').click(function () {
    $('#formContasPagar').prop('action', rootUrl + 'ContasPagar/Index');
    $('#formContasPagar').submit();
});


function carregarContaPagar(idContaPagar) {
    requisicaoAjax('ContasPagar/Consultar?codigo=' + idContaPagar, null, function (retorno) {
        if (retorno.IsValido) {

            var ContaPagar = retorno.Entity;
            $('#hdnContaPagarCodigo').val(ContaPagar.Codigo);

            $('#txtContaPagarDescricao').val(ContaPagar.Descricao);
            $('#ddlContaPagarFornecedor').val(ContaPagar.Fornecedor.Codigo);
            $('#ddlContaPagarTipoStatusContaPagar').val(ContaPagar.TipoStatusContaPagar.Codigo);
            $('#txtContaPagarDataEmissao').val(formatarData(ContaPagar.DataEmissao));
            $('#txtContaPagarDataVencimento').val(formatarData(ContaPagar.DataVencimento));
            $('#txtContaPagarValor').val(toFixed(ContaPagar.Valor, 2).replace('.', ','));
            $('#ddlContaPagarTipoContaPagar').val(ContaPagar.TipoContaPagar.Codigo);

            $('#txtContaPagarQuantidadeParcelas').prop('disabled', !ContaPagar.TipoContaPagar.ContemParcelas);

            if (ContaPagar.TipoContaPagar.ContemParcelas)
                $('#txtContaPagarQuantidadeParcelas').val(ContaPagar.QuantidadeParcelas);

            $('#hdnContaPagarCodigo').val(ContaPagar.Codigo);

            $('#modalIncluirContaPagar').modal("show");
        }
    });
}