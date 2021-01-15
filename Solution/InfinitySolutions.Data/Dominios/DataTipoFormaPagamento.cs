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

    public class DataTipoFormaPagamento : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoFormaPagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_FORMA_PAGAMENTO( ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("CONTEM_PARCELAS, ");
                CommandSQL.AppendLine("DIAS) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@CONTEM_PARCELAS, ");
                CommandSQL.AppendLine("@DIAS) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CONTEM_PARCELAS", Entity.ContemParcelas);
                Command.Parameters.AddWithValue("@DIAS", Entity.Dias);
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

        public Retorno Excluir(TipoFormaPagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_FORMA_PAGAMENTO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoFormaPagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_FORMA_PAGAMENTO SET ");
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
                List<TipoFormaPagamento> TipoFormaPagamentos = new List<TipoFormaPagamento>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoFormaPagamento Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoFormaPagamento> TipoFormaPagamentos = new List<TipoFormaPagamento>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_FORMA_PAGAMENTO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentos);
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
                List<TipoFormaPagamento> TipoFormaPagamentos = new List<TipoFormaPagamento>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DIAS ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(TipoFormaPagamentos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoFormaPagamento Entity)
        {
            try
            {
                TipoFormaPagamento TipoFormaPagamento = new TipoFormaPagamento();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CONTEM_PARCELAS, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DATA_COMBINADA ");
                CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO ");

                CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoFormaPagamento = FillEntity(Reader);
                }
                return new Retorno(TipoFormaPagamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoFormaPagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FORMA_PAGAMENTO ");
                CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_FORMA_PAGAMENTO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFormaPagamento", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoFormaPagamento FillEntity(IDataReader reader)
        {
            TipoFormaPagamento TipoFormaPagamento = new TipoFormaPagamento();
            try
            {
                TipoFormaPagamento.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoFormaPagamento.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty).ToUpper();
                TipoFormaPagamento.ContemParcelas = ConverterValorReader(reader, "CONTEM_PARCELAS", false);
                TipoFormaPagamento.Dias = ConverterValorReader(reader, "DIAS", 0);
                TipoFormaPagamento.DataCombinada = ConverterValorReader(reader, "DATA_COMBINADA", false);
            }
            catch (Exception ex) { throw ex; }
            return TipoFormaPagamento;
        }

        #endregion

    }
}

