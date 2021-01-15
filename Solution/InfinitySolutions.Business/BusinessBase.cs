using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinitySolutions.Commom;
using InfinitySolutions.Data;

namespace InfinitySolutions.Business
{
    public class BusinessBase
    {
        public static Retorno TotalRegistros(string NomeTabela, string ChavePrimaria, string where)
        {
            return new DataBase().TotalRegistros(NomeTabela, ChavePrimaria, where);
        }

        public static Retorno VerificaConexao()
        {
            return new DataBase().VerificaConexao();
        }
    }
}
