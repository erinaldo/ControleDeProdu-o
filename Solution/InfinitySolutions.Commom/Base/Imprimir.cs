using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using InfinitySolutions.Entity;

namespace InfinitySolutions.Commom
{
    public class Imprimir
    {
        #region MANIPULAÇÃO ARQUIVO

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private const int WM_CLOSE = 0x010;

        private static bool ArquivoAberto(string filePath)
        {
            bool fileOpened = false;
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenWrite(filePath);
                fs.Close();
            }
            catch (System.IO.IOException)
            {
                fileOpened = true;
            }

            return fileOpened;
        }

        private static void FechaAquivo(string tituloJanela)
        {
            IntPtr h = FindWindow(null, tituloJanela);
            if (h != IntPtr.Zero)
            {
                PostMessage(h, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #endregion
    }
}
