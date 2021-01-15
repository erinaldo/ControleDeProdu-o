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

    public class DataTipoFase : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoFase Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_FASE( ");
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

        public Retorno Excluir(TipoFase Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_FASE WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoFase Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_FASE SET ");
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

        public Retorno ConsultarProximoTipoFase(int codigoPedido)
        {
            try
            {
                TipoFase TipoFase = new TipoFase();
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FASE ");
                CommandSQL.AppendLine("WHERE ORDEM = ");
                CommandSQL.AppendLine("(SELECT MAX(ORDEM) + 1 ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_TIPO_FASE PTF ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PTF.CODIGO_TIPO_FASE ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO = @CODIGO_PEDIDO) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFase = FillEntity(Reader);
                }
                return new Retorno(TipoFase);
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
                List<TipoFase> TipoFases = new List<TipoFase>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FASE ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFases.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFases);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoFase Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFase> TipoFases = new List<TipoFase>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FASE ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_FASE.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFases.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFases);
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
                List<TipoFase> TipoFases = new List<TipoFase>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FASE ");

                CommandSQL.AppendLine("ORDER BY ORDEM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFases.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFases);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoFase Entity)
        {
            try
            {
                TipoFase TipoFase = new TipoFase();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.ORDEM ");
                CommandSQL.AppendLine("FROM TB_TIPO_FASE ");

                CommandSQL.AppendLine("WHERE TB_TIPO_FASE.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFase = FillEntity(Reader);
                }
                return new Retorno(TipoFase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoFase Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FASE ");
                CommandSQL.AppendLine("WHERE TB_TIPO_FASE.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_FASE.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFase", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoFase FillEntity(IDataReader reader)
        {
            TipoFase TipoFase = new TipoFase();
            try
            {
                TipoFase.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoFase.Ordem = ConverterValorReader(reader, "ORDEM", 0);
                TipoFase.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return TipoFase;
        }

        #endregion

    }
}

