using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Model
{
    public class Pedido
    {
        public int? codigo { get; set; }
        public string data { get; set; }
        public bool isfinalizado { get; set; }
        public string qrcode { get; set; }
        public double valortotal { get; set; }
        public Cliente cliente { get; set; }

        public string RandomQR()
        {
            var r = new Random();
            qrcode = r.Next().ToString();
            return qrcode;
        }
    }
}
