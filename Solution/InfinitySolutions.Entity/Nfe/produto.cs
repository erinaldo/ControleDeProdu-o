using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Nfe
{
    public class produto
    {
        public decimal totalDecimal;

        public string nome { get; set; }
        public string codigo { get; set; }
        public string ncm { get; set; }
        public string cest { get; set; }
        public int quantidade { get; set; }
        public string unidade { get; set; }
        public string peso { get; set; }
        public int origem { get; set; }
        public string subtotal { get; set; }
        public string total { get; set; }
        public string classe_imposto { get; set; }
    }
}
