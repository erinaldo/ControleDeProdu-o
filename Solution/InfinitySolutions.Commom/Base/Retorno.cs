using System;
using System.Diagnostics;
using System.Threading;

namespace InfinitySolutions.Commom
{
    public class Retorno
    {
        public string Mensagem { get; set; }
        public object Entity { get; set; }
        public bool IsValido { get; set; }
        public static Exception Excecao { get; set; }

        #region CONSTRUTORES

        public Retorno()
        {
            
        }

        public Retorno(bool isValido)
        {
            IsValido = isValido;
        }

        public Retorno(bool isValido, string mensagem)
        {
            IsValido = isValido;
            Mensagem = mensagem;
        }

        public Retorno(object entity)
        {
            IsValido = true;
            Entity = entity;
        }

        #endregion

        #region TRATAMENTO EXCEÇÃO

        public static Retorno CriarRetornoExcecao(Exception excecao)
        {
            return new Retorno(false, String.Format(Mensagens.MSG_05, excecao.TargetSite.Name, excecao.Message));
        }

        #endregion
    }
}
