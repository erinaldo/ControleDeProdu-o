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

    public class BusinessProduto : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Produto Entity)
        {
            try
            {
                Entity.Data = DateTime.Now;
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    retorno = VerificarExistencia(Entity);

                    if (retorno.IsValido)
                    {
                        using (var transaction = new TransactionScope())
                        {
                            retorno = new BusinessFichaTecnica().Salvar(Entity.FichaTecnica);

                            if (retorno.IsValido)
                            {
                                if (Entity.Codigo == 0)
                                    retorno = new DataProduto().Incluir(Entity);
                                else
                                {
                                    retorno = new DataProduto().Consultar(Entity);

                                    if (retorno.IsValido)
                                    {
                                        var produtoAtual = retorno.Entity as Produto;

                                        if (produtoAtual.Valor != Entity.Valor)
                                        {
                                            retorno = new DataProduto().AlterarValor(produtoAtual);

                                            if (retorno.IsValido)
                                            {
                                                retorno = new DataProduto().IncluirValor(Entity);
                                            }
                                        }

                                        if (retorno.IsValido)
                                            retorno = new DataProduto().Alterar(Entity);
                                    }
                                }

                                if (retorno.IsValido)
                                {
                                    retorno = new BusinessMateriaPrima().ExcluirDoProduto(Entity.Codigo);

                                    if (retorno.IsValido)
                                    {
                                        retorno = SalvarMateriaPrimaProduto(Entity);

                                        if (retorno.IsValido)
                                            transaction.Complete();
                                    }
                                }
                            }
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

        public Retorno SalvarMateriaPrimaProduto(Produto produto)
        {
            try
            {
                var retorno = new Retorno();

                foreach (var materiaPrima in produto.MateriasPrimas)
                {
                    retorno = Salvar(produto, materiaPrima);

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

        public Retorno ConsultarMateriaPrima(int codigoMateriaPrima)
        {
            try
            {
                return new BusinessMateriaPrima().Consultar(new MateriaPrima { Codigo = codigoMateriaPrima });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Salvar(Produto Entity, MateriaPrima materiaPrima)
        {
            try
            {
                return new DataProduto().Incluir(Entity, materiaPrima);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Produto Entity)
        {
            try
            {
                return new DataProduto().Excluir(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ExcluirDoPedido(int codigoPedido)
        {
            try
            {
                return new DataProduto().ExcluirDoPedido(codigoPedido);
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
                return new DataProduto().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Produto Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataProduto().Pesquisar(Entity, Pagina, QntPagina);
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
                var retorno = new DataProduto().Listar();

                if (retorno.IsValido)
                {
                    foreach (var produto in retorno.Entity as List<Produto>)
                    {
                        var retornoMateriaPrima = new BusinessMateriaPrima().Carregar(produto.Codigo);

                        if (retorno.IsValido)
                            produto.MateriasPrimas = retornoMateriaPrima.Entity as List<MateriaPrima>;
                        else
                            return retornoMateriaPrima;
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Produto Entity)
        {
            try
            {
                var retorno = new DataProduto().Consultar(Entity);

                if (retorno.IsValido)
                {
                    var produto = retorno.Entity as Produto;

                    retorno = new BusinessMateriaPrima().ConsultarDoProduto(Entity.Codigo);

                    if (retorno.IsValido)
                    {
                        produto.MateriasPrimas = retorno.Entity as List<MateriaPrima>;

                        return new Retorno(produto);
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno CarregarDominios()
        {
            var retorno = new BusinessMateriaPrima().Listar();

            if (retorno.IsValido)
            {
                var dominios = new DominiosDto { MateriasPrimas = retorno.Entity as List<MateriaPrima> };
                retorno.Entity = dominios;
            }

            return retorno;
        }

        public Retorno ConsultarTempoProducaoHoraFuncionario(Produto produto)
        {
            try
            {
                return new DataProduto().ConsultarTempoProducaoHoraFuncionario(produto);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarDoPedido(int codigoPedido)
        {
            try
            {
                return new DataPedido().ConsultarDoPedido(codigoPedido);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Produto Entity)
        {
            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (Entity.Valor == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Produto Entity)
        {
            try
            {
                return new DataProduto().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

