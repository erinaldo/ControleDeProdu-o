using InfinitySolutions.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class NotasFiscaisModel
    {
        private Retorno _retorno;
        public Retorno Retorno
        {
            get
            {
                if (_retorno == null)
                    _retorno = new Retorno();

                return _retorno;
            }
            set { _retorno = value; }
        }
    }
}