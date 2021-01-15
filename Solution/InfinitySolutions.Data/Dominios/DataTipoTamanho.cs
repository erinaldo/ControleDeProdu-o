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

    public class DataTipoTamanho : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoTamanho Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_TAMANHO( ");
                CommandSQL.AppendLine("DESCRICAO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
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

        public Retorno Excluir(TipoTamanho Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_TAMANHO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoTamanho Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_TAMANHO SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
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
                List<TipoTamanho> TipoTamanhos = new List<TipoTamanho>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_TAMANHO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoTamanhos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoTamanhos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoTamanho Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoTamanho> TipoTamanhos = new List<TipoTamanho>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_TAMANHO ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_TAMANHO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoTamanhos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoTamanhos);
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
                List<TipoTamanho> TipoTamanhos = new List<TipoTamanho>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_TAMANHO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoTamanhos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoTamanhos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoTamanho Entity)
        {
            try
            {
                TipoTamanho TipoTamanho = new TipoTamanho();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_TAMANHO ");

                CommandSQL.AppendLine("WHERE TB_TIPO_TAMANHO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoTamanho = FillEntity(Reader);
                }
                return new Retorno(TipoTamanho);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoTamanho Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_TAMANHO ");
                CommandSQL.AppendLine("WHERE TB_TIPO_TAMANHO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_TAMANHO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoTamanho", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoTamanho FillEntity(IDataReader reader)
        {
            TipoTamanho TipoTamanho = new TipoTamanho();
            try
            {
                TipoTamanho.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoTamanho.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty).ToUpper();
            }
            catch (Exception ex) { throw ex; }
            return TipoTamanho;
        }

        #endregion

    }
}

