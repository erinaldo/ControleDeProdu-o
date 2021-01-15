$(function () {
    codigosPedidosProdutosTerceirizados = [];
});


$('td').on('click', '.js-VerProdutos', function () {

    var pedido = new Object();
    pedido.codigo = +$(this).prop('id');

    $('#tblPedidoDetalharProduto tbody').html('');

    carregarProduto(pedido);

    $('#modalDetalharPedido').modal('toggle');
});

$('td').on('click', '.js-pedidoImprimir', function () {

    window.location.href = rootUrl + 'Pedido/ImprimirPedido?codigoPedido=' + $(this).prop('id');
});

$('td').on('click', '.js-ConfirmarOrcamento', function () {

    $('#divHiddensProdutosTercerizados').html('');
    montarLinhaProducaoPedido($(this).prop('id'), false, true);

});

$('body').on('click', '.js-DarBaixaPedidoProduto', function () {

    var $elemento = $(this);

    $('#hdnPedidoDetalharCodigoPedidoProduto').val($(this).prop('id'));

    var quantidade = codigoFuncao > enumTipoFase.CORTE ? + $elemento.parent().parent().find('#spanPedidoDetalharQuantidadeAnterior').text() : + $elemento.parent().parent().find('#spanPedidoDetalharQuantidade').text();
    var quantidadePronta = + $elemento.parent().parent().find('#spanPedidoDetalharQuantidadePronta').text();
    var quantidadeProduzida = + $elemento.parent().parent().find('#txtPedidoDetalharQuantidadeProduzida').val();
    var quantidadeRestante = + $elemento.parent().parent().find('#spanPedidoDetalharQuantidadeRestante').text();

    $('#divMensagemPedidoDetalharProduto').addClass('invisivel');
    $elemento.parent().parent().find('#txtPedidoDetalharQuantidadeProduzida').parent().removeClass('campoComErro');

    if ((quantidadePronta + quantidadeProduzida) > quantidade) {
        $('#divMensagemPedidoDetalharProduto span').text(mensagem.MSG33);
        $('#divMensagemPedidoDetalharProduto').removeClass('invisivel');
        $elemento.parent().parent().find('#txtPedidoDetalharQuantidadeProduzida').parent().addClass('campoComErro');
    }
    else {
        requisicaoAjax('Pedido/SalvarPedidoProdutoPronto', {
            quantidadePronta: quantidadeProduzida, codigoPedidoProduto: $('#hdnPedidoDetalharCodigoPedidoProduto').val(), codigoPedido: $('#hdnPedidoDetalharCodigo').val()
        }, function (retorno) {

            if (retorno.IsValido) {
                $elemento.parent().parent().find('#spanPedidoDetalharQuantidadePronta').text(quantidadePronta + quantidadeProduzida);
                $elemento.parent().parent().find('#spanPedidoDetalharQuantidadeRestante').text(quantidadeRestante - quantidadeProduzida);
                $elemento.parent().parent().find('#txtPedidoDetalharQuantidadeProduzida').val('');
            }
        }, false);
    }
})

$('body').on('blur', '.js-valorAplicado', function () {
    if (validarPreenchimento('#formIncluirPedido #divPedidoTabelaProdutos'))
        somarEAtribuirValorPedido();
})

$('body').on('blur', '.js-valorUnitario', function () {
    if (validarPreenchimento('#formIncluirPedido #divPedidoTabelaProdutos')) {

        $campoValorUnitario = $(this);

        tds = $($campoValorUnitario.parent().parent().find('td'));

        $campoValorTotalProduto = $(tds[8]).children();
        valorUnitario = +$campoValorUnitario.val().replaceAll('.', '').replace(',', '.');
        quantidade = +$(tds[6]).text().replaceAll('.', '').replace(',', '.');

        $campoValorTotalProduto.val(formatarMonetaria(valorUnitario * quantidade));

        somarEAtribuirValorPedido();
    }
});

$('#tblPedidoProduto').on('click', '.js-excluirProdutoPedido', function () {
    removerLinha($(this), function () {
        somarEAtribuirValorPedido();
    });
})

$('body').on('click', '.js-FichaTecnica', function () {
    requisicaoAjax('Pedido/ConsultarProduto?codigoProduto=' + $(this).prop('id'), null, function (retorno) {

        var produto = retorno.Entity;

        $('#hdnCodigoFichaTecnica').val(produto.FichaTecnica.Codigo);
        $('#hdnCodigoProduto').val(produto.Codigo);

        $('#txtFichaTecnicaNumeroPedidoCliente').val(produto.NumeroProdutoCliente);
        $('#h4FichaTecnicaNomeProduto').text(produto.Descricao);
        //if (produto.FichaTecnica.Foto64 != '')
        $('#imgProdutoDetalharFichaTecnica').prop('src', 'data:image/png;base64,' + produto.FichaTecnica.Foto64);
        $('#txtFichaTecnicaNumeroPedidoCliente').val(produto.NumeroProdutoCliente);
        $('#txtFichaTecnicaProdutoModelo').val(produto.FichaTecnica.Modelo);
        $('#txtFichaTecnicaProdutoTipo').val(produto.FichaTecnica.Tipo);
        $('#txtFichaTecnicaProdutoNcm').val(produto.FichaTecnica.Ncm);
        $('#txtFichaTecnicaProdutoDescricao').val(produto.FichaTecnica.Descricao);

        $('#tdFichaTecnicaMateriasPrimasProduto').html('');
        var tr = '';

        produto.MateriasPrimas.forEach(function (materiaPrima, index) {
            tr += materiaPrima.Descricao.toUpperCase() + '<br/>';
        });

        $('#tdFichaTecnicaMateriasPrimasProduto').html(tr);
        $('#modalFichaTecnica').modal('show');
    });
})

