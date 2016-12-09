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
        bool bloqueado = true;
        bool isBloqueado = false;

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
            TipoEntry.Text = produto.tipo;
            PrecoEntry.Text = Convert.ToString(produto.preco);
            qtdEstoqueEntry.Text = Convert.ToString(produto.qtdestoque);

            isBloqueado = true;
        }

        protected override void OnAppearing()
        {
            Active = true;
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

        public void BotaoClicked(object sender, EventArgs e)
        {
            if (Active)
            {
                Active = false;
                Botao.IsEnabled = false;

                NomeEntry.IsEnabled = false;
                DescricaoEntry.IsEnabled = false;
                TipoEntry.IsEnabled = false;
                PrecoEntry.IsEnabled = false;
                qtdEstoqueEntry.IsEnabled = false;

                if ((string.IsNullOrEmpty(NomeEntry.Text)) || (string.IsNullOrEmpty(DescricaoEntry.Text)) || (string.IsNullOrEmpty(TipoEntry.Text)) || (string.IsNullOrEmpty(PrecoEntry.Text)) || (string.IsNullOrEmpty(qtdEstoqueEntry.Text)))
                {
                    DisplayAlert("", "Não podem haver campos vazios!", "OK");
                    Active = true;
                    Botao.IsEnabled = true;
                }
                else
                {
                    produto.nome = NomeEntry.Text;
                    produto.descricao = DescricaoEntry.Text;

                    string tipoText = TipoEntry.Text;
                    tipoText = tipoText.TrimStart(new char[] { 'T', 'i', 'p', 'o', ':', ' ' });
                    produto.tipo = tipoText;                                     

                    string precoText = PrecoEntry.Text;
                    precoText = precoText.TrimStart(new char[] { 'R', '$', ' ' });
                    produto.preco = Convert.ToDouble(precoText);                    

                    string qtdText = qtdEstoqueEntry.Text;
                    qtdText = qtdText.TrimStart(new char[] { 'U', 'n', 'i', 'd', 'a', 'd', 'e', 's', ':', ' ' });
                    produto.qtdestoque = Convert.ToInt32(qtdText);                    

                    if (isUpdate)
                    {
                        Api.Api.UpdateProduto(produto);
                        DisplayAlert("Editar Produto", "Produto " + produto.nome + " atualizado.", "OK");
                        Navigation.PopAsync();
                    }
                    else
                    {
                        Api.Api.InsertProduto(produto);
                        DisplayAlert("Adicionar Produto", "Produto " + produto.nome + " adicionado.", "OK");
                        Navigation.PopAsync();
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
