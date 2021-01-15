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

    public class DataPagamento : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Pagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PAGAMENTO( ");
                CommandSQL.AppendLine("CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("JUROS, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("DATA_PAGAMENTO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@JUROS, ");
                CommandSQL.AppendLine("@DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("@DATA_PAGAMENTO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", Entity.ContaPagar.Codigo);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@JUROS", Entity.Juros);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", Entity.DataPagamento);
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

        public Retorno Excluir(Pagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PAGAMENTO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Pagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PAGAMENTO SET ");
                CommandSQL.AppendLine("CODIGO_CONTA_PAGAR = @CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("JUROS = @JUROS, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO = @DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("DATA_PAGAMENTO = @DATA_PAGAMENTO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", Entity.ContaPagar.Codigo);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@JUROS", Entity.Juros);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", Entity.DataPagamento);
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
                List<Pagamento> Pagamentos = new List<Pagamento>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.VALOR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.JUROS, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PAGAMENTO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(Pagamentos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Pagamento Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Pagamento> Pagamentos = new List<Pagamento>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.VALOR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.JUROS, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PAGAMENTO ");

                CommandSQL.AppendLine("WHERE (TB_PAGAMENTO.CODIGO_CONTA_PAGAR LIKE '%" + Entity.ContaPagar.Codigo + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(Pagamentos);
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
                List<Pagamento> Pagamentos = new List<Pagamento>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.VALOR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.JUROS, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PAGAMENTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pagamentos.Add(FillEntity(Reader));
                }
                return new Retorno(Pagamentos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Pagamento Entity)
        {
            try
            {
                Pagamento Pagamento = new Pagamento();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.VALOR, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.JUROS, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PAGAMENTO.DATA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PAGAMENTO ");

                CommandSQL.AppendLine("WHERE TB_PAGAMENTO.CODIGO_CONTA_PAGAR = @CODIGO_CONTA_PAGAR ");
                CommandSQL.AppendLine("AND TB_PAGAMENTO.DATA_VENCIMENTO = @DATA_VENCIMENTO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", Entity.ContaPagar.Codigo);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pagamento = FillEntity(Reader);
                }
                return new Retorno(Pagamento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Pagamento Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_PAGAMENTO ");
                CommandSQL.AppendLine("WHERE TB_PAGAMENTO.CODIGO_CONTA_PAGAR = @CODIGO_CONTA_PAGAR ");
                CommandSQL.AppendLine("AND TB_PAGAMENTO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", Entity.ContaPagar.Codigo);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Pagamento", "CodigoConta"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Pagamento FillEntity(IDataReader reader)
        {
            Pagamento Pagamento = new Pagamento();
            try
            {
                Pagamento.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                Pagamento.ContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_CONTA_PAGAR", 0);
                Pagamento.Valor = ConverterValorReader(reader, "VALOR", 0M);
                Pagamento.Juros = ConverterValorReader(reader, "JUROS", 0M);
                Pagamento.DataVencimento = ConverterValorReader(reader, "DATA_VENCIMENTO", DateTime.MinValue);
                Pagamento.DataPagamento = ConverterValorReader(reader, "DATA_PAGAMENTO", DateTime.MinValue);
            }
            catch (Exception ex) { throw ex; }
            return Pagamento;
        }

        #endregion

    }
}

