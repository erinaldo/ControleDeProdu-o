using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity
{
    public enum EnumTipoFase
    {
        [Description("ORÇAMENTO")]
        ORCAMENTO = 1,
        [Description("CORTE")]
        CORTE,
        [Description("COSTURA")]
        COSTURA,
        [Description("ACABAMENTO")]
        ACABAMENTO,
        [Description("FINALIZADO")]
        FINALIZADO,
        [Description("FATURADO")]
        FATURADO
    }
}
