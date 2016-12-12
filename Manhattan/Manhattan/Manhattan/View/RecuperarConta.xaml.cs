using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View
{
    public partial class RecuperarConta : ContentPage
    {
        bool Active = true;

        public RecuperarConta()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Login();
            return true;
        }

        protected override void OnAppearing()
        {
            Active = true;
            RecuperarButton.IsEnabled = true;
            CancelarButton.IsEnabled = true;
        }

        public async void RecuperarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                RecuperarButton.IsEnabled = false;

                if (string.IsNullOrEmpty(TelefoneEntry.Text) || string.IsNullOrEmpty(CPFEntry.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                    RecuperarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(TelefoneEntry.Text) || string.IsNullOrWhiteSpace(CPFEntry.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                    RecuperarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                var clientes = await Api.Api.GetClientes();

                for (int i = 0; i < clientes.Count; i++)
                {
                    if (clientes[i].cpf.Equals(CPFEntry.Text) && clientes[i].telefone.Equals(TelefoneEntry.Text))
                    {
                        await DisplayAlert("Conta encontrada!", "Conta encontrada com sucesso!\nLembre-se de alterar a senha!", "OK");

                        App.session = clientes[i];

                        Application.Current.MainPage = new MasterDetail.MainPage();

                        return;
                    }
                }

                await DisplayAlert("Conta não encontrada!", "Não foi possível localizar sua conta!\nVerifique as informações e tente novamente!", "OK");
                RecuperarButton.IsEnabled = true;
                CancelarButton.IsEnabled = true;
                Active = true;
            }
        }

        public async void CancelarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                CancelarButton.IsEnabled = false;

                await Task.Delay(250);

                Application.Current.MainPage = new Login();

                Active = false;
            }
        }

        public void CelChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^0-9]", "");

                text = text.PadRight(11);

                if (text.Length > 11)
                {
                    text = text.Remove(11);
                }

                text = text.Insert(0, "(").Insert(3, ")").Insert(4, " ").Insert(10, "-").TrimEnd(new char[] { ' ', '(', ')', '-' });
                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public void CpfChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^0-9]", "");

                text = text.PadRight(11);


                if (text.Length > 11)
                {
                    text = text.Remove(11);
                }

                text = text.Insert(3, ".").Insert(7, ".").Insert(11, "-").TrimEnd(new char[] { ' ', '.', '-' });
                if (entry.Text != text)
                    entry.Text = text;
            }
        }
    }
}
