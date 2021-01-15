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

namespace InfinitySolutions.Business
{
    public class BusinessMateriaPrima : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(MateriaPrima Entity)
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
                            if (Entity.Codigo == 0)
                                retorno = new DataMateriaPrima().Incluir(Entity);
                            else
                            {
                                retorno = new DataMateriaPrima().Consultar(Entity);

                                if (retorno.IsValido)
                                {
                                    var materiaPrimaAtual = retorno.Entity as MateriaPrima;

                                    if (materiaPrimaAtual.Valor != Entity.Valor)
                                    {
                                        retorno = new DataMateriaPrima().AlterarValor(materiaPrimaAtual);

                                        if (retorno.IsValido)
                                            retorno = new DataMateriaPrima().IncluirValor(Entity);
                                    }

                                    retorno = new DataMateriaPrima().Alterar(Entity);
                                }
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

        public Retorno ExcluirDoProduto(int codigoProduto)
        {
            try
            {
                return new DataMateriaPrima().ExcluirDoProduto(codigoProduto);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(MateriaPrima Entity)
        {
            try
            {
                return new DataMateriaPrima().Excluir(Entity);
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
                return new DataMateriaPrima().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(MateriaPrima Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataMateriaPrima().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataMateriaPrima().Listar();
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
                var retorno = new BusinessTipoUnidadeMedida().Listar();

                if (retorno.IsValido)
                {
                    var dominios = new DominiosDto { TiposUnidadeMedida = retorno.Entity as List<TipoUnidadeMedida> };
                    retorno.Entity = dominios;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Carregar(int codigo)
        {
            try
            {
                return new DataMateriaPrima().Carregar(codigo);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(MateriaPrima Entity)
        {
            try
            {
                return new DataMateriaPrima().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarDoProduto(int codigoProduto)
        {
            try
            {
                return new DataMateriaPrima().ConsultarDoProduto(codigoProduto);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(MateriaPrima Entity)
        {

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (String.IsNullOrEmpty(Entity.Fornecedor))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Fornecedor"));

            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor Custo"));

            if (Entity.Quantidade == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Quantidade"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(MateriaPrima Entity)
        {
            try
            {
                return new DataMateriaPrima().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

