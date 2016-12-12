using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.MenuItems
{
    public partial class Salgados : ContentPage
    {
        List<Model.Produto> salgados;

        bool Active = true;
        bool isLoading = false;

        ObservableCollection<Model.Produto> listaSalgados;
        public ObservableCollection<Model.Produto> ListaSalgados { get { return listaSalgados; } set { listaSalgados = value; } }

        public Salgados()
        {
            InitializeComponent();

            salgados = new List<Model.Produto>();
            listaSalgados = new ObservableCollection<Model.Produto>();

            Pesquisar();
        }

        protected override void OnAppearing()
        {
            if (isLoading)
            {
                Pesquisar();
            }
            Active = true;
        }

        public async void onItemTapped(object sender, ItemTappedEventArgs e)
        {
            listViewSalgados.SelectedItem = null;

            var produtoClicado = (Model.Produto)e.Item;

            if (Active)
            {
                Active = false;

                if (App.session.isadmin)
                {
                    var action = await DisplayActionSheet(produtoClicado.nome, "Cancelar", null, "Editar", "Excluir");

                    if (action != null)
                    {
                        if (action.Equals("Editar"))
                        {
                            await Navigation.PushAsync(new Funcionario.NovoProduto(produtoClicado));
                        }

                        if (action.Equals("Excluir"))
                        {
                            bool answer = await DisplayAlert(produtoClicado.nome, "Deseja excluir este produto?", "Sim", "Cancelar");
                            if (answer)
                            {
                                Api.Api.DeleteProduto(produtoClicado);

                                ListaSalgados.Clear();
                                Pesquisar();

                                Active = true;
                            }
                            else
                            {
                                Active = true;
                            }
                        }
                    }
                    Active = true;
                }

                else
                {
                    Model.Produto verificarProduto = new Model.Produto();

                    bool Existe = false;

                    var verificar = await Api.Api.GetProdutos();

                    for (int i = 0; i < verificar.Count; i++)
                    {
                        if (verificar[i].codigo.Equals(produtoClicado.codigo))
                        {
                            Existe = true;
                            verificarProduto = verificar[i];
                        }
                    }

                    if (Existe)
                    {
                        if (verificarProduto.nome.Equals(produtoClicado.nome) && verificarProduto.descricao.Equals(produtoClicado.descricao)
                            && verificarProduto.tipo.Equals(produtoClicado.tipo) && verificarProduto.preco.Equals(produtoClicado.preco)
                            && verificarProduto.qtdestoque.Equals(produtoClicado.qtdestoque))
                        {
                            await Navigation.PushAsync(new Cliente.InfoProduto(produtoClicado));
                            listViewSalgados.SelectedItem = null;
                        }
                        else
                        {
                            await DisplayAlert(null, "O Funcionário atualizou o produto.\nTente novamente.", "OK");
                            OnAppearing();
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert(null, "O Funcionário atualizou o produto.\nTente novamente.", "OK");
                        OnAppearing();
                        return;
                    }
                }
            }
        }

        public async void Pesquisar()
        {
            ListaSalgados.Clear();

            listViewSalgados.IsRefreshing = true;

            await Task.Delay(200);

            salgados = await Api.Api.GetProdutos();

            for (int i = 0; i < salgados.Count; i++)
            {
                if (salgados[i].tipo.Equals("salgado", StringComparison.OrdinalIgnoreCase))
                {
                    ListaSalgados.Add(new Model.Produto
                    {
                        codigo = salgados[i].codigo,
                        descricao = salgados[i].descricao,
                        qtdestoque = salgados[i].qtdestoque,
                        nome = salgados[i].nome,
                        preco = salgados[i].preco,
                        tipo = salgados[i].tipo
                    });
                }
            }
            
            listViewSalgados.IsRefreshing = false;
            isLoading = true;
            listViewSalgados.ItemsSource = ListaSalgados;
        }
    }
}
