using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View.MasterDetail
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            masterPage.ListView.ItemSelected += OnItemSelected;
        }

        public MainPage(int reload)
        {
            InitializeComponent();
            masterPage.ListView.ItemSelected += OnItemSelected;

            Model.MasterPageItem masterPageItem = new Model.MasterPageItem();

            masterPageItem.Title = "Pedidos";
            masterPageItem.IconSource = "meuspedidos.png";
            masterPageItem.TargetType = typeof(Funcionario.Pedidos);

            var item = masterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Model.MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
