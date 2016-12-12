using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    public partial class MeusPedidos : ContentPage
    {
        List<Model.Pedido> pedidos;
        bool Active = true;
        int contador;

        ObservableCollection<Model.PedidoVerificar> _listaPedidos;
        public ObservableCollection<Model.PedidoVerificar> _ListaPedidos { get { return _listaPedidos; } set { _listaPedidos = value; } }

        public MeusPedidos()
        {
            InitializeComponent();
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
            listViewMeusPedidos.IsRefreshing = false;
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

                Navigation.PushAsync(new InfoPedido(pedido));

                listViewMeusPedidos.SelectedItem = null;
            }
        }

        public async void Pesquisar()
        {
            contador = 0;
            pedidos = new List<Model.Pedido>();
            _listaPedidos = new ObservableCollection<Model.PedidoVerificar>();

            pedidos = await Api.Api.GetPedidos();

            for (int i = 0; i < pedidos.Count; i++)
            {
                if (pedidos[i].cliente.codigo.Equals(App.session.codigo))
                {
                    contador++;

                    _ListaPedidos.Add(new Model.PedidoVerificar
                    {
                        codigo = pedidos[i].codigo,
                        cliente = pedidos[i].cliente,
                        valortotal = pedidos[i].valortotal,
                        data = pedidos[i].data,
                        qrcode = pedidos[i].qrcode,
                        isfinalizado = pedidos[i].isfinalizado,
                        naofinalizado = pedidos[i].isfinalizado,
                        numeroPedido = Java.Lang.String.Format("%02d", contador)
                    });
                }
            }

            for (int i = 0; i < _ListaPedidos.Count; i++)
            {
                if (_ListaPedidos[i].isfinalizado) { _ListaPedidos[i].naofinalizado = false; }
                else { _ListaPedidos[i].naofinalizado = true; }
            }

            var ordenarLista = _ListaPedidos.OrderByDescending(x => x.numeroPedido).ToList();
            listViewMeusPedidos.ItemsSource = ordenarLista;
        }
    }
}
