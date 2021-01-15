using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class TipoUnidadeMedida
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }

        public string DescricaoSigla { get { return String.Format("{0} - {1}", Descricao, Sigla); } }
    }
}

