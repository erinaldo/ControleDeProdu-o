using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using System.Transactions;
using InfinitySolutions.Business.Nfe;
using InfinitySolutions.Entity.Enum;
using System.Net;

namespace InfinitySolutions.Business
{
    public class BusinessPedido : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Pedido Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    retorno = VerificarExistencia(Entity);

                    if (retorno.IsValido)
                    {
                        using (var transaction = new TransactionScope())
                        {
                            CalcularLucro(Entity);

                            if (Entity.Codigo == 0)
                                retorno = new DataPedido().Incluir(Entity);
                            else
                                retorno = new DataPedido().Alterar(Entity);

                            if (retorno.IsValido)
                            {
                                retorno = SalvarPedidoProduto(Entity);

                                if (retorno.IsValido)
                                    transaction.Complete();

                            }
                        }
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno MontarLinhaProducaoPedido(Pedido pedido, bool adicionarLinhaProducao, List<LinhaProducao> linhaProducaoTerceirizados, int codigoFuncao)
        {
            try
            {
                var retorno = new BusinessPedido().Consultar(pedido, codigoFuncao);

                if (retorno.IsValido)
                {
                    pedido = retorno.Entity as Pedido;
                    using (var transaction = new TransactionScope())
                    {
                        retorno = new BusinessLinhaProducao().MontarLinhaProducaoPedido(pedido, adicionarLinhaProducao, linhaProducaoTerceirizados);
                        if (retorno.IsValido && adicionarLinhaProducao)
                        {
                            retorno = new BusinessPedido().MudarTipoFase(pedido.Codigo);

                            if (retorno.IsValido)
                            {
                                retorno = new DataPedido().AlterarDataEntrega(pedido);
                            }

                            if (retorno.IsValido)
                                transaction.Complete();
                        }
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarPedidoProduto(Pedido pedido)
        {
            try
            {
                var retorno = new BusinessProduto().ConsultarDoPedido(pedido.Codigo);

                if (retorno.IsValido)
                {
                    var produtosExistentes = retorno.Entity as List<Produto>;
                    foreach (var produto in pedido.Produtos)
                    {
                        if (produto.CodigoPedidoProduto == 0)
                            retorno = new DataPedido().IncluirPedidoProduto(produto, pedido);
                        else
                            retorno = new DataPedido().AlterarPedidoProduto(produto, pedido);

                        if (!retorno.IsValido)
                            return retorno;
                    }

                    foreach (var produtoExistente in produtosExistentes)
                    {
                        if (!pedido.Produtos.Any(p => p.CodigoPedidoProduto == produtoExistente.CodigoPedidoProduto))
                        {
                            retorno = new DataPedido().ExcluirPedidoProduto(produtoExistente.CodigoPedidoProduto);

                            if (!retorno.IsValido)
                                return retorno;
                        }
                    }
                }

                return retorno;

            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarPedidoProdutoPronto(decimal quantidadePronta, int codigoTipoFase, int codigoPedidoProduto, int codigoPedido, string codigoNota = "")
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var retorno = new DataPedido().IncluirPedidoProdutoPronto(quantidadePronta, codigoTipoFase, codigoPedidoProduto, codigoNota);

                    if (retorno.IsValido)
                        retorno = VerificarEMudarTipoFasePronta(codigoTipoFase, codigoPedido);

                    if (retorno.IsValido)
                        transaction.Complete();

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno VerificarEMudarTipoFasePronta(int codigoTipoFase, int codigoPedido)
        {
            try
            {
                var retorno = new DataPedido().ConsultarQuantidadeProdutosFaltantes(codigoTipoFase, codigoPedido);

                if (retorno.IsValido)
                {
                    var produtosRestantes = retorno.Entity.ConverteValor(0M);

                    if (produtosRestantes == 0)
                        MudarTipoFase(codigoPedido);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ListarTerceirizados()
        {
            try
            {
                return new BusinessTerceirizado().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarFichaTecnica(FichaTecnica fichaTecnica)
        {
            try
            {
                return new BusinessFichaTecnica().Salvar(fichaTecnica);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Pedido Entity)
        {
            try
            {
                return new DataPedido().Excluir(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarTamanho(string descricao)
        {
            try
            {
                return new BusinessTipoTamanho().Salvar(new TipoTamanho { Descricao = descricao });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarCor(string descricao)
        {
            try
            {
                return new BusinessTipoCor().Salvar(new TipoCor { Descricao = descricao });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno SalvarTipoFormaPagamento(string descricao, bool contemParcelas)
        {
            try
            {
                return new BusinessTipoFormaPagamento().Salvar(new TipoFormaPagamento { Descricao = descricao, ContemParcelas = contemParcelas });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno MudarTipoFase(int codigoPedido)
        {
            try
            {
                var retorno = new BusinessTipoFase().ConsultarProximoTipoFase(codigoPedido);

                if (retorno.IsValido)
                {
                    var novoTipoFase = retorno.Entity as TipoFase;
                    retorno = new DataPedido().MudarTipoFase(codigoPedido, novoTipoFase.Codigo);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno EmitirNfe(Pedido pedido, Frete frete, List<Produto> produtos, bool ambienteProducao)
        {
            try
            {
                return new BusinessNfe().EmitirNfe(pedido, frete, produtos, ambienteProducao);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }


        public Retorno RegistrarFaturamento(Pedido pedido, List<Produto> produtos, string codigoNota)
        {
            try
            {
                var retorno = new Retorno();

                using (var transaction = new TransactionScope())
                {
                    var valorFaturado = 0M;

                    foreach (var produto in produtos.Where(p => p.QuantidadeFaturar.HasValue && p.QuantidadeFaturar.Value > 0))
                    {
                        retorno = SalvarPedidoProdutoPronto(produto.QuantidadeFaturar.Value, (int)EnumTipoFase.FATURADO, produto.CodigoPedidoProduto, pedido.Codigo, codigoNota);
                        valorFaturado += pedido.Produtos.FirstOrDefault(p => p.CodigoPedidoProduto == produto.CodigoPedidoProduto).ValorUnitario.Value * produto.QuantidadeFaturar.Value;
                    }

                    retorno = SalvarDataPagamento(pedido);

                    if (retorno.IsValido)
                    {
                        retorno = SalvarContaReceber(pedido, codigoNota, valorFaturado);

                        if (retorno.IsValido)
                        {
                            retorno = SalvarNotaFiscal(pedido, codigoNota, valorFaturado);

                            if (retorno.IsValido)
                            {
                                transaction.Complete();

                                WebClient webClientDownloadArquivo = new WebClient();
                                var danfe = webClientDownloadArquivo.DownloadData("http://nfe.webmaniabr.com/danfe/" + codigoNota);
                                var xml = webClientDownloadArquivo.DownloadData("http://nfe.webmaniabr.com/xmlnfe/" + codigoNota);

                                ISEmail.EnviarArquivosNfe(pedido, danfe, xml);
                            }
                        }
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno EnviarPorEmail(Pedido pedido)
        {
            try
            {
                WebClient webClientDownloadArquivo = new WebClient();
                var danfe = webClientDownloadArquivo.DownloadData("http://nfe.webmaniabr.com/danfe/" + pedido.NumeroNfe);
                var xml = webClientDownloadArquivo.DownloadData("http://nfe.webmaniabr.com/xmlnfe/" + pedido.NumeroNfe);

                return ISEmail.EnviarArquivosNfe(pedido, danfe, xml);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private Retorno SalvarNotaFiscal(Pedido pedido, string codigoNota, decimal valorFaturado)
        {
            return new BusinessNotaFiscal().Salvar(new NotaFiscal
            {
                Pedido = pedido,
                CodigoNotaFiscal = codigoNota,
                Valor = valorFaturado,
                DataPagamento = pedido.DataPagamento.Value
            });
        }

        private Retorno SalvarContaReceber(Pedido pedido, string codigoNota, decimal valorfaturado)
        {
            return new BusinessContaReceber().Salvar(new ContaReceber
            {
                Cliente = pedido.Cliente,
                DataEmissao = DateTime.Now,
                DataVencimento = pedido.DataPagamento,
                Pedido = pedido,
                Valor = valorfaturado,
                Status = EnumContaReceberStatus.EM_ABERTO,
                NotaFiscal = codigoNota
            });
        }

        private Retorno SalvarDataPagamento(Pedido pedido)
        {
            try
            {
                return new DataPedido().SalvarDataPagamento(pedido);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }


        #endregion

        #region CONSULTAS

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                return new DataPedido().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Pedido Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataPedido().Pesquisar(Entity, Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Listar(List<TipoFuncao> funcoes)
        {
            try
            {
                var retorno = new BusinessTipoFase().Consultar(new TipoFase { Codigo = funcoes.FirstOrDefault().Codigo });

                if (retorno.IsValido)
                {
                    var tipoFasePorFuncao = retorno.Entity as TipoFase;
                    return new DataPedido().Listar(tipoFasePorFuncao);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Pedido Entity, int codigoTipoFase)
        {
            try
            {
                var retorno = new DataPedido().Consultar(Entity);

                if (retorno.IsValido)
                {
                    var pedido = retorno.Entity as Pedido;
                    retorno = ConsultarPedidoProduto(pedido.Codigo, codigoTipoFase);

                    if (retorno.IsValido)
                    {
                        pedido.Produtos = retorno.Entity as List<Produto>;

                        pedido.Produtos.ForEach(p =>
                        {
                            retorno = new DataPedido().ConsultarPedidoProdutoPronto(pedido.Codigo, p.CodigoPedidoProduto);

                            if (retorno.IsValido)
                                p.FaseProduzido = retorno.Entity as decimal[];
                        });

                        retorno = new DataPedido().ConsultarNumeroNfe(pedido.Codigo);

                        if (retorno.IsValido && retorno.Entity != null)
                            pedido.NumeroNfe = retorno.Entity.ToString();

                        retorno.Entity = pedido;
                    }
                }

                return retorno;

            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private Retorno ConsultarPedidoProduto(int codigo, int codigoTipoFase)
        {
            try
            {
                return new DataPedido().ConsultarPedidoProduto(codigo, codigoTipoFase);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno CarregarDominios()
        {
            try
            {
                return new Retorno(new DominiosDto
                {
                    Clientes = RecuperarDominio<Cliente>(new BusinessCliente().Listar()),
                    TabelasPreco = RecuperarDominio<TabelaPreco>(new BusinessTabelaPreco().Listar()),
                    TiposFormasPagamento = RecuperarDominio<TipoFormaPagamento>(new BusinessTipoFormaPagamento().Listar()),
                    TiposFases = RecuperarDominio<TipoFase>(new BusinessTipoFase().Listar()),
                    Produtos = RecuperarDominio<Produto>(new BusinessProduto().Listar()),
                    TiposCores = RecuperarDominio<TipoCor>(new BusinessTipoCor().Listar()),
                    TiposTamanhos = RecuperarDominio<TipoTamanho>(new BusinessTipoTamanho().Listar()),
                    TiposFrete = RecuperarDominio<TipoFrete>(new BusinessTipoFrete().Listar()),
                });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }

        public Retorno ConsultarProduto(int codigoProduto, decimal? numeroPedidoCliente)
        {
            try
            {
                return new BusinessProduto().Consultar(new Produto { NumeroProdutoCliente = numeroPedidoCliente, Codigo = codigoProduto });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarTabelaPreco(int codigoTabelaPreco)
        {
            try
            {
                return new BusinessTabelaPreco().Consultar(new TabelaPreco { Codigo = codigoTabelaPreco });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarFormaPagamento(int codigoFormaPagamento)
        {
            try
            {
                return new BusinessTipoFormaPagamento().Consultar(new TipoFormaPagamento { Codigo = codigoFormaPagamento });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarPedidoProduto(int codigoPedidoProduto)
        {
            try
            {
                var retorno = new DataPedido().ConsultarPedidoProduto(codigoPedidoProduto);

                if (retorno.IsValido)
                {
                    var produto = retorno.Entity as Produto;
                    retorno = new BusinessMateriaPrima().ConsultarDoProduto(produto.Codigo);

                    if (retorno.IsValido)
                    {
                        produto.MateriasPrimas = retorno.Entity as List<MateriaPrima>;
                        retorno.Entity = produto;
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }



        public Retorno ListarParaDominio()
        {
            try
            {
                return new DataPedido().ListarParaDominio();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }


        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Pedido Entity)
        {

            if (Entity.NumeroPedidoCliente == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Numero Pedido Cliente"));




            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Pedido Entity)
        {
            try
            {
                return new DataPedido().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        protected List<T> RecuperarDominio<T>(Retorno retorno)
        {
            try
            {
                if (retorno.IsValido)
                {
                    return retorno.Entity as List<T>;
                }

                throw new Exception(retorno.Mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcularLucro(Pedido pedido)
        {
            if (pedido.TabelaPreco.Especial)
            {
                var valor = pedido.Produtos.Sum(p => p.Valor);
                pedido.PorcentagemLucro = (((pedido.ValorEspecial.Value - valor) * 100) / valor) - pedido.TabelaPreco.Imposto;
                pedido.Valor = pedido.ValorEspecial.Value;
            }
            else
                pedido.PorcentagemLucro = null;
        }


        #endregion
    }
}
