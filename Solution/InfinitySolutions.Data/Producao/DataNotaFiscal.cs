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

    public class DataNotaFiscal : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(NotaFiscal Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_NOTA_FISCAL( ");
                CommandSQL.AppendLine("CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("OBSERVACAO, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_PAGAMENTO, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("@OBSERVACAO, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_PAGAMENTO, ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_NOTA_FISCAL", Entity.CodigoNotaFiscal);
                Command.Parameters.AddWithValue("@OBSERVACAO", Entity.Observacao);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", Entity.DataPagamento);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", Entity.Pedido.Codigo);
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

        public Retorno Excluir(NotaFiscal Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_NOTA_FISCAL WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(NotaFiscal Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_NOTA_FISCAL SET ");
                CommandSQL.AppendLine("OBSERVACAO = @OBSERVACAO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@OBSERVACAO", Entity.Observacao);
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
                List<NotaFiscal> NotaFiscals = new List<NotaFiscal>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO AS CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.NUMERO, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.OBSERVACAO AS OBSERVACAO_NOTA_FISCAL ");
                CommandSQL.AppendLine("FROM TB_NOTA_FISCAL ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    NotaFiscals.Add(FillEntity(Reader));
                }
                return new Retorno(NotaFiscals);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(NotaFiscal Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<NotaFiscal> NotaFiscals = new List<NotaFiscal>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO AS CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.NUMERO, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.OBSERVACAO AS OBSERVACAO_NOTA_FISCAL, ");

                CommandSQL.AppendLine("FROM TB_NOTA_FISCAL ");

                CommandSQL.AppendLine("WHERE (TB_NOTA_FISCAL.NUMERO LIKE '%" + Entity.Numero + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    NotaFiscals.Add(FillEntity(Reader));
                }
                return new Retorno(NotaFiscals);
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
                List<NotaFiscal> NotaFiscals = new List<NotaFiscal>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.OBSERVACAO AS OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.VALOR, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.DATA_PAGAMENTO ");

                CommandSQL.AppendLine("FROM TB_NOTA_FISCAL ");

                CommandSQL.AppendLine("ORDER BY DATA_PAGAMENTO DESC ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    NotaFiscals.Add(FillEntity(Reader));
                }
                return new Retorno(NotaFiscals);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(NotaFiscal Entity)
        {
            try
            {
                NotaFiscal NotaFiscal = new NotaFiscal();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO AS CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.NUMERO, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.CODIGO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_NOTA_FISCAL.OBSERVACAO AS OBSERVACAO_NOTA_FISCAL ");

                CommandSQL.AppendLine("FROM TB_NOTA_FISCAL ");

                CommandSQL.AppendLine("WHERE TB_NOTA_FISCAL.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    NotaFiscal = FillEntity(Reader);
                }
                return new Retorno(NotaFiscal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(NotaFiscal Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_NOTA_FISCAL ");
                CommandSQL.AppendLine("WHERE TB_NOTA_FISCAL.NUMERO = @NUMERO ");
                CommandSQL.AppendLine("AND TB_NOTA_FISCAL.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NUMERO", Entity.Numero);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "NotaFiscal", "Numero"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private NotaFiscal FillEntity(IDataReader reader)
        {
            NotaFiscal NotaFiscal = new NotaFiscal();
            try
            {
                NotaFiscal.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                NotaFiscal.CodigoNotaFiscal = ConverterValorReader(reader, "CODIGO_NOTA_FISCAL", String.Empty);
                NotaFiscal.Observacao = ConverterValorReader(reader, "OBSERVACAO_NOTA_FISCAL", String.Empty);
                NotaFiscal.Pedido.Codigo = ConverterValorReader(reader, "CODIGO_PEDIDO", 0);
                NotaFiscal.Valor = ConverterValorReader(reader, "VALOR", 0M);
                NotaFiscal.DataPagamento = ConverterValorReader(reader, "DATA_PAGAMENTO", DateTime.MinValue);
            }
            catch (Exception ex) { throw ex; }
            return NotaFiscal;
        }

        #endregion

    }
}

