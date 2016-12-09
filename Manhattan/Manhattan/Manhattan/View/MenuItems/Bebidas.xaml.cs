using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.MenuItems
{
    public partial class Bebidas : ContentPage
    {
        List<Model.Produto> bebidas;

        bool Active = true;
        bool isLoading = false;

        ObservableCollection<Model.Produto> listaBebidas;
        public ObservableCollection<Model.Produto> ListaBebidas { get { return listaBebidas; } set { listaBebidas = value; } }

        public Bebidas()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            bebidas = new List<Model.Produto>();
            listaBebidas = new ObservableCollection<Model.Produto>();

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

                                    ListaBebidas.Clear();
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
                        await DisplayAlert(null, "Ocorreu um erro, tente novamente.", "OK");
                        Active = true;
                    }

                    listViewBebidas.SelectedItem = null;
                    Active = true;
                }

                else
                {
                    try
                    {
                        var produtoClicado = (Model.Produto)e.Item;
                        await Navigation.PushAsync(new Cliente.InfoProduto(produtoClicado));
                        listViewBebidas.SelectedItem = null;
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
            ListaBebidas.Clear();

            listViewBebidas.IsRefreshing = true;

            await Task.Delay(500);            

            bebidas = await Api.Api.GetProdutos();

            for (int i = 0; i < bebidas.Count; i++)
            {
                if (bebidas[i].tipo.Equals("bebida", StringComparison.OrdinalIgnoreCase))
                {                    

                    ListaBebidas.Add(new Model.Produto
                    {
                        codigo = bebidas[i].codigo,
                        descricao = bebidas[i].descricao,
                        qtdestoque = bebidas[i].qtdestoque,
                        nome = bebidas[i].nome,
                        preco = bebidas[i].preco,
                        tipo = bebidas[i].tipo
                    });
                }
            }

            listViewBebidas.IsRefreshing = false;
            isLoading = true;
            listViewBebidas.ItemsSource = ListaBebidas;
        }
    }
}
