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

    public class DataCliente : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Cliente Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_CLIENTE( ");
                CommandSQL.AppendLine("NOME, ");
                CommandSQL.AppendLine("NOME_FANTASIA, ");
                CommandSQL.AppendLine("CNPJ, ");
                CommandSQL.AppendLine("CONTATO, ");
                CommandSQL.AppendLine("IE, ");
                CommandSQL.AppendLine("CODIGO_ENDERECO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NOME, ");
                CommandSQL.AppendLine("@NOME_FANTASIA, ");
                CommandSQL.AppendLine("@CNPJ, ");
                CommandSQL.AppendLine("@CONTATO, ");
                CommandSQL.AppendLine("@IE, ");
                CommandSQL.AppendLine("@CODIGO_ENDERECO) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NOME", Entity.Nome.ToUpper());
                Command.Parameters.AddWithValue("@NOME_FANTASIA", Entity.NomeFantasia != null ? (object)Entity.NomeFantasia.ToUpper() : DBNull.Value);
                Command.Parameters.AddWithValue("@CNPJ", Entity.Cnpj);
                Command.Parameters.AddWithValue("@CONTATO", Entity.Contato);
                Command.Parameters.AddWithValue("@IE", Entity.Ie);
                Command.Parameters.AddWithValue("@CODIGO_ENDERECO", Entity.Endereco.Codigo);
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

        public Retorno Excluir(Cliente Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_CLIENTE WHERE CODIGO = @CODIGO");
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
                    CommandSQL.AppendLine("UPDATE TB_CLIENTE SET ATIVO = 0 WHERE CODIGO = @CODIGO");
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

        public Retorno Alterar(Cliente Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_CLIENTE SET ");
                CommandSQL.AppendLine("NOME = @NOME, ");
                CommandSQL.AppendLine("NOME_FANTASIA = @NOME_FANTASIA, ");
                CommandSQL.AppendLine("CNPJ = @CNPJ, ");
                CommandSQL.AppendLine("CONTATO = @CONTATO, ");
                CommandSQL.AppendLine("IE = @IE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Command.Parameters.AddWithValue("@NOME", Entity.Nome.ToUpper());
                Command.Parameters.AddWithValue("@NOME_FANTASIA", String.IsNullOrEmpty(Entity.NomeFantasia) ? DBNull.Value : (object)Entity.NomeFantasia.ToUpper());
                Command.Parameters.AddWithValue("@CNPJ", Entity.Cnpj);
                Command.Parameters.AddWithValue("@CONTATO", Entity.Contato);
                Command.Parameters.AddWithValue("@IE", Entity.Ie);
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
                List<Cliente> Clientes = new List<Cliente>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_ENDERECO.ESTADO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO AS CODIGO_ENDERECO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.COMPLEMENTO ");
                CommandSQL.AppendLine("FROM TB_CLIENTE ");
                CommandSQL.AppendLine("INNER JOIN TB_ENDERECO ON ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO_ENDERECO = TB_ENDERECO.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Clientes.Add(FillEntity(Reader));
                }
                return new Retorno(Clientes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Cliente Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Cliente> Clientes = new List<Cliente>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_ENDERECO.ESTADO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO AS CODIGO_ENDERECO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.COMPLEMENTO ");
                CommandSQL.AppendLine("FROM TB_CLIENTE ");
                CommandSQL.AppendLine("INNER JOIN TB_ENDERECO ON ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO_ENDERECO = TB_ENDERECO.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_CLIENTE.NOME LIKE '%" + Entity.Nome + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Clientes.Add(FillEntity(Reader));
                }
                return new Retorno(Clientes);
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
                List<Cliente> Clientes = new List<Cliente>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO AS CODIGO_ENDERECO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.UF ");
                CommandSQL.AppendLine("FROM TB_CLIENTE ");
                CommandSQL.AppendLine("INNER JOIN TB_ENDERECO ON ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO_ENDERECO = TB_ENDERECO.CODIGO ");
                CommandSQL.AppendLine("WHERE TB_CLIENTE.ATIVO = 1 ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Clientes.Add(FillEntity(Reader));
                }
                return new Retorno(Clientes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Cliente Entity)
        {
            try
            {
                Cliente Cliente = new Cliente();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_CLIENTE.IE, ");
                CommandSQL.AppendLine("TB_ENDERECO.UF, ");
                CommandSQL.AppendLine("TB_ENDERECO.CODIGO AS CODIGO_ENDERECO, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.CEP ");
                CommandSQL.AppendLine("FROM TB_CLIENTE ");
                CommandSQL.AppendLine("INNER JOIN TB_ENDERECO ON ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO_ENDERECO = TB_ENDERECO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_CLIENTE.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Cliente = FillEntity(Reader);
                }
                return new Retorno(Cliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Cliente Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_CLIENTE ");
                CommandSQL.AppendLine("WHERE TB_CLIENTE.NOME = @NOME ");
                CommandSQL.AppendLine("AND TB_CLIENTE.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NOME", Entity.Nome);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Cliente", "Nome"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Cliente FillEntity(IDataReader reader)
        {
            Cliente Cliente = new Cliente();
            try
            {
                Cliente.Codigo = ConverterValorReader(reader, "CODIGO_CLIENTE", 0);
                Cliente.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                Cliente.NomeFantasia = ConverterValorReader(reader, "NOME_FANTASIA", String.Empty);
                Cliente.Cnpj = ConverterValorReader(reader, "CNPJ", 0M);
                Cliente.Contato = ConverterValorReader(reader, "CONTATO", String.Empty);
                Cliente.Ie = ConverterValorReader(reader, "IE", 0M);
                Cliente.Endereco.Codigo = ConverterValorReader(reader, "CODIGO_ENDERECO", 0);
                Cliente.Endereco.Rua = ConverterValorReader(reader, "RUA", String.Empty);
                Cliente.Endereco.Bairro = ConverterValorReader(reader, "BAIRRO", String.Empty);
                Cliente.Endereco.Cidade = ConverterValorReader(reader, "CIDADE", String.Empty);
                Cliente.Endereco.Numero = ConverterValorReader(reader, "NUMERO", String.Empty);
                Cliente.Endereco.Uf = ConverterValorReader(reader, "UF", String.Empty);
                Cliente.Endereco.Cep = ConverterValorReader(reader, "CEP", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Cliente;
        }

        #endregion

    }
}

