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

    public class DataLinhaProducao : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(LinhaProducao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_LINHA_PRODUCAO( ");
                CommandSQL.AppendLine("DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("QUANTIDADE, ");
                CommandSQL.AppendLine("CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("CODIGO_TERCEIRIZADO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("@DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("@QUANTIDADE, ");
                CommandSQL.AppendLine("@CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("@CODIGO_TERCEIRIZADO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_PREVISAO_INICIO", Entity.DataPrevisaoInicio);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_FIM", Entity.DataPrevisaoFim);
                Command.Parameters.AddWithValue("@QUANTIDADE", Entity.Quantidade);
                Command.Parameters.AddWithValue("@CODIGO_FUNCIONARIO", Entity.Funcionario.Codigo == 0 ? DBNull.Value : (object)Entity.Funcionario.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_STATUS_LINHA_PRODUCAO", (int)EnumTipoStatusLinhaProducao.EM_PRODUCAO);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", Entity.Produto.CodigoPedidoProduto);
                Command.Parameters.AddWithValue("@CODIGO_TERCEIRIZADO", Entity.Terceirizado.Codigo == 0 ? DBNull.Value : (object)Entity.Terceirizado.Codigo);
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

        public Retorno Terceirizar(LinhaProducao linhaProducao)
        {
            try
            {
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("DELETE FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO_PRODUTO = @CODIGO_PEDIDO_PRODUTO");
                CommandSQL.AppendLine("AND CODIGO <> @CODIGO; ");

                CommandSQL.AppendLine("UPDATE TB_LINHA_PRODUCAO SET ");
                CommandSQL.AppendLine("DATA_PREVISAO_INICIO = @DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("DATA_PREVISAO_FIM = @DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("CODIGO_TERCEIRIZADO = @CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("CODIGO_FUNCIONARIO = @CODIGO_FUNCIONARIO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_PREVISAO_INICIO", linhaProducao.DataPrevisaoInicio);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_FIM", linhaProducao.DataPrevisaoFim);
                Command.Parameters.AddWithValue("@CODIGO_TERCEIRIZADO", linhaProducao.Terceirizado.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_FUNCIONARIO", DBNull.Value);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", linhaProducao.Produto.CodigoPedidoProduto);
                Command.Parameters.AddWithValue("@CODIGO", linhaProducao.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Produção Terceirizada"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(LinhaProducao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_LINHA_PRODUCAO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(LinhaProducao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_LINHA_PRODUCAO SET ");
                CommandSQL.AppendLine("DATA_PREVISAO_INICIO = @DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("DATA_PREVISAO_FIM = @DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("DATA_REAL_INICIO = @DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("DATA_REAL_FIM = @DATA_REAL_FIM, ");
                CommandSQL.AppendLine("QUANTIDADE = @QUANTIDADE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_INICIO", Entity.DataPrevisaoInicio);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_FIM", Entity.DataPrevisaoFim);
                Command.Parameters.AddWithValue("@DATA_REAL_INICIO", Entity.DataRealInicio);
                Command.Parameters.AddWithValue("@DATA_REAL_FIM", Entity.DataRealFim);
                Command.Parameters.AddWithValue("@QUANTIDADE", Entity.Quantidade);
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

        public Retorno ConfirmarProducao(LinhaProducao linhaProducao)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_LINHA_PRODUCAO SET ");
                CommandSQL.AppendLine("CODIGO_TIPO_STATUS_LINHA_PRODUCAO = @CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("DATA_REAL_FIM = @DATA_REAL_FIM ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO_PRODUTO = @CODIGO_PEDIDO_PRODUTO");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_TIPO_STATUS_LINHA_PRODUCAO", (int)EnumTipoStatusLinhaProducao.PRODUZIDO);
                Command.Parameters.AddWithValue("@DATA_REAL_FIM", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", linhaProducao.Produto.CodigoPedidoProduto);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Produção Confirmada"));
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
                List<LinhaProducao> LinhaProducaos = new List<LinhaProducao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO AS CODIGO_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO AS CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("INNER JOIN TB_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = TB_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_LINHA_PRODUCAO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TIPO_STATUS_LINHA_PRODUCAO = TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    LinhaProducaos.Add(FillEntity(Reader));
                }
                return new Retorno(LinhaProducaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(LinhaProducao Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<LinhaProducao> LinhaProducaos = new List<LinhaProducao>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO AS CODIGO_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO AS CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("INNER JOIN TB_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = TB_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_LINHA_PRODUCAO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TIPO_STATUS_LINHA_PRODUCAO = TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO LIKE '%" + Entity.DataPrevisaoInicio + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    LinhaProducaos.Add(FillEntity(Reader));
                }
                return new Retorno(LinhaProducaos);
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
                List<LinhaProducao> LinhaProducaos = new List<LinhaProducao>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO AS CODIGO_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_FIM, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_PRODUTO.DESCRICAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO AS CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.NOME AS NOME_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO AS CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO AS DESCRICAO_TIPO_STATUS_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("FROM TB_LINHA_PRODUCAO ");

                CommandSQL.AppendLine("LEFT JOIN TB_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = TB_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("LEFT JOIN TB_TERCEIRIZADO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TERCEIRIZADO = TB_TERCEIRIZADO.CODIGO ");

                CommandSQL.AppendLine("LEFT JOIN TB_PEDIDO_PRODUTO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_PEDIDO_PRODUTO = TB_PEDIDO_PRODUTO.CODIGO ");
                CommandSQL.AppendLine("LEFT JOIN TB_PRODUTO ON ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.CODIGO_PRODUTO = TB_PRODUTO.CODIGO ");

                CommandSQL.AppendLine("LEFT JOIN TB_TIPO_STATUS_LINHA_PRODUCAO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TIPO_STATUS_LINHA_PRODUCAO = TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO ");

                CommandSQL.AppendLine("WHERE DATA_PREVISAO_INICIO BETWEEN @DATA_INICIO AND @DATA_FIM ");

                CommandSQL.AppendLine("ORDER BY DATA_PREVISAO_INICIO, DATA_PREVISAO_FIM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_INICIO", dataInicio);
                Command.Parameters.AddWithValue("@DATA_FIM", dataFim);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    LinhaProducaos.Add(FillEntity(Reader));
                }
                return new Retorno(LinhaProducaos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(LinhaProducao Entity)
        {
            try
            {
                LinhaProducao LinhaProducao = new LinhaProducao();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO AS CODIGO_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO AS CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("INNER JOIN TB_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = TB_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_STATUS_LINHA_PRODUCAO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TIPO_STATUS_LINHA_PRODUCAO = TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_LINHA_PRODUCAO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    LinhaProducao = FillEntity(Reader);
                }
                return new Retorno(LinhaProducao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarDataDisponivel(Funcionario funcionario)
        {
            try
            {
                LinhaProducao LinhaProducao = new LinhaProducao();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO AS CODIGO_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_PREVISAO_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_INICIO, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.DATA_REAL_FIM, ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO AS CODIGO_TIPO_STATUS_LINHA_PRODUCAO, ");
                CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("LEFT JOIN TB_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = TB_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("LEFT JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");
                CommandSQL.AppendLine("LEFT JOIN TB_TIPO_STATUS_LINHA_PRODUCAO ON ");
                CommandSQL.AppendLine("TB_LINHA_PRODUCAO.CODIGO_TIPO_STATUS_LINHA_PRODUCAO = TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = @CODIGO_FUNCIONARIO ");
                CommandSQL.AppendLine("AND DATA_PREVISAO_FIM = (SELECT MAX(DATA_PREVISAO_FIM) FROM TB_LINHA_PRODUCAO WHERE TB_LINHA_PRODUCAO.CODIGO_FUNCIONARIO = @CODIGO_FUNCIONARIO) ");
                CommandSQL.AppendLine("ORDER BY DATA_PREVISAO_FIM ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_FUNCIONARIO", funcionario.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    LinhaProducao = FillEntity(Reader);
                }
                return new Retorno(LinhaProducao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(LinhaProducao Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_LINHA_PRODUCAO ");
                CommandSQL.AppendLine("WHERE TB_LINHA_PRODUCAO.DATA_PREVISAO_INICIO = @DATA_PREVISAO_INICIO ");
                CommandSQL.AppendLine("AND TB_LINHA_PRODUCAO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_PREVISAO_INICIO", Entity.DataPrevisaoInicio);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "LinhaProducao", "DataPrevisaoInicio"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private LinhaProducao FillEntity(IDataReader reader)
        {
            LinhaProducao LinhaProducao = new LinhaProducao();
            try
            {
                LinhaProducao.Codigo = ConverterValorReader(reader, "CODIGO_LINHA_PRODUCAO", 0);
                LinhaProducao.DataPrevisaoInicio = ConverterValorReader(reader, "DATA_PREVISAO_INICIO", DateTime.MinValue);
                LinhaProducao.DataPrevisaoFim = ConverterValorReader(reader, "DATA_PREVISAO_FIM", DateTime.MinValue);
                LinhaProducao.DataRealInicio = ConverterValorReader(reader, "DATA_REAL_INICIO", DateTime.MinValue);
                LinhaProducao.DataRealFim = ConverterValorReader(reader, "DATA_REAL_FIM", DateTime.MinValue);
                LinhaProducao.Funcionario.Codigo = ConverterValorReader(reader, "CODIGO_FUNCIONARIO", 0);
                LinhaProducao.Funcionario.Nome = ConverterValorReader(reader, "NOME_FUNCIONARIO", String.Empty);
                LinhaProducao.Terceirizado.Codigo = ConverterValorReader(reader, "CODIGO_TERCEIRIZADO", 0);
                LinhaProducao.Terceirizado.Nome = ConverterValorReader(reader, "NOME_TERCEIRIZADO", String.Empty);
                LinhaProducao.TipoStatusLinhaProducao.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_STATUS_LINHA_PRODUCAO", 0);
                LinhaProducao.TipoStatusLinhaProducao.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_STATUS_LINHA_PRODUCAO", String.Empty);
                LinhaProducao.Produto.CodigoPedidoProduto = ConverterValorReader(reader, "CODIGO_PEDIDO_PRODUTO", 0);
                LinhaProducao.Produto.Quantidade = ConverterValorReader(reader, "QUANTIDADE", 0M);
                LinhaProducao.Produto.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
                LinhaProducao.Pedido.Codigo = ConverterValorReader(reader, "CODIGO_PEDIDO", 0);
                LinhaProducao.EhTerceirizado = LinhaProducao.Terceirizado.Codigo > 0;
            }
            catch (Exception ex) { throw ex; }
            return LinhaProducao;
        }

        #endregion

    }
}

