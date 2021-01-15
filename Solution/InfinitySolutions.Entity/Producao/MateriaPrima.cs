using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class MateriaPrima
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Fornecedor { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Quantidade { get; set; }

        private TipoUnidadeMedida _tipounidademedida;
        public TipoUnidadeMedida TipoUnidadeMedida
        {
            get
            {
                if (_tipounidademedida == null)
                {
                    _tipounidademedida = new TipoUnidadeMedida();
                }
                return _tipounidademedida;
            }
            set
            { _tipounidademedida = value; }
        }
    }
}

