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

    public class DataProduto : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Produto Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PRODUTO( ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO, ");
                CommandSQL.AppendLine("DATA, ");
                CommandSQL.AppendLine("NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("CODIGO_FICHA_TECNICA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO, ");
                CommandSQL.AppendLine("@DATA, ");
                CommandSQL.AppendLine("@NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("@CODIGO_FICHA_TECNICA); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
                Command.Parameters.AddWithValue("@QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO", Entity.QuantidadeProducaoHoraFuncionario);
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                Command.Parameters.AddWithValue("@NUMERO_PRODUTO_CLIENTE", Entity.NumeroProdutoCliente);
                Command.Parameters.AddWithValue("@CODIGO_FICHA_TECNICA", Entity.FichaTecnica.Codigo);
                Abrir();
                Entity.Codigo = Command.ExecuteScalar().ConverteValor(0);

                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PRODUTO_VALOR( ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_PRODUTO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("@CODIGO_PRODUTO); ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@DATA_INICIO", Entity.Data);
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", Entity.Codigo);
                Abrir();
                Command.ExecuteScalar().ConverteValor(0);
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno IncluirValor(Produto produto)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PRODUTO_VALOR( ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_PRODUTO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("@CODIGO_PRODUTO) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@VALOR", produto.Valor);
                Command.Parameters.AddWithValue("@DATA_INICIO", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", produto.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Retorno AlterarValor(Produto produto)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PRODUTO_VALOR ");
                CommandSQL.AppendLine("SET DATA_FIM = @DATA_FIM ");
                CommandSQL.AppendLine("WHERE CODIGO_PRODUTO = @CODIGO_PRODUTO ");
                CommandSQL.AppendLine("AND DATA_FIM IS NULL ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_FIM", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", produto.Codigo);
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

        public Retorno ExcluirDoPedido(int codigoPedido)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO = @CODIGO_PEDIDO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);

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

        public Retorno Incluir(Produto Entity, MateriaPrima materiaPrima)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PRODUTO_MATERIA_PRIMA( ");
                CommandSQL.AppendLine("CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("QUANTIDADE, ");
                CommandSQL.AppendLine("DATA_INICIO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("@CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("@QUANTIDADE, ");
                CommandSQL.AppendLine("@DATA_INICIO); ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", Entity.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_MATERIA_PRIMA", materiaPrima.Codigo);
                Command.Parameters.AddWithValue("@QUANTIDADE", materiaPrima.Quantidade);
                Command.Parameters.AddWithValue("@DATA_INICIO", DateTime.Now);
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

        public Retorno Excluir(Produto Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PRODUTO WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex).Number == 1451)
                {
                    CommandSQL = new StringBuilder();
                    CommandSQL.AppendLine("UPDATE TB_PRODUTO SET ATIVO = 0 WHERE CODIGO = @CODIGO");
                    Command = CriaComandoSQL(CommandSQL.ToString());
                    Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                    Abrir();
                    Command.ExecuteNonQuery();
                }
                else
                    throw;
            }
            finally { Fechar(); }

            return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
        }

        public Retorno Alterar(Produto Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PRODUTO SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO, ");
                CommandSQL.AppendLine("QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO = @QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO, ");
                CommandSQL.AppendLine("CODIGO_FICHA_TECNICA = @CODIGO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("DATA = @DATA, ");
                CommandSQL.AppendLine("NUMERO_PRODUTO_CLIENTE = @NUMERO_PRODUTO_CLIENTE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
                Command.Parameters.AddWithValue("@QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO", Entity.QuantidadeProducaoHoraFuncionario);
                Command.Parameters.AddWithValue("@DATA", DateTime.Now.AddMinutes(1));
                Command.Parameters.AddWithValue("@NUMERO_PRODUTO_CLIENTE", Entity.NumeroProdutoCliente);
                Command.Parameters.AddWithValue("@CODIGO_FICHA_TECNICA", Entity.FichaTecnica.Codigo);
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
                List<Produto> Produtos = new List<Produto>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PRODUTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PRODUTO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_PRODUTO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Produtos.Add(FillEntity(Reader));
                }
                return new Retorno(Produtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Produto Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Produto> Produtos = new List<Produto>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PRODUTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PRODUTO.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_PRODUTO ");

                CommandSQL.AppendLine("WHERE (TB_PRODUTO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Produtos.Add(FillEntity(Reader));
                }
                return new Retorno(Produtos);
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
                List<Produto> Produtos = new List<Produto>();
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("P.CODIGO, ");
                CommandSQL.AppendLine("P.NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("P.DESCRICAO AS DESCRICAO_PRODUTO, ");
                CommandSQL.AppendLine("P.QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO, ");
                CommandSQL.AppendLine("PV.VALOR ");
                CommandSQL.AppendLine("FROM TB_PRODUTO AS P ");
                CommandSQL.AppendLine("LEFT JOIN TB_PRODUTO_VALOR PV ON PV.CODIGO_PRODUTO = P.CODIGO ");
                CommandSQL.AppendLine("AND P.DATA BETWEEN PV.DATA_INICIO AND IFNULL(PV.DATA_FIM, '9999-12-31 23:59:59.997') ");

                CommandSQL.AppendLine("WHERE P.ATIVO = 1 ");
                CommandSQL.AppendLine("ORDER BY NUMERO_PRODUTO_CLIENTE, DESCRICAO_PRODUTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Produtos.Add(FillEntity(Reader));
                }
                return new Retorno(Produtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Produto Entity)
        {
            try
            {
                Produto Produto = new Produto();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PRODUTO.CODIGO, ");
                CommandSQL.AppendLine("TB_PRODUTO.NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("UPPER(TB_PRODUTO.DESCRICAO) AS DESCRICAO_PRODUTO, ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO AS CODIGO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("UPPER(TB_FICHA_TECNICA.MODELO) AS MODELO, ");
                CommandSQL.AppendLine("UPPER(TB_FICHA_TECNICA.TIPO) AS TIPO, ");
                CommandSQL.AppendLine("UPPER(TB_FICHA_TECNICA.NCM) AS NCM, ");
                CommandSQL.AppendLine("UPPER(TB_FICHA_TECNICA.DESCRICAO) AS DESCRICAO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.FOTO, ");
                CommandSQL.AppendLine("(SELECT VALOR FROM TB_PRODUTO_VALOR P ");
                CommandSQL.AppendLine("WHERE NOW() BETWEEN DATA_INICIO AND IFNULL(DATA_FIM, '9999-12-31 23:59:59.997') ");
                CommandSQL.AppendLine("AND P.CODIGO_PRODUTO = TB_PRODUTO.CODIGO LIMIT 1) AS VALOR ");
                CommandSQL.AppendLine("FROM TB_PRODUTO ");

                CommandSQL.AppendLine("LEFT JOIN TB_FICHA_TECNICA ON ");
                CommandSQL.AppendLine("TB_PRODUTO.CODIGO_FICHA_TECNICA = TB_FICHA_TECNICA.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_PRODUTO.CODIGO = @CODIGO ");
                CommandSQL.AppendLine("OR TB_PRODUTO.NUMERO_PRODUTO_CLIENTE = @NUMERO_PRODUTO_CLIENTE ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@NUMERO_PRODUTO_CLIENTE", Entity.NumeroProdutoCliente);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Produto = FillEntity(Reader);
                }
                return new Retorno(Produto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarTempoProducaoHoraFuncionario(Produto produto)
        {
            try
            {
                Produto Produto = new Produto();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PRODUTO.QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO ");
                CommandSQL.AppendLine("FROM TB_PRODUTO ");

                CommandSQL.AppendLine("WHERE TB_PRODUTO.CODIGO = @CODIGO_PRODUTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", produto.Codigo);

                return new Retorno(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Produto Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_PRODUTO ");
                CommandSQL.AppendLine("WHERE TB_PRODUTO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_PRODUTO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Produto", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Produto FillEntity(IDataReader reader)
        {
            Produto Produto = new Produto();
            try
            {
                Produto.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                Produto.Descricao = ConverterValorReader(reader, "DESCRICAO_PRODUTO", String.Empty);
                Produto.QuantidadeProducaoHoraFuncionario = ConverterValorReader(reader, "QUANTIDADE_PRODUCAO_HORA_FUNCIONARIO", 0M);
                Produto.NumeroProdutoCliente = ConverterValorReader<decimal?>(Reader, "NUMERO_PRODUTO_CLIENTE", null);
                Produto.Valor = ConverterValorReader(reader, "VALOR", 0M);
                Produto.FichaTecnica.Codigo = ConverterValorReader(Reader, "CODIGO_FICHA_TECNICA", 0);
                Produto.FichaTecnica.Modelo = ConverterValorReader(Reader, "MODELO", String.Empty);
                Produto.FichaTecnica.Tipo = ConverterValorReader(Reader, "TIPO", String.Empty);
                Produto.FichaTecnica.Ncm = ConverterValorReader(Reader, "NCM", String.Empty);
                Produto.FichaTecnica.Descricao = ConverterValorReader(Reader, "DESCRICAO_FICHA_TECNICA", String.Empty);
                Produto.FichaTecnica.Foto = ConverterValorReader(Reader, "FOTO", new byte[] { });
            }
            catch (Exception ex) { throw ex; }
            return Produto;
        }

        #endregion

    }
}