$('td').on('click', '.js-ExcluirPedido', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Pedido/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarPedido', function (e) {
    e.preventDefault();

    if (confirm("Deseja Alterar?")) {
        carregarPedido($(this).prop('id'));
    }
});

$('body').on('click', '.js-TerceirizarProduto', function (e) {
    e.preventDefault();

    if (confirm("Deseja Terceirizar?")) {

        var index = $('.js-codigoProdutoTerceirizado').length;

        var dados = $(this).prop('id').split('|');

        $('#divHiddensProdutosTercerizados').append('<input name="linhaProducaoTerceirizados[' + index + '].Produto.CodigoPedidoProduto" class="js-codigoProdutoTerceirizado" type="hidden" value=' + dados[0] + '>');
        $('#divHiddensProdutosTercerizados').append('<input name="linhaProducaoTerceirizados[' + index + '].Produto.Quantidade" type="hidden" value=' + dados[1] + '>');
        $('#divHiddensProdutosTercerizados').append('<input name="linhaProducaoTerceirizados[' + index + '].Produto.Descricao" type="hidden" value=' + dados[2].replace('/', ' ') + '>');
        $('#divHiddensProdutosTercerizados').append('<input name="linhaProducaoTerceirizados[' + index + '].EhTerceirizado" type="hidden" value=true >');

        montarLinhaProducaoPedido($('#hdnPedidoLinhaProducaoCodigo').val(), false, false);
    }
});




$('#btnSalvarFichaTecnica').click(function (e) {
    requisicaoAjax('Pedido/SalvarFichaTecnica', {
        modelo: $('#txtFichaTecnicaProdutoModelo').val(), tipo: $('#txtFichaTecnicaProdutoTipo').val(),
        ncm: $('#txtFichaTecnicaProdutoNcm').val(), descricao: $('#txtFichaTecnicaProdutoDescricao').val()
    }, function (retorno) {

    }, false);
});

$('#btnSalvarPedido').click(function (e) {
    e.preventDefault();

    limparMensagem('formIncluirPedido');

    if (validarPreenchimento('#formIncluirPedido #divPedidoAdicionarPedido') && validarProduto()) {
        $('#txtPedidoValorEspecial').val($('#txtPedidoValorEspecial').val().toString().replaceAll('.', ''));
        $('#formIncluirPedido').submit();
    }

});

$('#btnAdicionarPedidoAdicionarProduto').click(function () {
    limparMensagem('#formIncluirPedido');
    removerMascaras();

    if ($('#txtPedidoProdutoNumeroCliente').val() != '')
        $('#ddlPedidoProduto').removeClass('js-obrigatorio');
    else
        $('#ddlPedidoProduto').addClass('js-obrigatorio');

    if (validarPreenchimento('#formIncluirPedido #divPedidoAdicionarProduto')) {

        var produto = new Object();
        produto.FichaTecnica = new Object();
        produto.NumeroProdutoCliente = +$('#txtPedidoProdutoNumeroCliente').val();
        produto.Codigo = $('#ddlPedidoProduto').val();

        requisicaoAjax('Pedido/ConsultarProduto?codigoProduto=' + produto.Codigo + '&numeroProdutoCliente=' + produto.NumeroProdutoCliente, null, function (retorno) {

            if (retorno.Entity.Codigo == 0) {
                exibirMensagem('divMensagemPedidoProduto', mensagem.MSG34);
                return false;
            }
            else {
                produto.Codigo = retorno.Entity.Codigo;
                produto.Descricao = retorno.Entity.Descricao;
                produto.Valor = retorno.Entity.Valor;
                produto.FichaTecnica.Ncm = retorno.Entity.FichaTecnica.Ncm;

                produto.CodigoTipoTamanho = $('#ddlPedidoTipoTamanho').val();
                produto.DescricaoTipoTamanho = $('#ddlPedidoTipoTamanho :selected').text();
                produto.CodigoTipoCor = $('#ddlPedidoProdutoTipoCor').val();
                produto.DescricaoTipoCor = $('#ddlPedidoProdutoTipoCor :selected').text();
                produto.Quantidade = $('#txtPedidoProdutoQuantidade').val();
                produto.NumeroPedidoCliente = $('#txtPedidoProdutoNumeroPedidoCliente').val();
                produto.LinhaPedidoCliente = $('#txtPedidoProdutoLinhaPedidoCliente').val();

                requisicaoAjax('Pedido/ConsultarTabelaPreco?codigoTabelaPreco=' + $('#ddlPedidoTabelaPreco').val(), null, function (retorno) {
                    $('#hddTabelaPreco').val(retorno.Entity.Imposto + retorno.Entity.Lucro);
                    $('#hddPedidoTabelaPrecoEspecial').val(retorno.Entity.Especial);
                    $('#hddPedidoTabelaPrecoImposto').val(retorno.Entity.Imposto);

                    listarProdutosDoPedido(produto);
                    somarEAtribuirValorPedido();
                    limparCampos('#formIncluirPedido #divPedidoAdicionarProduto');
                }, false);
            }
        }, false);
    }

});

