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

        bool isLoading = false;
        bool Active = true;

        ObservableCollection<Model.Pedido> _listaPedidos;
        public ObservableCollection<Model.Pedido> _ListaPedidos { get { return _listaPedidos; } set { _listaPedidos = value; } }

        public MeusPedidos()
        {
            InitializeComponent();

            pedidos = new List<Model.Pedido>();
            _listaPedidos = new ObservableCollection<Model.Pedido>();

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

            if (isLoading)
            {
                _ListaPedidos.Clear();
                Pesquisar();
            }
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Active)
            {
                Active = false;
                var pedidoClicado = (Model.Pedido)e.Item;
                Navigation.PushAsync(new InfoPedido(pedidoClicado));

                listViewMeusPedidos.SelectedItem = null;
            }
        }

        public async void Pesquisar()
        {            
            pedidos = await Api.Api.GetPedidos();

            for (int i = 0; i < pedidos.Count; i++)
            {
                if (pedidos[i].cliente.codigo.Equals(App.session.codigo))
                {
                    _ListaPedidos.Add(new Model.Pedido
                    {
                        codigo = pedidos[i].codigo,
                        cliente = pedidos[i].cliente,
                        valortotal = pedidos[i].valortotal,
                        data = pedidos[i].data,
                        qrcode = pedidos[i].qrcode,
                        isfinalizado = pedidos[i].isfinalizado
                    });
                }
            }

            isLoading = true;
            listViewMeusPedidos.ItemsSource = _ListaPedidos;
        }
    }
}
