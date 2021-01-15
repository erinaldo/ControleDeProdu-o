using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;

namespace InfinitySolutions.Business
{

    public class BusinessParcela : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Parcela Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataParcela().Incluir(Entity);
                    else
                        retorno = new DataParcela().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Salvar(ContaPagar contaPagar)
        {
            try
            {
                var retorno = new Retorno(true);

                foreach (var parcela in contaPagar.Parcelas)
                {
                    parcela.ContaPagar = contaPagar;
                    retorno = Salvar(parcela);

                    if (!retorno.IsValido)
                        break;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Parcela Entity)
        {
            try
            {
                return new DataParcela().Excluir(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(int codigoContaPagar)
        {
            try
            {
                return new DataParcela().Excluir(codigoContaPagar);
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
                return new DataParcela().Listar(Pagina, QntPagina);
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
                return new DataParcela().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Parcela Entity)
        {
            try
            {
                return new DataParcela().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(int codigoContaPagar, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return new DataParcela().Consultar(codigoContaPagar, dataInicio, dataFim);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Parcela Entity)
        {
            if (Entity.DataVencimento == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Vencimento"));

            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));

            if (Entity.Numero == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Numero"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Parcela Entity)
        {
            try
            {
                return new DataParcela().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

