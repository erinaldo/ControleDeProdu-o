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

    public class DataFornecedor : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Fornecedor Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_FORNECEDOR( ");
                CommandSQL.AppendLine("NOME, ");
                CommandSQL.AppendLine("EMAIL, ");
                CommandSQL.AppendLine("TELEFONE) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NOME, ");
                CommandSQL.AppendLine("@EMAIL, ");
                CommandSQL.AppendLine("@TELEFONE) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@EMAIL", Entity.Email);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
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

        public Retorno Excluir(Fornecedor Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_FORNECEDOR WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Fornecedor Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_FORNECEDOR SET ");
                CommandSQL.AppendLine("NOME = @NOME, ");
                CommandSQL.AppendLine("EMAIL = @EMAIL, ");
                CommandSQL.AppendLine("TELEFONE = @TELEFONE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@EMAIL", Entity.Email);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
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
                List<Fornecedor> Fornecedors = new List<Fornecedor>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO, ");
                CommandSQL.AppendLine("UPPER(TB_FORNECEDOR.NOME) AS NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE ");
                CommandSQL.AppendLine("FROM TB_FORNECEDOR ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fornecedors.Add(FillEntity(Reader));
                }
                return new Retorno(Fornecedors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Fornecedor Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Fornecedor> Fornecedors = new List<Fornecedor>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE ");
                CommandSQL.AppendLine("FROM TB_FORNECEDOR ");

                CommandSQL.AppendLine("WHERE (TB_FORNECEDOR.NOME LIKE '%" + Entity.Nome + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fornecedors.Add(FillEntity(Reader));
                }
                return new Retorno(Fornecedors);
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
                List<Fornecedor> Fornecedors = new List<Fornecedor>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO, ");
                CommandSQL.AppendLine("UPPER(TB_FORNECEDOR.NOME) AS NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE ");
                CommandSQL.AppendLine("FROM TB_FORNECEDOR ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fornecedors.Add(FillEntity(Reader));
                }
                return new Retorno(Fornecedors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Fornecedor Entity)
        {
            try
            {
                Fornecedor Fornecedor = new Fornecedor();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FORNECEDOR.CODIGO, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.NOME, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.EMAIL, ");
                CommandSQL.AppendLine("TB_FORNECEDOR.TELEFONE ");
                CommandSQL.AppendLine("FROM TB_FORNECEDOR ");

                CommandSQL.AppendLine("WHERE TB_FORNECEDOR.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fornecedor = FillEntity(Reader);
                }
                return new Retorno(Fornecedor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Fornecedor Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_FORNECEDOR ");
                CommandSQL.AppendLine("WHERE TB_FORNECEDOR.NOME = @NOME ");
                CommandSQL.AppendLine("AND TB_FORNECEDOR.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Fornecedor", "Nome"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Fornecedor FillEntity(IDataReader reader)
        {
            Fornecedor Fornecedor = new Fornecedor();
            try
            {
                Fornecedor.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                Fornecedor.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                Fornecedor.Email = ConverterValorReader(reader, "EMAIL", String.Empty);
                Fornecedor.Telefone = ConverterValorReader(reader, "TELEFONE", 0M);
            }
            catch (Exception ex) { throw ex; }
            return Fornecedor;
        }

        #endregion

    }
}

