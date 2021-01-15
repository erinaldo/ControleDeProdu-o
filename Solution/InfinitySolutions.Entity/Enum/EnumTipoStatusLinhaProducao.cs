using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Enum
{
    public enum EnumTipoStatusLinhaProducao
    {
        [Description("EM PRODUÇÃO")]
        EM_PRODUCAO = 1,
        [Description("ATRASADO")]
        ATRASADO,
        [Description("PRODUZIDO")]
        PRODUZIDO
    }
}
