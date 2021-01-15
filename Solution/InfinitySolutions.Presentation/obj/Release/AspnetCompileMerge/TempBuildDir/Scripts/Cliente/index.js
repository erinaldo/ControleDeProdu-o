$(function () {
});

$('#btnSalvarCliente').click(function (e) {
    e.preventDefault();

    limparMensagem('#formIncluirCliente');

    if (validarPreenchimento('#formIncluirCliente')) {
        $('#txtClienteCnpj').val(removerMascara($('#txtClienteCnpj').val()));
        $('#txtClienteIe').val(removerMascara($('#txtClienteIe').val()));
        $('#formIncluirCliente').submit();
    }

});

$('#txtClienteCnpj').on('keyup', function (e) {
    carregarDadosCnpj($('#txtClienteCnpj').val());
});

$('td').on('click', '.js-ExcluirCliente', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Cliente/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarCliente', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('Cliente/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var cliente = retorno.Entity
                $('#hdnClienteCodigo').val(cliente.Codigo);
                $('#hdnClienteEnderecoCodigo').val(cliente.Endereco.Codigo);

                if (cliente.Cnpj > 0) {
                    $('#txtClienteCnpj').val(padLeft(cliente.Cnpj, 14, '0').replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5"));
                    carregarDadosCnpj($('#txtClienteCnpj').val());
                }
                else {
                    $('#txtClienteNome').val(cliente.Nome);
                    $('#txtClienteNomeFantasia').val(cliente.NomeFantasia);

                    $('#txtClienteEnderecoRua').val(cliente.Endereco.Rua);
                    $('#txtClienteEnderecoNumero').val(cliente.Endereco.Numero);
                    $('#txtClienteEnderecoBairro').val(cliente.Endereco.Bairro);
                    $('#txtClienteEnderecoCidade').val(cliente.Endereco.Cidade);
                    $('#txtClienteEnderecoUf').val(cliente.Endereco.Uf);
                    $('#txtClienteEnderecoCep').val(cliente.Endereco.Cep);

                    $('#txtClienteContato').val(cliente.Contato);
                }
                if (cliente.Ie > 0)
                    $('#txtClienteIe').val(padLeft(cliente.Ie, 12, '0').replace(/^(\d{3})(\d{3})(\d{3})(\d{3})/, "$1.$2.$3.$4"));


                $('#modalIncluirCliente').modal('show');
            }
        }, false);
    }
});

function carregarDadosCnpj(cnpj) {

    if (cnpj.length == 18) {

        requisicaoAjax('Cliente/ConsultarDadosCliente?cnpj=' + +removerMascara(cnpj), null, function (retorno) {

            $('#txtClienteNome').val(retorno.Nome);
            $('#txtClienteNomeFantasia').val(retorno.NomeFantasia);

            $('#txtClienteEnderecoRua').val(retorno.Endereco.Rua);
            $('#txtClienteEnderecoNumero').val(retorno.Endereco.Numero);
            $('#txtClienteEnderecoBairro').val(retorno.Endereco.Bairro);
            $('#txtClienteEnderecoCidade').val(retorno.Endereco.Cidade);
            $('#txtClienteEnderecoUf').val(retorno.Endereco.Uf);
            $('#txtClienteEnderecoCep').val(retorno.Endereco.Cep);

            $('#txtClienteContato').val(retorno.Contato);

        }, false);
    }
}