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
using PhoneApp1.Views;
using System.Collections.ObjectModel;
using PhoneApp1.ViewModel;
using System.Collections.Specialized;

namespace PhoneApp1
{
    public partial class LectureDetailsView : PhoneApplicationPage
    {
        public LectureDetailsView() {
            InitializeComponent();

            var lecture = App.Current.Resources["SelectedLecture"] as Lecture;
        }

        private void OnClick_AddNewTutor(object sender, System.EventArgs e) {
            NavigationService.Navigate(new Uri("/Pages/TutorNewPage.xaml", UriKind.Relative));
        }

        private void OnSelectionChanged_AllTutorsList(object sender, SelectionChangedEventArgs e) {
        }

        private void OnSelectionChanged_AssignedTutorsList(object sender, SelectionChangedEventArgs e) {
        }

        // TODO
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
        }
    }
}