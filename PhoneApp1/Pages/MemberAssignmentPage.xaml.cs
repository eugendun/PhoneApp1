﻿using System;
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

            if (Lecture != null) {
                AssignedMembers = new ObservableCollection<Member>(Lecture.Members);
                UnassignedMembers = new ObservableCollection<Member>(from m in phoneAppDB.Members
                                                                     where !AssignedMembers.Contains(m)
                                                                     select m);
            }
        }

        public void SaveChangesToDB() {
            var dbLecture = phoneAppDB.Lectures.Single(l => l.Equals(Lecture));
            if (dbLecture != null) {
                foreach (Member member in dbLecture.Members) {
                    dbLecture.Members.Remove(member);
                }
                phoneAppDB.SubmitChanges();

                foreach (Member member in AssignedMembers) {
                    if (!dbLecture.Members.Contains(member)) {
                        dbLecture.Members.Add(member);
                    }
                }
                phoneAppDB.SubmitChanges();
            }
        }
    }

    public partial class MemberAssignmentPage : PhoneApplicationPage
    {
        private MemberAssignmentViewModel _viewModel;

        public MemberAssignmentPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (App.Current.Resources.Contains("SelectedLecture")) {
                var selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
                App.Current.Resources.Remove("SelectedLecture");
                _viewModel = new MemberAssignmentViewModel(selectedLecture);
                DataContext = _viewModel;
            } else {
                NavigationService.Navigate(new Uri("/Pages/LectureSelectPage.xaml", UriKind.Relative));
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