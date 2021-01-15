using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Cliente
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string Apelido { get { return String.Format("{0} {1}", NomeFantasia, Nome); } private set { } }
        public decimal? Cnpj { get; set; }
        public string Contato { get; set; }
        private Endereco _endereco;
        public Endereco Endereco
        {
            get
            {
                if (_endereco == null)
                {
                    _endereco = new Endereco();
                }
                return _endereco;
            }
            set
            { _endereco = value; }
        }
        public decimal? Ie { get; set; }

        public string EnderecoCompleto
        {
            get
            {
                if (String.IsNullOrEmpty(Endereco.Rua))
                    return "-";

                return @String.Format("{0}, {1}. {2}. {3}.", Endereco.Rua, Endereco.Numero, Endereco.Bairro, Endereco.Uf);
            }
        }
    }
}

