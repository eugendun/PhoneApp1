using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Models;

namespace PhoneApp1
{
    public partial class LectureSelectPage : PhoneApplicationPage
    {
        public LectureSelectPage() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnSelectionChanged_LectureList(object sender, SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var lecture = selector.SelectedItem as Lecture;
            if (App.Current.Resources.Contains("SelectedLecture")) {
                App.Current.Resources.Remove("SelectedLecture");
            }
            App.Current.Resources.Add("SelectedLecture", lecture);
            if (NavigationService.CanGoBack) {
                NavigationService.GoBack();
            }
        }
    }
}