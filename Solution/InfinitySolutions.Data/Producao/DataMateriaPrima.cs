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

    public class DataMateriaPrima : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(MateriaPrima Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_MATERIA_PRIMA( ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("FORNECEDOR, ");
                CommandSQL.AppendLine("QUANTIDADE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_UNIDADE_MEDIDA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@FORNECEDOR, ");
                CommandSQL.AppendLine("@QUANTIDADE, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_UNIDADE_MEDIDA); ");

                CommandSQL.AppendLine("INSERT INTO TB_MATERIA_PRIMA_VALOR( ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_MATERIA_PRIMA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("(SELECT LAST_INSERT_ID())); ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@FORNECEDOR", Entity.Fornecedor);
                Command.Parameters.AddWithValue("@QUANTIDADE", Entity.Quantidade);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_UNIDADE_MEDIDA", Entity.TipoUnidadeMedida.Codigo);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
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

        public Retorno ExcluirDoProduto(int codigoProduto)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PRODUTO_MATERIA_PRIMA ");
                CommandSQL.AppendLine("WHERE CODIGO_PRODUTO = @CODIGO_PRODUTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", codigoProduto);

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

        public Retorno AlterarValor(MateriaPrima materiaPrima)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_MATERIA_PRIMA_VALOR ");
                CommandSQL.AppendLine("SET DATA_FIM = @DATA_FIM ");
                CommandSQL.AppendLine("WHERE CODIGO_MATERIA_PRIMA = @CODIGO_MATERIA_PRIMA ");
                CommandSQL.AppendLine("AND DATA_FIM IS NULL ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_FIM", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_MATERIA_PRIMA", materiaPrima.Codigo);
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

        public Retorno IncluirProdutoMateriaPrima(Produto entity, MateriaPrima materiaPrima)
        {
            throw new NotImplementedException();
        }

        public Retorno IncluirValor(MateriaPrima materiaPrima)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_MATERIA_PRIMA_VALOR( ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_MATERIA_PRIMA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("@CODIGO_MATERIA_PRIMA) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@VALOR", materiaPrima.Valor);
                Command.Parameters.AddWithValue("@DATA_INICIO", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_MATERIA_PRIMA", materiaPrima.Codigo);
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

        public Retorno Excluir(MateriaPrima Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_MATERIA_PRIMA WHERE CODIGO = @CODIGO ");
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
                    CommandSQL.AppendLine("UPDATE TB_MATERIA_PRIMA SET ATIVO = 0 WHERE CODIGO = @CODIGO");
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


        public Retorno Alterar(MateriaPrima Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_MATERIA_PRIMA SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO, ");
                CommandSQL.AppendLine("FORNECEDOR = @FORNECEDOR, ");
                CommandSQL.AppendLine("QUANTIDADE = @QUANTIDADE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_UNIDADE_MEDIDA = @CODIGO_TIPO_UNIDADE_MEDIDA ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@FORNECEDOR", Entity.Fornecedor);
                Command.Parameters.AddWithValue("@QUANTIDADE", Entity.Quantidade);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_UNIDADE_MEDIDA", Entity.TipoUnidadeMedida.Codigo);
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
                List<MateriaPrima> MateriaPrimas = new List<MateriaPrima>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO AS CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.FORNECEDOR, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.VALOR_CUSTO, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO AS CODIGO_TIPO_UNIDADE_MEDIDA, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO AS DESCRICAO_TIPO_UNIDADE_MEDIDA ");
                CommandSQL.AppendLine("FROM TB_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA ON ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO_TIPO_UNIDADE_MEDIDA = TB_TIPO_UNIDADE_MEDIDA.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    MateriaPrimas.Add(FillEntity(Reader));
                }
                return new Retorno(MateriaPrimas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(MateriaPrima Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<MateriaPrima> MateriaPrimas = new List<MateriaPrima>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO AS CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.FORNECEDOR, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.QUANTIDADE, ");
                CommandSQL.AppendLine("(SELECT VALOR FROM TB_MATERIA_PRIMA_VALOR M ");
                CommandSQL.AppendLine("WHERE @DATA_ATUAL BETWEEN DATA_INICIO AND IFNULL(DATA_FIM, '9999-12-31 23:59:59.997')");
                CommandSQL.AppendLine("AND M.CODIGO_MATERIA_PRIMA = TB_MATERIA_PRIMA.CODIGO) AS VALOR, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO AS CODIGO_TIPO_UNIDADE_MEDIDA, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO AS DESCRICAO_TIPO_UNIDADE_MEDIDA ");
                CommandSQL.AppendLine("FROM TB_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA ON ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO_TIPO_UNIDADE_MEDIDA = TB_TIPO_UNIDADE_MEDIDA.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_MATERIA_PRIMA.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Command.Parameters.AddWithValue("@DATA_ATUAL", DateTime.Now);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    MateriaPrimas.Add(FillEntity(Reader));
                }
                return new Retorno(MateriaPrimas);
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
                List<MateriaPrima> MateriaPrimas = new List<MateriaPrima>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO AS CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.FORNECEDOR, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.QUANTIDADE, ");

                CommandSQL.AppendLine("(SELECT VALOR FROM TB_MATERIA_PRIMA_VALOR M ");
                CommandSQL.AppendLine("WHERE @DATA_ATUAL BETWEEN DATA_INICIO AND IFNULL(DATA_FIM, '9999-12-31 23:59:59.997')");
                CommandSQL.AppendLine("AND M.CODIGO_MATERIA_PRIMA = TB_MATERIA_PRIMA.CODIGO LIMIT 1) AS VALOR, ");

                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO AS CODIGO_TIPO_UNIDADE_MEDIDA, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO AS DESCRICAO_TIPO_UNIDADE_MEDIDA ");
                CommandSQL.AppendLine("FROM TB_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA ON ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO_TIPO_UNIDADE_MEDIDA = TB_TIPO_UNIDADE_MEDIDA.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_MATERIA_PRIMA.ATIVO = 1 ");
                CommandSQL.AppendLine("ORDER BY TB_MATERIA_PRIMA.DESCRICAO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DATA_ATUAL", DateTime.Now);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    MateriaPrimas.Add(FillEntity(Reader));
                }
                return new Retorno(MateriaPrimas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(MateriaPrima Entity)
        {
            try
            {
                MateriaPrima MateriaPrima = new MateriaPrima();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO AS CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.FORNECEDOR, ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.QUANTIDADE, ");
                CommandSQL.AppendLine("(SELECT VALOR FROM TB_MATERIA_PRIMA_VALOR M ");
                CommandSQL.AppendLine("WHERE @DATA_ATUAL BETWEEN DATA_INICIO AND IFNULL(DATA_FIM, '9999-12-31 23:59:59.997') ");
                CommandSQL.AppendLine("AND M.CODIGO_MATERIA_PRIMA = TB_MATERIA_PRIMA.CODIGO) AS VALOR, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO AS CODIGO_TIPO_UNIDADE_MEDIDA, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO AS DESCRICAO_TIPO_UNIDADE_MEDIDA, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.SIGLA ");
                CommandSQL.AppendLine("FROM TB_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA ON ");
                CommandSQL.AppendLine("TB_MATERIA_PRIMA.CODIGO_TIPO_UNIDADE_MEDIDA = TB_TIPO_UNIDADE_MEDIDA.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_MATERIA_PRIMA.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DATA_ATUAL", DateTime.Now);

                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    MateriaPrima = FillEntity(Reader);
                }
                return new Retorno(MateriaPrima);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Carregar(int codigoProduto)
        {
            try
            {
                var materiasPrimas = new List<MateriaPrima>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("M.CODIGO, ");
                CommandSQL.AppendLine("M.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("PM.QUANTIDADE, ");

                CommandSQL.AppendLine("(SELECT VALOR FROM TB_MATERIA_PRIMA_VALOR MV ");
                CommandSQL.AppendLine("WHERE @DATA_ATUAL BETWEEN DATA_INICIO AND IFNULL(DATA_FIM, '9999-12-31 23:59:59.997')");
                CommandSQL.AppendLine("AND MV.CODIGO_MATERIA_PRIMA = M.CODIGO) AS VALOR, ");

                CommandSQL.AppendLine("U.SIGLA ");
                CommandSQL.AppendLine("FROM TB_PRODUTO_MATERIA_PRIMA PM ");
                CommandSQL.AppendLine("INNER JOIN TB_MATERIA_PRIMA M ON M.CODIGO = PM.CODIGO_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA U ON U.CODIGO = M.CODIGO_TIPO_UNIDADE_MEDIDA ");

                CommandSQL.AppendLine("WHERE PM.CODIGO_PRODUTO = @CODIGO_PRODUTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", codigoProduto);
                Command.Parameters.AddWithValue("@DATA_ATUAL", DateTime.Now);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    materiasPrimas.Add(new MateriaPrima
                    {
                        Codigo = ConverterValorReader(Reader, "CODIGO_MATERIA_PRIMA", 0),
                        Descricao = ConverterValorReader(Reader, "DESCRICAO_MATERIA_PRIMA", String.Empty),
                        Quantidade = ConverterValorReader(Reader, "QUANTIDADE", 0M),
                        Valor = ConverterValorReader(Reader, "VALOR", 0M),
                        TipoUnidadeMedida = new TipoUnidadeMedida { Sigla = ConverterValorReader(Reader, "SIGLA", String.Empty) }
                    });
                }
                return new Retorno(materiasPrimas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarDoProduto(int codigoProduto)
        {
            try
            {
                var materiasPrimas = new List<MateriaPrima>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("M.CODIGO AS CODIGO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("M.DESCRICAO AS DESCRICAO_MATERIA_PRIMA, ");
                CommandSQL.AppendLine("MV.VALOR, ");
                CommandSQL.AppendLine("U.SIGLA, ");
                CommandSQL.AppendLine("PM.QUANTIDADE ");
                CommandSQL.AppendLine("FROM TB_PRODUTO_MATERIA_PRIMA PM ");
                CommandSQL.AppendLine("INNER JOIN TB_PRODUTO P ON P.CODIGO = PM.CODIGO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_MATERIA_PRIMA M ON M.CODIGO = PM.CODIGO_MATERIA_PRIMA ");
                CommandSQL.AppendLine("INNER JOIN TB_MATERIA_PRIMA_VALOR MV ON MV.CODIGO_MATERIA_PRIMA = M.CODIGO ");
                CommandSQL.AppendLine("AND P.DATA BETWEEN MV.DATA_INICIO AND IFNULL(MV.DATA_FIM, '9999-12-31 23:59:59.997') ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_UNIDADE_MEDIDA U ON U.CODIGO = M.CODIGO_TIPO_UNIDADE_MEDIDA ");

                CommandSQL.AppendLine("WHERE P.CODIGO = @CODIGO_PRODUTO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", codigoProduto);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    materiasPrimas.Add(new MateriaPrima
                    {
                        Codigo = ConverterValorReader(Reader, "CODIGO_MATERIA_PRIMA", 0),
                        Descricao = ConverterValorReader(Reader, "DESCRICAO_MATERIA_PRIMA", String.Empty),
                        Quantidade = ConverterValorReader(Reader, "QUANTIDADE", 0M),
                        Valor = ConverterValorReader(Reader, "VALOR", 0M),
                        TipoUnidadeMedida = new TipoUnidadeMedida { Sigla = ConverterValorReader(Reader, "SIGLA", String.Empty) }
                    });
                }
                return new Retorno(materiasPrimas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(MateriaPrima Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_MATERIA_PRIMA ");
                CommandSQL.AppendLine("WHERE TB_MATERIA_PRIMA.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_MATERIA_PRIMA.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "MateriaPrima", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private MateriaPrima FillEntity(IDataReader reader)
        {
            MateriaPrima MateriaPrima = new MateriaPrima();
            try
            {
                MateriaPrima.Codigo = ConverterValorReader(reader, "CODIGO_MATERIA_PRIMA", 0);
                MateriaPrima.Descricao = ConverterValorReader(reader, "DESCRICAO_MATERIA_PRIMA", String.Empty).ToUpper();
                MateriaPrima.Fornecedor = ConverterValorReader(reader, "FORNECEDOR", String.Empty);
                MateriaPrima.Quantidade = ConverterValorReader<decimal?>(reader, "QUANTIDADE", null);
                MateriaPrima.Valor = ConverterValorReader<decimal?>(reader, "VALOR", null);
                MateriaPrima.TipoUnidadeMedida.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_UNIDADE_MEDIDA", 0);
                MateriaPrima.TipoUnidadeMedida.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_UNIDADE_MEDIDA", String.Empty);
                MateriaPrima.TipoUnidadeMedida.Sigla = ConverterValorReader(reader, "SIGLA", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return MateriaPrima;
        }

        #endregion

    }
}

