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
        public Lecture Lecture { get; set; }
        public ObservableCollection<Member> AssignedMembers { get; set; }
        public ObservableCollection<Member> UnassignedMembers { get; set; }

        public MemberAssignmentViewModel(Lecture lecture)
            : base() {
            Lecture = lecture;

            AssignedMembers = new ObservableCollection<Member>(Lecture.Members);
            UnassignedMembers = new ObservableCollection<Member>(phoneAppDB.Members);
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
            if (_viewModel != null) {
                _viewModel.SaveChangesToDB();
            }
            base.OnNavigatedFrom(e);
        }

        private void OnSelectionChanged_AssignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.UnassignedMembers.Add(Member);
                _viewModel.AssignedMembers.Remove(Member);
            }
        }

        private void OnSelectionChanged_UnassignedMemberList(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var Member = selector.SelectedItem as Member;
            if (Member!=null) {
                _viewModel.AssignedMembers.Add(Member);
                _viewModel.UnassignedMembers.Remove(Member);
            }
        }
    }
}