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
using InfinitySolutions.Entity.Enum;

namespace InfinitySolutions.Business
{

    public class BusinessContaPagar : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(ContaPagar Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    using (var transaction = new TransactionScope())
                    {
                        if (Entity.Codigo == 0)
                        {
                            retorno = new DataContaPagar().Incluir(Entity);
                            if (retorno.IsValido)
                                retorno = new BusinessParcela().Salvar(Entity);
                        }
                        else
                            retorno = new DataContaPagar().Alterar(Entity);



                        if (retorno.IsValido)
                            transaction.Complete();
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(ContaPagar Entity)
        {
            try
            {
                var retorno = new Retorno(true);

                using (var transaction = new TransactionScope())
                {
                    retorno = new BusinessParcela().Excluir(Entity.Codigo);

                    if (retorno.IsValido)
                    {
                        retorno = new DataContaPagar().Excluir(Entity);

                        if (retorno.IsValido)
                            transaction.Complete();
                    }

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno DarBaixaPagamento(Pagamento pagamento)
        {
            try
            {
                return new BusinessPagamento().Salvar(pagamento);
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
                return new DataContaPagar().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(ContaPagar Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataContaPagar().Pesquisar(Entity, Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Listar()
        {
            try
            {
                return new DataContaPagar().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Listar(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var retorno = new DataContaPagar().Listar(dataInicio, dataFim);
                var contasPagarDetalhada = new List<ContaPagar>();

                if (retorno.IsValido)
                {
                    var contasPagar = retorno.Entity as List<ContaPagar>;

                    foreach (var contaPagar in contasPagar)
                    {
                        switch ((EnumTipoContaPagar)contaPagar.TipoContaPagar.Codigo)
                        {
                            case EnumTipoContaPagar.PARCELADA:
                                retorno = new BusinessParcela().Consultar(contaPagar.Codigo, dataInicio, dataFim);

                                if (retorno.IsValido)
                                {
                                    contaPagar.Parcelas = retorno.Entity as List<Parcela>;

                                    foreach (var parcela in contaPagar.Parcelas)
                                    {
                                        retorno = new BusinessPagamento().Consultar(new Pagamento { ContaPagar = contaPagar, DataVencimento = parcela.DataVencimento });

                                        if (retorno.IsValido)
                                        {
                                            var pagamento = retorno.Entity as Pagamento;
                                            contasPagarDetalhada.Add(CriarContaParcelada(contaPagar, parcela, pagamento.Codigo > 0));
                                        }
                                        else
                                            return retorno;
                                    }
                                }
                                break;
                            case EnumTipoContaPagar.FIXA:
                                var dataReferencia = dataInicio;
                                var meses = 0;

                                while (dataReferencia < dataFim)
                                {
                                    retorno = new BusinessPagamento().Consultar(new Pagamento { ContaPagar = contaPagar, DataVencimento = contaPagar.DataVencimento.Value.AddMonths(meses) });

                                    if (retorno.IsValido)
                                    {
                                        var pagamento = retorno.Entity as Pagamento;

                                        contasPagarDetalhada.Add(CriarContaFixa(contaPagar, meses, pagamento.Codigo > 0));
                                        dataReferencia = dataReferencia.AddMonths(1);
                                        meses++;
                                    }
                                    else
                                        return retorno;
                                }
                                break;
                            case EnumTipoContaPagar.A_VISTA:
                                retorno = new BusinessPagamento().Consultar(new Pagamento { ContaPagar = contaPagar, DataVencimento = contaPagar.DataVencimento.Value });

                                if (retorno.IsValido)
                                {
                                    var pagamento = retorno.Entity as Pagamento;
                                    contaPagar.TipoStatusContaPagar.Codigo = pagamento.Codigo > 0 ? (int)EnumTipoStatusContaPagar.PAGO : contaPagar.DataVencimento < DateTime.Now ?
                                            (int)EnumTipoStatusContaPagar.ATRASADO : (int)EnumTipoStatusContaPagar.EM_ABERTO;
                                    contasPagarDetalhada.Add(contaPagar);
                                }
                                else
                                    return retorno;
                                break;
                            default:
                                break;
                        }


                    }

                    if (retorno.IsValido)
                        retorno.Entity = contasPagarDetalhada.OrderBy(c => c.DataVencimento).ToList();

                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(ContaPagar Entity)
        {
            try
            {
                return new DataContaPagar().Consultar(Entity);
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
                    TiposFormasPagamentoContaPagar = RecuperarDominio<TipoFormaPagamentoContaPagar>(new BusinessTipoFormaPagamentoContaPagar().Listar()),
                    TiposStatusContaPagar = RecuperarDominio<TipoStatusContaPagar>(new BusinessTipoStatusContaPagar().Listar()),
                    TiposStatusParcela = RecuperarDominio<TipoStatusParcela>(new BusinessTipoStatusParcela().Listar()),
                    TiposContasPàgar = RecuperarDominio<TipoContaPagar>(new BusinessTipoContaPagar().Listar()),
                    TiposFormasPagamentoParcela = RecuperarDominio<TipoFormaPagamentoParcela>(new BusinessTipoFormaPagamentoParcela().Listar()),
                    Fornecedores = RecuperarDominio<Fornecedor>(new BusinessFornecedor().Listar())
                });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }

        #endregion

        #region METODOS AUXILIARES

        private ContaPagar CriarContaFixa(ContaPagar contaPagar, int meses, bool contemPagamento)
        {
            var contaPagarClonada = (ContaPagar)contaPagar.Clone();
            contaPagarClonada.DataVencimento = contaPagarClonada.DataVencimento.Value.AddMonths(meses);

            contaPagarClonada.TipoStatusContaPagar = new TipoStatusContaPagar();
            contaPagarClonada.TipoStatusContaPagar.Codigo = contemPagamento ? (int)EnumTipoStatusContaPagar.PAGO : contaPagarClonada.DataVencimento < DateTime.Now ?
                (int)EnumTipoStatusContaPagar.ATRASADO : (int)EnumTipoStatusContaPagar.EM_ABERTO;
            return contaPagarClonada;
        }
        private ContaPagar CriarContaParcelada(ContaPagar contaPagar, Parcela parcela, bool contemPagamento)
        {
            var contaPagarClonada = new ContaPagar();
            contaPagarClonada = (ContaPagar)contaPagar.Clone();
            contaPagarClonada.Descricao += String.Format(" {0}/{1}", parcela.Numero, contaPagarClonada.Parcelas.Count);
            contaPagarClonada.DataVencimento = parcela.DataVencimento;
            contaPagarClonada.Valor = parcela.Valor;
            contaPagarClonada.NumeroParcela = parcela.Numero;

            contaPagarClonada.TipoStatusContaPagar = new TipoStatusContaPagar();
            contaPagarClonada.TipoStatusContaPagar.Codigo = contemPagamento ? (int)EnumTipoStatusContaPagar.PAGO : contaPagarClonada.DataVencimento < DateTime.Now ?
                (int)EnumTipoStatusContaPagar.ATRASADO : (int)EnumTipoStatusContaPagar.EM_ABERTO;

            return contaPagarClonada;
        }

        public Retorno PreenchimentoObrigatorio(ContaPagar Entity)
        {

            if (Entity.DataEmissao == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Emissao"));

            if (Entity.DataVencimento == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Vencimento"));

            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (Entity.TipoContaPagar.Codigo == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Tipo"));

            if (Entity.TipoContaPagar.ContemParcelas && Entity.Parcelas.Count == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Parcelas"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(ContaPagar Entity)
        {
            try
            {
                return new DataContaPagar().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

