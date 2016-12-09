using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.View.Funcionario
{
    class InfoPedidoVM : OnPropertyChanged
    {
        private int? codigo;
        public int? Codigo { get { return codigo; } set { codigo = value; Notify("Codigo"); } }

        private string data;
        public string Data { get { return data; } set { data = value; Notify("Data"); } }

        //private bool isfinalizado;
        //public bool IsFinalizado { get { return isfinalizado; } set { isfinalizado = value; Notify("IsFinalizado"); } }

        //private string qrcode;
        //public string QRCode { get { return qrcode; } set { qrcode = value; Notify("QRCode"); } }

        private double valortotal;
        public double ValorTotal { get { return valortotal; } set { valortotal = value; Notify("ValorTotal"); } }

        private Model.Cliente cliente;
        public Model.Cliente Cliente { get { return cliente; } set { cliente = value; Notify("Cliente"); } }

        public InfoPedidoVM(Model.Pedido pedido)
        {
            Codigo = pedido.codigo;
            Data = pedido.data;
            ValorTotal = pedido.valortotal;
            Cliente = pedido.cliente;
        }
    }
}
