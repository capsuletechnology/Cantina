using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.Funcionario
{
    public partial class NovoProduto : ContentPage
    {
        bool isUpdate = false;
        bool Active = true;
        int contador = 0;

        Model.Produto produto = new Model.Produto();

        public NovoProduto()
        {
            InitializeComponent();

            Title = "Novo Produto";
            Botao.Text = "Adicionar";

            isUpdate = false;            
        }

        public NovoProduto(Model.Produto _produto)
        {
            InitializeComponent();

            produto = _produto;

            Title = "Editar Produto";
            Botao.Text = "Atualizar";
            isUpdate = true;

            NomeEntry.Text = produto.nome;
            DescricaoEntry.Text = produto.descricao;
            PrecoEntry.Text = Convert.ToString(produto.preco);
            QuantidadeLabel.Text = Convert.ToString(produto.qtdestoque);
        }

        protected override void OnAppearing()
        {
            Active = true;

            NomeEntry.IsEnabled = true;
            DescricaoEntry.IsEnabled = true;
            TipoPicker.IsEnabled = true;
            PrecoEntry.IsEnabled = true;
            QuantidadeLabel.IsEnabled = true;
        }

        public void TipoChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^A-Za-z]", "");

                if (text.Length > 7)
                {
                    text = text.Remove(7);
                }

                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public void PrecoChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^0-9.]", "");

                text = text.Insert(0, "R").Insert(1, "$").Insert(2, " ");
                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public void UnidChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;

            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                string text = Regex.Replace(ev.NewTextValue, @"[^0-9]", "");

                text = text.Insert(0, "U").Insert(1, "n").Insert(2, "i").Insert(3, "d").Insert(4, "a").Insert(5, "d").Insert(6, "e").Insert(7, "s").Insert(8, ":").Insert(9, " ");
                if (entry.Text != text)
                    entry.Text = text;
            }
        }

        public void DiminuirClicked(object sender, EventArgs e)
        {
            if (!QuantidadeLabel.Text.Equals("0"))
            {
                contador--;
                QuantidadeLabel.Text = Convert.ToString(contador);
            }
        }

        public void AumentarClicked(object sender, EventArgs e)
        {
            contador++;
            QuantidadeLabel.Text = Convert.ToString(contador);
        }

        public async void BotaoClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                Botao.IsEnabled = false;

                NomeEntry.IsEnabled = false;
                DescricaoEntry.IsEnabled = false;
                TipoPicker.IsEnabled = false;
                PrecoEntry.IsEnabled = false;
                QuantidadeLabel.IsEnabled = false;

                var selectedValue = "";

                try { selectedValue = TipoPicker.Items[TipoPicker.SelectedIndex]; }
                catch (Exception)
                {
                    await DisplayAlert("", "Não podem haver campos vazios!", "OK");
                    Active = true;
                    Botao.IsEnabled = true;

                    NomeEntry.IsEnabled = true;
                    DescricaoEntry.IsEnabled = true;
                    TipoPicker.IsEnabled = true;
                    PrecoEntry.IsEnabled = true;
                    QuantidadeLabel.IsEnabled = true;
                    return;
                }



                if ((string.IsNullOrEmpty(NomeEntry.Text)) || (string.IsNullOrEmpty(DescricaoEntry.Text)) || (string.IsNullOrEmpty(selectedValue)) 
                    || (string.IsNullOrEmpty(PrecoEntry.Text)) || (string.IsNullOrEmpty(QuantidadeLabel.Text)) || PrecoEntry.Text.Equals("R$ ") 
                    || (string.IsNullOrWhiteSpace(NomeEntry.Text)) || (string.IsNullOrWhiteSpace(DescricaoEntry.Text)))
                {
                    await DisplayAlert("", "Não podem haver campos vazios!", "OK");
                    Active = true;
                    Botao.IsEnabled = true;

                    NomeEntry.IsEnabled = true;
                    DescricaoEntry.IsEnabled = true;
                    TipoPicker.IsEnabled = true;
                    PrecoEntry.IsEnabled = true;
                    QuantidadeLabel.IsEnabled = true;
                }
                else
                {
                    string nomeText = NomeEntry.Text;
                    nomeText = nomeText.Trim(new char[] { ' ' });
                    produto.nome = nomeText;

                    string descricaoText = DescricaoEntry.Text;
                    descricaoText = descricaoText.Trim(new char[] { ' ' });
                    produto.descricao = descricaoText;

                    produto.tipo = selectedValue;                                   

                    string precoText = PrecoEntry.Text;
                    precoText = precoText.TrimStart(new char[] { 'R', '$', ' ' });
                    produto.preco = Convert.ToDouble(precoText); 
                                       
                    produto.qtdestoque = Convert.ToInt32(QuantidadeLabel.Text);                    

                    if (isUpdate)
                    {
                        Api.Api.UpdateProduto(produto);
                        await DisplayAlert("Editar Produto", "Produto " + produto.nome + " atualizado.", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Api.Api.InsertProduto(produto);
                        await DisplayAlert("Adicionar Produto", "Produto " + produto.nome + " adicionado.", "OK");
                        await Navigation.PopAsync();
                    }
                }
            }            
        }

        public void CancelarClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                Navigation.PopAsync();
            }            
        }
    }
}
