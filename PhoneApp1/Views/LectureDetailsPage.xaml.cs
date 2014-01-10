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
    internal class LectureDetailsViewModel : NotifyModel
    {
        private Lecture _currentLecture;
        public Lecture CurrentLecture {
            get { return _currentLecture; }
            set {
                if (_currentLecture!=value) {
                    _currentLecture = value;
                    NotifyPropertyChanged("CurrentLecture");
                }
            }
        }

        private ObservableCollection<Tutor> _assignedTutors;
        public ObservableCollection<Tutor> AssignedTutors {
            get { return _assignedTutors; }
            set {
                if (_assignedTutors!=value) {
                    _assignedTutors=value;
                    NotifyPropertyChanged("AssignedTutors");
                }
            }
        }

        private ObservableCollection<Tutor> _unassignedTutors;
        public ObservableCollection<Tutor> UnassignedTutors {
            get { return _unassignedTutors; }
            set {
                if (_unassignedTutors!=value) {
                    _unassignedTutors=value;
                    NotifyPropertyChanged("UnassginedTutors");
                }
            }
        }

        public LectureDetailsViewModel(Lecture lecture)
            : base() {
            CurrentLecture = lecture;
            AssignedTutors = new ObservableCollection<Tutor>(lecture.Tutors);
            AssignedTutors.CollectionChanged += AssignedTutorCollectionChanged;
            var allTutors = phoneAppDB.Tutors.ToList();
            var unassigned = phoneAppDB.Tutors.ToList().Except(lecture.Tutors.ToList()).ToList();
            UnassignedTutors = new ObservableCollection<Tutor>(unassigned);
            UnassignedTutors.CollectionChanged += UnassignedTutorCollectionChanged;
        }

        private void UnassignedTutorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var tutor = item as Tutor;
                    if (tutor!=null && AssignedTutors.Contains(tutor)) {
                        AssignedTutors.Remove(tutor);
                        NotifyPropertyChanged("AssignedTutors");
                    }
                }
            }
        }

        private void AssignedTutorCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var tutor = item as Tutor;
                    if (tutor!=null && UnassignedTutors.Contains(tutor)) {
                        UnassignedTutors.Remove(tutor);
                        NotifyPropertyChanged("UnassignedTutors");
                    }
                }
            }
        }

        public void SaveChangesToDataBase() {
            phoneAppDB.SubmitChanges();
        }
    }

    public partial class LectureDetailsView : PhoneApplicationPage
    {
        private LectureDetailsViewModel _viewModel;

        public LectureDetailsView() {
            InitializeComponent();

            var lecture = App.Current.Resources["SelectedLecture"] as Lecture;
            if (lecture!=null) {
                _viewModel = new LectureDetailsViewModel(lecture);
            }

            DataContext = _viewModel;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _viewModel.SaveChangesToDataBase();
            base.OnNavigatedFrom(e);
        }

        private void OnClick_AddNewTutor(object sender, System.EventArgs e) {
            NavigationService.Navigate(new Uri("/Views/NewTutorView.xaml", UriKind.Relative));
        }

        private void OnSelectionChanged_AllTutorsList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var selectedTutor = selector.SelectedItem as Tutor;
            if (selectedTutor!=null) {
                _viewModel.AssignedTutors.Add(selectedTutor);
            }
        }

        private void OnSelectionChanged_AssignedTutorsList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var tutor = selector.SelectedItem as Tutor;
            if (tutor!=null) {
                _viewModel.UnassignedTutors.Add(tutor);
            }
        }
    }
}