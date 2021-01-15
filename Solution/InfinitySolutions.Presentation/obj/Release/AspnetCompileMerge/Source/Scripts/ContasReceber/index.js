$('td').on('click', '.js-ExcluirContaReceber', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('ContasReceber/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });
    }
});

$('td').on('click', '.js-AlterarContaReceber', function (e) {
    e.preventDefault();

    if (confirm("Deseja Alterar?")) {
        carregarContaReceber($(this).prop('id'));
    }
});

$('td').on('click', '.js-DarBaixaPagamentoContaReceber', function () {

    if (confirm("Deseja Dar Baixa no Pagamento?")) {

        requisicaoAjax('ContasReceber/DarBaixaPagamento?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });
    }

});

$('#btnSalvarContaReceber').click(function (e) {
    e.preventDefault();

    if (validarPreenchimento('#formContasReceber')) {
        $('#txtContaReceberValor').val($('#txtContaReceberValor').val().replaceAll('.', ''));
        $('#formContasReceber').prop('action', rootUrl + 'ContasReceber/Salvar');
        $('#formContasReceber').submit();
    }

});

$('#btnContasReceberPesquisar').click(function () {
    $('#formContasReceber').prop('action', rootUrl + 'ContasReceber/Index');
    $('#formContasReceber').submit();
})

function carregarContaReceber(idContaReceber) {
    requisicaoAjax('ContasReceber/Consultar?codigo=' + idContaReceber, null, function (retorno) {
        if (retorno.IsValido) {

            var contaReceber = retorno.Entity;
            $('#hdnContaReceberCodigo').val(contaReceber.Codigo);
            $('#ddlContaReceberCliente').val(contaReceber.Cliente.Codigo);
            $('#txtContaReceberDataVencimento').val(formatarData(contaReceber.DataVencimento));
            $('#txtContaReceberDataEmissao').val(formatarData(contaReceber.DataEmissao));
            $('#ddlContaReceberPedido').val(contaReceber.Pedido.Codigo);
            $('#txtContaReceberValor').val(toFixed(contaReceber.Valor, 2).replace('.', ','));
            $('#txtContaReceberNotaFiscal').val(contaReceber.NotaFiscal);

            $('#txtContaReceberPedidoExterno').val(contaReceber.PedidoExterno);
            $('#txtContaReceberNotaFiscalExterna').val(contaReceber.NotaFiscalExterna);

            $('#ddlContaReceberFormaPagamentoContaReceber').val(contaReceber.TipoFormaPagamentoContaReceber.Codigo);

            $('#modalIncluirContaReceber').modal("show");
        }
    });
}