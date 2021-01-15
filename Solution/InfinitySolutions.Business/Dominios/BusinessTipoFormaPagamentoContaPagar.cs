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

    public class BusinessTipoFormaPagamentoContaPagar : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataTipoFormaPagamentoContaPagar().Incluir(Entity);
                    else
                        retorno = new DataTipoFormaPagamentoContaPagar().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                return new DataTipoFormaPagamentoContaPagar().Excluir(Entity);
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
                return new DataTipoFormaPagamentoContaPagar().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(TipoFormaPagamentoContaPagar Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataTipoFormaPagamentoContaPagar().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataTipoFormaPagamentoContaPagar().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                return new DataTipoFormaPagamentoContaPagar().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(TipoFormaPagamentoContaPagar Entity)
        {

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (Entity.ContemParcelas == false)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Contem Parcelas"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                return new DataTipoFormaPagamentoContaPagar().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

