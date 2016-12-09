using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View
{
    public partial class Registrar : ContentPage
    {
        bool ErrorUsuario = false;
        bool ErrorCPF = false;
        bool Active = true;

        public Registrar()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {            
            RegistrarButton.IsEnabled = true;
            CancelarButton.IsEnabled = true;
            Active = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new Login();
            return true;
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

        public void NameChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^A-Za-z]", "");

                if (entry.Text != text)
                    entry.Text = text;
            }
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

        public async void RegistrarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                RegistrarButton.IsEnabled = false;
                Active = false;

                if (string.IsNullOrEmpty(NomeEntry.Text) || string.IsNullOrEmpty(SobrenomeEntry.Text) || string.IsNullOrEmpty(TelefoneEntry.Text) || string.IsNullOrEmpty(CPFEntry.Text) 
                    || string.IsNullOrEmpty(UsuarioEntry.Text) || string.IsNullOrEmpty(SenhaEntry.Text) || string.IsNullOrEmpty(ConfirmarSenhaEntry.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(SobrenomeEntry.Text) || string.IsNullOrWhiteSpace(TelefoneEntry.Text) || string.IsNullOrWhiteSpace(CPFEntry.Text)
                    || string.IsNullOrWhiteSpace(UsuarioEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text) || string.IsNullOrWhiteSpace(ConfirmarSenhaEntry.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                if (TelefoneEntry.Text.Length < 15)
                {
                    await DisplayAlert("Aviso", "Telefone incorreto", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                if (CPFEntry.Text.Length < 14)
                {
                    await DisplayAlert("Aviso", "CPF incorreto", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                if (!ConfirmarSenhaEntry.Text.Equals(SenhaEntry.Text)) 
                {
                    await DisplayAlert("Aviso", "Senhas Desiguais", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    return;
                }

                var user = Api.Api.GetClientes();

                List<Model.Cliente> clientes = new List<Model.Cliente>();

                clientes = await user;

                Model.Cliente cliente = new Model.Cliente();

                cliente.nome = NomeEntry.Text;
                cliente.sobrenome = SobrenomeEntry.Text;
                cliente.cpf = CPFEntry.Text;
                cliente.telefone = TelefoneEntry.Text;
                cliente.usuario = UsuarioEntry.Text;
                cliente.senha = SenhaEntry.Text;                

                for (int i = 0; i < clientes.Count; i++)
                {
                    if (UsuarioEntry.Text.Equals(clientes[i].usuario)) { ErrorUsuario = true; }
                    if (CPFEntry.Text.Equals(clientes[i].cpf)) { ErrorCPF = true; }
                }
                if (ErrorUsuario)
                {
                    await DisplayAlert("Erro", "O usuário digitado já existe.", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    ErrorUsuario = false;
                }
                else if (ErrorCPF)
                {
                    await DisplayAlert("Erro", "O CPF digitado já existe.", "OK");
                    RegistrarButton.IsEnabled = true;
                    Active = true;
                    ErrorCPF = false;
                }
                else
                {
                    Api.Api.InsertCliente(cliente);

                    await DisplayAlert("Concluído", "Usuário " + cliente.usuario + " cadastrado com sucesso!", "OK");

                    App.session = cliente;
                    Application.Current.MainPage = new MasterDetail.MainPage();
                }
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
    }
}
