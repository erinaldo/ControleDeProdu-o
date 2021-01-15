using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using System.Transactions;

namespace InfinitySolutions.Business
{

    public class BusinessTabelaPreco : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(TabelaPreco Entity)
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
                            if (Entity.Codigo == 0)
                                retorno = new DataTabelaPreco().Incluir(Entity);
                            else
                            {
                                retorno = new DataTabelaPreco().Consultar(Entity);

                                if (retorno.IsValido)
                                {
                                    var tabelaPrecoAtual = retorno.Entity as TabelaPreco;

                                    if (tabelaPrecoAtual.Imposto != Entity.Imposto || tabelaPrecoAtual.Lucro != Entity.Lucro)
                                    {
                                        retorno = new DataTabelaPreco().AlterarValor(tabelaPrecoAtual);

                                        if (retorno.IsValido)
                                        {
                                            retorno = new DataTabelaPreco().IncluirValor(Entity);

                                            if (retorno.IsValido)
                                                retorno = new DataTabelaPreco().Alterar(Entity);
                                        }


                                    }
                                    else
                                        retorno = new DataTabelaPreco().Alterar(Entity);
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

        public Retorno Excluir(TabelaPreco Entity)
        {
            try
            {
                return new DataTabelaPreco().Excluir(Entity);
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
                return new DataTabelaPreco().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(TabelaPreco Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataTabelaPreco().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataTabelaPreco().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(TabelaPreco Entity)
        {
            try
            {
                return new DataTabelaPreco().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(TabelaPreco Entity)
        {

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (Entity.Imposto == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Imposto"));

            if (Entity.Lucro == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Lucro"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(TabelaPreco Entity)
        {
            try
            {
                return new DataTabelaPreco().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

