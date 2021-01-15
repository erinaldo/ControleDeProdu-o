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
        protected MySqlConnection Connection = new MySqlConnection(Configuracao.CONNECTION_STRING);
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

        public Retorno RecuperarTotalRegistros(string NomeTabela, string ChavePrimaria, string where)
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
                throw ex;
            }
            finally { Fechar(); }

            return new Retorno(0);
        }

        public Retorno VerificarConexao()
        {
            try
            {
                Abrir();
                Fechar();
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T ConverterValor<T>(object value, T defaultValue)
        {
            var requestedType = typeof(T);

            if (value != null && requestedType.Name == "Decimal")
                value = value.ToString().Replace(".", ",");
            T result;
            try
            {

                var nullableType = Nullable.GetUnderlyingType(requestedType);

                if (nullableType != null)
                {
                    if (value == null)
                    {
                        result = defaultValue;
                    }
                    else
                    {
                        result = (T)Convert.ChangeType(value, nullableType);
                    }
                }
                else
                {
                    result = (T)Convert.ChangeType(value, requestedType);
                }
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        public T ConverterValorReader<T>(IDataReader reader, string nomeReader, T defaultValue)
        {
            try
            {
                return ConverterValor<T>(reader[nomeReader], defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
