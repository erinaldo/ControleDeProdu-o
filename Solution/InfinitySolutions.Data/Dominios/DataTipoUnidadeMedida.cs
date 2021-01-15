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

    public class DataTipoUnidadeMedida : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(TipoUnidadeMedida Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_TIPO_UNIDADE_MEDIDA( ");
                CommandSQL.AppendLine("DESCRICAO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@DESCRICAO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
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

        public Retorno Excluir(TipoUnidadeMedida Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_TIPO_UNIDADE_MEDIDA WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(TipoUnidadeMedida Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_TIPO_UNIDADE_MEDIDA SET ");
                CommandSQL.AppendLine("DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
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
                List<TipoUnidadeMedida> TipoUnidadeMedidas = new List<TipoUnidadeMedida>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_UNIDADE_MEDIDA ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoUnidadeMedidas.Add(FillEntity(Reader));
                }
                return new Retorno(TipoUnidadeMedidas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(TipoUnidadeMedida Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<TipoUnidadeMedida> TipoUnidadeMedidas = new List<TipoUnidadeMedida>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_UNIDADE_MEDIDA ");

                CommandSQL.AppendLine("WHERE (TB_TIPO_UNIDADE_MEDIDA.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoUnidadeMedidas.Add(FillEntity(Reader));
                }
                return new Retorno(TipoUnidadeMedidas);
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
                List<TipoUnidadeMedida> TipoUnidadeMedidas = new List<TipoUnidadeMedida>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.SIGLA ");
                CommandSQL.AppendLine("FROM TB_TIPO_UNIDADE_MEDIDA ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoUnidadeMedidas.Add(FillEntity(Reader));
                }
                return new Retorno(TipoUnidadeMedidas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(TipoUnidadeMedida Entity)
        {
            try
            {
                TipoUnidadeMedida TipoUnidadeMedida = new TipoUnidadeMedida();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.CODIGO, ");
                CommandSQL.AppendLine("TB_TIPO_UNIDADE_MEDIDA.DESCRICAO ");
                CommandSQL.AppendLine("FROM TB_TIPO_UNIDADE_MEDIDA ");

                CommandSQL.AppendLine("WHERE TB_TIPO_UNIDADE_MEDIDA.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    TipoUnidadeMedida = FillEntity(Reader);
                }
                return new Retorno(TipoUnidadeMedida);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(TipoUnidadeMedida Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_UNIDADE_MEDIDA ");
                CommandSQL.AppendLine("WHERE TB_TIPO_UNIDADE_MEDIDA.DESCRICAO = @DESCRICAO ");
                CommandSQL.AppendLine("AND TB_TIPO_UNIDADE_MEDIDA.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoUnidadeMedida", "Descricao"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private TipoUnidadeMedida FillEntity(IDataReader reader)
        {
            TipoUnidadeMedida TipoUnidadeMedida = new TipoUnidadeMedida();
            try
            {
                TipoUnidadeMedida.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                TipoUnidadeMedida.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
                TipoUnidadeMedida.Sigla = ConverterValorReader(reader, "SIGLA", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return TipoUnidadeMedida;
        }

        #endregion

    }
}

