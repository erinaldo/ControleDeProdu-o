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

    public class BusinessFornecedor : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Fornecedor Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    retorno = VerificarExistencia(Entity);

                    if (retorno.IsValido)
                    {
                        if (Entity.Codigo == 0)
                            retorno = new DataFornecedor().Incluir(Entity);
                        else
                            retorno = new DataFornecedor().Alterar(Entity);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Fornecedor Entity)
        {
            try
            {
                return new DataFornecedor().Excluir(Entity);
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
                return new DataFornecedor().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Fornecedor Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataFornecedor().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataFornecedor().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Fornecedor Entity)
        {
            try
            {
                return new DataFornecedor().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Fornecedor Entity)
        {

            if (String.IsNullOrEmpty(Entity.Nome))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Nome"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Fornecedor Entity)
        {
            try
            {
                return new DataFornecedor().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

