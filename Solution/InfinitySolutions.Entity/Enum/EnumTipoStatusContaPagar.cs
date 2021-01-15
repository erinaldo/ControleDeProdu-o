using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity
{
    public enum EnumTipoStatusContaPagar
    {
        [Description("EM ABERTO")]
        EM_ABERTO = 1,
        [Description("PAGO")]
        PAGO,
        [Description("ATRASADO")]
        ATRASADO
    }
}
