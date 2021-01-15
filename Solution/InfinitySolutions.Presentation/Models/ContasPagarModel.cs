using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class ContasPagarModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Total
        {
            get
            {
                var contas = Retorno.Entity as List<ContaPagar>;

                if (contas != null)
                    return contas.Sum(c => c.Valor).Value;

                return 0;
            }
        }

        private Retorno _retorno;
        public Retorno Retorno
        {
            get
            {
                if (_retorno == null)
                    _retorno = new Retorno();
                return _retorno;
            }
            set { _retorno = value; }
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

        private DominiosDto _dominios;
        public DominiosDto Dominios
        {
            get
            {
                if (_dominios == null)
                    _dominios = new DominiosDto();
                return _dominios;
            }
            set { _dominios = value; }
        }

        private Pagamento _pagamento;
        public Pagamento Pagamento
        {
            get { return _pagamento; }
            set { _pagamento = value; }
        }

    }
}