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
        private ObservableCollection<Member> _lecturesMembers;
        public ObservableCollection<Member> LecturesMembers {
            get { return _lecturesMembers; }
            set {
                if (_lecturesMembers!=value) {
                    _lecturesMembers=value;
                    NotifyPropertyChanged("LecturesMembers");
                }
            }
        }

        private ObservableCollection<Member> _allMembers;
        public ObservableCollection<Member> AllMembers {
            get { return _allMembers; }
            set {
                if (_allMembers!=value) {
                    _allMembers=value;
                    NotifyPropertyChanged("AllMembers");
                }
            }
        }

        public MemberAssignmentViewModel(Lecture lecture)
            : base() {
            LecturesMembers = new ObservableCollection<Member>();   // TODO
            LecturesMembers.CollectionChanged += LecturesMembersCollectionChanged;

            AllMembers = new ObservableCollection<Member>(phoneAppDB.Members);
            AllMembers.CollectionChanged += AllMembersCollectionChanged;
        }

        private void AllMembersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var Member = item as Member;
                    if (Member!=null && LecturesMembers.Contains(Member)) {
                        LecturesMembers.Remove(Member);
                        NotifyPropertyChanged("LecturesMembers");
                    }
                }
            }
        }

        private void LecturesMembersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var item in e.NewItems) {
                    var Member = item as Member;
                    if (Member!=null && AllMembers.Contains(Member)) {
                        AllMembers.Remove(Member);
                        NotifyPropertyChanged("AllMembers");
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

        private void OnSelectionChanged_AssignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.AllMembers.Add(Member);
            }
        }

        private void OnSelectionChanged_UnassignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.LecturesMembers.Add(Member);
            }
        }
    }
}