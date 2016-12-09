using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    public partial class InfoProduto : ContentPage
    {
        Model.Produto produtoMandar = new Model.Produto();
        Model.ListaPedidoProduto pedidoProdutoMandar = new Model.ListaPedidoProduto();
        bool Active = true;

        public InfoProduto(Model.Produto produto)
        {
            InitializeComponent();
            this.BindingContext = new InfoProdutoVM(produto);

            if (produto.qtdestoque < 1)
            {
                DiminuirButton.IsVisible = false;
                AumentarButton.IsVisible = false;
                qntdCompra.IsVisible = false;
                precoTotal.IsVisible = false;
                AddCarrinhoButton.IsVisible = false;
            }

            produtoMandar = produto;
        }

        protected override void OnAppearing()
        {
            Active = true;
        }

        public async void AddCarrinhoClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;

                AddCarrinhoButton.IsEnabled = false;
                AumentarButton.IsEnabled = false;
                DiminuirButton.IsEnabled = false;

                try
                {
                    pedidoProdutoMandar.codigo = produtoMandar.codigo;
                    pedidoProdutoMandar.cliente = App.session;
                    pedidoProdutoMandar.produto = produtoMandar;
                    pedidoProdutoMandar.qtdrequisitada = Convert.ToInt32(qntdCompra.Text);
                    pedidoProdutoMandar.valortotal = Convert.ToDouble(precoTotal.Text.Replace("R$", ""));


                    for (int i = 0; i < App._GlobalPedidoProduto.Count; i++)
                    {
                        if (App._GlobalPedidoProduto[i].produto.codigo.Equals(pedidoProdutoMandar.produto.codigo))
                        {
                            var qntdAnterior = App._GlobalPedidoProduto[i].qtdrequisitada;
                            var valorTotalAnterior = App._GlobalPedidoProduto[i].valortotal;
                                              

                            if ( (qntdAnterior + pedidoProdutoMandar.qtdrequisitada) <= produtoMandar.qtdestoque)
                            {
                                App._GlobalPedidoProduto.Remove(App._GlobalPedidoProduto[i]);


                                App._GlobalPedidoProduto.Add(new Model.ListaPedidoProduto
                                {
                                    cliente = pedidoProdutoMandar.cliente,
                                    produto = pedidoProdutoMandar.produto,
                                    qtdrequisitada = qntdAnterior + pedidoProdutoMandar.qtdrequisitada,
                                    valortotal = valorTotalAnterior + pedidoProdutoMandar.valortotal
                                });


                                await DisplayAlert(null, "O produto foi adicionado ao carrinho!", "OK");
                                await Navigation.PopAsync();
                                return;
                            }
                            else
                            {
                                await DisplayAlert("Erro", "O produto não foi adicionado ao carrinho!\nQuantidade máxima de unidades atingida!", "OK");

                                AddCarrinhoButton.IsEnabled = true;
                                AumentarButton.IsEnabled = true;
                                DiminuirButton.IsEnabled = true;
                                Active = true;

                                return;
                            }                            
                        }
                    }

                    App._GlobalPedidoProduto.Add(pedidoProdutoMandar);

                    await DisplayAlert(null, "O produto foi adicionado ao carrinho!", "OK");
                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert(null, "Foi detectado um erro. Por favor tente novamente", "OK");
                    await Navigation.PopAsync();
                }

                AddCarrinhoButton.IsEnabled = true;
                AumentarButton.IsEnabled = true;
                DiminuirButton.IsEnabled = true;
            }            
        }
    }
}
