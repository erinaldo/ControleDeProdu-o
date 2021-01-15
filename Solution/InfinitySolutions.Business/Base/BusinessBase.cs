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
        public static Retorno RecuperarTotalRegistros(string NomeTabela, string ChavePrimaria, string where)
        {
            return new DataBase().RecuperarTotalRegistros(NomeTabela, ChavePrimaria, where);
        }

        public static Retorno VerificarConexao()
        {
            try
            {
                return new DataBase().VerificarConexao();
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }

        }
        protected List<T> RecuperarDominio<T>(Retorno retorno)
        {
            try
            {
                if (retorno.IsValido)
                {
                    return retorno.Entity as List<T>;
                }

                throw new Exception(retorno.Mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
