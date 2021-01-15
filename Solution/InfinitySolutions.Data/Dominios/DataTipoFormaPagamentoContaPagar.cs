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

    public class DataTipoFormaPagamentoContaPagar : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR( ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("CONTEM_PARCELAS) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@CONTEM_PARCELAS) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CONTEM_PARCELAS", Entity.ContemParcelas);
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

        public Retorno Excluir(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO, ");
                CommandSQL.AppendLine("CONTEM_PARCELAS = @CONTEM_PARCELAS ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CONTEM_PARCELAS", Entity.ContemParcelas);
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
                List<TipoFormaPagamentoContaPagar> TipoFormaPagamentoContaPagars = new List<TipoFormaPagamentoContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentoContaPagars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoFormaPagamentoContaPagar Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFormaPagamentoContaPagar> TipoFormaPagamentoContaPagars = new List<TipoFormaPagamentoContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentoContaPagars);
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
                List<TipoFormaPagamentoContaPagar> TipoFormaPagamentoContaPagars = new List<TipoFormaPagamentoContaPagar>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentoContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentoContaPagars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                TipoFormaPagamentoContaPagar TipoFormaPagamentoContaPagar = new TipoFormaPagamentoContaPagar();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");

                CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentoContaPagar = FillEntity(Reader);
                }
                return new Retorno(TipoFormaPagamentoContaPagar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoFormaPagamentoContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");
                CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFormaPagamentoContaPagar", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoFormaPagamentoContaPagar FillEntity(IDataReader reader)
        {
            TipoFormaPagamentoContaPagar TipoFormaPagamentoContaPagar = new TipoFormaPagamentoContaPagar();
            try
            {
                TipoFormaPagamentoContaPagar.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoFormaPagamentoContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
                TipoFormaPagamentoContaPagar.ContemParcelas = ConverterValorReader(reader, "CONTEM_PARCELAS", false);
            }
            catch (Exception ex) { throw ex; }
            return TipoFormaPagamentoContaPagar;
        }

        #endregion

    }
}

