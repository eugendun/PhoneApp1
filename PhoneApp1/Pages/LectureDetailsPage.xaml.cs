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
        private Lecture _selectedLecture;
        private ObservableCollection<Tutor> AssignedTutors;
        private ObservableCollection<Tutor> UnassignedTutors;

        public LectureDetailsView() {
            InitializeComponent();
            _selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (_selectedLecture != null) {
                AssignedTutors = new ObservableCollection<Tutor>(_selectedLecture.Tutors);
                UnassignedTutors = new ObservableCollection<Tutor>(from t in App.ViewModel.Tutors
                                                                   where !AssignedTutors.Contains(t)
                                                                   select t);
            }

            DataContext = _selectedLecture;
            AssignedTutorsList.DataContext = AssignedTutors;
            UnassignedTutorsList.DataContext = UnassignedTutors;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            foreach (Tutor tutor in _selectedLecture.Tutors) {
                _selectedLecture.Tutors.Remove(tutor);
            }
            App.ViewModel.SaveChangesToDB();

            foreach (Tutor tutor in AssignedTutors) {
                _selectedLecture.Tutors.Add(tutor);
            }
            App.ViewModel.SaveChangesToDB();

            base.OnNavigatedFrom(e);
        }

        private void OnClick_AddNewTutor(object sender, System.EventArgs e) {
            NavigationService.Navigate(new Uri("/Pages/TutorNewPage.xaml", UriKind.Relative));
        }

        private void AssignedTutorsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var tutor = selector.SelectedItem as Tutor;
            if (tutor != null) {
                AssignedTutors.Remove(tutor);
                UnassignedTutors.Add(tutor);
            }
        }

        private void UnassignedTutorsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var tutor = selector.SelectedItem as Tutor;
            if (tutor != null) {
                UnassignedTutors.Remove(tutor);
                AssignedTutors.Add(tutor);
            }
        }
    }
}