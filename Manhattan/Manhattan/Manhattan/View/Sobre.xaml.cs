using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Manhattan.View
{
    public partial class Sobre : ContentPage
    {
        public Sobre()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetail.MainPage();
            return true;
        }
    }
}
