$('td').on('click', '.js-ConfirmarObservacao', function () {

    if (confirm('Deseja confirmar a Observação?')) {
        requisicaoAjax('NotasFiscais/Salvar', { codigo: $(this).prop('id'), observacao: $(this).parent().parent().find('.js-NotaFiscalObservacao').val() }, function (retorno) {

            if (retorno.IsValido) {
                alert('Observação Registrada com Sucesso!');
            }

        }, false);
    }

});