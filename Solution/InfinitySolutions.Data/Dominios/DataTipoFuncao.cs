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

    public class DataTipoFuncao : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoFuncao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_FUNCAO( ");
                CommandSQL.AppendLine("DESCRICAO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
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

        public Retorno Excluir(TipoFuncao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_FUNCAO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoFuncao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_FUNCAO SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
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

        #endregion

        #region CONSULTAS

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFuncao> TipoFuncaos = new List<TipoFuncao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFuncaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFuncaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoFuncao Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFuncao> TipoFuncaos = new List<TipoFuncao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_FUNCAO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFuncaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFuncaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(int codigoLogin)
        {
            try
            {
                var tipoFuncaos = new List<TipoFuncao>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LOGIN_TIPO_FUNCAO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO ON TB_TIPO_FUNCAO.CODIGO = TB_LOGIN_TIPO_FUNCAO.CODIGO_TIPO_FUNCAO ");

                CommandSQL.AppendLine("WHERE TB_LOGIN_TIPO_FUNCAO.CODIGO_LOGIN = @CODIGO_LOGIN ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_LOGIN", codigoLogin);
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    tipoFuncaos.Add(FillEntity(Reader));
                }
                return new Retorno(tipoFuncaos);
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
                List<TipoFuncao> TipoFuncaos = new List<TipoFuncao>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFuncaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFuncaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoFuncao Entity)
        {
            try
            {
                TipoFuncao TipoFuncao = new TipoFuncao();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO ");

                CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFuncao = FillEntity(Reader);
                }
                return new Retorno(TipoFuncao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoFuncao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FUNCAO ");
                CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_FUNCAO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFuncao", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoFuncao FillEntity(IDataReader reader)
        {
            TipoFuncao TipoFuncao = new TipoFuncao();
            try
            {
                TipoFuncao.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoFuncao.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return TipoFuncao;
        }

        #endregion

    }
}

