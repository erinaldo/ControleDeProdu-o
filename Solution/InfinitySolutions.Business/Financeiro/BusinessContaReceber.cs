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

    public class BusinessContaReceber : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(ContaReceber Entity)
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
                            retorno = new DataContaReceber().Incluir(Entity);
                        else
                            retorno = new DataContaReceber().Alterar(Entity);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(ContaReceber Entity)
        {
            try
            {
                return new DataContaReceber().Excluir(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno DarBaixaPagamento(int codigo)
        {
            try
            {
                return new DataContaReceber().DarBaixaPagamento(codigo);
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
                return new DataContaReceber().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(ContaReceber Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataContaReceber().Pesquisar(Entity, Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Listar(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return new DataContaReceber().Listar(dataInicio, dataFim);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(ContaReceber Entity)
        {
            try
            {
                return new DataContaReceber().Consultar(Entity);
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
                    Clientes = RecuperarDominio<Cliente>(new BusinessCliente().Listar()),
                    Pedidos = RecuperarDominio<Pedido>(new BusinessPedido().ListarParaDominio()),
                    TiposFormasPagamentoContaReceber = RecuperarDominio<TipoFormaPagamentoContaReceber>(new BusinessTipoFormaPagamentoContaReceber().Listar())
                });
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(ContaReceber Entity)
        {
            if (Entity.Cliente.Codigo == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Cliente"));

            if (Entity.Valor == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Valor"));

            if (Entity.DataEmissao == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Emissao"));

            if (Entity.DataVencimento == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Vencimento"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(ContaReceber Entity)
        {
            try
            {
                return new DataContaReceber().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

