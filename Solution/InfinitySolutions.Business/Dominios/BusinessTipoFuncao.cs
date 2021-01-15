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

    public class BusinessTipoFuncao : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(TipoFuncao Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataTipoFuncao().Incluir(Entity);
                    else
                        retorno = new DataTipoFuncao().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(TipoFuncao Entity)
        {
            try
            {
                return new DataTipoFuncao().Excluir(Entity);
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
                return new DataTipoFuncao().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(TipoFuncao Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataTipoFuncao().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataTipoFuncao().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(TipoFuncao Entity)
        {
            try
            {
                return new DataTipoFuncao().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(TipoFuncao Entity)
        {

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(TipoFuncao Entity)
        {
            try
            {
                return new DataTipoFuncao().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(int codigoLogin)
        {
            try
            {
                return new DataTipoFuncao().Consultar(codigoLogin);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

