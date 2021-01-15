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

    public class DataFrete : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Frete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_FRETE( ");
                CommandSQL.AppendLine("TRANSPORTADORA, ");
                CommandSQL.AppendLine("VOLUME, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FRETE) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@TRANSPORTADORA, ");
                CommandSQL.AppendLine("@VOLUME, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FRETE) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@TRANSPORTADORA", Entity.Transportadora);
                Command.Parameters.AddWithValue("@VOLUME", Entity.Volume);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FRETE", Entity.TipoFrete.Codigo);
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

        public Retorno Excluir(Frete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_FRETE WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Frete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_FRETE SET ");
                CommandSQL.AppendLine("TRANSPORTADORA = @TRANSPORTADORA, ");
                CommandSQL.AppendLine("VOLUME = @VOLUME ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@TRANSPORTADORA", Entity.Transportadora);
                Command.Parameters.AddWithValue("@VOLUME", Entity.Volume);
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
                List<Frete> Fretes = new List<Frete>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO AS CODIGO_FRETE, ");
                CommandSQL.AppendLine("TB_FRETE.TRANSPORTADORA, ");
                CommandSQL.AppendLine("TB_FRETE.VOLUME, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO AS CODIGO_TIPO_FRETE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CIDADE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.ESTADO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.VALOR ");
                CommandSQL.AppendLine("FROM TB_FRETE ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FRETE ON ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO_TIPO_FRETE = TB_TIPO_FRETE.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fretes.Add(FillEntity(Reader));
                }
                return new Retorno(Fretes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Frete Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Frete> Fretes = new List<Frete>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO AS CODIGO_FRETE, ");
                CommandSQL.AppendLine("TB_FRETE.TRANSPORTADORA, ");
                CommandSQL.AppendLine("TB_FRETE.VOLUME, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO AS CODIGO_TIPO_FRETE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CIDADE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.ESTADO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.VALOR ");
                CommandSQL.AppendLine("FROM TB_FRETE ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FRETE ON ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO_TIPO_FRETE = TB_TIPO_FRETE.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_FRETE.TRANSPORTADORA LIKE '%" + Entity.Transportadora + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fretes.Add(FillEntity(Reader));
                }
                return new Retorno(Fretes);
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
                List<Frete> Fretes = new List<Frete>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO AS CODIGO_FRETE, ");
                CommandSQL.AppendLine("TB_FRETE.TRANSPORTADORA, ");
                CommandSQL.AppendLine("TB_FRETE.VOLUME, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO AS CODIGO_TIPO_FRETE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CIDADE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.ESTADO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.VALOR ");
                CommandSQL.AppendLine("FROM TB_FRETE ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FRETE ON ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO_TIPO_FRETE = TB_TIPO_FRETE.CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Fretes.Add(FillEntity(Reader));
                }
                return new Retorno(Fretes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Frete Entity)
        {
            try
            {
                Frete Frete = new Frete();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO AS CODIGO_FRETE, ");
                CommandSQL.AppendLine("TB_FRETE.TRANSPORTADORA, ");
                CommandSQL.AppendLine("TB_FRETE.VOLUME, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CODIGO AS CODIGO_TIPO_FRETE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.DESCRICAO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.CIDADE, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.ESTADO, ");
                CommandSQL.AppendLine("TB_TIPO_FRETE.VALOR ");
                CommandSQL.AppendLine("FROM TB_FRETE ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FRETE ON ");
                CommandSQL.AppendLine("TB_FRETE.CODIGO_TIPO_FRETE = TB_TIPO_FRETE.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_FRETE.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Frete = FillEntity(Reader);
                }
                return new Retorno(Frete);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Frete Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_FRETE ");
                CommandSQL.AppendLine("WHERE TB_FRETE.TRANSPORTADORA = @TRANSPORTADORA ");
                CommandSQL.AppendLine("AND TB_FRETE.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@TRANSPORTADORA", Entity.Transportadora);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Frete", "Transportadora"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Frete FillEntity(IDataReader reader)
        {
            Frete Frete = new Frete();
            try
            {
                Frete.Codigo = ConverterValorReader(reader, "CODIGO_FRETE", 0);
                Frete.Transportadora = ConverterValorReader(reader, "TRANSPORTADORA", String.Empty);
                Frete.Volume = ConverterValorReader(reader, "VOLUME", 0);
                Frete.TipoFrete.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FRETE", 0);
                Frete.TipoFrete.Descricao = ConverterValorReader(reader, "DESCRICAO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Frete;
        }

        #endregion

    }
}

