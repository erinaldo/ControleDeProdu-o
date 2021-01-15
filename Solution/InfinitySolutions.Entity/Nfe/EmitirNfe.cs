using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfinitySolutions.Entity.Nfe
{
    public class EmitirNfe
    {
        public EmitirNfe()
        {
            cliente = cliente ?? new cliente();
            produtos = produtos ?? new List<produto>();
            pedido = pedido ?? new pedido();
        }

        public int ID { get; set; }
        public string url_notificacao { get; set; }
        public int operacao { get; set; }
        public string natureza_operacao { get; set; }
        public int modelo { get; set; }
        public int finalidade { get; set; }
        public int ambiente { get; set; }

        public cliente cliente { get; set; }
        public List<produto> produtos { get; set; }
        public pedido pedido { get; set; }
        public parcela[] parcelas { get; set; }
        public fatura fatura { get; set; }
        public transporte transporte { get; set; }
        public transportadora transportadora { get; set; }
    }
}
