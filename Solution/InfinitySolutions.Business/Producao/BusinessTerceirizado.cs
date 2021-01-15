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

    public class BusinessTerceirizado : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Terceirizado Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataTerceirizado().Incluir(Entity);
                    else
                        retorno = new DataTerceirizado().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Terceirizado Entity)
        {
            try
            {
                return new DataTerceirizado().Excluir(Entity);
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
                return new DataTerceirizado().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Terceirizado Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataTerceirizado().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataTerceirizado().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Terceirizado Entity)
        {
            try
            {
                return new DataTerceirizado().Consultar(Entity);
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
                    TiposFuncoesTerceirizado = RecuperarDominio<TipoFuncaoTerceirizado>(new BusinessTipoFuncaoTerceirizado().Listar())
                });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Terceirizado Entity)
        {

            if (String.IsNullOrEmpty(Entity.Nome))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Nome"));


            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Terceirizado Entity)
        {
            try
            {
                return new DataTerceirizado().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

