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
            InitializeComponent();

            bebidas = new List<Model.Produto>();
            listaBebidas = new ObservableCollection<Model.Produto>();

            Pesquisar();
            listViewBebidas.IsRefreshing = false;
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
            listViewBebidas.SelectedItem = null;

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
                            listViewBebidas.SelectedItem = null;
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
            ListaBebidas.Clear();

            listViewBebidas.IsRefreshing = true;

            await Task.Delay(200);            

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

            isLoading = true;
            listViewBebidas.ItemsSource = ListaBebidas;
            listViewBebidas.IsRefreshing = false;
        }
    }
}
