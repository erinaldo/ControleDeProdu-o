using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity
{
    public class Produto
    {
        public Produto()
        {
            MateriasPrimas = MateriasPrimas ?? new List<MateriaPrima>();
            TipoCor = TipoCor ?? new TipoCor();
            TipoTamanho = TipoTamanho ?? new TipoTamanho();
            FaseProduzido = FaseProduzido ?? new decimal[] { };
        }

        public int Codigo { get; set; }
        public decimal? NumeroProdutoCliente { get; set; }
        public int CodigoPedidoProduto { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeAnteriorPronta { get; set; }
        public decimal? QuantidadePronta { get; set; }
        public decimal? QuantidadeProduzida { get; set; }
        public decimal? QuantidadeFaturar { get; set; }
        public decimal? Desconto { get; set; }
        public DateTime Data { get; set; }
        public decimal? QuantidadeProducaoHoraFuncionario { get; set; }
        public decimal[] FaseProduzido { get; set; }

        public List<MateriaPrima> MateriasPrimas { get; set; }
        public TipoCor TipoCor { get; set; }
        public TipoTamanho TipoTamanho { get; set; }

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

        private Terceirizado _terceirizado;
        public Terceirizado Terceirizado
        {
            get
            {
                if (_terceirizado == null)
                    _terceirizado = new Terceirizado();

                return _terceirizado;
            }
            set { _terceirizado = value; }
        }

        public decimal? NumeroPedidoCliente { get; set; }
        public decimal? LinhaPedidoCliente { get; set; }


        public string DescricaoNfe
        {
            get
            {
                if (NumeroPedidoCliente.HasValue)
                    return String.Format("{0} {1} {2}* PEDIDO COMPRA: {3} SEQ: {4}", Descricao, TipoCor.Descricao, TipoTamanho.Descricao, NumeroPedidoCliente, LinhaPedidoCliente);

                return String.Format("{0} {1} {2}", Descricao, TipoTamanho.Descricao, TipoCor.Descricao);
            }
        }
        public string DescricaoCompleta
        {
            get
            {
                return String.Format("{0} {1} {2}", Descricao, TipoTamanho.Descricao, TipoCor.Descricao);
            }
        }
    }
}