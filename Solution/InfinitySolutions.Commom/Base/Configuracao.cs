using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace InfinitySolutions.Commom
{
    public class Configuracao
    {
        public static string NOME_EMPRESA { get { return ConfigurationManager.AppSettings.Get(Constantes.NOME_EMPRESA); } }
        public static string TITULO_JANELA { get { return ConfigurationManager.AppSettings.Get(Constantes.TITULO_JANELA); } }
        public static string NOME_BANCO { get { return ConfigurationManager.AppSettings["BANCO"].ToString(); } }
        public static string CAMINHO_DUMP
        {
            get
            {
                if (Util.VerificarSistema64Bits())
                    return @"C:\Program Files (x86)\MySQL\MySQL Server 5.6\bin\mysqldump.exe";
                else
                    return @"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysqldump.exe";
            }
        }

        public static string LOGIN
        {
            get
            {
                return Seguranca.Descriptografar(ConfigurationManager.AppSettings["LOGIN"].ToString(), NOME_EMPRESA);
            }
        }

        public static string SENHA
        {
            get
            {
                return Seguranca.Descriptografar(ConfigurationManager.AppSettings["SENHA"].ToString(), NOME_EMPRESA);
            }
        }

        public static string SERVIDOR { get { return ConfigurationManager.AppSettings["SERVIDOR"].ToString(); } }

        public static string VERSAO { get { return ConfigurationManager.AppSettings["VERSAO"].ToString(); } }

        public static string LOCAL_ARQUIVO_BACKUP
        {
            get
            {
                return Seguranca.Descriptografar(ConfigurationManager.AppSettings["LOCAL_ARQUIVO_BACKUP"].ToString(), NOME_EMPRESA);
            }
        }

        public static int QNT_REGISTRO_PAGINA_RELATORIO { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_RELATORIO"].ConverteValor<int>(0); } }

        public static bool IMPRIME_CUPOM { get { return ConfigurationManager.AppSettings["IMPRIME_CUPOM"].ConverteValor<int>(0) == 1; } }

        public static int DIAS_AVISO { get { return ConfigurationManager.ConnectionStrings["DIAS_AVISO"].ConverteValor<int>(0); } }

        public static string DATA_BLOQUEIO { get { return ConfigurationManager.AppSettings["DATA_BLOQUEIO"].ConverteValor<string>(String.Empty); } }

        public static int QNT_REGISTRO_PAGINA_FALTA { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_FALTA"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_CLIENTE { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_CLIENTE"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_FECHAMENTO { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_FECHAMENTO"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_CONTA { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_CONTA"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_FUNCIONARIO { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_FUNCIONARIO"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_CONTAS_A_VENCER { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_CONTA_A_VENCER"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_PRODUTOS_VENDIDOS { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_PRODUTOS_VENDIDOS"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_PRODUTO { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_PRODUTO"].ConverteValor<int>(0); } }

        public static int QNT_REGISTRO_PAGINA_PRODUTO_VENDA { get { return ConfigurationManager.AppSettings["QNT_REGISTRO_PAGINA_PRODUTO_VENDA"].ConverteValor<int>(0); } }

        public static int DUAS_VIAS { get { return ConfigurationManager.AppSettings["DUAS_VIAS"].ConverteValor<int>(0); } }

        public static string CONNECTION_STRING
        {
            get
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ToString();

                    string[] elementos = connectionString.Split(';');
                    if (elementos != null && elementos.Count() > 4)
                    {
                        string senha = Seguranca.Descriptografar(elementos[3].Replace("Pwd=", string.Empty), Configuracao.NOME_EMPRESA);
                        string usuario = Seguranca.Descriptografar(elementos[2].Replace("Uid=", string.Empty), Configuracao.NOME_EMPRESA);
                        elementos[2] = "Uid=" + usuario;
                        elementos[3] = "Pwd=" + senha;

                        connectionString = string.Empty;
                        elementos.ToList().ForEach(e => connectionString += e + ";");
                    }

                    return connectionString;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
