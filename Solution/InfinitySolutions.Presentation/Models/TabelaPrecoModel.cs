using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class TabelaPrecoModel
    {
        public TabelaPrecoModel()
        {
            TabelaPreco = TabelaPreco ?? new TabelaPreco();
            Retorno = Retorno ?? new Retorno();
        }

        public TabelaPreco TabelaPreco { get; set; }
        public Retorno Retorno { get; set; }
    }
}