$('#ddlPedidoTabelaPreco').change(function () {

    requisicaoAjax('Pedido/ConsultarTabelaPreco?codigoTabelaPreco=' + $('#ddlPedidoTabelaPreco').val(), null, function (retorno) {
        $('#hddTabelaPreco').val(retorno.Entity.Imposto + retorno.Entity.Lucro);
        $('#hddPedidoTabelaPrecoEspecial').val(retorno.Entity.Especial);
        $('#hddPedidoTabelaPrecoImposto').val(retorno.Entity.Imposto);
    }, false);
    recalcularProdutosPedido();
});

$('#ddlPedidoFormaPagamento').change(function () {
    verificarFormaPagamento();
});

$('#btnSalvarTipoCor').click(function (e) {
    e.preventDefault();

    var descricao = $('#txtPedidoTipoCor').val();

    if (descricao == '') {

    }
    else {
        var option = '';
        requisicaoAjax('Pedido/SalvarCor?descricao=' + descricao, null, function (retorno) {
            if (retorno.IsValido) {
                option = '<option value="' + $('#ddlPedidoProdutoTipoCor option').length + '">' + descricao.toUpperCase() + '</option>';
                alert('Salvo com Sucesso!');
                $('#txtPedidoTipoCor').val('');
            }
        }, false);

        $('#ddlPedidoProdutoTipoCor').append(option);
        $("#modalIncluirTipoCor .close").click();

    }
});

$('#btnSalvarTipoTamanho').click(function (e) {
    e.preventDefault();
    var descricao = $('#txtPedidoTipoTamanho').val();

    if (descricao == '') {

    }
    else {
        var option = '';
        requisicaoAjax('Pedido/SalvarTamanho?descricao=' + descricao, null, function (retorno) {
            if (retorno.IsValido) {
                option = '<option value="' + $('#ddlPedidoTipoTamanho option').length + '">' + descricao.toUpperCase() + '</option>';
                alert('Salvo com Sucesso!');
                $('#txtPedidoTipoTamanho').val('');
            }
        }, false);

        $('#ddlPedidoTipoTamanho').append(option);
        $("#modalIncluirTipoTamanho .close").click();
    }
});

$('#btnRatearValor').click(function () {

    var valorPedido = 0;
    $('.js-valorAplicado').each(function (index, item) {
        valorPedido += +$(item).val().toString().replaceAll('.', '').replaceAll(',', '.').replaceAll('R$', '').replaceAll(' ', '');
    });

    var valorDesejado = $('#txtPedidoValorEspecial').val().replaceAll('.', '').replace(',', '.');
    var valorDiferenca = +toFixed(valorPedido - valorDesejado, 2);
    var quantidadeProdutos = 0;

    $('.js-QuantidadeProdutoPedido').each(function (index, item) {
        quantidadeProdutos += +$(item).text().replaceAll('.', '').replace(',', '.');
    });

    var valorDiferencaUnitario = +toFixed(valorDiferenca / quantidadeProdutos, 2);

    $('#tblPedidoProduto tbody tr').each(function () {
        var valorUnitarioAtual = +$(this).find('.js-valorUnitario').val().replaceAll('.', '').replace(',', '.');
        var quantidade = +$(this).find('.js-QuantidadeProdutoPedido').text();

        if (valorDiferenca > 0)
            var valorUnitarioNovo = toFixed(valorUnitarioAtual - valorDiferencaUnitario, 2);
        else
            var valorUnitarioNovo = toFixed(valorUnitarioAtual + (valorDiferencaUnitario * -1), 2);

        $(this).find('.js-valorUnitario').val(valorUnitarioNovo.replace('.', ','));
        $(this).find('.js-valorAplicado').val(formatarMonetaria(valorUnitarioNovo * quantidade));

    });

    somarEAtribuirValorPedido();

});

$('#btnGerarNotaFiscal').click(function () {
    var msg = "Deseja Gerar a Nota Fiscal do Pedido #" + padLeft($('#hdnPedidoDetalharCodigo').val(), 7, '0') + " ?";

    limparMensagem('divMensagemPedidoDetalharProdutoFases');

    if (confirm(msg)) {

        if (validarPreenchimentoQuantidadeProdutosNota()) {
            $('#hdnAmbienteProducao').val(true);
            requisicaoAjax('Pedido/EmitirNfe', $('#formDarBaixaPedidoProdutoPronto').serializeArray(), function (retorno) {
                if (retorno.IsValido) {
                    window.location.href = rootUrl + 'Pedido/index';
                    window.open(retorno.Entity, '_blank');
                }
                else
                    exibirMensagem('divMensagemPedidoDetalharProdutoFases', retorno.Mensagem);

            }, false);
        }
    }

});

