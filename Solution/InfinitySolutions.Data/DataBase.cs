using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using InfinitySolutions.Commom;
using InfinitySolutions.Entity;

namespace InfinitySolutions.Data
{
    public class DataBase
    {
        protected MySqlConnection Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ToString());
        protected MySqlCommand Command;
        protected StringBuilder CommandSQL;
        protected MySqlDataReader Reader;

        protected void Abrir()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        protected void Fechar()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        protected MySqlCommand CriaComandoSQL(string ComandoSql)
        {
            return new MySqlCommand(ComandoSql, Connection);
        }

        public Retorno TotalRegistros(string NomeTabela, string ChavePrimaria, string where)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT COUNT(" + ChavePrimaria + ") AS TOTAL ");
                CommandSQL.AppendLine("FROM " + NomeTabela + " ");
                CommandSQL.AppendLine(where);

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    return new Retorno(Convert.ToInt32(reader["TOTAL"]));
                }
            }
            catch (Exception ex)
            {
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(new Erro()
                    {
                        Descricao = ex.Message,
                        Imagem = Guid.NewGuid().ToString(),
                        CasoDeUso = EnumCasoDeUso.BASE,
                        Camada = EnumCamada.DATA,
                        Funcionalidade = EnumFuncionalidade.TOTAL_REGISTRO,
                        Entidade = "Total de Registros"
                    }
                    );
                }
                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Recuperar Total Registros"));
            }
            finally { Fechar(); }

            return new Retorno(0);
        }

        public Retorno CarregaUltimoRegistro(string tabela, string chavePrimaria)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.Append("SELECT MAX(" + chavePrimaria + ") FROM " + tabela);

                Command = CriaComandoSQL(CommandSQL.ToString());


                return new Retorno(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(new Erro()
                    {
                        Descricao = ex.Message,
                        Imagem = Guid.NewGuid().ToString(),
                        CasoDeUso = EnumCasoDeUso.BASE,
                        Camada = EnumCamada.DATA,
                        Funcionalidade = EnumFuncionalidade.RECUPERA_ULTIMO_REGISTRO,
                        Entidade = "Ultimo Registro"
                    }
                    );
                }
                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Recuperar Ultimo Registro"));
            }
        }

        public Retorno VerificaConexao()
        {
            try
            {
                Abrir();
                Fechar();
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                if (Internet.Conectado())
                {
                    Erro erro = new Erro()
                    {
                        Descricao = ex.Message,
                        Imagem = Guid.NewGuid().ToString(),
                        CasoDeUso = EnumCasoDeUso.BASE,
                        Camada = EnumCamada.DATA,
                        Funcionalidade = EnumFuncionalidade.VERIFICA_CONEXAO,
                        Entidade = "Conexão"
                    };

                    Util.Print(erro);

                    if (Internet.Conectado())
                    {
                        ISEmail.EnviarErro(erro);
                    }
                }
                return new Retorno(false, String.Format(Mensagens.MSG_03, ex.Message));
            }
        }
    }
}
