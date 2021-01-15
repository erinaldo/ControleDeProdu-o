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

    public class DataTipoPermissao : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoPermissao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_PERMISSAO( ");
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

        public Retorno Excluir(TipoPermissao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_PERMISSAO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoPermissao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_PERMISSAO SET ");
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

        public Retorno Consultar(int codigoLogin)
        {
            try
            {
                var tipoPermissaos = new List<TipoPermissao>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("P.CODIGO, ");
                CommandSQL.AppendLine("P.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LOGIN_TIPO_PERMISSAO LP ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_PERMISSAO P ON P.CODIGO = LP.CODIGO_TIPO_PERMISSAO ");

                CommandSQL.AppendLine("WHERE LP.CODIGO_LOGIN = @CODIGO_LOGIN ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_LOGIN", codigoLogin);
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    tipoPermissaos.Add(FillEntity(Reader));
                }
                return new Retorno(tipoPermissaos);
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
                List<TipoPermissao> TipoPermissaos = new List<TipoPermissao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_PERMISSAO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoPermissaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoPermissaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoPermissao Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoPermissao> TipoPermissaos = new List<TipoPermissao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_PERMISSAO ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_PERMISSAO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoPermissaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoPermissaos);
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
                List<TipoPermissao> TipoPermissaos = new List<TipoPermissao>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_PERMISSAO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoPermissaos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoPermissaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoPermissao Entity)
        {
            try
            {
                TipoPermissao TipoPermissao = new TipoPermissao();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_PERMISSAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_PERMISSAO ");

                CommandSQL.AppendLine("WHERE TB_TIPO_PERMISSAO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoPermissao = FillEntity(Reader);
                }
                return new Retorno(TipoPermissao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoPermissao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_PERMISSAO ");
                CommandSQL.AppendLine("WHERE TB_TIPO_PERMISSAO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_PERMISSAO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoPermissao", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoPermissao FillEntity(IDataReader reader)
        {
            TipoPermissao TipoPermissao = new TipoPermissao();
            try
            {
                TipoPermissao.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoPermissao.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return TipoPermissao;
        }

        #endregion

    }
}