$('#btnSimularNotaFiscal').click(function () {
    var msg = "O sistema irá simular a Nota Fiscal do Pedido #" + padLeft($('#hdnPedidoDetalharCodigo').val(), 7, '0') + " ?";

    limparMensagem('divMensagemPedidoDetalharProdutoFases');

    if (confirm(msg)) {

        if (validarPreenchimentoQuantidadeProdutosNota()) {
            $('#hdnAmbienteProducao').val(false);
            requisicaoAjax('Pedido/EmitirNfe', $('#formDarBaixaPedidoProdutoPronto').serializeArray(), function (retorno) {
                if (retorno.IsValido) {
                    window.open(retorno.Entity, '_blank');
                }
                else
                    exibirMensagem('divMensagemPedidoDetalharProdutoFases', retorno.Mensagem);
            });
        }
    }

});


$('#btnSalvarTipoFormaPagamento').click(function (e) {
    e.preventDefault();
    var descricao = $('#txtPedidoTipoFormaPagamento').val();

    if (validarPreenchimento('#modalIncluirTipoPagamento')) {
        var option = '';
        requisicaoAjax('Pedido/SalvarTipoFormaPagamento?descricao=' + descricao + '&contemParcelas=' + $('#ckbPedidoTipoFormaPagamentoContemParcelas').is(":checked"), null, function (retorno) {
            if (retorno.IsValido) {
                option = '<option value="' + $('#ddlPedidoFormaPagamento option').length + '">' + descricao.toUpperCase() + '</option>';
                alert('Salvo com Sucesso!');
                $('#txtPedidoTipoFormaPagamento').val('');
            }
        }, false);

        $('#ddlPedidoFormaPagamento').append(option);
        $("#modalIncluirTipoFormaPagamento .close").click();
    }
});

$('#txtPedidoDesconto').blur(function () {
    somarEAtribuirValorPedido();
});

$('#btnImprimirFichaTecnica').click(function () {

    window.location.href = rootUrl + 'Pedido/ImprimirFichaTecnica?codigoProduto=' + $('#hdnCodigoProduto').val();

});

$('#enviarPorEmail').click(function () {

    if (confirm('Deseja enviar os Arquivos por Email?')) {
        requisicaoAjax('Pedido/EnviarPorEmail', {
            codigoPedido: +$('#hdnPedidoDetalharCodigo').val(),
            nomeCliente: $('#txtPedidoDetalharNomeCliente').text(),
            notaFiscal: $('#hdnPedidoDetalharNumeroNfe').val()
        }, function (retorno) {

            if (retorno.IsValido) {
                alert('Email enviado com Sucesso!');
            }

        }, false);
    }
});

$('#btnAdicionarLinhaProducao').click(function (e) {
    e.preventDefault();

    if (confirm('Deseja confirmar o Pedido?')) {
        exibirLoading();
        montarLinhaProducaoPedido($('#hdnPedidoLinhaProducaoCodigo').val(), true, false);
    }
});


function validarProduto() {

    var valido = ($('#tblPedidoProduto tr').length > 1);

    if (valido)
        $('#divMensagemPedidoProduto').addClass('invisivel');
    else
        exibirMensagem('divMensagemPedidoProduto', mensagem.MSG32);

    return valido;
}

function somarEAtribuirValorPedido() {

    var valorPedido = 0;
    $('.js-valorAplicado').each(function (index, item) {
        valorPedido += +$(item).val().toString().replaceAll('.', '').replaceAll(',', '.').replaceAll('R$', '').replaceAll(' ', '');
    });

    $('#divValorTotalFixo,#divValorTotalEditavel,#divBtnRatearValor').addClass('invisivel');
    valorPedido = valorPedido - +$('#txtPedidoDesconto').val().replace('.', '').replace(',', '.');

    if ($('#hddPedidoTabelaPrecoEspecial').val() == 'false')
        $('#divValorTotalFixo').removeClass('invisivel');
    else
        $('#divValorTotalEditavel,#divBtnRatearValor').removeClass('invisivel');

    $('#spanPedidoValorTotal').text(formatarMonetaria(valorPedido));
    $('#hddPedidoValorTotal').val(toFixed(valorPedido, 2).toString().replace('.', ','));
    $('#txtPedidoValorEspecial').val(toFixed(valorPedido, 2).toString().replace('.', ','));

    atualizarIndex();
}

