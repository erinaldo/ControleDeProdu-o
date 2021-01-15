using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace InfinitySolutions.Commom
{
    public class MetodosDaruma
    {
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusImpressora_DUAL_DarumaFramework();

        #region Métodos DUAL

        public static string NaoFiscal_Mostrar_Retorno(int iRetorno)
        {
            string vRet = string.Empty;

            if (iRetorno == 1 | iRetorno == 0 | iRetorno == -27 | iRetorno == -50 | iRetorno == 51 | iRetorno == 52)
                switch (iRetorno)
                {
                    case 0: vRet = "0(zero) - Impressora Desligada!";
                        break;
                    //case 1: TB_Status.Text = "1(um) - Impressora OK!";
                    //    break;
                    case -50: vRet = "-50 - Impressora OFF-LINE!";
                        break;
                    case -51: vRet = "-51 - Impressora Sem Papel!";
                        break;
                    case -27: vRet = "-27 - Erro Generico!";
                        break;
                    case -52: vRet = "-52 - Impressora inicializando!";
                        break;
                }

            else
            {
                vRet = "Retorno não esperado!";
            }

            return vRet;
        }
        //*************Métodos para Impressoras Dual*************

        [DllImport("DarumaFrameWork.dll")]
        public static extern int iEnviarBMP_DUAL_DarumaFramework(string stArqOrigem);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iImprimirArquivo_DUAL_DarumaFramework(string stPath);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iAcionarGaveta_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusGaveta_DUAL_DarumaFramework(ref int iStatusGaveta);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int rStatusDocumento_DUAL_DarumaFramework();
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regVelocidade_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regTermica_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regTabulacao_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regPortaComunicacao_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regModoGaveta_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regLinhasGuilhotina_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regEnterFinal_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regAguardarProcesso_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iImprimirTexto_DUAL_DarumaFramework(string stTexto, int iTam);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int iAutenticarDocumento_DUAL_DarumaFramework(string stTexto, string stLocal, string stTimeOut);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regCodePageAutomatico_DUAL_DarumaFramework(System.String stParametro);
        [DllImport("DarumaFrameWork.dll")]
        public static extern int regZeroCortado_DUAL_DarumaFramework(System.String stParametro);

        #endregion

        public static void AbreGaveta()
        {
            try
            {
                int StatusGaveta = 0;

                MetodosDaruma.rStatusGaveta_DUAL_DarumaFramework(ref StatusGaveta);

                if (StatusGaveta == 0)
                    MetodosDaruma.iAcionarGaveta_DUAL_DarumaFramework();
            }
            catch { }
        }
    }
}