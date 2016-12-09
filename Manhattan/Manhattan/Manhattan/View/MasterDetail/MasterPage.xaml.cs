using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.MasterDetail
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MasterPage()
        {
            InitializeComponent();
            this.UsuarioLogado.Text = App.session.nome + " " + App.session.sobrenome;

            var masterPageItems = new List<Model.MasterPageItem>();

            if (App.session.isadmin)
            {
                /// <summary>
                /// Páginas FUNCIONÁRIO
                /// </summary>
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Pedidos",
                    IconSource = "meuspedidos.png",
                    TargetType = typeof(Funcionario.Pedidos)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Estoque",
                    IconSource = "cardapio.png",
                    TargetType = typeof(Menu)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Sobre",
                    IconSource = "sobre.png",
                    TargetType = typeof(Sobre)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Deslogar",
                    IconSource = "logout.png",
                    TargetType = typeof(Login)
                });
            }
            else
            {
                /// <summary>
                /// Páginas CLIENTE
                /// </summary>
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Minha Conta",
                    IconSource = "minhaconta.png",
                    TargetType = typeof(Cliente.MinhaConta)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Cardápio",
                    IconSource = "cardapio.png",
                    TargetType = typeof(Menu)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Carrinho",
                    IconSource = "carrinho.png",
                    TargetType = typeof(Cliente.Carrinho)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Meus Pedidos",
                    IconSource = "meuspedidos.png",
                    TargetType = typeof(Cliente.MeusPedidos)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Sobre",
                    IconSource = "sobre.png",
                    TargetType = typeof(Sobre)
                });
                masterPageItems.Add(new Model.MasterPageItem
                {
                    Title = "Deslogar",
                    IconSource = "logout.png",
                    TargetType = typeof(Login)
                });
            }
            listView.ItemsSource = masterPageItems;
        }
    }
}
