using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class TipoFormaPagamento
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public bool ContemParcelas { get; set; }
        public int? Dias { get; set; }
        public bool DataCombinada { get; set; }
    }
}

