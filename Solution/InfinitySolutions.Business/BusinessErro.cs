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

    public class BusinessErro : IBusiness<Erro>
    {
        public Retorno Salvar(Erro Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {                 
                    retorno = new DataErro().Salvar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.SALVAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Salvar Erro"));
            }
        }

        public Retorno Excluir(Erro Entity)
        {
            try
            {
                return new DataErro().Excluir(Entity);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.EXCLUIR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Excluir Erro"));
            }
        }

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                return new DataErro().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.LISTAR_PAGINADO,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Listar Erro"));
            }
        }

        public Retorno Listar()
        {
            try
            {
                return new DataErro().Listar();
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.LISTAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Listar Erro"));
            }
        }

        public Retorno Carregar(Erro Entity)
        {
            try
            {
                return new DataErro().Carregar(Entity);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.CARREGAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Carregar Erro"));
            }
        }

        public Retorno Alterar(Erro Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    retorno = new DataErro().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.BUSINESS,
                    Funcionalidade = EnumFuncionalidade.ALTERAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Alterar Erro"));
            }
        }

        public Retorno PreenchimentoObrigatorio(Erro Entity)
        {
            if (Entity.Id == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Id"));

            if (String.IsNullOrEmpty(Entity.Descricao))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Descricao"));

            if (Entity.CasoDeUso.ToString() == "-1")
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Caso De Uso"));

            if (Entity.Camada.ToString() == "-1")
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Camada"));

            if (Entity.Funcionalidade.ToString() == "-1")
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Funcionalidade"));

            if (String.IsNullOrEmpty(Entity.Entidade))
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Entidade"));

            return new Retorno(true);
        }

    }
}

