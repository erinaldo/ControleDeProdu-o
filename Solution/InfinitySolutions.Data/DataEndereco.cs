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

    public class DataEndereco : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Endereco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_ENDERECO( ");
                CommandSQL.AppendLine("RUA, ");
                CommandSQL.AppendLine("NUMERO, ");
                CommandSQL.AppendLine("BAIRRO, ");
                CommandSQL.AppendLine("CIDADE, ");
                CommandSQL.AppendLine("CEP, ");
                CommandSQL.AppendLine("UF) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@RUA, ");
                CommandSQL.AppendLine("@NUMERO, ");
                CommandSQL.AppendLine("@BAIRRO, ");
                CommandSQL.AppendLine("@CIDADE, ");
                CommandSQL.AppendLine("@CEP, ");
                CommandSQL.AppendLine("@UF); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@RUA", Entity.Rua);
                Command.Parameters.AddWithValue("@NUMERO", Entity.Numero);
                Command.Parameters.AddWithValue("@BAIRRO", Entity.Bairro);
                Command.Parameters.AddWithValue("@CIDADE", Entity.Cidade);
                Command.Parameters.AddWithValue("@CEP", Entity.Cep);
                Command.Parameters.AddWithValue("@UF", Entity.Uf);

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

        public Retorno Excluir(Endereco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_ENDERECO WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Endereco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_ENDERECO SET ");
                CommandSQL.AppendLine("RUA = @RUA, ");
                CommandSQL.AppendLine("NUMERO = @NUMERO, ");
                CommandSQL.AppendLine("BAIRRO = @BAIRRO, ");
                CommandSQL.AppendLine("CEP = @CEP, ");
                CommandSQL.AppendLine("UF = @UF, ");
                CommandSQL.AppendLine("CIDADE = @CIDADE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@RUA", Entity.Rua);
                Command.Parameters.AddWithValue("@NUMERO", Entity.Numero);
                Command.Parameters.AddWithValue("@BAIRRO", Entity.Bairro);
                Command.Parameters.AddWithValue("@CEP", Entity.Cep);
                Command.Parameters.AddWithValue("@CIDADE", Entity.Cidade);
                Command.Parameters.AddWithValue("@UF", Entity.Uf);
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
                List<Endereco> Enderecos = new List<Endereco>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.UF ");
                CommandSQL.AppendLine("FROM TB_ENDERECO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Enderecos.Add(FillEntity(Reader));
                }
                return new Retorno(Enderecos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Endereco Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Endereco> Enderecos = new List<Endereco>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE ");
                CommandSQL.AppendLine("FROM TB_ENDERECO ");

                CommandSQL.AppendLine("WHERE (TB_ENDERECO.RUA LIKE '%" + Entity.Rua + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Enderecos.Add(FillEntity(Reader));
                }
                return new Retorno(Enderecos);
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
                List<Endereco> Enderecos = new List<Endereco>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE ");
                CommandSQL.AppendLine("FROM TB_ENDERECO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Enderecos.Add(FillEntity(Reader));
                }
                return new Retorno(Enderecos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Endereco Entity)
        {
            try
            {
                Endereco Endereco = new Endereco();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE ");
                CommandSQL.AppendLine("FROM TB_ENDERECO ");

                CommandSQL.AppendLine("WHERE TB_ENDERECO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Endereco = FillEntity(Reader);
                }
                return new Retorno(Endereco);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Endereco Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_ENDERECO ");
                CommandSQL.AppendLine("WHERE TB_ENDERECO.RUA = @RUA ");
                CommandSQL.AppendLine("AND TB_ENDERECO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@RUA", Entity.Rua);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Endereco", "Rua"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Endereco FillEntity(IDataReader reader)
        {
            Endereco Endereco = new Endereco();
            try
            {
                Endereco.Codigo = ConverterValorReader(reader, "CODIGO", 0);
                Endereco.Rua = ConverterValorReader(reader, "RUA", String.Empty);
                Endereco.Numero = ConverterValorReader(reader, "NUMERO", String.Empty);
                Endereco.Bairro = ConverterValorReader(reader, "BAIRRO", String.Empty);
                Endereco.Cidade = ConverterValorReader(reader, "CIDADE", String.Empty);
                Endereco.Uf = ConverterValorReader(reader, "UF", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Endereco;
        }

        #endregion

    }
}

