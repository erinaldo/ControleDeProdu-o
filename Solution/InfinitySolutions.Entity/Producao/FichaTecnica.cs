using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class FichaTecnica
    {
        public int Codigo { get; set; }
        public byte[] Foto { get; set; }

        public string Foto64
        {
            get
            {
                if (Foto != null)
                    return Convert.ToBase64String(Foto);

                return String.Empty;
            }
        }

        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public string Ncm { get; set; }
        public string Descricao { get; set; }
        public string Tecido { get; set; }
        public string LinhaUm { get; set; }
        public string LinhaDois { get; set; }
        public string Cor { get; set; }
    }
}

