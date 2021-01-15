using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Commom.Dtos
{
    public class LinhaProducaoExibicaoDto
    {
        private List<LinhaProducaoProdutoDto> _linhaProducaoProdutos;
        public List<LinhaProducaoProdutoDto> LinhaProducaoProdutos
        {
            get
            {
                if (_linhaProducaoProdutos == null)
                    _linhaProducaoProdutos = new List<LinhaProducaoProdutoDto>();

                return _linhaProducaoProdutos;
            }
            set { _linhaProducaoProdutos = value; }
        }
    }
}
