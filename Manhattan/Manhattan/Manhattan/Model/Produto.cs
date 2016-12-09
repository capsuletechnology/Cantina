using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class Produto
    {
        public int? codigo { get; set; }
        public string descricao { get; set; }
        public int qtdestoque { get; set; }
        public string nome { get; set; }
        public double preco { get; set; }
        public string tipo { get; set; }
    }
}