function listarProdutosDoPedido(produto) {

    produto.Posicao = +$('#tblPedidoProduto tbody tr').length;

    if (produto.DescricaoTipoCor == undefined)
        produto.DescricaoTipoCor = produto.TipoCor.Descricao;
    if (produto.DescricaoTipoTamanho == undefined)
        produto.DescricaoTipoTamanho = produto.TipoTamanho.Descricao;

    if (produto.CodigoTipoCor == undefined)
        produto.CodigoTipoCor = produto.TipoCor.Codigo;
    if (produto.CodigoTipoTamanho == undefined)
        produto.CodigoTipoTamanho = produto.TipoTamanho.Codigo;

    var html = '<tr>' +
        '   <td scope="col">' + produto.Codigo + '</td>' +
        '   <td scope="col">' + produto.FichaTecnica.Ncm + '</td>' +
        '   <td scope="col">' + produto.NumeroProdutoCliente + '</td>' +
        '   <td scope="col">' + produto.Descricao + '</td>' +
        '   <td scope="col">' + produto.DescricaoTipoCor + '</td>' +
        '   <td scope="col">' + produto.DescricaoTipoTamanho + '</td>' +
        '   <td scope="col" class="js-QuantidadeProdutoPedido">' + produto.Quantidade + '</td>';

    produto.Valor *= 1 + (+$('#hddTabelaPreco').val() / 100);
    if (produto.ValorUnitario == undefined || produto.ValorUnitario == 0) produto.ValorUnitario = produto.Valor;

    if ($('#hddPedidoTabelaPrecoEspecial').val() == 'true') {
        html += '   <td scope="col"><input class="form-control js-obrigatorio js-monetaria js-valorUnitario" name="pedido.Produtos[' + produto.Posicao + '].ValorUnitario" type=text value="' + formatarMonetaria(produto.ValorUnitario) + '"/></td>' +
            '   <td scope="col"><input class="form-control js-obrigatorio js-valorAplicado" name="pedido.Produtos[' + produto.Posicao + '].ValorAplicado" type=text value="' + formatarMonetaria((produto.ValorUnitario * +(produto.Quantidade.toString().replace(',', '.')))) + '"/></td>';
    } else {
        html += '   <td scope="col">' + formatarMonetaria(produto.Valor) + '</td>' +
            '   <td scope="col"><input class="form-control js-obrigatorio js-valorAplicado" name="pedido.Produtos[' + produto.Posicao + '].ValorAplicado" type=text value="' + formatarMonetaria((produto.Valor * +(produto.Quantidade.toString().replace(',', '.')))) + '"/></td>';
    }

    var numeroPedidoCliente = produto.NumeroPedidoCliente == null ? '-' : produto.NumeroPedidoCliente;
    var linhaPedidoCliente = produto.LinhaPedidoCliente == null ? '-' : produto.LinhaPedidoCliente;
    var numeroProdutoCliente = produto.NumeroProdutoCliente == 0 ? '-' : produto.NumeroProdutoCliente;

    html +=
        '<td scope="col">' +
        '' + numeroPedidoCliente +
        '</td>' +
        '<td scope="col">' +
        '' + linhaPedidoCliente +
        '</td>' +
        '<td scope="col">' +
        '       <img src="' + rootUrl + 'Content/imagens/ExcluirMenor.png" alt="EXCLUIR PRODUTO DO PEDIDO" title="EXCLUIR PRODUTO DO PEDIDO" class="js-excluirProdutoPedido" />' +
        '       <img src="' + rootUrl + 'Content/imagens/FichaTecnica.png" id="' + produto.CodigoPedidoProduto + '" class="js-FichaTecnica" />' +
        '   </td>' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].Codigo" type="hidden" value="' + produto.Codigo + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].CodigoPedidoProduto" type="hidden" value="' + produto.CodigoPedidoProduto + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].NumeroProdutoCliente" type="hidden" value="' + numeroProdutoCliente + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].Descricao" type="hidden" value="' + produto.Descricao + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].Quantidade" type="hidden" value="' + produto.Quantidade.toString().replaceAll('.', ',') + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].TipoTamanho.Codigo" type="hidden" value="' + produto.CodigoTipoTamanho + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].TipoCor.Codigo" type="hidden" value="' + produto.CodigoTipoCor + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].Valor" type="hidden" value="' + (produto.Valor * +(produto.Quantidade.toString().replace(',', '.'))).toString().replace('.', ',') + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].ValorUnitario" type="hidden" value="' + produto.ValorUnitario.toString().replaceAll(',', '').replace('.', ',') + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].NumeroPedidoCliente" type="hidden" value="' + produto.NumeroPedidoCliente + '">' +
        ' <input name="pedido.Produtos[' + produto.Posicao + '].LinhaPedidoCliente" type="hidden" value="' + produto.LinhaPedidoCliente + '">' +
        ' <input class="js-totalValorLinhaPedido" type="hidden" value="' + (produto.Valor * +(produto.Quantidade.toString().replace(',', '.'))) + '">' +
        '</tr >'

    $('#tblPedidoProduto tbody').append(html);
}

function verificarFormaPagamento() {

    requisicaoAjax('Pedido/ConsultarFormaPagamento?codigoFormaPagamento=' + $('#ddlPedidoFormaPagamento').val(), null, function (retorno) {
        $('#txtPedidoNumeroParcelas').prop("disabled", !retorno.Entity.ContemParcelas);
        $('#txtPedidoDataPagamento').prop("disabled", !retorno.Entity.DataCombinada);
    }, false);
}

