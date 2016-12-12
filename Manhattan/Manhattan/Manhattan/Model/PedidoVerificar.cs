using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class PedidoVerificar
    {
        public int? codigo { get; set; }
        public string data { get; set; }
        public bool isfinalizado { get; set; }
        public bool naofinalizado { get; set; }
        public string qrcode { get; set; }
        public double valortotal { get; set; }
        public string numeroPedido { get; set; }
        public Cliente cliente { get; set; }
    }
}
