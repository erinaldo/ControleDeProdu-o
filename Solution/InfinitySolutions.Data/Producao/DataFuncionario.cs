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

    public class DataFuncionario : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Funcionario Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_FUNCIONARIO( ");
                CommandSQL.AppendLine("NOME, ");
                CommandSQL.AppendLine("TELEFONE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FUNCAO_FUNCIONARIO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NOME, ");
                CommandSQL.AppendLine("@TELEFONE, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FUNCAO_FUNCIONARIO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FUNCAO_FUNCIONARIO", Entity.TipoFuncaoFuncionario.Codigo);
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

        public Retorno Excluir(Funcionario Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_FUNCIONARIO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Funcionario Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_FUNCIONARIO SET ");
                CommandSQL.AppendLine("NOME = @NOME, ");
                CommandSQL.AppendLine("TELEFONE = @TELEFONE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FUNCAO_FUNCIONARIO = @CODIGO_TIPO_FUNCAO_FUNCIONARIO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FUNCAO_FUNCIONARIO", Entity.TipoFuncaoFuncionario.Codigo);
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
                List<Funcionario> Funcionarios = new List<Funcionario>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO ");
                CommandSQL.AppendLine("FROM TB_FUNCIONARIO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Funcionarios.Add(FillEntity(Reader));
                }
                return new Retorno(Funcionarios);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Funcionario Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Funcionario> Funcionarios = new List<Funcionario>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO ");
                CommandSQL.AppendLine("FROM TB_FUNCIONARIO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_FUNCIONARIO.NOME LIKE '%" + Entity.Nome + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Funcionarios.Add(FillEntity(Reader));
                }
                return new Retorno(Funcionarios);
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
                List<Funcionario> Funcionarios = new List<Funcionario>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO ");
                CommandSQL.AppendLine("FROM TB_FUNCIONARIO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Funcionarios.Add(FillEntity(Reader));
                }
                return new Retorno(Funcionarios);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Funcionario Entity)
        {
            try
            {
                Funcionario Funcionario = new Funcionario();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO AS CODIGO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.NOME AS NOME_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO AS CODIGO_TIPO_FUNCAO_FUNCIONARIO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME AS NOME_TIPO_FUNCAO_FUNCIONARIO ");
                CommandSQL.AppendLine("FROM TB_FUNCIONARIO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_FUNCIONARIO ON ");
                CommandSQL.AppendLine("TB_FUNCIONARIO.CODIGO_TIPO_FUNCAO_FUNCIONARIO = TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_FUNCIONARIO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Funcionario = FillEntity(Reader);
                }
                return new Retorno(Funcionario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Funcionario Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_FUNCIONARIO ");
                CommandSQL.AppendLine("WHERE TB_FUNCIONARIO.NOME = @NOME ");
                CommandSQL.AppendLine("AND TB_FUNCIONARIO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Funcionario", "Nome"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Funcionario FillEntity(IDataReader reader)
        {
            Funcionario Funcionario = new Funcionario();
            try
            {
                Funcionario.Codigo = ConverterValorReader(reader, "CODIGO_FUNCIONARIO", 0);
                Funcionario.Nome = ConverterValorReader(reader, "NOME_FUNCIONARIO", String.Empty);
                Funcionario.Telefone = ConverterValorReader(reader, "TELEFONE", 0M);
                Funcionario.TipoFuncaoFuncionario.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FUNCAO_FUNCIONARIO", 0);
                Funcionario.TipoFuncaoFuncionario.Nome = ConverterValorReader(reader, "NOME_TIPO_FUNCAO_FUNCIONARIO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Funcionario;
        }

        #endregion

    }
}

