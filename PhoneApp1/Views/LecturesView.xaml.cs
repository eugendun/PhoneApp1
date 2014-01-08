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
using PhoneApp1.Models;

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

        private void OnClick_DeleteLectureButton(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            if (button!=null) {
                Lecture lectureToRemove = button.DataContext as Lecture;
                App.ViewModel.Lectures.Remove(lectureToRemove);
            }
            this.Focus();
        }
    }
}