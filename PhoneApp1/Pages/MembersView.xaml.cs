using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp1.Views
{
    public partial class MembersView : PhoneApplicationPage
    {
        public MembersView() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddMemberButton(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/Pages/MemberNewView.xaml", UriKind.Relative));
        }
    }
}