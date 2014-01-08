using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using PhoneApp1.ViewModel;
using System.Diagnostics;

namespace PhoneApp1.Views
{
    public partial class LecturesView : PhoneApplicationPage
    {
        public LecturesView() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddLectureBarIconButton(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/Views/NewLectureView.xaml", UriKind.Relative));
        }
    }
}