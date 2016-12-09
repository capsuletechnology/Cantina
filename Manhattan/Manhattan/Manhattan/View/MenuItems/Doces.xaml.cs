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

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Active)
            {
                Active = false;
                if (App.session.isadmin)
                {
                    var produtoClicado = (Model.Produto)e.Item;

                    try
                    {
                        var action = await DisplayActionSheet(produtoClicado.nome, "Cancelar", null, "Editar", "Excluir");

                        if (action != null)
                        {
                            if (action.Equals("Editar"))
                            {
                                //var mi = ((MenuItem)sender);
                                bool answer = await DisplayAlert(produtoClicado.nome, "Deseja alterar as informações deste produto?", "Sim", "Cancelar");
                                if (answer)
                                {
                                    await Navigation.PushAsync(new Funcionario.NovoProduto(produtoClicado));
                                }
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
                    }

                    catch (Exception)
                    {
                        Active = true;
                    }

                    listViewDoces.SelectedItem = null;
                    Active = true;
                }
                else
                {
                    try
                    { 
                        var produtoClicado = (Model.Produto)e.Item;
                        await Navigation.PushAsync(new Cliente.InfoProduto(produtoClicado));
                        listViewDoces.SelectedItem = null;
                    }
                    catch (Exception)
                    {
                        await DisplayAlert(null, "Ocorreu um erro. Tente novamente.", "OK");
                    }
                }
            }            
        }

        public async void Pesquisar()
        {
            ListaDoces.Clear();

            listViewDoces.IsRefreshing = true;

            await Task.Delay(500);

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
