using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Commom
{
    public class Seguranca
    {
        public static string Criptografar(string Data, string Cliente)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Constantes.SENHA_CODIFICACAO + Cliente));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Data);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        public static string Descriptografar(string dados, string Cliente)
        {
            try
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Constantes.SENHA_CODIFICACAO + Cliente));
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] DataToDecrypt = Convert.FromBase64String(dados);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }
                return UTF8.GetString(Results);
            }
            catch { return String.Empty; }
        }

        public static Retorno GerarBackup(string caminho = "")
        {
            try
            {
                string[] elementos = Configuracao.CONNECTION_STRING.Split(';');
                var servidor = elementos[0].Split('=')[1];
                var dataBase = elementos[1].Split('=')[1];
                var usuario = elementos[2].Split('=')[1];
                var senha = elementos[3].Split('=')[1];

                string nomeDoArquivo = "BACKUP_" + Configuracao.NOME_EMPRESA + "_" + Configuracao.VERSAO + "_" + dataBase + "_" + DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year + ".sql";

                if (String.IsNullOrEmpty(caminho))
                {
                    caminho = Configuracao.LOCAL_ARQUIVO_BACKUP;

                    if (String.IsNullOrEmpty(caminho))
                        caminho = Environment.CurrentDirectory + "/";
                }

                caminho += nomeDoArquivo;

                ProcessStartInfo informacoesProcesso = new ProcessStartInfo();
                informacoesProcesso.FileName = Configuracao.CAMINHO_DUMP;
                informacoesProcesso.RedirectStandardInput = false;
                informacoesProcesso.RedirectStandardOutput = true;
                informacoesProcesso.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", usuario, senha, servidor, dataBase);
                informacoesProcesso.UseShellExecute = false;

                informacoesProcesso.CreateNoWindow = true;

                Process processo = Process.Start(informacoesProcesso);
                string informacoesBanco = processo.StandardOutput.ReadToEnd();

                StreamWriter arquivo = new StreamWriter(caminho);
                arquivo.WriteLine("SET FOREIGN_KEY_CHECKS=0;");
                arquivo.WriteLine(informacoesBanco);
                arquivo.WriteLine("SET FOREIGN_KEY_CHECKS=1;");
                arquivo.Close();
                processo.WaitForExit();

                return new Retorno(caminho);

            }
            catch (Exception ex)
            {
                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Gerar backup"));
            }
        }
    }
}
