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

    public class BusinessLogin : BusinessBase
    {
        #region AÇÕES

        public Retorno Salvar(Login Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataLogin().Incluir(Entity);
                    else
                        retorno = new DataLogin().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno VerificarLogin(Login login)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(login);
                if (retorno.IsValido)
                {
                    login.Senha = Seguranca.Criptografar(login.Senha, "VESTIRY");

                    retorno = ConsultarExistenciaSenha();

                    if (retorno.IsValido)
                    {
                        if (retorno.Entity != null && retorno.Entity.ConverteValor(0) == 1)
                        {
                            retorno = new DataLogin().Consultar(login);

                            if (retorno.IsValido)
                            {
                                retorno.IsValido = retorno.Entity != null && ((Login)retorno.Entity).Codigo > 0;

                                if (retorno.IsValido)
                                    CarregarPermissoesFuncoes(retorno.Entity as Login);
                                else
                                    retorno.Mensagem = Mensagens.MSG_30;
                            }
                        }
                        else
                            return new BusinessLogin().Salvar(login);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private Retorno CarregarPermissoesFuncoes(Login login)
        {
            var retorno = new BusinessTipoPermissao().Consultar(login.Codigo);

            if (retorno.IsValido)
            {
                login.TiposPermissoes = retorno.Entity as List<TipoPermissao>;
                retorno = new BusinessTipoFuncao().Consultar(login.Codigo);

                if (retorno.IsValido)
                    login.TiposFuncoes = retorno.Entity as List<TipoFuncao>;
            }
            return retorno;
        }

        private Retorno ConsultarExistenciaSenha()
        {
            try
            {
                return new DataLogin().ConsultarExistenciaSenha();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(Login Entity)
        {
            try
            {
                return new DataLogin().Excluir(Entity);
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
                return new DataLogin().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(Login Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataLogin().Pesquisar(Entity, Pagina, QntPagina);
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
                return new DataLogin().Listar();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(Login Entity)
        {
            try
            {
                return new DataLogin().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(Login Entity)
        {
            if (String.IsNullOrEmpty(Entity.Usuario))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Usuario"));

            if (String.IsNullOrEmpty(Entity.Senha))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Senha"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(Login Entity)
        {
            try
            {
                return new DataLogin().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

    }
}

