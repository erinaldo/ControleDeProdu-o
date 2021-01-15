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

    public class BusinessFichaTecnica : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(FichaTecnica Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                //if (Entity.Foto != null)
                //{
                    if (retorno.IsValido)
                    {
                        if (Entity.Codigo == 0)
                            retorno = new DataFichaTecnica().Incluir(Entity);
                        else
                            retorno = new DataFichaTecnica().Alterar(Entity);
                    }
                //}
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(FichaTecnica Entity)
        {
            try
            {
                return new DataFichaTecnica().Excluir(Entity);
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
                return new DataFichaTecnica().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(FichaTecnica Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataFichaTecnica().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataFichaTecnica().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(FichaTecnica Entity)
        {
            try
            {
                return new DataFichaTecnica().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(FichaTecnica Entity)
        {

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(FichaTecnica Entity)
        {
            try
            {
                return new DataFichaTecnica().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

