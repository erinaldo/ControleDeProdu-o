using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class ClienteModel
    {
        public ClienteModel()
        {
            Cliente = Cliente ?? new Cliente();
            Retorno = Retorno ?? new Retorno();
        }

        public Cliente Cliente { get; set; }
        public Retorno Retorno { get; set; }
    }
}