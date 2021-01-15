using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{
    public class Login
    {
        public Login()
        {
            TiposPermissoes = TiposPermissoes ?? new List<TipoPermissao>();
            TiposFuncoes = TiposFuncoes ?? new List<TipoFuncao>();
        }

        public int Codigo { get; set; }

        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public List<TipoPermissao> TiposPermissoes { get; set; }
        public List<TipoFuncao> TiposFuncoes { get; set; }

    }
}

