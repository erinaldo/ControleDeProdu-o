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

    public class BusinessAcessoSistema
    {
        public Retorno Salvar(AcessoSistema Entity)
        {
            try
            {
                return new DataAcessoSistema().Salvar(Entity);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao SALVAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Salvar Acesso Sistema"));
            }
        }

        public Retorno Excluir(AcessoSistema Entity)
        {
            try
            {
                return new DataAcessoSistema().Excluir(Entity);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao EXCLUIR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Excluir Acesso Sistema"));
            }
        }

        public Retorno Carregar(AcessoSistema Entity)
        {
            try
            {
                return new DataAcessoSistema().Carregar(Entity);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao CARREGAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Carregar Acesso Sistema"));
            }
        }

        public Retorno Alterar(AcessoSistema Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    retorno = new DataAcessoSistema().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao ALTERAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Alterar Acesso Sistema"));
            }
        }

        public Retorno PreenchimentoObrigatorio(AcessoSistema Entity)
        {
            if (Entity.Data == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data"));

            if (Entity.Backup == false)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Backup"));

            if (Entity.Bloqueado == false)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Bloqueado"));

            return new Retorno(true);
        }

        public Retorno VerificarAcessoBloqueado()
        {
            try
            {
                return new DataAcessoSistema().VerificarAcessoBloqueado();
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao VERIFICAR ACESSO BLOQUEADO Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Acesso Sistema Desbloqueado"));
            }
        }

        public Retorno VerificarUltimoAcesso()
        {
            try
            {
                Retorno retorno = new DataAcessoSistema().VerificarUltimoAcesso();

                if (retorno.IsValido)
                {
                    retorno = new Retorno(retorno.Entity.ConverteValor<DateTime>(DateTime.MinValue) < DateTime.Now, Mensagens.MSG_16);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao VERIFICAR ULTIMO ACESSO Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Acesso Sistema Data Alterada"));
            }
        }

        public Retorno CarregarDataUlTimoBackup()
        {
            try
            {
                return new DataAcessoSistema().CarregarDataUlTimoBackup();
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada Business ao CARREGAR DATA ULTIMO BACKUP Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Data do Ultimo Backup"));
            }
        }

        public Retorno Listar(int Pagina, int QntPaginas)
        {
            throw new NotImplementedException();
        }

        public Retorno PrazoBloqueio(DateTime dataBloqueio, int diasAviso)
        {
            int diasParaBloqueio = dataBloqueio.Date.Subtract(DateTime.Now.Date).Days;

            if (diasParaBloqueio <= 3)
                return new Retorno(true, String.Format(Mensagens.MSG_17, diasParaBloqueio));
            else
                return new Retorno(false);
        }

        public Retorno NaoBloquear(DateTime dataBloqueio)
        {   
            if (dataBloqueio.Date <= DateTime.Now.Date)
            {
                Retorno retorno = new BusinessAcessoSistema().Salvar(new AcessoSistema() { Data = DateTime.Now, Backup = false, Bloqueado = true });

                if (retorno.IsValido)
                {
                    return new Retorno(false, Mensagens.MSG_15);
                }
            }

            return new Retorno(true);
        }

        public Retorno UltrapassouPrazoBackup()
        {
            Retorno retorno = CarregarDataUlTimoBackup();

            if (retorno.IsValido)
            {
                int diasUltimoBackup = DateTime.Now.Subtract(retorno.Entity.ConverteValor<DateTime>(DateTime.MinValue)).Days;

                return new Retorno(diasUltimoBackup > 15);

            }

            return retorno;
        }
    }
}

