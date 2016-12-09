using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    public partial class InfoPedido : ContentPage
    {
        Model.Pedido _pedido = new Model.Pedido();

        bool Active = true;

        ObservableCollection<Model.PedidoProduto> _listaPedidoProduto;
        public ObservableCollection<Model.PedidoProduto> _ListaPedidoProduto { get { return _listaPedidoProduto; } set { _listaPedidoProduto = value; } }

        public InfoPedido(Model.Pedido pedido)
        {
            InitializeComponent();

            listViewPedidos.SelectedItem = null;

            this.BindingContext = new InfoPedidoVM(pedido);

            _listaPedidoProduto = new ObservableCollection<Model.PedidoProduto>();        

            listViewPedidos.ItemsSource = _ListaPedidoProduto;

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
                        QRCodeButton.IsVisible = false;
                    }
                    else
                    {
                        FinalizadoLabel.IsVisible = false;
                        AguardandoLabel.IsVisible = true;
                        QRCodeButton.IsVisible = true;
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

        public void ScanClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                Navigation.PushAsync(new PedidoQR(App.session, _pedido));
            }            
        }
    }
}
