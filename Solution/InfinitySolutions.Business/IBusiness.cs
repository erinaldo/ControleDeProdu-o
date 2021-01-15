using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinitySolutions.Commom;

namespace InfinitySolutions.Business
{
    public interface IBusiness<T>
    {
        Retorno Salvar(T Entity);
        Retorno Carregar(T Entity);
        Retorno Listar(int Pagina, int QntPaginas);
        Retorno Excluir(T Entity);

        Retorno PreenchimentoObrigatorio(T Entity);
    }
}
