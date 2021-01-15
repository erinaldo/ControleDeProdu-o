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

    public class DataLogin : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Login Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_LOGIN( ");
                CommandSQL.AppendLine("NOME, ");
                CommandSQL.AppendLine("USUARIO, ");
                CommandSQL.AppendLine("SENHA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NOME, ");
                CommandSQL.AppendLine("@USUARIO, ");
                CommandSQL.AppendLine("@SENHA) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@USUARIO", Entity.Usuario);
                Command.Parameters.AddWithValue("@SENHA", Entity.Senha);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(Login Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_LOGIN WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex).Number == 1451)
                    return new Retorno(false, Mensagens.MSG_16);
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Alterar(Login Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_LOGIN SET ");
                CommandSQL.AppendLine("SENHA = @SENHA ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@SENHA", Entity.Senha);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Alterado "));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarExistenciaSenha()
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 ");
                CommandSQL.AppendLine("FROM TB_LOGIN ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();

                return new Retorno(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region CONSULTAS

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                List<Login> Logins = new List<Login>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LOGIN.CODIGO, ");
                CommandSQL.AppendLine("TB_LOGIN.USUARIO, ");
                CommandSQL.AppendLine("TB_LOGIN.SENHA ");
                CommandSQL.AppendLine("FROM TB_LOGIN ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Logins.Add(FillEntity(Reader));
                }
                return new Retorno(Logins);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Login Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Login> Logins = new List<Login>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LOGIN.CODIGO, ");
                CommandSQL.AppendLine("TB_LOGIN.SENHA ");
                CommandSQL.AppendLine("FROM TB_LOGIN ");

                CommandSQL.AppendLine("WHERE (TB_LOGIN.SENHA LIKE '%" + Entity.Senha + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Logins.Add(FillEntity(Reader));
                }
                return new Retorno(Logins);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Listar()
        {
            try
            {
                List<Login> Logins = new List<Login>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LOGIN.CODIGO, ");
                CommandSQL.AppendLine("TB_LOGIN.SENHA ");
                CommandSQL.AppendLine("FROM TB_LOGIN ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Logins.Add(FillEntity(Reader));
                }
                return new Retorno(Logins);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Login Entity)
        {
            try
            {
                Login Login = new Login();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LOGIN.CODIGO, ");
                CommandSQL.AppendLine("TB_LOGIN.NOME, ");
                CommandSQL.AppendLine("TB_LOGIN.USUARIO ");
                CommandSQL.AppendLine("FROM TB_LOGIN ");

                CommandSQL.AppendLine("WHERE TB_LOGIN.USUARIO = @USUARIO AND TB_LOGIN.SENHA = @SENHA ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@USUARIO", Entity.Usuario);
                Command.Parameters.AddWithValue("@SENHA", Entity.Senha);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Login = FillEntity(Reader);
                }
                return new Retorno(Login);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Login Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_LOGIN ");
                CommandSQL.AppendLine("WHERE TB_LOGIN.USUARIO = @USUARIO ");
                CommandSQL.AppendLine("AND TB_LOGIN.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@SENHA", Entity.Senha);
                Command.Parameters.AddWithValue("@USUARIO", Entity.Usuario);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Login", "Senha"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Login FillEntity(IDataReader reader)
        {
            Login Login = new Login();
            try
            {
                Login.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                Login.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                Login.Usuario = ConverterValorReader(reader, "USUARIO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Login;
        }

        #endregion

    }
}

