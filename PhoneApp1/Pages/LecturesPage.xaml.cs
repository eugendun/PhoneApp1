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
    public partial class LecturesPage : PhoneApplicationPage
    {

        private Lecture _selectedLecture;
        public Lecture SelectedLecture {
            get { return _selectedLecture; }
            set {
                _selectedLecture = value;
                if (_selectedLecture!=null) {
                    App.Current.Resources.Remove("SelectedLecture");
                    App.Current.Resources.Add("SelectedLecture", SelectedLecture);
                    EnableEditableApplicationBarButtons(true);
                } else {
                    EnableEditableApplicationBarButtons(false);
                }
            }
        }

        public LecturesPage() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddLectureBarIconButton(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/Pages/LectureNewView.xaml", UriKind.Relative));
        }

        private void OnSelectionChanged_LectureSelector(object sender, SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var lecture = selector.SelectedItem as Lecture;
            if (lecture!=null) {
                SelectedLecture = lecture;
            }

        }

        private void OnClick_EditButton(object sender, System.EventArgs e) {
            if (SelectedLecture!=null) {
                NavigationService.Navigate(new Uri("/Pages/LectureNewView.xaml", UriKind.Relative));
            }
        }

        private void OnClick_DeleteButton(object sender, System.EventArgs e) {
            if (SelectedLecture!=null) {
                App.ViewModel.Lectures.Remove(SelectedLecture);
                SelectedLecture = null;

            }
            this.Focus();
        }

        private void EnableEditableApplicationBarButtons(bool isEnabled) {
            for (int i = 1; i <= 3; i++) {
                var button = ApplicationBar.Buttons[i] as ApplicationBarIconButton;
                button.IsEnabled = isEnabled;
            }
        }

        private void OnClick_DetailsButton(object sender, System.EventArgs e) {
            if (SelectedLecture!=null) {
                NavigationService.Navigate(new Uri("/Pages/LectureDetailsPage.xaml", UriKind.Relative));
            }
        }
    }
}