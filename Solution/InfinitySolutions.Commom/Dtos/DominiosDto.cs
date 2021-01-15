using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Commom.Dtos
{
    public class DominiosDto
    {
        public DominiosDto()
        {
            TiposUnidadeMedida = TiposUnidadeMedida ?? new List<TipoUnidadeMedida>();
            MateriasPrimas = MateriasPrimas ?? new List<MateriaPrima>();
            Clientes = Clientes ?? new List<Cliente>();
            TiposFormasPagamento = TiposFormasPagamento ?? new List<TipoFormaPagamento>();
            TiposFases = TiposFases ?? new List<TipoFase>();
            TabelasPreco = TabelasPreco ?? new List<TabelaPreco>();
            Produtos = Produtos ?? new List<Produto>();
            TiposCores = TiposCores ?? new List<TipoCor>();
            TiposTamanhos = TiposTamanhos ?? new List<TipoTamanho>();
            TiposFrete = TiposFrete ?? new List<TipoFrete>();
            Pedidos = Pedidos ?? new List<Pedido>();
            TiposFormasPagamentoContaReceber = TiposFormasPagamentoContaReceber ?? new List<TipoFormaPagamentoContaReceber>();
        }

        public List<TipoUnidadeMedida> TiposUnidadeMedida { get; set; }
        public List<MateriaPrima> MateriasPrimas { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<TipoFormaPagamento> TiposFormasPagamento { get; set; }
        public List<TipoFase> TiposFases { get; set; }
        public List<TabelaPreco> TabelasPreco { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<TipoCor> TiposCores { get; set; }
        public List<TipoTamanho> TiposTamanhos { get; set; }
        public List<TipoFrete> TiposFrete { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public List<TipoFormaPagamentoContaReceber> TiposFormasPagamentoContaReceber { get; set; }


        private List<Fornecedor> _fornecedores;
        public List<Fornecedor> Fornecedores
        {
            get
            {
                if (_fornecedores == null)
                    _fornecedores = new List<Fornecedor>();
                return _fornecedores;
            }
            set { _fornecedores = value; }
        }

        private List<TipoStatusContaPagar> _tiposStatusContasPagar;
        public List<TipoStatusContaPagar> TiposStatusContaPagar
        {
            get
            {
                if (_tiposStatusContasPagar == null)
                    _tiposStatusContasPagar = new List<TipoStatusContaPagar>();
                return _tiposStatusContasPagar;
            }
            set { _tiposStatusContasPagar = value; }
        }

        private List<TipoContaPagar> _tiposContasPagar;
        public List<TipoContaPagar> TiposContasPàgar
        {
            get
            {
                if (_tiposContasPagar == null)
                    _tiposContasPagar = new List<TipoContaPagar>();
                return _tiposContasPagar;
            }
            set { _tiposContasPagar = value; }
        }

        private List<TipoStatusParcela> _tiposStatusParcela;
        public List<TipoStatusParcela> TiposStatusParcela
        {
            get
            {
                if (_tiposStatusContasPagar == null)
                    _tiposStatusContasPagar = new List<TipoStatusContaPagar>();
                return _tiposStatusParcela;
            }
            set { _tiposStatusParcela = value; }
        }

        private List<TipoFormaPagamentoContaPagar> _tiposFormasPagamentoContaPagar;
        public List<TipoFormaPagamentoContaPagar> TiposFormasPagamentoContaPagar
        {
            get
            {
                if (_tiposFormasPagamentoContaPagar == null)
                    _tiposFormasPagamentoContaPagar = new List<TipoFormaPagamentoContaPagar>();
                return _tiposFormasPagamentoContaPagar;
            }
            set { _tiposFormasPagamentoContaPagar = value; }
        }

        private List<TipoFormaPagamentoParcela> _tiposFormasPagamentoParcela;
        public List<TipoFormaPagamentoParcela> TiposFormasPagamentoParcela
        {
            get
            {
                if (_tiposFormasPagamentoParcela == null)
                    _tiposFormasPagamentoParcela = new List<TipoFormaPagamentoParcela>();
                return _tiposFormasPagamentoParcela;
            }
            set { _tiposFormasPagamentoParcela = value; }
        }

        private List<TipoFuncaoFuncionario> _tiposFuncoesFuncionario;
        public List<TipoFuncaoFuncionario> TiposFuncoesFuncionario
        {
            get
            {
                if (_tiposFuncoesFuncionario == null)
                    _tiposFuncoesFuncionario = new List<TipoFuncaoFuncionario>();

                return _tiposFuncoesFuncionario;
            }
            set { _tiposFuncoesFuncionario = value; }
        }

        private List<TipoFuncaoTerceirizado> _tiposFuncoesTerceirizado;
        public List<TipoFuncaoTerceirizado> TiposFuncoesTerceirizado
        {
            get
            {
                if (_tiposFuncoesTerceirizado == null)
                    _tiposFuncoesTerceirizado = new List<TipoFuncaoTerceirizado>();

                return _tiposFuncoesTerceirizado;
            }
            set { _tiposFuncoesTerceirizado = value; }
        }


    }
}