function carregarProduto(pedido) {

    requisicaoAjax('Pedido/Consultar?codigo=' + pedido.codigo, null, function (retorno) {

        $('#tblPedidoDetalharProduto tbody').html('');
        $('#tblPedidoDetalharProdutoFases tbody').html('');

        pedido = retorno.Entity;
        $('#txtPedidoDetalharNumeroPedido').text(pedido.Codigo.padLeft(6));
        $('#txtPedidoDetalharDataPrevisaoEntrega').text(formatarData(pedido.DataPrevisaoEntrega));
        $('#txtPedidoDetalharNomeCliente').text(pedido.Cliente.Nome);
        $('#txtPedidoDetalharObservação').text(pedido.Observacao);
        $('#txtPedidoDetalharFasePedido').text(pedido.TipoFase.Descricao);
        $('#hdnPedidoDetalharCodigo').val(pedido.Codigo);
        $('#hdnPedidoDetalharCodigoFasePedido').val(pedido.TipoFase.Codigo);
        $('#hdnPedidoDetalharTipoFormaPagamentoCodigo').val(pedido.TipoFormaPagamento.Codigo);
        $('#hdnPedidoDetalharTipoFormaPagamentoDias').val(pedido.TipoFormaPagamento.Dias);
        $('#hdnPedidoDetalharNumeroNfe').val(pedido.NumeroNfe);

        if (pedido.NumeroNfe != null) {
            $('#downloadDanfe').prop('href', 'http://nfe.webmaniabr.com/danfe/' + pedido.NumeroNfe);
            $('#downloadXml').prop('href', 'http://nfe.webmaniabr.com/xmlnfe/' + pedido.NumeroNfe);
            $('#divDetalharPedidoDownloads').show();
        }
        else {
            $('#divDetalharPedidoDownloads').hide();
        }

        if (pedido.TipoFase.Codigo == enumTipoFase.ORCAMENTO || pedido.TipoFase.Codigo == enumTipoFase.FINALIZADO)
            $('.js-colunaQuantidadePronta').addClass('invisivel');
        else
            $('.js-colunaQuantidadePronta').removeClass('invisivel');

        pedido.Produtos.forEach(function (produto, index) {

            var quantidadeRestante = produto.Quantidade - produto.QuantidadePronta;

            if (codigoFuncao != enumTipoFuncao.ADMINISTRATIVO && quantidadeRestante > 0) {
                var linhaPedidoProduto =
                    '<tr>' +
                    '   <td scope="col">' + produto.Codigo.padLeft(6) + '</td>' +
                    '   <td scope="col">' + produto.Descricao + '</td>' +
                    '   <td scope="col">' + produto.TipoCor.Descricao + '</td>' +
                    '   <td scope="col">' + produto.TipoTamanho.Descricao + '</td>' +
                    '   <td scope="col"><span id="spanPedidoDetalharQuantidade">' + produto.Quantidade + '</td>';

                if (codigoFuncao > enumTipoFase.CORTE)
                    linhaPedidoProduto +=
                        '   <td scope="col"><span id="spanPedidoDetalharQuantidadeAnterior">' + produto.QuantidadeAnteriorPronta + '</td>';

                if (pedido.TipoFase.Codigo != enumTipoFase.ORCAMENTO && pedido.TipoFase.Codigo != enumTipoFase.FINALIZADO) {
                    linhaPedidoProduto +=
                        '   <td scope="col"><span id="spanPedidoDetalharQuantidadePronta">' + produto.QuantidadePronta + '</td>' +
                        '   <td scope="col"><span id="spanPedidoDetalharQuantidadeRestante">' + (produto.Quantidade - produto.QuantidadePronta) + '</td>' +
                        '   <td scope="col"><input id="txtPedidoDetalharQuantidadeProduzida" class="form-control js-numero produto-quantidade-produzida" name="produto.QuantidadeProduzida" type="text" /></td>' +
                        '   <td scope="col">' +
                        '   <img src="' + rootUrl + 'Content/imagens/DAR BAIXA.png" id="' + produto.CodigoPedidoProduto + '" class="js-DarBaixaPedidoProduto" />' +
                        '   </td >';
                }


                linhaPedidoProduto +=
                    '   <td scope="col">' +
                    '   <img src="' + rootUrl + 'Content/imagens/FichaTecnica.png" id="' + produto.Codigo + '" class="js-FichaTecnica" />' +
                    '   </td >' +
                    '</tr >';

                $('#tblPedidoDetalharProduto tbody').append(linhaPedidoProduto);
            }

            var desabilitar = codigoFuncao == enumTipoFuncao.ADMINISTRATIVO || (produto.FaseProduzido[3] > 0 || pedido.TipoFase.Codigo == enumTipoFase.FINALIZADO) && (produto.FaseProduzido[5] != produto.Quantidade) ? '' : 'disabled';

            var linhaProdutoProduzido =
                '<tr>' +
                '   <td scope="col">' + produto.Descricao + " " + produto.TipoCor.Descricao + " " + produto.TipoTamanho.Descricao + '</td>' +
                '   <td scope="col">' + produto.Quantidade + '</td>' +
                '   <td scope="col">' + produto.FaseProduzido[1] + '</td>' +
                '   <td scope="col">' + produto.FaseProduzido[2] + '</td>' +
                '   <td scope="col">' + produto.FaseProduzido[3] + '</td>' +
                '   <td scope="col">' + produto.FaseProduzido[5] + '</td>' +
                '   <td scope="col">' +
                '       <input class="form-control" name="produtos[' + index + '].CodigoPedidoProduto" type="hidden" value="' + produto.CodigoPedidoProduto + '" />' +
                '       <input id="txtProdutoFaturar" ' + desabilitar + '  class="form-control js-numero" name="produtos[' + index + '].QuantidadeFaturar" type="text" />' +
                '   </td > ' +
                '   <td scope="col">' +
                '   <img src="' + rootUrl + 'Content/imagens/FichaTecnica.png" id="' + produto.Codigo + '" class="js-FichaTecnica" />' +
                '   </td >' +
                '</tr >';

            $('#tblPedidoDetalharProdutoFases tbody').append(linhaProdutoProduzido);

        });

    }, false);
}

