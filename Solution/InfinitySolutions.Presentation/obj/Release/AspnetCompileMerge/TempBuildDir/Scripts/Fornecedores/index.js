$(function () {
});

$('#btnSalvarFornecedor').click(function (e) {
    e.preventDefault();

    limparMensagem('#formIncluirFornecedor');

    if (validarPreenchimento('#formIncluirFornecedor')) {
        $('#txtFornecedorTelefone').val(removerMascara($('#txtFornecedorTelefone').val().replace(' ', '')));
        $('#formIncluirFornecedor').submit();
    }

});


$('td').on('click', '.js-ExcluirFornecedor', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Fornecedores/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarFornecedor', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('Fornecedores/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var fornecedor = retorno.Entity;
                $('#hdnFornecedorCodigo').val(fornecedor.Codigo);
                $('#txtFornecedorNome').val(fornecedor.Nome);
                $('#txtFornecedorEmail').val(fornecedor.Email);
                $('#txtFornecedorTelefone').val(fornecedor.Telefone);

                $('#modalIncluirFornecedor').modal('show');
            }
        }, false);
    }
});