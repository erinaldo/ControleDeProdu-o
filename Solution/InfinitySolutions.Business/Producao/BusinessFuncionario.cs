using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;

namespace InfinitySolutions.Business
{

    public class BusinessFuncionario : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Funcionario Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataFuncionario().Incluir(Entity);
                    else
                        retorno = new DataFuncionario().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Funcionario Entity)
        {
            try
            {
                return new DataFuncionario().Excluir(Entity);
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
                return new DataFuncionario().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Funcionario Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataFuncionario().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataFuncionario().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Funcionario Entity)
        {
            try
            {
                return new DataFuncionario().Consultar(Entity);
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
                    TiposFuncoesFuncionario = RecuperarDominio<TipoFuncaoFuncionario>(new BusinessTipoFuncaoFuncionario().Listar())
                });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }


        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Funcionario Entity)
        {

            if (String.IsNullOrEmpty(Entity.Nome))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Nome"));


            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Funcionario Entity)
        {
            try
            {
                return new DataFuncionario().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