function carregarPedido(idPedido) {
    requisicaoAjax('Pedido/Consultar?codigo=' + idPedido, null, function (retorno) {
        if (retorno.IsValido) {

            var pedido = retorno.Entity;
            $('#lblNumeroPedido').text('#' + pedido.Codigo.padLeft(7, '0'))
            $('#hdnPedidoCodigo').val(pedido.Codigo);
            $('#hdnPedidofichaTecnicaCodigo').val(pedido.FichaTecnica.Codigo);

            $('#ddlPedidoCliente').val(pedido.Cliente.Codigo);
            $('#txtNumeroPedidoCliente').val(pedido.NumeroPedidoCliente);
            $('#ddlPedidoTabelaPreco').val(pedido.TabelaPreco.Codigo);

            $('#hddTabelaPreco').val(pedido.TabelaPreco.Imposto + pedido.TabelaPreco.Lucro);
            $('#hddPedidoTabelaPrecoEspecial').val(pedido.TabelaPreco.Especial);
            $('#hddPedidoTabelaPrecoImposto').val(pedido.TabelaPreco.Imposto);
            $('#hddPedidoTabelaPrecoLucro').val(pedido.TabelaPreco.Lucro);

            $('#ddlPedidoFase').val(pedido.TipoFase.Codigo);
            $('#ddlPedidoFormaPagamento').val(pedido.TipoFormaPagamento.Codigo);
            $('#txtPedidoNumeroParcelas').val(pedido.NumeroParcelas);
            $('#txtPedidoDataPrevisaoEntrega').val(formatarData(pedido.DataPrevisaoEntrega));
            $('#txtPedidoObservacao').val(pedido.Observacao);
            $('#txtPedidoObservacaoNotaFiscal').val(pedido.ObservacaoNotaFiscal);
            $('#txtPedidoDesconto').val(pedido.Desconto != null ? toFixed(pedido.Desconto, 2).toString().replace(',', '.').replace('.', ',') : '');

            if (pedido.PorcentagemLucro != null)
                $('#txtPedidoValorEspecial').val(toFixed(pedido.Valor, 2).toString().replace(',', '.').replace('.', ','));

            $('#txtPedidoDataPagamento').val(formatarData(pedido.DataPagamento));

            $('#tblPedidoProduto tbody').html('');

            $.each(pedido.Produtos, function (index, item) {
                listarProdutosDoPedido(item);
            });

            verificarFormaPagamento();
            somarEAtribuirValorPedido();
            $('#modalIncluirPedido').modal("show");
        }
    });
}

function recalcularProdutosPedido() {

    var posicao = 0;

    $('#tblPedidoProduto tbody tr').each(function () {
        var tds = $(this).find('td');
        var valorUnitario = 0;
        var quantidade = $(tds[6]).text().replaceAll('.', '').replace(',', '.');
        var codigo = $(tds[0]).text();

        requisicaoAjax('Pedido/ConsultarProduto?codigoProduto=' + codigo, null, function (retorno) {
            valorUnitario = retorno.Entity.Valor;
        }, false);

        valorUnitario *= 1 + (+$('#hddTabelaPreco').val() / 100);

        if ($('#hddPedidoTabelaPrecoEspecial').val() == 'true') {
            if ($(tds[7]).children().length > 0)
                $(tds[7]).children().val(formatarMonetaria(valorUnitario));
            else
                $(tds[7]).html('<input class="form-control js-obrigatorio js-monetaria js-valorUnitario" name="pedido.Produtos[' + posicao + '].ValorUnitario" type=text value="' + formatarMonetaria(valorUnitario) + '"/>');
        }
        else
            $(tds[7]).text(formatarMonetaria(valorUnitario));

        $(tds[8]).children().val(formatarMonetaria(quantidade * valorUnitario.toFixed(2)));
        posicao++;
    });

    somarEAtribuirValorPedido();
}

