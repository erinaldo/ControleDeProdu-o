$(function () {
});

$('#btnSalvarFuncionario').click(function (e) {
    e.preventDefault();

    limparMensagem('#formIncluirFuncionario');

    if (validarPreenchimento('#formIncluirFuncionario')) {
        $('#txtFuncionarioTelefone').val(removerMascara($('#txtFuncionarioTelefone').val().replace(' ', '')));
        $('#formIncluirFuncionario').submit();
    }

});


$('td').on('click', '.js-ExcluirFuncionario', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Funcionario/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarFuncionario', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('Funcionario/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var funcionario = retorno.Entity;
                $('#hdnFuncionarioCodigo').val(funcionario.Codigo);
                $('#txtFuncionarioNome').val(funcionario.Nome);
                $('#ddlFuncionarioTipoFuncaoFuncionario').val(funcionario.TipoFuncaoFuncionario.Codigo);

                if (funcionario.Telefone > 0)
                    $('#txtFuncionarioTelefone').val(funcionario.Telefone);
                else
                    $('#txtFuncionarioTelefone').val('');

                $('#modalIncluirFuncionario').modal('show');
            }
        }, false);
    }
});