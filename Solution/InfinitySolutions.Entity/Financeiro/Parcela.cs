using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Parcela
    {
        public int Codigo { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public int Numero { get; set; }

        private TipoStatusParcela _status;
        public TipoStatusParcela Status
        {
            get
            {
                if (_status == null)
                {
                    _status = new TipoStatusParcela();
                }
                return _status;
            }
            set
            { _status = value; }
        }

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