function atualizarIndex() {

    var linha = -1;

    $('#tblPedidoProduto tr').each(function () {

        $(this).find('input[name *="pedido.Produtos["]').each(function () {

            var nameAntigo = $(this).prop('name');

            var nameNovo = nameAntigo.substring(0, nameAntigo.indexOf('[') + 1) +
                linha +
                nameAntigo.substring(nameAntigo.indexOf(']'), nameAntigo.length);

            $(this).prop('name', nameNovo);
        });
        linha++;
    });
}

function validarPreenchimentoQuantidadeProdutosNota() {

    var valido = true;
    var codigoFasePedido = $('#hdnPedidoDetalharCodigoFasePedido').val();

    if (codigoFasePedido != enumTipoFase.FINALIZADO && codigoFuncao != enumTipoFuncao.ADMINISTRATIVO) {

        $('#tblPedidoDetalharProdutoFases tbody tr').each(function (index, pedidoProduto) {

            var tds = $(pedidoProduto).find('td');

            var quantidadePronta = +$(tds[4]).text();
            var quantidadeFaturada = +$(tds[5]).text();
            var quantidadeFaturar = + $(pedidoProduto).find('#txtProdutoFaturar').val();

            if (quantidadeFaturar > (quantidadePronta - quantidadeFaturada)) {
                $(pedidoProduto).addClass('campoComErro');
                valido = false;
            }

        });

        if (!valido)
            exibirMensagem('divMensagemPedidoDetalharProdutoFases', 'Quantidade não pode ser maior que a produzida.');
    }
    return valido;

}

function montarLinhaProducaoPedido(codigoPedido, adicionarLinhaProducao, abrirModal) {

    $('#hdnPedidoLinhaProducaoCodigo').val(codigoPedido);
    $('#hdnAdicionarLinhaProducao').val(adicionarLinhaProducao);

    requisicaoAjax('Pedido/MontarLinhaProducaoPedido', $('#formIncluirPedido').serializeArray()
        , function (retorno) {

            if (retorno.IsValido) {
                if (!adicionarLinhaProducao) {
                    var pedido = retorno.Entity;
                    var html = '';

                    $.each(pedido.LinhaProducaoExibicao.LinhaProducaoProdutos, function (index, linhaProducaoProduto) {

                        var classe = index % 2 == 0 ? 'linha-branca' : 'linha-cinza';

                        html += '<tr class="' + classe + '">';
                        html += '    <td>' + linhaProducaoProduto.Produto.Quantidade + '</td>';
                        html += '    <td>' + linhaProducaoProduto.Produto.Descricao + '</td>';

                        if (linhaProducaoProduto.EhTerceirizado) {
                            html += '    <td><input class="form-control js-obrigatorio js-data" id="txtDataPrevisaoInicio" name="linhaProducaoTerceirizados[' + index + '].DataPrevisaoInicio" type="text" maxlength="10"></td>';
                            html += '    <td><input class="form-control js-obrigatorio js-data" id="txtDataPrevisaoFim" name="linhaProducaoTerceirizados[' + index + '].DataPrevisaoFim" type="text" maxlength="10"></td>';
                            html += '    <td>';
                            html += '       <select class="form-control js-obrigatorio js-LinhaProducaoTerceirizados" id="ddlLinhaProducaoTerceirizados" name="linhaProducaoTerceirizados[' + index + '].Terceirizado.Codigo">';
                            html += '           <option value="">...</option>';
                            html += '       </select>';
                            html += '    </td>';
                            html += '    <td> - </td>';
                        } else {
                            html += '    <td>' + formatarDataHora(linhaProducaoProduto.DataPrevisaoInicio) + '</td>';
                            html += '    <td>' + formatarDataHora(linhaProducaoProduto.DataPrevisaoFim) + '</td>';
                            html += '    <td>' + linhaProducaoProduto.NomesFuncionarios.toUpperCase() + '</td>';
                            html += '    <td>';
                            html += '       <img id="' + linhaProducaoProduto.Produto.CodigoPedidoProduto + '|' + linhaProducaoProduto.Produto.Quantidade + '|' + linhaProducaoProduto.Produto.Descricao + '" src="' + rootUrl + 'Content/imagens/Terceirizar.png" alt="TERCEIRIZAR" title="TERCEIRIZAR" class="js-TerceirizarProduto" />';
                            html += '    </td>';
                        }


                        html += '</tr>';
                    });

                    $('#tblLinhaProducao tbody').html(html);
                    $('#hdnPedidoLinhaProducaoCodigo').val(codigoPedido);
                    if (abrirModal)
                        $('#modalLinhaProducao').modal('toggle');

                    montarComboTerceirizados();
                }
                else {
                    window.location.href = window.location.href;
                }
            }

        }, false);
}

function montarComboTerceirizados() {
    requisicaoAjax('Pedido/ListarTerceirizados', null, function (retorno) {
        if (retorno.IsValido) {
            var terceirizados = retorno.Entity;
            var options = '';

            $('.js-LinhaProducaoTerceirizados').html('');
            $.each(terceirizados, function (index, terceirizado) {
                options += '<option value="' + terceirizado.Codigo + '">' + terceirizado.Nome.toUpperCase() + '</option>';
            }, false);

            $('.js-LinhaProducaoTerceirizados').append(options);
        }
    }, false);
}