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

    public class DataFichaTecnica : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(FichaTecnica Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_FICHA_TECNICA( ");
                CommandSQL.AppendLine("MODELO, ");
                CommandSQL.AppendLine("TIPO, ");
                CommandSQL.AppendLine("NCM, ");
                CommandSQL.AppendLine("DESCRICAO, ");
                CommandSQL.AppendLine("FOTO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@MODELO, ");
                CommandSQL.AppendLine("@TIPO, ");
                CommandSQL.AppendLine("@NCM, ");
                CommandSQL.AppendLine("@DESCRICAO, ");
                CommandSQL.AppendLine("@FOTO); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID() ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@MODELO", Entity.Modelo);
                Command.Parameters.AddWithValue("@TIPO", Entity.Tipo);
                Command.Parameters.AddWithValue("@NCM", Entity.Ncm);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@FOTO", Entity.Foto != null ? (object)Entity.Foto : DBNull.Value);

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

        public Retorno Excluir(FichaTecnica Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_FICHA_TECNICA WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(FichaTecnica Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_FICHA_TECNICA ");
                CommandSQL.AppendLine("SET MODELO = @MODELO, ");
                CommandSQL.AppendLine("TIPO = @TIPO, ");
                CommandSQL.AppendLine("NCM = @NCM, ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO ");

                if (Entity.Foto != null)
                    CommandSQL.AppendLine(",FOTO = @FOTO ");

                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@MODELO", Entity.Modelo);
                Command.Parameters.AddWithValue("@TIPO", Entity.Tipo);
                Command.Parameters.AddWithValue("@NCM", Entity.Ncm);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);

                if (Entity.Foto != null)
                    Command.Parameters.AddWithValue("@FOTO", Entity.Foto != null ? (object)Entity.Foto : DBNull.Value);

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
                List<FichaTecnica> FichaTecnicas = new List<FichaTecnica>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("FROM TB_FICHA_TECNICA ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    FichaTecnicas.Add(FillEntity(Reader));
                }
                return new Retorno(FichaTecnicas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(FichaTecnica Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<FichaTecnica> FichaTecnicas = new List<FichaTecnica>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("FROM TB_FICHA_TECNICA ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    FichaTecnicas.Add(FillEntity(Reader));
                }
                return new Retorno(FichaTecnicas);
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
                List<FichaTecnica> FichaTecnicas = new List<FichaTecnica>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("FROM TB_FICHA_TECNICA ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    FichaTecnicas.Add(FillEntity(Reader));
                }
                return new Retorno(FichaTecnicas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(FichaTecnica Entity)
        {
            try
            {
                FichaTecnica FichaTecnica = new FichaTecnica();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("FROM TB_FICHA_TECNICA ");

                CommandSQL.AppendLine("WHERE TB_FICHA_TECNICA.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    FichaTecnica = FillEntity(Reader);
                }
                return new Retorno(FichaTecnica);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(FichaTecnica Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_FICHA_TECNICA ");
                CommandSQL.AppendLine("AND TB_FICHA_TECNICA.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "FichaTecnica", ""));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private FichaTecnica FillEntity(IDataReader reader)
        {
            FichaTecnica FichaTecnica = new FichaTecnica();
            try
            {
                FichaTecnica.Codigo = ConverterValorReader(reader, "CODIGO", 0);
            }
            catch (Exception ex) { throw ex; }
            return FichaTecnica;
        }

        #endregion

    }
}

