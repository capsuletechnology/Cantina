using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.MenuItems
{
    public partial class Doces : ContentPage
    {
        List<Model.Produto> doces;

        bool Active = true;
        bool isLoading = false;

        ObservableCollection<Model.Produto> listaDoces;
        public ObservableCollection<Model.Produto> ListaDoces { get { return listaDoces; } set { listaDoces = value; } }

        public Doces()
        {
            InitializeComponent();

            doces = new List<Model.Produto>();
            listaDoces = new ObservableCollection<Model.Produto>();

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
            listViewDoces.SelectedItem = null;

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

                                ListaDoces.Clear();
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
                            listViewDoces.SelectedItem = null;
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
            ListaDoces.Clear();

            listViewDoces.IsRefreshing = true;

            await Task.Delay(200);

            doces = await Api.Api.GetProdutos();

            for (int i = 0; i < doces.Count; i++)
            {
                if (doces[i].tipo.Equals("doce", StringComparison.OrdinalIgnoreCase))
                {
                    ListaDoces.Add(new Model.Produto
                    {
                        codigo = doces[i].codigo,
                        descricao = doces[i].descricao,
                        qtdestoque = doces[i].qtdestoque,
                        nome = doces[i].nome,
                        preco = doces[i].preco,
                        tipo = doces[i].tipo
                    });
                }
            }

            listViewDoces.IsRefreshing = false;
            isLoading = true;
            listViewDoces.ItemsSource = ListaDoces;            
        }
    }
}
