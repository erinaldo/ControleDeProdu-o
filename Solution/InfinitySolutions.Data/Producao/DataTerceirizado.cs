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

    public class DataTerceirizado : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Terceirizado Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TERCEIRIZADO( ");
                CommandSQL.AppendLine("NOME, ");
                CommandSQL.AppendLine("TELEFONE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FUNCAO_TERCEIRIZADO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NOME, ");
                CommandSQL.AppendLine("@TELEFONE, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FUNCAO_TERCEIRIZADO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FUNCAO_TERCEIRIZADO", Entity.TipoFuncaoTerceirizado.Codigo);
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

        public Retorno Excluir(Terceirizado Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TERCEIRIZADO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Terceirizado Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TERCEIRIZADO SET ");
                CommandSQL.AppendLine("NOME = @NOME, ");
                CommandSQL.AppendLine("TELEFONE = @TELEFONE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FUNCAO_TERCEIRIZADO = @CODIGO_TIPO_FUNCAO_TERCEIRIZADO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@TELEFONE", Entity.Telefone);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FUNCAO_TERCEIRIZADO", Entity.TipoFuncaoTerceirizado.Codigo);
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
                List<Terceirizado> Terceirizados = new List<Terceirizado>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO AS CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.NOME AS NOME_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO AS CODIGO_TIPO_FUNCAO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME AS NOME_TIPO_FUNCAO_TERCEIRIZADO ");
                CommandSQL.AppendLine("FROM TB_TERCEIRIZADO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_TERCEIRIZADO ON ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO_TIPO_FUNCAO_TERCEIRIZADO = TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Terceirizados.Add(FillEntity(Reader));
                }
                return new Retorno(Terceirizados);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Terceirizado Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Terceirizado> Terceirizados = new List<Terceirizado>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO AS CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.NOME AS NOME_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO AS CODIGO_TIPO_FUNCAO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME AS NOME_TIPO_FUNCAO_TERCEIRIZADO ");
                CommandSQL.AppendLine("FROM TB_TERCEIRIZADO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_TERCEIRIZADO ON ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO_TIPO_FUNCAO_TERCEIRIZADO = TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_TERCEIRIZADO.NOME LIKE '%" + Entity.Nome + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Terceirizados.Add(FillEntity(Reader));
                }
                return new Retorno(Terceirizados);
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
                List<Terceirizado> Terceirizados = new List<Terceirizado>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO AS CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.NOME AS NOME_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO AS CODIGO_TIPO_FUNCAO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME AS NOME_TIPO_FUNCAO_TERCEIRIZADO ");
                CommandSQL.AppendLine("FROM TB_TERCEIRIZADO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_TERCEIRIZADO ON ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO_TIPO_FUNCAO_TERCEIRIZADO = TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Terceirizados.Add(FillEntity(Reader));
                }
                return new Retorno(Terceirizados);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Terceirizado Entity)
        {
            try
            {
                Terceirizado Terceirizado = new Terceirizado();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO AS CODIGO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.NOME AS NOME_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.TELEFONE, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO AS CODIGO_TIPO_FUNCAO_TERCEIRIZADO, ");
                CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME AS NOME_TIPO_FUNCAO_TERCEIRIZADO ");
                CommandSQL.AppendLine("FROM TB_TERCEIRIZADO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FUNCAO_TERCEIRIZADO ON ");
                CommandSQL.AppendLine("TB_TERCEIRIZADO.CODIGO_TIPO_FUNCAO_TERCEIRIZADO = TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_TERCEIRIZADO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Terceirizado = FillEntity(Reader);
                }
                return new Retorno(Terceirizado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Terceirizado Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TERCEIRIZADO ");
                CommandSQL.AppendLine("WHERE TB_TERCEIRIZADO.NOME = @NOME ");
                CommandSQL.AppendLine("AND TB_TERCEIRIZADO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Terceirizado", "Nome"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Terceirizado FillEntity(IDataReader reader)
        {
            Terceirizado Terceirizado = new Terceirizado();
            try
            {
                Terceirizado.Codigo = ConverterValorReader(reader, "CODIGO_TERCEIRIZADO", 0);
                Terceirizado.Nome = ConverterValorReader(reader, "NOME_TERCEIRIZADO", String.Empty);
                Terceirizado.Telefone = ConverterValorReader(reader, "TELEFONE", 0M);
                Terceirizado.TipoFuncaoTerceirizado.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FUNCAO_TERCEIRIZADO", 0);
                Terceirizado.TipoFuncaoTerceirizado.Nome = ConverterValorReader(reader, "NOME_TIPO_FUNCAO_TERCEIRIZADO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Terceirizado;
        }

        #endregion

    }
}

