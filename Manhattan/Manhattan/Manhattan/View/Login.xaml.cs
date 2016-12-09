using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View
{
    public partial class Login : ContentPage
    {
        bool ErrorUser = true;
        bool ErrorPass = true;
        bool Full = false;
        bool Active = true;

        public Login()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();            
            App.session = new Model.Cliente();           
        }

        protected override void OnAppearing()
        {
            LoginButton.IsEnabled = true;
            RegistrarButton.IsEnabled = true;
            UsuarioEntry.IsEnabled = true;
            SenhaEntry.IsEnabled = true;
            Active = true;
        }

        //
        // SAÍDA COM SIM OU NÃO ---------- TALVEZ INSTÁVEL ?
        //
        protected override bool OnBackButtonPressed()
        {
            Active = false;
            Task<bool> canClose = DisplayAlert("SAIR DO APLICATIVO", "Deseja sair do aplicativo?", "Sim", "Não");
            canClose.ContinueWith(task =>
            {
                if (task.Result) { DependencyService.Get<ICloseApplication>().closeApplication(); }
                else { Active = true; }
            });
            return true;
        }

        //
        // SAÍDA AO APERTAR NOVAMENTE - TEMPO DE 3s (ORIGINAL ESTÁVEL)
        //
        //protected override bool OnBackButtonPressed()
        //{
        //    if (Ativo == false)
        //    {
        //        DisplayAlert(null, "Pressione novamente o botão para sair.", "OK");
        //        WaitAndExecute(3000, () => canClose = true);
        //    }
        //    if (Ativo) { canClose = false; }
        //    Ativo = true;
        //    return canClose;
        //}

        private async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);
            actionToExecute();
        }

        public void UserChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^A-Za-z0-9]", "");

                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public void PassChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[ ,.:]", "");

                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public async void LoginClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                LoginButton.IsEnabled = false;
                UsuarioEntry.IsEnabled = false;
                SenhaEntry.IsEnabled = false;
                Active = false;

                if (string.IsNullOrEmpty(UsuarioEntry.Text) || string.IsNullOrEmpty(SenhaEntry.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos!", "Ok");

                    LoginButton.IsEnabled = true;
                    UsuarioEntry.IsEnabled = true;
                    SenhaEntry.IsEnabled = true;
                    Active = true;

                    return;
                }
                else
                {
                    Full = true;
                }

                var user = Api.Api.GetClientes();

                List<Model.Cliente> c = new List<Model.Cliente>();

                c = await user;

                if (Full)
                {
                    for (int i = 0; i < c.Count; i++)
                    {
                        if ((c[i].usuario.Equals(UsuarioEntry.Text)) && (c[i].senha.Equals(SenhaEntry.Text)))
                        {
                            ErrorUser = false;
                            ErrorPass = false;

                            App.session = c[i];
                            Application.Current.MainPage = new MasterDetail.MainPage();
                            
                        }

                        if (ErrorUser)
                        {
                            if (c[i].usuario.Equals(UsuarioEntry.Text)) { ErrorUser = false; }                            
                        }

                        if (ErrorPass)
                        {
                            if ((ErrorUser == false) && (c[i].senha.Equals(SenhaEntry.Text))) { ErrorPass = false; }                                
                        }
                    }
                }

                if (ErrorUser)
                {
                    await DisplayAlert("Erro", "Usuário não cadastrado.", "OK");
                    LoginButton.IsEnabled = true;
                    UsuarioEntry.IsEnabled = true;
                    SenhaEntry.IsEnabled = true;
                    Active = true;
                }

                if ((ErrorUser == false) && (ErrorPass))
                {
                    await DisplayAlert("Erro", "Senha inválida.", "OK");
                    LoginButton.IsEnabled = true;
                    UsuarioEntry.IsEnabled = true;
                    SenhaEntry.IsEnabled = true;
                    Active = true;
                    ErrorUser = true;
                }
            }
        }

        public async void RegistrarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                RegistrarButton.IsEnabled = false;

                await Task.Delay(250);

                Application.Current.MainPage = new Registrar();
            }
        }
    }
}
