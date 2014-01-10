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
using System.Collections.ObjectModel;
using PhoneApp1.ViewModel;
using System.Collections.Specialized;

namespace PhoneApp1
{
    internal class MemberAssignmentViewModel : NotifyModel
    {
        private ObservableCollection<Follower> _lecturesFollowers;
        public ObservableCollection<Follower> LecturesFollowers {
            get { return _lecturesFollowers; }
            set {
                if (_lecturesFollowers!=value) {
                    _lecturesFollowers=value;
                    NotifyPropertyChanged("LecturesFollowers");
                }
            }
        }

        private ObservableCollection<Follower> _allFollowers;
        public ObservableCollection<Follower> AllFollowers {
            get { return _allFollowers; }
            set {
                if (_allFollowers!=value) {
                    _allFollowers=value;
                    NotifyPropertyChanged("AllFollowers");
                }
            }
        }

        public MemberAssignmentViewModel(Lecture lecture)
            : base() {
            LecturesFollowers = new ObservableCollection<Follower>();   // TODO
            LecturesFollowers.CollectionChanged += LecturesFollowersCollectionChanged;

            AllFollowers = new ObservableCollection<Follower>(phoneAppDB.Followers);
            AllFollowers.CollectionChanged += AllFollowersCollectionChanged;
        }

        private void AllFollowersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var Follower = item as Follower;
                    if (Follower!=null && LecturesFollowers.Contains(Follower)) {
                        LecturesFollowers.Remove(Follower);
                        NotifyPropertyChanged("LecturesFollowers");
                    }
                }
            }
        }

        private void LecturesFollowersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var follower = item as Follower;
                    if (follower!=null && AllFollowers.Contains(follower)) {
                        AllFollowers.Remove(follower);
                        NotifyPropertyChanged("AllFollowers");
                    }
                }
            }
        }
    }

    public partial class MemberAssignmentPage : PhoneApplicationPage
    {
        private Lecture _selectedLecture;
        private MemberAssignmentViewModel _viewModel;

        public MemberAssignmentPage() {
            InitializeComponent();
            _selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
            _viewModel = new MemberAssignmentViewModel(_selectedLecture);
            DataContext = _viewModel;
        }

        private void OnSelectionChanged_AssignedFollowerList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var follower = selector.SelectedItem as Follower;
            if (follower!=null) {
                _viewModel.AllFollowers.Add(follower);
            }
        }

        private void OnSelectionChanged_UnassignedFollowerList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var follower = selector.SelectedItem as Follower;
            if (follower!=null) {
                _viewModel.LecturesFollowers.Add(follower);
            }
        }
    }
}