using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class FichaTecnicaModel
    {
        public FichaTecnicaModel()
        {
            Produto = Produto ?? new Produto();
        }

        public Produto Produto { get; set; }

    }
}