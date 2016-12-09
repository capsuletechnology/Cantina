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
                    }

                    catch (Exception)
                    {
                        Active = true;
                    }

                    listViewSalgados.SelectedItem = null;
                    Active = true;
                }
                else
                {
                    try
                    {
                        var produtoClicado = (Model.Produto)e.Item;
                        await Navigation.PushAsync(new Cliente.InfoProduto(produtoClicado));
                        listViewSalgados.SelectedItem = null;
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
            ListaSalgados.Clear();

            listViewSalgados.IsRefreshing = true;

            await Task.Delay(500);

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
