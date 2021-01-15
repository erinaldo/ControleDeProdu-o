using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class TabelaPreco
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal? Imposto { get; set; }
        public decimal? Lucro { get; set; }
        public DateTime Data { get; set; }
        public bool Especial { get; set; }
    }
}

