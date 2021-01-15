using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySolutions.Entity.Nfe
{
    public class cliente
    {
        public string cnpj { get; set; }
        public string razao_social;
        public string nome_completo { get; set; }
        public string endereco { get; set; }
        public string complemento { get; set; }
        public int numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string ie { get; set; }
        public int consumidor_final { get; set; }
    }
}
