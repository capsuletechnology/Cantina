using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View
{
    public partial class Menu : TabbedPage
    {
        public Menu()
        {
            InitializeComponent();

            if (App.session.isadmin) { this.Title = "Estoque"; } else { this.ToolbarItems.Clear(); }
        }

        public void NovoProdutoClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Funcionario.NovoProduto());
        }

        protected override bool OnBackButtonPressed()
        {
            Task<bool> canClose = DisplayAlert("SAIR DO APLICATIVO", "Deseja sair do aplicativo?", "Sim", "Não");
            canClose.ContinueWith(task =>
            {
                if (task.Result)
                {
                    DependencyService.Get<ICloseApplication>().closeApplication();
                }
            });
            return true;
        }
    }
}
