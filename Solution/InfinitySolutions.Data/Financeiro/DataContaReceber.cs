using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using MySql.Data.MySqlClient;
using InfinitySolutions.Entity.Enum;

namespace InfinitySolutions.Data
{

    public class DataContaReceber : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(ContaReceber Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_CONTA_RECEBER( ");
                CommandSQL.AppendLine("CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("NOTA_FISCAL, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("DATA_EMISSAO, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("PEDIDO_EXTERNO, ");
                CommandSQL.AppendLine("NOTA_FISCAL_EXTERNA, ");
                CommandSQL.AppendLine("CODIGO_STATUS, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("@NOTA_FISCAL, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("@DATA_EMISSAO, ");
                CommandSQL.AppendLine("@DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("@PEDIDO_EXTERNO, ");
                CommandSQL.AppendLine("@NOTA_FISCAL_EXTERNA, ");
                CommandSQL.AppendLine("@CODIGO_STATUS, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER) ");

                Command = CriaComandoSQL(CommandSQL.ToString());

                Command.Parameters.AddWithValue("@CODIGO_CLIENTE", Entity.Cliente.Codigo);
                Command.Parameters.AddWithValue("@NOTA_FISCAL", Entity.NotaFiscal);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", Entity.Pedido.Codigo);
                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@PEDIDO_EXTERNO", Entity.PedidoExterno);
                Command.Parameters.AddWithValue("@NOTA_FISCAL_EXTERNA", Entity.NotaFiscalExterna);
                Command.Parameters.AddWithValue("@CODIGO_STATUS", (int)EnumContaReceberStatus.EM_ABERTO);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER", Entity.TipoFormaPagamentoContaReceber.Codigo);
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

        public Retorno Excluir(ContaReceber Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_CONTA_RECEBER WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(ContaReceber Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_CONTA_RECEBER SET ");
                CommandSQL.AppendLine("CODIGO_CLIENTE = @CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("NOTA_FISCAL = @NOTA_FISCAL, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO = @CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("DATA_EMISSAO = @DATA_EMISSAO, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO = @DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("PEDIDO_EXTERNO = @PEDIDO_EXTERNO, ");
                CommandSQL.AppendLine("NOTA_FISCAL_EXTERNA = @NOTA_FISCAL_EXTERNA, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER = @CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_CLIENTE", Entity.Cliente.Codigo);
                Command.Parameters.AddWithValue("@NOTA_FISCAL", Entity.NotaFiscal);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", Entity.Pedido.Codigo);
                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@PEDIDO_EXTERNO", Entity.PedidoExterno);
                Command.Parameters.AddWithValue("@NOTA_FISCAL_EXTERNA", Entity.NotaFiscalExterna);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER", Entity.TipoFormaPagamentoContaReceber.Codigo);
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

        public Retorno DarBaixaPagamento(int codigo)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_CONTA_RECEBER SET ");
                CommandSQL.AppendLine("CODIGO_STATUS = @CODIGO_STATUS ");

                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", codigo);
                Command.Parameters.AddWithValue("@CODIGO_STATUS", (int)EnumContaReceberStatus.RECEBIDA);
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
                List<ContaReceber> ContaRecebers = new List<ContaReceber>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CLIENTE, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.PEDIDO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.STATUS ");
                CommandSQL.AppendLine("FROM TB_CONTA_RECEBER ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaRecebers.Add(FillEntity(Reader));
                }
                return new Retorno(ContaRecebers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(ContaReceber Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<ContaReceber> ContaRecebers = new List<ContaReceber>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CLIENTE, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.PEDIDO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.STATUS ");
                CommandSQL.AppendLine("FROM TB_CONTA_RECEBER ");

                CommandSQL.AppendLine("WHERE (TB_CONTA_RECEBER.CLIENTE LIKE '%" + Entity.Cliente + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaRecebers.Add(FillEntity(Reader));
                }
                return new Retorno(ContaRecebers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Listar(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                List<ContaReceber> ContaRecebers = new List<ContaReceber>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_STATUS, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME ");
                CommandSQL.AppendLine("FROM TB_CONTA_RECEBER ");
                CommandSQL.AppendLine("INNER JOIN TB_CLIENTE ON TB_CLIENTE.CODIGO = TB_CONTA_RECEBER.CODIGO_CLIENTE ");

                CommandSQL.AppendLine("WHERE DATA_VENCIMENTO BETWEEN @DATA_INICIO AND @DATA_FIM ");

                CommandSQL.AppendLine("ORDER BY DATA_VENCIMENTO DESC ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_INICIO", dataInicio);
                Command.Parameters.AddWithValue("@DATA_FIM", dataFim);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaRecebers.Add(FillEntity(Reader));
                }
                return new Retorno(ContaRecebers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(ContaReceber Entity)
        {
            try
            {
                ContaReceber ContaReceber = new ContaReceber();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.PEDIDO_EXTERNO, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.NOTA_FISCAL_EXTERNA, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_STATUS, ");
                CommandSQL.AppendLine("TB_CONTA_RECEBER.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");
                CommandSQL.AppendLine("FROM TB_CONTA_RECEBER ");

                CommandSQL.AppendLine("WHERE TB_CONTA_RECEBER.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaReceber = FillEntity(Reader);
                }
                return new Retorno(ContaReceber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(ContaReceber Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_CONTA_RECEBER ");
                CommandSQL.AppendLine("WHERE TB_CONTA_RECEBER.CODIGO_CLIENTE = @CODIGO_CLIENTE ");
                CommandSQL.AppendLine("AND TB_CONTA_RECEBER.VALOR = @VALOR ");
                CommandSQL.AppendLine("AND DATE(TB_CONTA_RECEBER.DATA_EMISSAO) = @DATA_EMISSAO ");
                CommandSQL.AppendLine("AND DATE(TB_CONTA_RECEBER.DATA_VENCIMENTO) = @DATA_VENCIMENTO ");
                CommandSQL.AppendLine("AND TB_CONTA_RECEBER.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_CLIENTE", Entity.Cliente.Codigo);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao.Value);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, Mensagens.MSG_35);
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private ContaReceber FillEntity(IDataReader reader)
        {
            ContaReceber ContaReceber = new ContaReceber();
            try
            {
                ContaReceber.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                ContaReceber.Cliente.Codigo = ConverterValorReader(reader, "CODIGO_CLIENTE", 0);
                ContaReceber.Cliente.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                ContaReceber.NotaFiscal = ConverterValorReader(reader, "NOTA_FISCAL", String.Empty);
                ContaReceber.Valor = ConverterValorReader(reader, "VALOR", 0M);
                ContaReceber.Pedido.Codigo = ConverterValorReader(reader, "CODIGO_PEDIDO", 0);
                ContaReceber.DataEmissao = ConverterValorReader(reader, "DATA_EMISSAO", DateTime.MinValue);
                ContaReceber.DataVencimento = ConverterValorReader(reader, "DATA_VENCIMENTO", DateTime.MinValue);
                ContaReceber.PedidoExterno = ConverterValorReader(reader, "PEDIDO_EXTERNO", String.Empty);
                ContaReceber.NotaFiscalExterna = ConverterValorReader(reader, "NOTA_FISCAL_EXTERNA", String.Empty);
                ContaReceber.Status = (EnumContaReceberStatus)ConverterValorReader(reader, "CODIGO_STATUS", 0);
                ContaReceber.TipoFormaPagamentoContaReceber.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER", 0);
            }
            catch (Exception ex) { throw ex; }
            return ContaReceber;
        }

        #endregion

    }
}

