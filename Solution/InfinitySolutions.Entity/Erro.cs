using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinitySolutions.Entity
{
    public class Erro
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public EnumCasoDeUso CasoDeUso { get; set; }
        public EnumCamada Camada { get; set; }
        public EnumFuncionalidade Funcionalidade { get; set; }
        public string Entidade { get; set; }
        public DateTime Data { get; set; }
        public string Imagem { get; set; }
    }
}
