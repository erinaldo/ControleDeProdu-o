using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Nfe
{
    public class fatura
    {
        public string numero { get; set; }
        public string valor { get; set; }
        public string desconto { get; set; }
        public string valor_liquido { get; set; }
    }
}
