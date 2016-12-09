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

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Active)
            {
                Active = false;

                var produtoClicado = (Model.ListaPedidoProduto)e.Item;

                try
                {
                    var action = await DisplayActionSheet(produtoClicado.produto.nome, "Cancelar", null, "Excluir");

                    if (action != null)
                    {
                        if (action.Equals("Excluir"))
                        {
                            bool answer = await DisplayAlert(produtoClicado.produto.nome, "Deseja excluir este produto?", "Sim", "Cancelar");
                            if (answer)
                            {
                                App._GlobalPedidoProduto.Remove(produtoClicado);

                                Active = true;
                            }
                            else
                            {
                                Active = true;
                            }
                        }
                    }
                }

                catch (Exception)
                {
                    Active = true;
                }

                listViewCarrinho.SelectedItem = null;
                Active = true;
            }            
        }

        public void XClicked(object sender, EventArgs e)
        {
            var mi = (((Button)sender).CommandParameter);
            //var produto = (Model.ListaPedidoProduto)mi;
            App._GlobalPedidoProduto.Remove((Model.ListaPedidoProduto)mi);
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
