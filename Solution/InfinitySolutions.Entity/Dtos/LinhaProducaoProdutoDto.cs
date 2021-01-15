using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Commom.Dtos
{
    public class LinhaProducaoProdutoDto
    {
        public int Codigo { get; set; }

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


        public DateTime DataPrevisaoInicio { get; set; }
        public DateTime DataPrevisaoFim { get; set; }

        public string NomesFuncionarios
        {
            get
            {
                return String.Join("<br/> ", Funcionarios.OrderBy(f => f.Nome).Select(f => f.Nome != null ? f.Nome.ToUpper() : String.Empty).ToArray());
            }
        }

        public bool EhTerceirizado { get; set; }

        public TipoStatusLinhaProducao TipoStatusLinhaProducao { get; set; }

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
