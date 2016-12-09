using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class PedidoProduto
    {
        public int? codigo { get; set; }
        public int? codpedido { get; set; }
        public string nome { get; set; }
        public int qtdrequisitada { get; set; }
        public double valortotal { get; set; }
    }
}
