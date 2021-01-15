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

    public class DataTipoContaPagar : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_CONTA_PAGAR( ");
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

        public Retorno Excluir(TipoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_CONTA_PAGAR WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_CONTA_PAGAR SET ");
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
                List<TipoContaPagar> TipoContaPagars = new List<TipoContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_CONTA_PAGAR ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoContaPagars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoContaPagar Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoContaPagar> TipoContaPagars = new List<TipoContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_CONTA_PAGAR ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_CONTA_PAGAR.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoContaPagars);
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
                List<TipoContaPagar> TipoContaPagars = new List<TipoContaPagar>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_CONTA_PAGAR ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoContaPagars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoContaPagar Entity)
        {
            try
            {
                TipoContaPagar TipoContaPagar = new TipoContaPagar();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CONTEM_PARCELA ");
                CommandSQL.AppendLine("FROM TB_TIPO_CONTA_PAGAR ");

                CommandSQL.AppendLine("WHERE TB_TIPO_CONTA_PAGAR.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoContaPagar = FillEntity(Reader);
                }
                return new Retorno(TipoContaPagar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_CONTA_PAGAR ");
                CommandSQL.AppendLine("WHERE TB_TIPO_CONTA_PAGAR.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_CONTA_PAGAR.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoContaPagar", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoContaPagar FillEntity(IDataReader reader)
        {
            TipoContaPagar TipoContaPagar = new TipoContaPagar();
            try
            {
                TipoContaPagar.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
                TipoContaPagar.ContemParcelas = ConverterValorReader(reader, "CONTEM_PARCELA", false);
            }
            catch (Exception ex) { throw ex; }
            return TipoContaPagar;
        }

        #endregion

    }
}

