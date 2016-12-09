using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Manhattan.View.Funcionario
{
    public partial class InfoPedido : ContentPage
    {
        Model.Pedido _pedido = new Model.Pedido();

        bool Answer = false;
        bool Active = true;       

        ObservableCollection<Model.PedidoProduto> _listaPedidoProduto;
        public ObservableCollection<Model.PedidoProduto> _ListaPedidoProduto { get { return _listaPedidoProduto; } set { _listaPedidoProduto = value; } }


        public InfoPedido(Model.Pedido pedido)
        {
            InitializeComponent();

            listViewPedidos.SelectedItem = null;

            this.BindingContext = new InfoPedidoVM(pedido);

            _listaPedidoProduto = new ObservableCollection<Model.PedidoProduto>();  

            _pedido = pedido;

            Pesquisar();
        }

        protected override async void OnAppearing()
        {
            Active = true;

            var verificar = await Api.Api.GetPedidos();

            for (int i = 0; i < verificar.Count; i++)
            {
                if (verificar[i].codigo.Equals(_pedido.codigo))
                {
                    if (verificar[i].isfinalizado)
                    {
                        FinalizadoLabel.IsVisible = true;
                        AguardandoLabel.IsVisible = false;
                        EscanearButton.IsVisible = false;
                    }
                    else
                    {
                        FinalizadoLabel.IsVisible = false;
                        AguardandoLabel.IsVisible = true;
                        EscanearButton.IsVisible = true;
                    }
                }
            }
        }

        public async void Pesquisar()
        {
            var _lista = await Api.Api.GetPedidoProduto();

            for (int i = 0; i < _lista.Count; i++)
            {
                if (_lista[i].codpedido.Equals(this._pedido.codigo))
                {
                    _ListaPedidoProduto.Add(new Model.PedidoProduto
                    {
                        codigo = _lista[i].codigo,
                        nome = _lista[i].nome,
                        codpedido = _lista[i].codpedido,
                        qtdrequisitada = _lista[i].qtdrequisitada,
                        valortotal = _lista[i].valortotal
                    });
                }
            }

            listViewPedidos.ItemsSource = _ListaPedidoProduto;            
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            listViewPedidos.SelectedItem = null;
        }

        public async void ScanClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;

                try
                {
                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                    scanner.BottomText = "Toque na tela para focar";
                    scanner.TopText = "Posicione o código dentro do quadro";
                    scanner.CameraUnsupportedMessage = "Câmera não compatível";

                    var result = await scanner.Scan();

                    if (result.Text.Equals(_pedido.qrcode))
                    {
                        Answer = await DisplayAlert("Código Escaneado", "O Código foi confirmado: " + result.Text +
                                       " Deseja finalizar o pedido?", "Sim", "Não");
                    }
                    else
                    {
                        await DisplayAlert("Código Escaneado", "Código Inválido!", "OK");
                    }

                    if (Answer)
                    {
                        _pedido.isfinalizado = true;
                        Api.Api.UpdatePedido(_pedido);
                        await DisplayAlert("Finalizar Pedido", "Pedido Finalizado.", "OK");
                        Navigation.InsertPageBefore(new InfoPedido(_pedido), this);
                        await Navigation.PopAsync();
                        Navigation.RemovePage(this);
                        Active = true;
                    }
                }
                catch (Exception)
                {
                    Active = true;
                }
            }            
        }
    }
}
