$(function () {
});

$('#btnSalvarTerceirizado').click(function (e) {
    e.preventDefault();

    limparMensagem('#formIncluirTerceirizado');

    if (validarPreenchimento('#formIncluirTerceirizado')) {
        $('#txtTerceirizadoTelefone').val(removerMascara($('#txtTerceirizadoTelefone').val().replace(' ', '')));
        $('#formIncluirTerceirizado').submit();
    }

});


$('td').on('click', '.js-ExcluirTerceirizado', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Terceirizado/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarTerceirizado', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('Terceirizado/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var terceirizado = retorno.Entity;
                $('#hdnTerceirizadoCodigo').val(terceirizado.Codigo);
                $('#txtTerceirizadoNome').val(terceirizado.Nome);
                $('#ddlTerceirizadoTipoFuncaoTerceirizado').val(terceirizado.TipoFuncaoTerceirizado.Codigo);

                if (terceirizado.Telefone > 0)
                    $('#txtTerceirizadoTelefone').val(terceirizado.Telefone);
                else
                    $('#txtTerceirizadoTelefone').val('');

                $('#modalIncluirTerceirizado').modal('show');
            }
        }, false);
    }
});