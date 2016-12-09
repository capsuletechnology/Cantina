using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class Cliente
    {
        public int? codigo { get; set; }
        public bool isadmin { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string cpf { get; set; }
        public string senha { get; set; }
        public string telefone { get; set; }
        public string usuario { get; set; }
    }
}
