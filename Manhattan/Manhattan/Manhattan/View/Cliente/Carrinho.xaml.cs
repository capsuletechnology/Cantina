using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    public partial class Carrinho : ContentPage
    {
        double somaValorTotal = 0;
        bool Active = true;

        public Carrinho()
        {
            InitializeComponent();

            var ordenarLista = App._GlobalPedidoProduto.OrderBy(x => x.codigo).ToList();
            App._GlobalPedidoProduto.Clear();

            for (int i = 0; i < ordenarLista.Count; i++) { App._GlobalPedidoProduto.Add(ordenarLista[i]); }

            listViewCarrinho.ItemsSource = App._GlobalPedidoProduto;
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetail.MainPage();
            return true;
        }

        protected override void OnAppearing()
        {
            Active = true;
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            listViewCarrinho.SelectedItem = null;
        }

        public void XClicked(object sender, EventArgs e)
        {
            var mi = (((Button)sender).CommandParameter);
            var produto = (Model.ListaPedidoProduto)mi;

            if (produto.qtdrequisitada > 1)
            {
                App._GlobalPedidoProduto.Remove(produto);
                produto.qtdrequisitada = produto.qtdrequisitada - 1;
                produto.valortotal = produto.qtdrequisitada * produto.produto.preco;
                App._GlobalPedidoProduto.Add(produto);

                var ordenarLista = App._GlobalPedidoProduto.OrderBy(x => x.codigo).ToList();
                App._GlobalPedidoProduto.Clear();

                for (int i = 0; i < ordenarLista.Count; i++) { App._GlobalPedidoProduto.Add(ordenarLista[i]); }
            }
            else
            {
                App._GlobalPedidoProduto.Remove(produto);

                var ordenarLista = App._GlobalPedidoProduto.OrderBy(x => x.codigo).ToList();
                App._GlobalPedidoProduto.Clear();

                for (int i = 0; i < ordenarLista.Count; i++) { App._GlobalPedidoProduto.Add(ordenarLista[i]); }
            }
        }

        public async void FinalizarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;

                if (App._GlobalPedidoProduto.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < App._GlobalPedidoProduto.Count; i++)
                        {
                            double soma = App._GlobalPedidoProduto[i].valortotal;
                            somaValorTotal += soma;
                        }

                        var _lista = new List<Model.ListaPedidoProduto>();

                        for (int i = 0; i < App._GlobalPedidoProduto.Count; i++)
                        {
                            _lista.Add(App._GlobalPedidoProduto[i]);
                        }

                        Api.Api.InsertPedido(App.session, somaValorTotal, _lista);

                        Api.Api.UpdateProduto(_lista);

                        await DisplayAlert(null, "Pedido feito com sucesso!", "OK");

                        App._GlobalPedidoProduto.Clear();

                        listViewCarrinho.ItemsSource = App._GlobalPedidoProduto;

                        await Task.Delay(250);

                        Active = true;
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("", "Foi detectado um erro no banco. Por favor tente novamente", "OK");
                        Active = true;
                    }
                }
                else
                {
                    await DisplayAlert("", "Não há produtos no carrinho!", "OK");
                    App._GlobalPedidoProduto.Clear();

                    await Task.Delay(250);

                    Active = true;
                }
            }            
        }
    }
}
