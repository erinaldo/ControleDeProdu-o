using InfinitySolutions.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class ContaReceber
    {
        public int Codigo { get; set; }

        public string NotaFiscal { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public EnumContaReceberStatus Status { get; set; }

        private Cliente _cliente;
        public Cliente Cliente
        {
            get
            {
                if (_cliente == null)
                    _cliente = new Cliente();

                return _cliente;
            }
            set { _cliente = value; }
        }

        private Pedido _pedido;
        public Pedido Pedido
        {
            get
            {
                if (_pedido == null)
                    _pedido = new Pedido();

                return _pedido;
            }
            set { _pedido = value; }
        }

        public string PedidoExterno { get; set; }

        public string NotaFiscalExterna { get; set; }

        private TipoFormaPagamentoContaReceber _tipoFormaPagamentoContaReceber;
        public TipoFormaPagamentoContaReceber TipoFormaPagamentoContaReceber
        {
            get
            {
                if (_tipoFormaPagamentoContaReceber == null)
                    _tipoFormaPagamentoContaReceber = new TipoFormaPagamentoContaReceber();
                
                return _tipoFormaPagamentoContaReceber;
            }
            set { _tipoFormaPagamentoContaReceber = value; }
        }
    }
}

