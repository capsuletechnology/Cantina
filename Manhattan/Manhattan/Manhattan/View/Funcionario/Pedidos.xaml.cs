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

        bool isLoading = false;
        bool Active = true;

        ObservableCollection<Model.Pedido> _listaPedidos;
        public ObservableCollection<Model.Pedido> _ListaPedidos { get { return _listaPedidos; } set { _listaPedidos = value; } }

        public Pedidos()
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

        public void onItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Active)
            {
                Active = false;
                var pedidoClicado = (Model.Pedido)e.Item;
                Navigation.PushAsync(new InfoPedido(pedidoClicado));

                listViewPedidos.SelectedItem = null;
            }
        }

        public async void Pesquisar()
        {
            pedidos = await Api.Api.GetPedidos();

            for (int i = 0; i < pedidos.Count; i++)
            {
                _ListaPedidos.Add(new Model.Pedido
                {
                    cliente = pedidos[i].cliente,
                    codigo = pedidos[i].codigo,
                    data = pedidos[i].data,
                    isfinalizado = pedidos[i].isfinalizado,
                    qrcode = pedidos[i].qrcode,
                    valortotal = pedidos[i].valortotal
                });
            }

            isLoading = true;
            listViewPedidos.ItemsSource = _ListaPedidos;
        }
    }
}
