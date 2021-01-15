using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class ContaPagar : ICloneable
    {
        public int Codigo { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal? Valor { get; set; }
        public int? QuantidadeParcelas { get; set; }
        public string Descricao { get; set; }

        private Fornecedor _fornecedor;
        public Fornecedor Fornecedor
        {
            get
            {
                if (_fornecedor == null)
                {
                    _fornecedor = new Fornecedor();
                }
                return _fornecedor;
            }
            set
            { _fornecedor = value; }
        }


        private TipoStatusContaPagar _tipoStatusContaPagar;
        public TipoStatusContaPagar TipoStatusContaPagar
        {
            get
            {
                if (_tipoStatusContaPagar == null)
                {
                    _tipoStatusContaPagar = new TipoStatusContaPagar();
                }
                return _tipoStatusContaPagar;
            }
            set
            { _tipoStatusContaPagar = value; }
        }
        private TipoContaPagar _tipocontapagar;


        public TipoContaPagar TipoContaPagar
        {
            get
            {
                if (_tipocontapagar == null)
                {
                    _tipocontapagar = new TipoContaPagar();
                }
                return _tipocontapagar;
            }
            set
            { _tipocontapagar = value; }
        }
        private TipoFormaPagamentoContaPagar _tipoformapagamentocontapagar;
        public TipoFormaPagamentoContaPagar TipoFormaPagamentoContaPagar
        {
            get
            {
                if (_tipoformapagamentocontapagar == null)
                {
                    _tipoformapagamentocontapagar = new TipoFormaPagamentoContaPagar();
                }
                return _tipoformapagamentocontapagar;
            }
            set
            { _tipoformapagamentocontapagar = value; }
        }


        private List<Parcela> _parcelas;
        public List<Parcela> Parcelas
        {
            get
            {
                if (_parcelas == null)
                    _parcelas = new List<Parcela>();

                return _parcelas;
            }
            set { _parcelas = value; }
        }

        public int NumeroParcela { get; set; }

        public object Clone()
        {
            return (ContaPagar)this.MemberwiseClone();
        }
    }
}

