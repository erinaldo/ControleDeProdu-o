using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using InfinitySolutions.Commom;

namespace InfinitySolutions
{
    public class InstaladorBanco
    {
        public static string CaminhoBanco
        {
            get
            {
                if (Util.VerificarSistema64Bits())
                    return @"C:\Program Files (x86)\MySQL\MySQL Server 5.6\bin\{0}";
                else
                    return @"C:\Program Files\MySQL\MySQL Server 5.6\bin\{0}";
            }
        }

        public static string CaminhoInstaladorBanco
        {
            get
            {
                return Environment.CurrentDirectory + "\\mysql-5.5.28-win32.msi";
            }
        }

        public static void InstalarBanco()
        {
            ProcessStartInfo informacoesProcesso = new ProcessStartInfo();
            informacoesProcesso.FileName = "msiexec.exe";
            informacoesProcesso.Arguments = string.Format("/i \"{0}\" /qn ", CaminhoInstaladorBanco);
            informacoesProcesso.UseShellExecute = true;

            informacoesProcesso.CreateNoWindow = true;

            Process processo = Process.Start(informacoesProcesso);
            processo.WaitForExit();
            processo.Close();
        }

        public static void ConfigurarBanco()
        {
            ProcessStartInfo informacoesProcesso = new ProcessStartInfo();

            informacoesProcesso.FileName = String.Format(CaminhoBanco, "MySQLInstanceConfig.exe");
            informacoesProcesso.Arguments = "-i -q ServiceName=MySQL RootPassword=caio ServerType=SERVER DatabaseType=INODB Port=3306 Charset=utf8";
            informacoesProcesso.UseShellExecute = true;

            informacoesProcesso.CreateNoWindow = true;

            Process processo = Process.Start(informacoesProcesso);
            processo.WaitForExit();
            processo.Close();
        }

        public static void ExecutarScript()
        {
            StreamReader file = new StreamReader(Environment.CurrentDirectory + "\\Backup.sql");
            string input = file.ReadToEnd();
            file.Close();

            ProcessStartInfo informacoesProcesso = new ProcessStartInfo();
            informacoesProcesso.FileName = string.Format(CaminhoBanco, "mysql.exe");
            informacoesProcesso.RedirectStandardInput = true;
            informacoesProcesso.RedirectStandardOutput = false;
            informacoesProcesso.Arguments = "-uroot -pcaio -hlocalhost";
            informacoesProcesso.UseShellExecute = false;

            informacoesProcesso.CreateNoWindow = true;

            Console.WriteLine(informacoesProcesso);

            Process process = Process.Start(informacoesProcesso);
            process.StandardInput.WriteLine(input);
            process.StandardInput.Close();

            process.WaitForExit();
            process.Close();
        }
    }
}
