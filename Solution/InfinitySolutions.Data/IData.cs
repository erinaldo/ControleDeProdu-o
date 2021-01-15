using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinitySolutions.Commom;
using System.Data;

namespace InfinitySolutions.Data
{
    public interface IData<T>
    {
        Retorno Salvar(T Entity);
        Retorno Carregar(T Entity);
        Retorno Listar(int Pagina, int QntPaginas);
        Retorno Excluir(T Entity);

        T FillEntity(IDataReader reader);
    }
}
