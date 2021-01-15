$(function () {
});

$('#btnSalvarProduto').click(function (e) {
    e.preventDefault();

    limparMensagem('#formIncluirProduto');

    if (validarPreenchimento('#formIncluirProduto #divProdutoAdicionarProduto') && validarMateriaPrima()) {
        //$('#txtProdutoQuantidadeProducaoHoraFuncionario').val($('#txtProdutoQuantidadeProducaoHoraFuncionario').val().toFixed(2));
        $('#formIncluirProduto').submit();
    }

});

$('#btnAdicionarProdutoMateriaPrima').click(function (e) {

    limparMensagem('#formIncluirProduto');

    if (validarPreenchimento('#formIncluirProduto #divProdutoAdicionarMateriaPrima')) {

        removerMascaras();

        var materiaPrima = new Object();
        materiaPrima.Codigo = $('#ddlProdutoMateriaPrima').val();
        materiaPrima.Descricao = $('#ddlProdutoMateriaPrima :selected').text();
        materiaPrima.Quantidade = $('#txtProdutoMateriaPrimaQuantidade').val();

        requisicaoAjax('Produto/ConsultarMateriaPrima?codigoMateriaPrima=' + materiaPrima.Codigo, null, function (retorno) {
            materiaPrima.Valor = retorno.Entity.Valor;
            materiaPrima.SiglaUnidadeMedida = retorno.Entity.TipoUnidadeMedida.Sigla.toLowerCase();
        }, false);

        listarMateriasPrimasDoProduto(materiaPrima);

        somarEAtribuirValorProduto();
        limparCampos('#formIncluirProduto #divProdutoAdicionarMateriaPrima');
    }
});

$("#tblProdutoMateriasPrimas").on("click", ".js-excluirProdutoMateriaPrima", function () {

    removerLinha($(this), function () {
        somarEAtribuirValorProduto();
    });

});

$('td').on('click', '.js-ExcluirProduto', function () {

    if (confirm("Deseja Excluir?")) {

        requisicaoAjax('Produto/Excluir?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido)
                window.location.href = window.location.href;
        });

    }
});

$('td').on('click', '.js-AlterarProduto', function () {

    if (confirm("Deseja Alterar?")) {

        requisicaoAjax('Produto/Consultar?codigo=' + $(this).prop('id'), null, function (retorno) {
            if (retorno.IsValido) {

                var produto = retorno.Entity;
                $('#hdnProdutoCodigo').val(produto.Codigo);
                $('#txtProdutoDescricao').val(produto.Descricao);
                $('#txtProdutoNumeroProdutoCliente').val(produto.NumeroProdutoCliente);
                $('#txtProdutoFichaTecnicaModelo').val(produto.FichaTecnica.Modelo);
                $('#txtProdutoFichaTecnicaTipo').val(produto.FichaTecnica.Tipo);
                $('#txtProdutoFichaTecnicaNcm').val(produto.FichaTecnica.Ncm);
                $('#txtProdutoFichaTecnicaDescricao').val(produto.FichaTecnica.Descricao);
                $('#hdnPedidoFichaTecnicaCodigo').val(produto.FichaTecnica.Codigo);

                if (produto.FichaTecnica.Foto64 != '')
                    $('#imgProdutoDetalharFichaTecnica').prop('src', 'data:image/png;base64,' + produto.FichaTecnica.Foto64);

                $('#tblProdutoMateriasPrimas tbody').html('');

                $.each(produto.MateriasPrimas, function (index, item) {
                    listarMateriasPrimasDoProduto(item);
                });

                somarEAtribuirValorProduto();
                $('#modalIncluirProduto').modal('show');
            }
        });

    }
});

$("#input-file-now").change(function () {
    readURL(this);
});


function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgProdutoDetalharFichaTecnica').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
}

function validarMateriaPrima() {

    var valido = ($('#tblProdutoMateriasPrimas tr').length > 1);

    if (valido) {
        $('#divMensagemProdutoMateriaPrima').addClass('invisivel');
    }
    else {
        $('#divMensagemProdutoMateriaPrima span').text(mensagem.MSG31);
        $('#divMensagemProdutoMateriaPrima').removeClass('invisivel');
    }


    return valido;
}

function somarEAtribuirValorProduto() {

    var valorProduto = 0;

    $('.js-totalValorLinha').each(function (index, item) {
        valorProduto += +$(item).val().toString().replace(',', '.');
    });

    $('#spanProdutoValorTotal').text(formatarMonetaria(valorProduto));
    $('#hddProdutoValorProduto').val(toFixed(valorProduto, 2).toString().replace('.', ','));

    atualizarIndex();
}

function listarMateriasPrimasDoProduto(materiaPrima) {
    materiaPrima.Posicao = $('#tblProdutoMateriaPrima tbody tr').length;

    if (materiaPrima.SiglaUnidadeMedida == undefined)
        materiaPrima.SiglaUnidadeMedida = materiaPrima.TipoUnidadeMedida.Sigla;

    $('#tblProdutoMateriasPrimas tbody').append(

        '<tr>' +
        '   <td scope="col">' + materiaPrima.Codigo + '</td>' +
        '   <td scope="col">' + materiaPrima.Descricao.toUpperCase() + '</td>' +
        '   <td scope="col">' + materiaPrima.Quantidade.toString().replace('.', ',') + ' ' + materiaPrima.SiglaUnidadeMedida.toLowerCase() + '</td>' +
        '   <td scope="col">' + formatarMonetaria(materiaPrima.Valor) + '</td>' +
        '   <td scope="col">' + formatarMonetaria(materiaPrima.Valor * materiaPrima.Quantidade.toString().replace(',', '.')) + '</td>' +
        '   <td scope="col">' +
        '   <img src="' + rootUrl + 'Content/imagens/ExcluirMenor.png" alt="EXCLUIR MATÉRIA PRIMA DO PRODUTO" title="EXCLUIR MATÉRIA PRIMA DO PRODUTO" class="js-excluirProdutoMateriaPrima" />' +
        '   </td>' +
        ' <input name="produto.MateriasPrimas[' + materiaPrima.Posicao + '].Codigo" type="hidden" value="' + materiaPrima.Codigo + '">' +
        ' <input name="produto.MateriasPrimas[' + materiaPrima.Posicao + '].Descricao" type="hidden" value="' + materiaPrima.Descricao + '">' +
        ' <input name="produto.MateriasPrimas[' + materiaPrima.Posicao + '].Quantidade" type="hidden" value="' + materiaPrima.Quantidade.toString().replace('.', ',') + '">' +
        ' <input name="produto.MateriasPrimas[' + materiaPrima.Posicao + '].Valor" type="hidden" value="' + materiaPrima.Valor + '">' +
        ' <input class="js-totalValorLinha" type="hidden" value="' + (materiaPrima.Valor * +(materiaPrima.Quantidade.toString().replace(',', '.'))) + '">' +
        '</tr >'

    );
}

function atualizarIndex() {

    var linha = -1;

    $('#tblProdutoMateriasPrimas tr').each(function () {

        $(this).find('input[name *="produto.MateriasPrimas["]').each(function () {

            var nameAntigo = $(this).prop('name');

            var nameNovo = nameAntigo.substring(0, nameAntigo.indexOf('[') + 1) +
                linha +
                nameAntigo.substring(nameAntigo.indexOf(']'), nameAntigo.length);

            $(this).prop('name', nameNovo);
        });
        linha++;
    });
}
