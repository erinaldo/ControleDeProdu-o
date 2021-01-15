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

    public class DataTipoFrete : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoFrete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_FRETE( ");
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

        public Retorno Excluir(TipoFrete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_FRETE WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoFrete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_FRETE SET ");
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
                List<TipoFrete> TipoFretes = new List<TipoFrete>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FRETE ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFretes.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFretes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoFrete Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFrete> TipoFretes = new List<TipoFrete>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FRETE ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_FRETE.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFretes.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFretes);
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
                List<TipoFrete> TipoFretes = new List<TipoFrete>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO, ");
                CommandSQL.AppendLine("UPPER(TB_TIPO_FRETE.DESCRICAO) AS DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FRETE ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFretes.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFretes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoFrete Entity)
        {
            try
            {
                TipoFrete TipoFrete = new TipoFrete();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FRETE ");

                CommandSQL.AppendLine("WHERE TB_TIPO_FRETE.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFrete = FillEntity(Reader);
                }
                return new Retorno(TipoFrete);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoFrete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FRETE ");
                CommandSQL.AppendLine("WHERE TB_TIPO_FRETE.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_FRETE.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFrete", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoFrete FillEntity(IDataReader reader)
        {
            TipoFrete TipoFrete = new TipoFrete();
            try
            {
                TipoFrete.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoFrete.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return TipoFrete;
        }

        #endregion

    }
}

