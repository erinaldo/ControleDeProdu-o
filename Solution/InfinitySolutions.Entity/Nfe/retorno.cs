using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Nfe
{
    public class retorno
    {

        public string uuid { get; set; }
        public string status { get; set; }
        public string nfe { get; set; }
        public string serie { get; set; }
        public string chave { get; set; }
        public string modelo { get; set; }
        public string xml { get; set; }
        public string danfe { get; set; }

        public log log { get; set; }

        public string recibo { get; set; }
        public string error { get; set; }

    }
}
