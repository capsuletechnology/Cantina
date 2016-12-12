using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Funcionario
{
    public partial class Pedidos : ContentPage
    {
        List<Model.Pedido> pedidos;
        bool Active = true;

        ObservableCollection<Model.PedidoVerificar> _listaPedidos;
        public ObservableCollection<Model.PedidoVerificar> _ListaPedidos { get { return _listaPedidos; } set { _listaPedidos = value; } }

        public Pedidos()
        {
            InitializeComponent();
            Pesquisar();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetail.MainPage();
            return true;
        }

        protected override void OnAppearing()
        {
            Active = true;
            Pesquisar();
        }

        protected void OnRefresh(object sender, EventArgs e)
        {
            Pesquisar();
            listViewPedidos.IsRefreshing = false;
        }

        public void onItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Active)
            {
                Active = false;
                var pedido = (Model.PedidoVerificar)e.Item;

                Model.Pedido pedidoClicado = new Model.Pedido();

                pedidoClicado.cliente = pedido.cliente;
                pedidoClicado.codigo = pedido.codigo;
                pedidoClicado.data = pedido.data;
                pedidoClicado.isfinalizado = pedido.isfinalizado;
                pedidoClicado.qrcode = pedido.qrcode;
                pedidoClicado.valortotal = pedido.valortotal;

                //Navigation.PushAsync(new InfoPedido(pedidoClicado));
                Application.Current.MainPage = new InfoPedido(pedidoClicado);
                listViewPedidos.SelectedItem = null;
            }
        }

        public async void Pesquisar()
        {
            pedidos = new List<Model.Pedido>();
            _listaPedidos = new ObservableCollection<Model.PedidoVerificar>();

            pedidos = await Api.Api.GetPedidos();

            _ListaPedidos.Clear();

            for (int i = 0; i < pedidos.Count; i++)
            {
                _ListaPedidos.Add(new Model.PedidoVerificar
                {
                    cliente = pedidos[i].cliente,
                    codigo = pedidos[i].codigo,
                    data = pedidos[i].data,
                    isfinalizado = pedidos[i].isfinalizado,
                    naofinalizado = pedidos[i].isfinalizado,
                    qrcode = pedidos[i].qrcode,
                    valortotal = pedidos[i].valortotal             
                });
            }

            for (int i = 0; i < _ListaPedidos.Count; i++)
            {
                if (_ListaPedidos[i].isfinalizado) { _ListaPedidos[i].naofinalizado = false; }
                else { _ListaPedidos[i].naofinalizado = true; }
            }

            var ordenarLista = _ListaPedidos.OrderByDescending(x => x.codigo).ToList();
            listViewPedidos.ItemsSource = ordenarLista;
        }
    }
}
