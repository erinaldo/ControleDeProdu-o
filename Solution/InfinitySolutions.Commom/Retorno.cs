using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Commom
{
    /// <summary>
    /// Classe de Retorno de Metodos, tipagem sera atribuida a Entity.
    /// </summary>
    public class Retorno
    {
        public string Mensagem { get; set; }
        public object Entity { get; set; }
        public bool IsValido { get; set; }
        public bool Exception { get; set; }

        public Retorno() { }

        public Retorno(bool isValido)
        {
            IsValido = isValido;
        }

        public Retorno(bool isValido, string mensagem)
        {
            IsValido = isValido;
            Mensagem = mensagem;
        }

        public Retorno(bool isValido, string mensagem, bool exception)
        {
            Exception = exception;
            IsValido = isValido;
            Mensagem = mensagem;
        }

        public Retorno(object entity)
        {
            IsValido = true;
            Entity = entity;
        }
    }
}
