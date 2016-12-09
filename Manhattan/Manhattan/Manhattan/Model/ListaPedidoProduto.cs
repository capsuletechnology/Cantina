using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class ListaPedidoProduto
    {
        public int? codigo { get; set; }
        public int qtdrequisitada { get; set; }
        public double valortotal { get; set; }
        public Cliente cliente { get; set; }
        public Produto produto { get; set; }
    }
}
