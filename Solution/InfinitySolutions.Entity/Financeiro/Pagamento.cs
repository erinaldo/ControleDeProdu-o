using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Pagamento
    {
        public int Codigo { get; set; }
        public decimal Valor { get; set; }
        public decimal Juros { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataPagamento { get; set; }

        private ContaPagar _contaPagar;
        public ContaPagar ContaPagar
        {
            get
            {
                if (_contaPagar == null)
                    _contaPagar = new ContaPagar();

                return _contaPagar;
            }
            set { _contaPagar = value; }
        }

    }
}

