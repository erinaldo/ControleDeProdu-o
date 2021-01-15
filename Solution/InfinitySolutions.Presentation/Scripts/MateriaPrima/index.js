$('#btnSalvarMateriaPrima').click(function (e) {
    e.preventDefault();

    removerMascaras();

    if (validarPreenchimento('#formIncluirMateriaPrima'))
        $('#formIncluirMateriaPrima').submit();

});


$('td').on('click', '.js-ExcluirMateriaPrima', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('MateriaPrima/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarMateriaPrima', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('MateriaPrima/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var materiaPrima = retorno.Entity;

                $('#hdnMateriaPrimaCodigo').val(materiaPrima.Codigo);
                $('#txtMateriaPrimaDescricao').val(materiaPrima.Descricao);
                $('#ddlMateriaPrimaTipoUnidadeMedida').val(materiaPrima.TipoUnidadeMedida.Codigo);
                $('#txtMateriaPrimaQuantidade').val(materiaPrima.Quantidade);
                $('#txtMateriaPrimaFornecedor').val(materiaPrima.Fornecedor);
                $('#txtMateriaPrimaValor').val(replaceAll(materiaPrima.Valor, '.', ','));

                $('#modalIncluirMateriaPrima').modal('show');
            }
        });

    }
});