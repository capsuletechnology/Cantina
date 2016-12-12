using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Manhattan.View.Cliente
{
    public partial class PedidoQR : ContentPage
    {
        ZXingBarcodeImageView barcode;

        public PedidoQR(Model.Cliente cliente, Model.PedidoVerificar pedido)
        {
            InitializeComponent();

            barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            barcode.BarcodeOptions.Width = 500;
            barcode.BarcodeOptions.Height = 500;

            if (pedido.cliente.codigo.Equals(cliente.codigo))
            {
                barcode.BarcodeValue = pedido.qrcode;
            }

            Content = barcode;
        }
    }
}
