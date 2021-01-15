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

    public class DataTabelaPreco : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TabelaPreco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TABELA_PRECO( ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("DATA) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@DATA); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
                Command.Parameters.AddWithValue("@DATA", Entity.Data);
                Abrir();
                Entity.Codigo = Command.ExecuteScalar().ConverteValor(0);
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TABELA_PRECO_VALORES( ");
                CommandSQL.AppendLine("IMPOSTO, ");
                CommandSQL.AppendLine("LUCRO, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_TABELA_PRECO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@IMPOSTO, ");
                CommandSQL.AppendLine("@LUCRO, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("@CODIGO_TABELA_PRECO); ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@IMPOSTO", Entity.Imposto);
                Command.Parameters.AddWithValue("@LUCRO", Entity.Lucro);
                Command.Parameters.AddWithValue("@DATA_INICIO", Entity.Data);
                Command.Parameters.AddWithValue("@CODIGO_TABELA_PRECO", Entity.Codigo);
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

        public Retorno IncluirValor(TabelaPreco entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TABELA_PRECO_VALORES( ");
                CommandSQL.AppendLine("IMPOSTO, ");
                CommandSQL.AppendLine("LUCRO, ");
                CommandSQL.AppendLine("DATA_INICIO, ");
                CommandSQL.AppendLine("CODIGO_TABELA_PRECO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@IMPOSTO, ");
                CommandSQL.AppendLine("@LUCRO, ");
                CommandSQL.AppendLine("@DATA_INICIO, ");
                CommandSQL.AppendLine("@CODIGO_TABELA_PRECO) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@IMPOSTO", entity.Imposto);
                Command.Parameters.AddWithValue("@LUCRO", entity.Lucro);
                Command.Parameters.AddWithValue("@DATA_INICIO", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_TABELA_PRECO", entity.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Retorno AlterarValor(TabelaPreco tabelaPreco)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TABELA_PRECO_VALORES ");
                CommandSQL.AppendLine("SET DATA_FIM = @DATA_FIM ");
                CommandSQL.AppendLine("WHERE CODIGO_TABELA_PRECO = @CODIGO_TABELA_PRECO ");
                CommandSQL.AppendLine("AND DATA_FIM IS NULL ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_FIM", DateTime.Now);
                Command.Parameters.AddWithValue("@CODIGO_TABELA_PRECO", tabelaPreco.Codigo);
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

        public Retorno Excluir(TabelaPreco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TABELA_PRECO WHERE CODIGO = @CODIGO ");
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
                    if (((MySqlException)ex).Number == 1451)
                    {
                        CommandSQL = new StringBuilder();
                        CommandSQL.AppendLine("UPDATE TB_TABELA_PRECO SET ATIVO = 0 WHERE CODIGO = @CODIGO");
                        Command = CriaComandoSQL(CommandSQL.ToString());
                        Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                        Abrir();
                        Command.ExecuteNonQuery();
                    }
                    else
                        throw;
                }
            }
            finally { Fechar(); }

            return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
        }

        public Retorno Alterar(TabelaPreco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TABELA_PRECO SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO, ");
                CommandSQL.AppendLine("DATA = @DATA ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao.ToUpper());
                Command.Parameters.AddWithValue("@DATA", DateTime.Now.AddMinutes(1));
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
                List<TabelaPreco> TabelaPrecos = new List<TabelaPreco>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.CODIGO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.IMPOSTO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.LUCRO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_INICIO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_FIM ");
                CommandSQL.AppendLine("FROM TB_TABELA_PRECO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TabelaPrecos.Add(FillEntity(Reader));
                }
                return new Retorno(TabelaPrecos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TabelaPreco Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TabelaPreco> TabelaPrecos = new List<TabelaPreco>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.CODIGO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.IMPOSTO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.LUCRO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_INICIO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_FIM ");
                CommandSQL.AppendLine("FROM TB_TABELA_PRECO ");

                CommandSQL.AppendLine("WHERE (TB_TABELA_PRECO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TabelaPrecos.Add(FillEntity(Reader));
                }
                return new Retorno(TabelaPrecos);
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
                List<TabelaPreco> TabelaPrecos = new List<TabelaPreco>();
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("T.CODIGO, ");
                CommandSQL.AppendLine("T.DESCRICAO, ");
                CommandSQL.AppendLine("TV.IMPOSTO, ");
                CommandSQL.AppendLine("TV.LUCRO ");
                CommandSQL.AppendLine("FROM TB_TABELA_PRECO AS T ");
                CommandSQL.AppendLine("LEFT JOIN TB_TABELA_PRECO_VALORES TV ON TV.CODIGO_TABELA_PRECO = T.CODIGO ");
                CommandSQL.AppendLine("AND T.DATA BETWEEN TV.DATA_INICIO AND IFNULL(TV.DATA_FIM, '9999-12-31 23:59:59.997') ");

                CommandSQL.AppendLine("WHERE T.ATIVO = 1 ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TabelaPrecos.Add(FillEntity(Reader));
                }
                return new Retorno(TabelaPrecos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TabelaPreco Entity)
        {
            try
            {
                TabelaPreco TabelaPreco = new TabelaPreco();
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("T.CODIGO, ");
                CommandSQL.AppendLine("T.DESCRICAO, ");
                CommandSQL.AppendLine("T.ESPECIAL, ");
                CommandSQL.AppendLine("TV.IMPOSTO, ");
                CommandSQL.AppendLine("TV.LUCRO ");
                CommandSQL.AppendLine("FROM TB_TABELA_PRECO AS T ");
                CommandSQL.AppendLine("LEFT JOIN TB_TABELA_PRECO_VALORES TV ON TV.CODIGO_TABELA_PRECO = T.CODIGO ");
                CommandSQL.AppendLine("AND T.DATA BETWEEN TV.DATA_INICIO AND IFNULL(TV.DATA_FIM, '9999-12-31 23:59:59.997') ");

                CommandSQL.AppendLine("WHERE T.CODIGO = @CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TabelaPreco = FillEntity(Reader);
                }
                return new Retorno(TabelaPreco);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TabelaPreco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TABELA_PRECO ");
                CommandSQL.AppendLine("WHERE TB_TABELA_PRECO.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TABELA_PRECO.CODIGO <> @CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TabelaPreco", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TabelaPreco FillEntity(IDataReader reader)
        {
            TabelaPreco TabelaPreco = new TabelaPreco();
            try
            {
                TabelaPreco.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TabelaPreco.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
                TabelaPreco.Imposto = ConverterValorReader<decimal?>(reader, "IMPOSTO", null);
                TabelaPreco.Lucro = ConverterValorReader<decimal?>(reader, "LUCRO", null);
                TabelaPreco.Especial = ConverterValorReader(reader, "ESPECIAL", false);
            }
            catch (Exception ex) { throw ex; }
            return TabelaPreco;
        }

        #endregion

    }
}

