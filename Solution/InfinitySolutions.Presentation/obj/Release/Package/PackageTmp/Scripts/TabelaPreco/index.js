$('#btnSalvarTabelaPreco').click(function (e) {
    e.preventDefault();

    if (validarPreenchimento('#formIncluirTabelaPreco'))
        $('#formIncluirTabelaPreco').submit();

});

$('td').on('click', '.js-ExcluirTabelaPreco', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('TabelaPreco/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarTabelaPreco', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('TabelaPreco/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var tabelaPreco = retorno.Entity;

                $('#hdnTabelaPrecoCodigo').val(tabelaPreco.Codigo);
                $('#txtTabelaPrecoDescricao').val(tabelaPreco.Descricao);
                $('#txtTabelaPrecoImposto').val(tabelaPreco.Imposto.toString().replaceAll('.', ','));
                $('#txtTabelaPrecoLucro').val(tabelaPreco.Lucro.toString().replaceAll('.', ','));

                $('#modalIncluirMateriaPrima').modal('show');
            }
        });

    }
});