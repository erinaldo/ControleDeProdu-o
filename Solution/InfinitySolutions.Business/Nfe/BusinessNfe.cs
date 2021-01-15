using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using InfinitySolutions.Entity.Nfe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Business.Nfe
{
    public class BusinessNfe
    {
        private EmitirNfe ConverterPedidoParaEmitirNfe(Pedido pedido, Frete frete, List<Produto> produtos, bool ambienteProducao)
        {
            var emitirNfe = new EmitirNfe
            {
                ID = pedido.Codigo,
                url_notificacao = "",
                operacao = 1,
                natureza_operacao = "Venda de produção do estabelecimento",
                modelo = 1,
                finalidade = 1,
                ambiente = ambienteProducao ? 1 : 2
            };
            emitirNfe.cliente = new cliente
            {
                consumidor_final = 1,
                cnpj = pedido.Cliente.Cnpj.Value.ToString(@"00\.000\.000\/0000\-00"),
                razao_social = pedido.Cliente.Nome,
                nome_completo = pedido.Cliente.NomeFantasia,
                ie = pedido.Cliente.Ie.Value.ToString(),
                endereco = pedido.Cliente.Endereco.Rua,
                complemento = "",
                numero = pedido.Cliente.Endereco.Numero.ConverteValor(0),
                bairro = pedido.Cliente.Endereco.Bairro,
                cidade = pedido.Cliente.Endereco.Cidade,
                uf = pedido.Cliente.Endereco.Uf,
                cep = pedido.Cliente.Endereco.Cep,
                //telefone = pedido.Cliente.Contato.Split('-')[0],
                //email = pedido.Cliente.Contato.Split('-')[1]
            };

            foreach (var produto in pedido.Produtos)
            {
                if (produtos.Any(p => p.CodigoPedidoProduto == produto.CodigoPedidoProduto && p.QuantidadeFaturar > 0))
                {
                    var quantidade = produtos.FirstOrDefault(p => p.CodigoPedidoProduto == produto.CodigoPedidoProduto).QuantidadeFaturar;

                    emitirNfe.produtos.Add(new produto
                    {
                        nome = produto.DescricaoNfe,
                        codigo = produto.NumeroProdutoCliente.ToString(),
                        ncm = produto.FichaTecnica.Ncm,
                        quantidade = (int)quantidade,
                        unidade = "UN",
                        origem = 0,
                        subtotal = produto.ValorUnitario.ToString().Replace(",", "."),
                        total = (produto.ValorUnitario * quantidade).ToString().Replace(",", "."),
                        totalDecimal = (produto.ValorUnitario.Value * quantidade.Value),
                        classe_imposto = "REF10550559"
                    });
                }
            }

            emitirNfe.pedido = new pedido
            {
                pagamento = pedido.TipoFormaPagamento.Dias == 0 && pedido.DataPagamento < DateTime.Now ? 0 : 1,
                presenca = 2,
                modalidade_frete = frete.TipoFrete.Codigo,
                frete = "0.00",
                desconto = "0.00",
                total = emitirNfe.produtos.Sum(p => p.totalDecimal).ToString("F"),
                forma_pagamento = 99,
                informacoes_complementares = String.Format("{0} <br />{1}", Constantes.DESCRICAO_OBSERVACAO, pedido.ObservacaoNotaFiscal)
            };

            if (frete.TipoFrete.Codigo != 9)
            {
                emitirNfe.transporte = new transporte
                {
                    volume = frete.Volume.ToString(),
                    razao_social = frete.Transportadora
                };
            }

            if ((pedido.TipoFormaPagamento.Dias.HasValue && pedido.TipoFormaPagamento.Dias > 0) || pedido.TipoFormaPagamento.DataCombinada)
            {
                emitirNfe.parcelas = new parcela[1] { new parcela() };
                emitirNfe.parcelas[0] = new parcela
                {
                    valor = emitirNfe.pedido.total,
                    vencimento = pedido.DataPagamento.Value.ToString("yyyy-MM-dd")
                };
                emitirNfe.fatura = new fatura
                {
                    numero = "000001",
                    desconto = "0.00",
                    valor = emitirNfe.pedido.total,
                    valor_liquido = emitirNfe.pedido.total
                };
            }

            return emitirNfe;
        }

        public Retorno EmitirNfe(Pedido pedido, Frete frete, List<Produto> produtos, bool ambienteProducao)
        {
            if (pedido == null)
                pedido = new Pedido();
            try
            {
                var retorno = new BusinessPedido().Consultar(new Pedido { Codigo = pedido.Codigo }, (int)EnumTipoFase.FINALIZADO);

                if (retorno.IsValido)
                {
                    pedido = retorno.Entity as Pedido;

                    if (!pedido.DataPagamento.HasValue)
                        pedido.DataPagamento = DateTime.Now.AddDays(pedido.TipoFormaPagamento.Dias ?? 0);

                    return new Retorno(new Tuple<EmitirNfe, Pedido>(ConverterPedidoParaEmitirNfe(pedido, frete, produtos, ambienteProducao), pedido));
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }
    }
}
