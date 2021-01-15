using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Enum
{
    public enum EnumPedidoStatus
    {
        [Description("ADIANTADO")]
        ADIANTADO,
        [Description("NO PRAZO")]
        NO_PRAZO,
        [Description("ATRASADO")]
        ATRASADO
    }
}
