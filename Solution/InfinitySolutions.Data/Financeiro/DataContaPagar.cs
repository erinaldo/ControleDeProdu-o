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

    public class DataContaPagar : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(ContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_CONTA_PAGAR( ");
                CommandSQL.AppendLine("DATA_EMISSAO, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DATA_EMISSAO, ");
                CommandSQL.AppendLine("@DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());

                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@QUANTIDADE_PARCELAS", Entity.QuantidadeParcelas);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO_FORNECEDOR", Entity.Fornecedor.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_STATUS_CONTA_PAGAR", Entity.TipoStatusContaPagar.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_CONTA_PAGAR", Entity.TipoContaPagar.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR", Entity.TipoFormaPagamentoContaPagar.Codigo);
                Abrir();
                Entity.Codigo = Command.ExecuteScalar().ConverteValor(0);
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(ContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_CONTA_PAGAR WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(ContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_CONTA_PAGAR SET ");
                CommandSQL.AppendLine("DATA_EMISSAO = @DATA_EMISSAO, ");
                CommandSQL.AppendLine("DATA_VENCIMENTO = @DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("QUANTIDADE_PARCELAS = @QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO, ");
                CommandSQL.AppendLine("CODIGO_FORNECEDOR = @CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_STATUS_CONTA_PAGAR = @CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_CONTA_PAGAR = @CODIGO_TIPO_CONTA_PAGAR ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@QUANTIDADE_PARCELAS", Entity.QuantidadeParcelas);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO_FORNECEDOR", Entity.Fornecedor.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_STATUS_CONTA_PAGAR", Entity.TipoStatusContaPagar.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_CONTA_PAGAR", Entity.TipoContaPagar.Codigo);
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
                List<ContaPagar> ContaPagars = new List<ContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR = TB_FORNECEDOR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR = TB_TIPO_STATUS_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR = TB_TIPO_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR = TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(ContaPagars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(ContaPagar Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<ContaPagar> ContaPagars = new List<ContaPagar>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");
                CommandSQL.AppendLine("FROM TB_CONTA_PAGAR ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR = TB_FORNECEDOR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR = TB_TIPO_STATUS_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR = TB_TIPO_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR = TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_CONTA_PAGAR.DATA_EMISSAO LIKE '%" + Entity.DataEmissao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(ContaPagars);
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
                List<ContaPagar> ContaPagars = new List<ContaPagar>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");
                CommandSQL.AppendLine("FROM TB_CONTA_PAGAR ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR = TB_FORNECEDOR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR = TB_TIPO_STATUS_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR = TB_TIPO_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR = TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaPagars.Add(FillEntity(Reader));
                }
                return new Retorno(ContaPagars);
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
                List<ContaPagar> contasPagar = new List<ContaPagar>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR, ");
                CommandSQL.AppendLine("UPPER(TB_CONTA_PAGAR.DESCRICAO) AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR ");

                CommandSQL.AppendLine("FROM TB_CONTA_PAGAR ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON TB_FORNECEDOR.CODIGO = TB_CONTA_PAGAR.CODIGO_FORNECEDOR ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON TB_TIPO_CONTA_PAGAR.CODIGO = TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON TB_TIPO_STATUS_CONTA_PAGAR.CODIGO = TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR ");

                CommandSQL.AppendLine("WHERE (DATA_VENCIMENTO BETWEEN @DATA_INICIO AND @DATA_FIM)");

                CommandSQL.AppendLine("ORDER BY DATA_VENCIMENTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_INICIO", dataInicio);
                Command.Parameters.AddWithValue("@DATA_FIM", dataFim);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_CONTA_FIXA", (int)EnumTipoContaPagar.FIXA);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    contasPagar.Add(FillEntity(Reader));
                }
                return new Retorno(contasPagar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(ContaPagar Entity)
        {
            try
            {
                ContaPagar ContaPagar = new ContaPagar();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CONTEM_PARCELA ");
                //CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ");
                CommandSQL.AppendLine("FROM TB_CONTA_PAGAR ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR = TB_FORNECEDOR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR = TB_TIPO_STATUS_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR = TB_TIPO_CONTA_PAGAR.CODIGO ");
                //CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ON ");
                //CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR = TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_CONTA_PAGAR.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    ContaPagar = FillEntity(Reader);
                }
                return new Retorno(ContaPagar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(ContaPagar Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_CONTA_PAGAR ");
                CommandSQL.AppendLine("WHERE TB_CONTA_PAGAR.DATA_EMISSAO = @DATA_EMISSAO ");
                CommandSQL.AppendLine("AND TB_CONTA_PAGAR.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_EMISSAO", Entity.DataEmissao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "ContaPagar", "DataEmissao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private ContaPagar FillEntity(IDataReader reader)
        {
            ContaPagar ContaPagar = new ContaPagar();
            try
            {
                ContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_CONTA_PAGAR", 0);
                ContaPagar.DataEmissao = ConverterValorReader(reader, "DATA_EMISSAO", DateTime.MinValue);
                ContaPagar.DataVencimento = ConverterValorReader(reader, "DATA_VENCIMENTO", DateTime.MinValue);
                ContaPagar.Valor = ConverterValorReader(reader, "VALOR", 0M);
                ContaPagar.QuantidadeParcelas = ConverterValorReader(reader, "QUANTIDADE_PARCELAS", 0);
                ContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO_CONTA_PAGAR", String.Empty);
                ContaPagar.Fornecedor.Codigo = ConverterValorReader(reader, "CODIGO_FORNECEDOR", 0);
                ContaPagar.Fornecedor.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                ContaPagar.TipoStatusContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_STATUS_CONTA_PAGAR", 0);
                ContaPagar.TipoStatusContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_STATUS_CONTA_PAGAR", String.Empty);
                ContaPagar.TipoContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_CONTA_PAGAR", 0);
                ContaPagar.TipoContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_CONTA_PAGAR", String.Empty);
                ContaPagar.TipoContaPagar.ContemParcelas = ConverterValorReader(reader, "CONTEM_PARCELA", false);
                ContaPagar.TipoFormaPagamentoContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR", 0);
                ContaPagar.TipoFormaPagamentoContaPagar.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return ContaPagar;
        }

        #endregion

    }
}

