using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class LinhaProducao
    {
        public int Codigo { get; set; }
        public DateTime DataPrevisaoInicio { get; set; }
        public DateTime DataPrevisaoFim { get; set; }
        public DateTime DataRealInicio { get; set; }
        public DateTime DataRealFim { get; set; }
        public decimal Quantidade { get; set; }

        private Funcionario _funcionario;
        public Funcionario Funcionario
        {
            get
            {
                if (_funcionario == null)
                {
                    _funcionario = new Funcionario();
                }
                return _funcionario;
            }
            set
            { _funcionario = value; }
        }

        private Terceirizado _terceirizado;
        public Terceirizado Terceirizado
        {
            get
            {
                if (_terceirizado == null)
                    _terceirizado = new Terceirizado();
                return _terceirizado;
            }
            set
            { _terceirizado = value; }
        }

        private TipoStatusLinhaProducao _tipostatuslinhaproducao;
        public TipoStatusLinhaProducao TipoStatusLinhaProducao
        {
            get
            {
                if (_tipostatuslinhaproducao == null)
                {
                    _tipostatuslinhaproducao = new TipoStatusLinhaProducao();
                }
                return _tipostatuslinhaproducao;
            }
            set
            { _tipostatuslinhaproducao = value; }
        }

        private Produto _produto;
        public Produto Produto
        {
            get
            {
                if (_produto == null)
                    _produto = new Produto();

                return _produto;
            }
            set { _produto = value; }
        }

        public bool EhTerceirizado { get; set; }

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


    }
}

