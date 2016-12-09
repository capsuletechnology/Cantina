using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    public partial class MinhaConta : ContentPage
    {
        bool Active = true;
        bool AlterarSenha = false;

        public MinhaConta()
        {
            InitializeComponent();

            UsuarioLabel.Text = "";

            PreencherCampos();   
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetail.MainPage();
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

        public void AlterarClicked(object sender, EventArgs e)
        {          
            if (Active)
            {
                Active = false;

                AlterarSenhaButton.IsVisible = true;

                NomeEntry.IsEnabled = true;
                SobrenomeEntry.IsEnabled = true;
                TelefoneEntry.IsEnabled = true;
                CPFEntry.IsEnabled = true;
                AlterarButton.IsVisible = false;
                SalvarButton.IsVisible = true;

                Active = true;
            }
        }

        public void AlterarSenhaClicked(object sender, EventArgs e)
        {
            AlterarSenha = true;

            AlterarSenhaButton.IsVisible = false;
            SalvarButton.IsVisible = false;

            SenhaEntry.Text = "";
            ConfirmarSenhaEntry.Text = "";

            SenhaEntry.IsVisible = true;
            SenhaEntry.IsEnabled = true;
            ConfirmarSenhaEntry.IsVisible = true;
            ConfirmarSenhaEntry.IsEnabled = true;

            SalvarButtonSenha.IsVisible = true;
        }

        public async void SalvarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;

                SalvarButton.IsEnabled = false;
                SalvarButtonSenha.IsEnabled = false;

                if (AlterarSenha)
                {
                    if (string.IsNullOrEmpty(NomeEntry.Text) || string.IsNullOrEmpty(SobrenomeEntry.Text) || string.IsNullOrEmpty(TelefoneEntry.Text) 
                        || string.IsNullOrEmpty(CPFEntry.Text) || string.IsNullOrEmpty(SenhaEntry.Text) || string.IsNullOrEmpty(ConfirmarSenhaEntry.Text))
                    {
                        await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                        SalvarButtonSenha.IsEnabled = true;
                        Active = true;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(SobrenomeEntry.Text) || string.IsNullOrWhiteSpace(TelefoneEntry.Text) 
                        || string.IsNullOrWhiteSpace(CPFEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text) || string.IsNullOrWhiteSpace(ConfirmarSenhaEntry.Text))
                    {
                        await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                        SalvarButtonSenha.IsEnabled = true;
                        Active = true;
                        return;
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(NomeEntry.Text) || string.IsNullOrEmpty(SobrenomeEntry.Text) 
                        || string.IsNullOrEmpty(TelefoneEntry.Text) || string.IsNullOrEmpty(CPFEntry.Text))                     
                    {
                        await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                        SalvarButton.IsEnabled = true;
                        Active = true;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(SobrenomeEntry.Text)
                        || string.IsNullOrWhiteSpace(TelefoneEntry.Text) || string.IsNullOrWhiteSpace(CPFEntry.Text))
                    {
                        await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
                        SalvarButton.IsEnabled = true;
                        Active = true;
                        return;
                    }
                }

                if (TelefoneEntry.Text.Length < 15)
                {
                    await DisplayAlert("Aviso", "Telefone incorreto", "OK");
                    if (AlterarSenha) { SalvarButtonSenha.IsEnabled = true; } else { SalvarButton.IsEnabled = true; }                    
                    Active = true;
                    return;
                }

                if (CPFEntry.Text.Length < 14)
                {
                    await DisplayAlert("Aviso", "CPF incorreto", "OK");
                    if (AlterarSenha) { SalvarButtonSenha.IsEnabled = true; } else { SalvarButton.IsEnabled = true; }
                    Active = true;
                    return;
                }

                if (SenhaEntry.Text.Contains(" "))
                {
                    await DisplayAlert("Aviso", "Senha não pode ter espaços.", "OK");
                    if (AlterarSenha) { SalvarButtonSenha.IsEnabled = true; } else { SalvarButton.IsEnabled = true; }
                    Active = true;
                    return;
                }

                if (AlterarSenha)
                {
                    if (!ConfirmarSenhaEntry.Text.Equals(SenhaEntry.Text))
                    {
                        await DisplayAlert("Aviso", "Senhas Desiguais", "OK");
                        SalvarButtonSenha.IsEnabled = true;
                        Active = true;
                        return;
                    }
                }

                Model.Cliente cliente = new Model.Cliente();

                NomeEntry.IsEnabled = false;
                SobrenomeEntry.IsEnabled = false;
                TelefoneEntry.IsEnabled = false;
                CPFEntry.IsEnabled = false;
                SenhaEntry.IsEnabled = false;
                ConfirmarSenhaEntry.IsEnabled = false;

                cliente.codigo = App.session.codigo;
                cliente.nome = NomeEntry.Text;
                cliente.sobrenome = SobrenomeEntry.Text;
                cliente.telefone = TelefoneEntry.Text;
                cliente.cpf = CPFEntry.Text;
                cliente.usuario = App.session.usuario;
                cliente.senha = SenhaEntry.Text;

                await DisplayAlert("Conta Alterada", "Com alterada com sucesso.", "OK");

                Api.Api.UpdateCliente(cliente);
                App.session = cliente;                

                SenhaEntry.IsVisible = false;
                ConfirmarSenhaEntry.IsVisible = false;
                AlterarSenhaButton.IsVisible = false;
                

                AlterarButton.IsVisible = true;
                SalvarButton.IsVisible = false;
                SalvarButton.IsEnabled = true;
                SalvarButtonSenha.IsVisible = false;

                Active = true;

                Application.Current.MainPage = new MasterDetail.MainPage();
            }            
        }

        public void PreencherCampos()
        {
            NomeEntry.Text = App.session.nome;
            SobrenomeEntry.Text = App.session.sobrenome;
            TelefoneEntry.Text = App.session.telefone;
            CPFEntry.Text = App.session.cpf;
            UsuarioLabel.Text = "Usuário: " + App.session.usuario;
            SenhaEntry.Text = App.session.senha;
            ConfirmarSenhaEntry.Text = App.session.senha;
        }
    }
}
