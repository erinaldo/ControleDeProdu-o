using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace InfinitySolutions.Commom
{
    public class Internet
    {
        //Cria uma Função Externa
        [DllImport("wininet.dll")]
        extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool Conectado()
        {
            //Testa se existe conexão com a internet
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
    }
}
