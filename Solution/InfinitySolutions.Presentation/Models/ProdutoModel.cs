using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class ProdutoModel
    {
        public ProdutoModel()
        {
            Produto = Produto ?? new Produto();
            Retorno = Retorno ?? new Retorno();
            Dominios = Dominios ?? new DominiosDto();
            MateriaPrima = MateriaPrima ?? new MateriaPrima();
        }

        public Retorno Retorno { get; set; }
        public Produto Produto { get; set; }
        public MateriaPrima MateriaPrima { get; set; }
        public DominiosDto Dominios { get; internal set; }
    }
}