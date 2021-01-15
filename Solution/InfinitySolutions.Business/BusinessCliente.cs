using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using System.Transactions;
using System.Net;

namespace InfinitySolutions.Business
{

    public class BusinessCliente : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Cliente Entity)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var retorno = new BusinessEndereco().Salvar(Entity.Endereco);
                    if (retorno.IsValido)
                    {
                        retorno = PreenchimentoObrigatorio(Entity);
                        if (retorno.IsValido)
                        {
                            if (Entity.Codigo == 0)
                                retorno = new DataCliente().Incluir(Entity);
                            else
                                retorno = new DataCliente().Alterar(Entity);

                            if (retorno.IsValido)
                                transaction.Complete();
                        }
                    }
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Cliente Entity)
        {
            try
            {
                return new DataCliente().Excluir(Entity);
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
                return new DataCliente().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Cliente Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataCliente().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataCliente().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Cliente Entity)
        {
            try
            {
                return new DataCliente().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno ConsultarDados(decimal cnpj)
        {
            try
            {
                return ConsultarDadosServico(cnpj);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Cliente Entity)
        {
            if (String.IsNullOrEmpty(Entity.Nome))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Nome"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Cliente Entity)
        {
            try
            {
                return new DataCliente().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private Retorno ConsultarDadosServico(decimal cnpj)
        {
            try
            {
                using (var w = new WebClient())
                {
                    IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
                    defaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                    w.Proxy = defaultWebProxy;

                    var retornoJsonServico = w.DownloadString(String.Format("https://www.receitaws.com.br/v1/cnpj/{0}", cnpj.ToString().PadLeft(14, '0')));

                    var clienteJson = retornoJsonServico.ConverteJSonParaObject<ClienteJson>();
                    var cliente = new Cliente
                    {
                        Cnpj = cnpj,
                        Nome = clienteJson.nome,
                        NomeFantasia = clienteJson.fantasia,
                        Contato = String.Format("{0} - {1}", clienteJson.telefone, clienteJson.email),
                        Endereco = new Endereco
                        {
                            Rua = clienteJson.logradouro,
                            Bairro = clienteJson.bairro,
                            Cidade = clienteJson.municipio,
                            Numero = clienteJson.numero,
                            Uf = clienteJson.uf,
                            Cep = clienteJson.cep
                        }
                    };

                    return new Retorno(cliente);
                }
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

