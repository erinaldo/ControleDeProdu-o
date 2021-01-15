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

    public class DataParcela : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Parcela Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PARCELA( ");
                CommandSQL.AppendLine("DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("NUMERO, ");
                CommandSQL.AppendLine("CODIGO_STATUS, ");
                CommandSQL.AppendLine("CODIGO_CONTA_PAGAR) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@NUMERO, ");
                CommandSQL.AppendLine("@CODIGO_STATUS, ");
                CommandSQL.AppendLine("@CODIGO_CONTA_PAGAR) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@NUMERO", Entity.Numero);
                Command.Parameters.AddWithValue("@CODIGO_STATUS", Entity.Status.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", Entity.ContaPagar.Codigo);
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

        public Retorno Excluir(Parcela Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PARCELA WHERE CODIGO = @CODIGO");
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



        public Retorno Excluir(int codigoContaPagar)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PARCELA WHERE CODIGO_CONTA_PAGAR = @CODIGO_CONTA_PAGAR ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", codigoContaPagar);
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

        public Retorno Alterar(Parcela Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PARCELA SET ");
                CommandSQL.AppendLine("DATA_VENCIMENTO = @DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("NUMERO = @NUMERO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DATA_VENCIMENTO", Entity.DataVencimento);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@NUMERO", Entity.Numero);
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
                List<Parcela> Parcelas = new List<Parcela>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO AS CODIGO_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PARCELA.VALOR AS VALOR_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.NUMERO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO AS CODIGO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO AS DESCRICAO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR AS VALOR_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_PARCELA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_PARCELA ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_TIPO_STATUS_PARCELA = TB_TIPO_STATUS_PARCELA.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_CONTA_PAGAR = TB_CONTA_PAGAR.CODIGO ");
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
                    Parcelas.Add(FillEntity(Reader));
                }
                return new Retorno(Parcelas);
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
                List<Parcela> Parcelas = new List<Parcela>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO AS CODIGO_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PARCELA.VALOR AS VALOR_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.NUMERO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO AS CODIGO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO AS DESCRICAO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR AS VALOR_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_PARCELA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_PARCELA ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_TIPO_STATUS_PARCELA = TB_TIPO_STATUS_PARCELA.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_CONTA_PAGAR = TB_CONTA_PAGAR.CODIGO ");
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
                    Parcelas.Add(FillEntity(Reader));
                }
                return new Retorno(Parcelas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Parcela Entity)
        {
            try
            {
                Parcela Parcela = new Parcela();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO AS CODIGO_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PARCELA.VALOR AS VALOR_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.NUMERO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO AS CODIGO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO AS DESCRICAO_TIPO_STATUS_PARCELA, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO AS CODIGO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DESCRICAO AS DESCRICAO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_EMISSAO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.VALOR AS VALOR_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.QUANTIDADE_PARCELAS, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO AS CODIGO_FORNECEDOR, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_STATUS_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CONTEM_PARCELAS ");
                CommandSQL.AppendLine("FROM TB_PARCELA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_PARCELA ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_TIPO_STATUS_PARCELA = TB_TIPO_STATUS_PARCELA.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO_CONTA_PAGAR = TB_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_FORNECEDOR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_FORNECEDOR = TB_FORNECEDOR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_STATUS_CONTA_PAGAR = TB_TIPO_STATUS_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_CONTA_PAGAR = TB_TIPO_CONTA_PAGAR.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR ON ");
                CommandSQL.AppendLine("TB_CONTA_PAGAR.CODIGO_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR = TB_TIPO_FORMA_PAGAMENTO_CONTA_PAGAR.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_PARCELA.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Parcela = FillEntity(Reader);
                }
                return new Retorno(Parcela);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(int codigoContaPagar, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var parcelas = new List<Parcela>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PARCELA.CODIGO AS CODIGO_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.DATA_VENCIMENTO, ");
                CommandSQL.AppendLine("TB_PARCELA.VALOR AS VALOR_PARCELA, ");
                CommandSQL.AppendLine("TB_PARCELA.NUMERO ");
                CommandSQL.AppendLine("FROM TB_PARCELA ");

                CommandSQL.AppendLine("WHERE TB_PARCELA.CODIGO_CONTA_PAGAR = @CODIGO_CONTA_PAGAR AND DATA_VENCIMENTO BETWEEN @DATA_INICIO AND @DATA_FIM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_CONTA_PAGAR", codigoContaPagar);
                Command.Parameters.AddWithValue("@DATA_INICIO", dataInicio);
                Command.Parameters.AddWithValue("@DATA_FIM", dataFim);
                Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    parcelas.Add(new Parcela
                    {
                        Codigo = ConverterValorReader(Reader, "CODIGO_PARCELA", 0),
                        DataVencimento = ConverterValorReader(Reader, "DATA_VENCIMENTO", DateTime.MinValue),
                        Valor = ConverterValorReader(Reader, "VALOR_PARCELA", 0M),
                        Numero = ConverterValorReader(Reader, "NUMERO", 0)
                    });
                }

                return new Retorno(parcelas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Parcela Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_PARCELA ");
                CommandSQL.AppendLine("AND TB_PARCELA.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Parcela", "DataEmissao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Parcela FillEntity(IDataReader reader)
        {
            Parcela Parcela = new Parcela();
            try
            {
                Parcela.Codigo = ConverterValorReader(reader, "CODIGO_PARCELA", 0);
                Parcela.DataVencimento = ConverterValorReader(reader, "DATA_VENCIMENTO", DateTime.MinValue);
                Parcela.Valor = ConverterValorReader(reader, "VALOR_PARCELA", 0M);
                Parcela.Numero = ConverterValorReader(reader, "NUMERO", 0);
                Parcela.Status.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_STATUS_PARCELA", 0);
                Parcela.Status.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_STATUS_PARCELA", String.Empty);
                Parcela.ContaPagar.Codigo = ConverterValorReader(reader, "CODIGO_CONTA_PAGAR", 0);
            }
            catch (Exception ex) { throw ex; }
            return Parcela;
        }

        #endregion

    }
}

