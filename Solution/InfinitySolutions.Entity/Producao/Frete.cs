using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Frete
    {
        public int Codigo { get; set; }
        public string Transportadora { get; set; }
        public int Volume { get; set; }
        private TipoFrete _tipofrete;
        public TipoFrete TipoFrete
        {
            get
            {
                if (_tipofrete == null)
                {
                    _tipofrete = new TipoFrete();
                }
                return _tipofrete;
            }
            set
            { _tipofrete = value; }
        }
    }
}

