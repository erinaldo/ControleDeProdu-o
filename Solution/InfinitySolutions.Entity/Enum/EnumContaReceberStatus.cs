using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Enum
{
    public enum EnumContaReceberStatus
    {
        [Description("-")]
        _ = 0,
        [Description("Em Aberto")]
        EM_ABERTO = 1,
        [Description("Recebida")]
        RECEBIDA = 2
    }
}
