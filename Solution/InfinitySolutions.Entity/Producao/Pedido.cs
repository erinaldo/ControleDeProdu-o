using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Pedido
    {
        public Pedido()
        {
            Frete = Frete ?? new Frete();
        }

        public int Codigo { get; set; }
        public decimal? NumeroPedidoCliente { get; set; }
        public int? NumeroParcelas { get; set; }
        public string Observacao { get; set; }
        public string ObservacaoNotaFiscal { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAplicado { get; set; }
        public decimal? Desconto { get; set; }
        public DateTime? DataPrevisaoEntrega { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public EnumPedidoStatus Status
        {
            get
            {
                if (TipoFase.Codigo < (int)EnumTipoFase.FINALIZADO && DataPrevisaoEntrega.HasValue && DataPrevisaoEntrega.Value < DateTime.Now)
                    return EnumPedidoStatus.ATRASADO;
                return EnumPedidoStatus.NO_PRAZO;
            }
            private set { }
        }

        private Cliente _cliente;
        public Cliente Cliente
        {
            get
            {
                if (_cliente == null)
                {
                    _cliente = new Cliente();
                }
                return _cliente;
            }
            set
            { _cliente = value; }
        }

        private TabelaPreco _tabelapreco;
        public TabelaPreco TabelaPreco
        {
            get
            {
                if (_tabelapreco == null)
                {
                    _tabelapreco = new TabelaPreco();
                }
                return _tabelapreco;
            }
            set
            { _tabelapreco = value; }
        }

        private TipoFase _fase;
        public TipoFase TipoFase
        {
            get
            {
                if (_fase == null)
                {
                    _fase = new TipoFase();
                }
                return _fase;
            }
            set
            { _fase = value; }
        }

        private FichaTecnica _fichatecnica;
        public FichaTecnica FichaTecnica
        {
            get
            {
                if (_fichatecnica == null)
                {
                    _fichatecnica = new FichaTecnica();
                }
                return _fichatecnica;
            }
            set
            { _fichatecnica = value; }
        }

        private TipoFormaPagamento _tipoformapagamento;
        public TipoFormaPagamento TipoFormaPagamento
        {
            get
            {
                if (_tipoformapagamento == null)
                {
                    _tipoformapagamento = new TipoFormaPagamento();
                }
                return _tipoformapagamento;
            }
            set
            { _tipoformapagamento = value; }
        }

        private List<Produto> _produtos;
        public List<Produto> Produtos
        {
            get
            {
                if (_produtos == null)
                {
                    _produtos = new List<Produto>();
                }
                return _produtos;
            }
            set
            { _produtos = value; }
        }

        private List<Funcionario> _funcionarios;
        public List<Funcionario> Funcionarios
        {
            get
            {
                if (_funcionarios == null)
                    _funcionarios = new List<Funcionario>();

                return _funcionarios;
            }
            set { _funcionarios = value; }
        }

        private List<Terceirizado> _terceirizados;
        public List<Terceirizado> Terceirizados
        {
            get
            {
                if (_terceirizados == null)
                    _terceirizados = new List<Terceirizado>();

                return _terceirizados;
            }
            set { _terceirizados = value; }
        }

        private LinhaProducaoExibicaoDto _linhaProducaoExibicao;
        public LinhaProducaoExibicaoDto LinhaProducaoExibicao
        {
            get
            {
                if (_linhaProducaoExibicao == null)
                    _linhaProducaoExibicao = new LinhaProducaoExibicaoDto();

                return _linhaProducaoExibicao;
            }
            set { _linhaProducaoExibicao = value; }
        }



        public decimal? PorcentagemLucro { get; set; }
        public decimal? ValorEspecial { get; set; }

        public Frete Frete { get; set; }
        public string RecuperaDescricaoEnum(object value)
        {
            Type objType = value.GetType();
            FieldInfo[] propriedades = objType.GetFields();
            FieldInfo field = propriedades.First(p => p.Name == value.ToString());
            return ((DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false))[0].Description;
        }

        public string NumeroNfe { get; set; }

        public string DescricaoCodigo { get { return Codigo.ToString("0000000"); } }
    }
}

