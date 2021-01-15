using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class FornecedorModel
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


        private Fornecedor _fornecedor;
        public Fornecedor Fornecedor
        {
            get
            {
                if (_fornecedor == null)
                    _fornecedor = new Fornecedor();

                return _fornecedor;
            }
            set { _fornecedor = value; }
        }
    }
}