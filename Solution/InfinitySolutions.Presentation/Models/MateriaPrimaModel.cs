using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class MateriaPrimaModel
    {

        public MateriaPrimaModel()
        {
            TiposUnidadeMedida = TiposUnidadeMedida ?? new List<TipoUnidadeMedida>();
            Dominios = Dominios ?? new DominiosDto();
            MateriaPrima = MateriaPrima ?? new MateriaPrima();
            Retorno = Retorno ?? new Retorno();
        }

        public List<TipoUnidadeMedida> TiposUnidadeMedida { get; set; }

        public DominiosDto Dominios { get; set; }
        public MateriaPrima MateriaPrima { get; set; }
        public Retorno Retorno { get; set; }
    }
}