using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{

    public class Funcionario
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public decimal? Telefone { get; set; }

        private TipoFuncaoFuncionario _tipofuncaofuncionario;
        public TipoFuncaoFuncionario TipoFuncaoFuncionario
        {
            get
            {
                if (_tipofuncaofuncionario == null)
                {
                    _tipofuncaofuncionario = new TipoFuncaoFuncionario();
                }
                return _tipofuncaofuncionario;
            }
            set
            { _tipofuncaofuncionario = value; }
        }

        private List<LinhaProducao> _linhasProducao;
        public List<LinhaProducao> LinhasProducao
        {
            get
            {
                if (_linhasProducao == null)
                    _linhasProducao = new List<LinhaProducao>();

                return _linhasProducao;
            }
            set { _linhasProducao = value; }
        }

        public double HorasProducao { get; set; }

    }
}

