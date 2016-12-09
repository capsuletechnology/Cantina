using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan
{
    public partial class App : Application
    {
        public static Model.Cliente session;
        public static ObservableCollection<Model.ListaPedidoProduto> _GlobalPedidoProduto;

        public App()
        {
            InitializeComponent();       

            _GlobalPedidoProduto = new ObservableCollection<Model.ListaPedidoProduto>();
            session = new Model.Cliente();

            MainPage = new View.Login();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
