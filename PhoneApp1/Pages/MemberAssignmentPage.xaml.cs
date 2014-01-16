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
        private Lecture _lecture;
        public Lecture Lecture {
            get { return _lecture; }
            set {
                if (_lecture != value) {
                    _lecture = value;
                }
            }
        }

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
            Lecture = lecture;

            LecturesMembers = new ObservableCollection<Member>(Lecture.Members);
            AllMembers = new ObservableCollection<Member>(phoneAppDB.Members);
        }

        public void SaveChangesToDB() {
            phoneAppDB.SubmitChanges();
        }
    }

    public partial class MemberAssignmentPage : PhoneApplicationPage
    {
        private MemberAssignmentViewModel _viewModel;

        public MemberAssignmentPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
            if (selectedLecture == null) {
                NavigationService.Navigate(new Uri("/Pages/LectureSelectPage.xaml", UriKind.Relative));
            } else {
                _viewModel = new MemberAssignmentViewModel(selectedLecture);
                DataContext = _viewModel;
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            
            base.OnNavigatedFrom(e);
        }

        private void OnSelectionChanged_AssignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.AllMembers.Add(Member);
                _viewModel.LecturesMembers.Remove(Member);
            }
        }

        private void OnSelectionChanged_UnassignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.LecturesMembers.Add(Member);
                _viewModel.AllMembers.Remove(Member);
            }
        }
    }
}