using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using MySql.Data.MySqlClient;

namespace InfinitySolutions.Data
{

    public class DataAcessoSistema : DataBase
    {
        public Retorno Salvar(AcessoSistema Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_ACESSO_SISTEMA( ");
                CommandSQL.AppendLine("DATA, ");
                CommandSQL.AppendLine("BACKUP, ");
                CommandSQL.AppendLine("BLOQUEADO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DATA, ");
                CommandSQL.AppendLine("@BACKUP, ");
                CommandSQL.AppendLine("@BLOQUEADO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                Command.Parameters.AddWithValue("@BACKUP", Entity.Backup);
                Command.Parameters.AddWithValue("@BLOQUEADO", Entity.Bloqueado);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao SALVAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Salvar Acesso Sistema"));
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(AcessoSistema Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_ACESSO_SISTEMA WHERE DATA = @DATA");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Acesso Sistema", "Excluido "));
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao EXCLUIR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Excluir Acesso Sistema"));
            }
            finally { Fechar(); }
        }

        public Retorno Carregar(AcessoSistema Entity)
        {
            try
            {
                AcessoSistema AcessoSistema = new AcessoSistema();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_ACESSO_SISTEMA.DATA, ");
                CommandSQL.AppendLine("TB_ACESSO_SISTEMA.BACKUP, ");
                CommandSQL.AppendLine("TB_ACESSO_SISTEMA.BLOQUEADO ");
                CommandSQL.AppendLine("FROM TB_ACESSO_SISTEMA ");

                CommandSQL.AppendLine("WHERE TB_ACESSO_SISTEMA.DATA = @DATA ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    AcessoSistema = FillEntity(reader);
                }
                return new Retorno(AcessoSistema);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao CARREGAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Carregar Acesso Sistema"));
            }
            finally { Fechar(); }
        }

        public Retorno Alterar(AcessoSistema Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_ACESSO_SISTEMA SET ");
                CommandSQL.AppendLine("BACKUP = @BACKUP, ");
                CommandSQL.AppendLine("BLOQUEADO = @BLOQUEADO ");
                CommandSQL.AppendLine("WHERE DATA = @DATA");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                Command.Parameters.AddWithValue("@BACKUP", Entity.Backup);
                Command.Parameters.AddWithValue("@BLOQUEADO", Entity.Bloqueado);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Acesso Sistema", "Alterado "));
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao ALTERAR Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Alterar Acesso Sistema"));
            }
            finally { Fechar(); }
        }

        public AcessoSistema FillEntity(IDataReader reader)
        {
            AcessoSistema AcessoSistema = new AcessoSistema();
            try
            {
                AcessoSistema.Data = reader["DATA"].ConverteValor<DateTime>(DateTime.MinValue);
                AcessoSistema.Backup = reader["BACKUP"].ConverteValor<bool>(false);
                AcessoSistema.Bloqueado = reader["BLOQUEADO"].ConverteValor<bool>(false);
            }
            catch (Exception) { throw; }
            return AcessoSistema;
        }

        public Retorno VerificarAcessoBloqueado()
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 ");
                CommandSQL.AppendLine("FROM TB_ACESSO_SISTEMA ");
                CommandSQL.AppendLine("WHERE DATA IN( ");
                CommandSQL.AppendLine("SELECT MAX(DATA) ");
                CommandSQL.AppendLine("FROM TB_ACESSO_SISTEMA) ");
                CommandSQL.AppendLine("AND BLOQUEADO = @SIM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@SIM", true);
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    return new Retorno(false, Mensagens.MSG_15);
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao VERIFICAR ACESSO BLOQUEADO Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Acesso Sistema Desbloqueado "));
            }
            finally { Fechar(); }
        }

        public Retorno Listar(int Pagina, int QntPaginas)
        {
            throw new NotImplementedException();
        }

        public Retorno VerificarUltimoAcesso()
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT MAX(DATA) ");
                CommandSQL.AppendLine("FROM TB_ACESSO_SISTEMA ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();

                return new Retorno(Command.ExecuteScalar().ConverteValor<DateTime>(DateTime.MinValue));
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao VERIFICAR ULTIMO ACESSO ACESSO BLOQUEADO Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Ultimo Acesso Sistema "));
            }
            finally { Fechar(); }
        }

        public Retorno CarregarDataUlTimoBackup()
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT MAX(DATA) ");
                CommandSQL.AppendLine("FROM TB_ACESSO_SISTEMA ");

                CommandSQL.AppendLine("WHERE BACKUP = @SIM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@SIM", true);
                Abrir();
                return new Retorno(Command.ExecuteScalar().ConverteValor<DateTime>(DateTime.MinValue));
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao DATA ULTIMO BACKUP Acesso Sistema"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Verifica Data do Ultimo Backup externo "));
            }
            finally { Fechar(); }
        }
    }
}

