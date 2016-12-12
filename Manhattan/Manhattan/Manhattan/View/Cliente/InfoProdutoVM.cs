using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Manhattan.View.Cliente
{
    class InfoProdutoVM : OnPropertyChanged
    {
        Model.Produto VerificarProduto = new Model.Produto();

        public string nome;
        public string Nome
        {
            get { return nome; }
            set { nome = value; Notify("Nome"); }
        }

        public double preco;
        public double Preco
        {
            get { return preco; }
            set { preco = value; Notify("Preco"); }
        }

        public string descricao;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; Notify("Descricao"); }
        }

        public double precoTotal;
        public double PrecoTotal
        {
            get { return precoTotal; }
            set { precoTotal = value; Notify("PrecoTotal"); }
        }

        public int quantidadeCompra;
        public int QuantidadeCompra
        {
            get { return quantidadeCompra; }
            set { quantidadeCompra = value; Notify("QuantidadeCompra"); }
        }

        public ICommand DiminuirCommand { get; set; }
        public ICommand AumentarCommand { get; set; }

        public InfoProdutoVM(Model.Produto produto)
        {
            VerificarProduto = produto;

            Nome = produto.nome;
            Preco = produto.preco;
            Descricao = produto.descricao;
            PrecoTotal = produto.preco;

            if (produto.qtdestoque < 1)
            {
                QuantidadeCompra = 0;

                Application.Current.MainPage.DisplayAlert("Produto indisponível","Produto em falta no estoque,\n Tente novamente mais tarde.","OK");
            }
            else
            {
                QuantidadeCompra = 1;
            }

            this.DiminuirCommand = new Command(this.Diminuir);
            this.AumentarCommand = new Command(this.Aumentar);
        }

        public void Diminuir()
        {
            if (QuantidadeCompra == 1)
            {
                
            }
            else
            {
                QuantidadeCompra--;
                PrecoTotal -= Preco;
            }
        }        

        public void Aumentar()
        {
            if (QuantidadeCompra == VerificarProduto.qtdestoque)
            {                
                Application.Current.MainPage.DisplayAlert("Quantidade máxima alcançada!", "Não há mais unidades do produto disponíveis", "OK");
            }
            else
            {
                QuantidadeCompra++;
                PrecoTotal += Preco;
            }
        }
    }
}
