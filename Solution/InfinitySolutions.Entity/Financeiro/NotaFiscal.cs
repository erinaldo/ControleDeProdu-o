using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity
{

    public class NotaFiscal
    {
        public int Codigo { get; set; }
        public string Numero { get { return CodigoNotaFiscal.Substring(25, 9); } }
        public string CodigoNotaFiscal { get; set; }
        public string Observacao { get; set; }


        private Pedido _pedido;
        public Pedido Pedido
        {
            get
            {
                if (_pedido == null)
                    _pedido = new Pedido();

                return _pedido;
            }
            set { _pedido = value; }
        }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
