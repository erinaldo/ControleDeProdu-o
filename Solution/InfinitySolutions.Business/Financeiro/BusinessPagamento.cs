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

    public class BusinessPagamento : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Pagamento Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataPagamento().Incluir(Entity);
                    else
                        retorno = new DataPagamento().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Pagamento Entity)
        {
            try
            {
                return new DataPagamento().Excluir(Entity);
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
                return new DataPagamento().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Pagamento Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataPagamento().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataPagamento().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Pagamento Entity)
        {
            try
            {
                return new DataPagamento().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Pagamento Entity)
        {

            if (Entity.ContaPagar.Codigo == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Codigo Conta"));

            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));


            if (Entity.DataVencimento == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Vencimento"));

            if (Entity.DataPagamento == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Pagamento"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Pagamento Entity)
        {
            try
            {
                return new DataPagamento().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

