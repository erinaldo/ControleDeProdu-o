using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class ContasReceberModel
    {
        public ContasReceberModel()
        {
            Retorno = Retorno ?? new Retorno();
            ContaReceber = ContaReceber ?? new ContaReceber();
            Dominios = Dominios ?? new DominiosDto();
        }

        public Retorno Retorno { get; set; }
        public DominiosDto Dominios { get; set; }

        public ContaReceber ContaReceber { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public decimal Total
        {
            get
            {
                var contas = Retorno.Entity as List<ContaReceber>;

                if (contas != null)
                    return contas.Sum(c => c.Valor).Value;

                return 0;
            }
        }
    }
}