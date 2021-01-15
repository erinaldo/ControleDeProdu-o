using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Nfe
{
    public class pedido
    {
        public int pagamento { get; set; }
        public int presenca { get; set; }
        public int modalidade_frete { get; set; }
        public string frete { get; set; }
        public string desconto { get; set; }
        public string total { get; set; }
        public int forma_pagamento { get; set; }
        public string informacoes_complementares { get; set; }
    }
}
