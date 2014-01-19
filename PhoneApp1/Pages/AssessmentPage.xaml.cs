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
using PhoneApp1.ViewModel;

namespace PhoneApp1
{
    public class DateViewModel
    {
        private string _state;
        private DateTime _date;

        public DateViewModel(DateTime date, string state) {
            _date = date;
            _state = state;
        }

        public DateTime Date { get { return _date; } }
        public string State { get { return _state; } }
    }

    public class AssessmentMemberViewModel : NotifyModel
    {
        private Member _member;
        private List<DateViewModel> _assessmentDetails;

        public AssessmentMemberViewModel(Member member, List<DateTime> dates)
            : base() {
            _member = member;
            _assessmentDetails = new List<DateViewModel>();
            foreach (var date in dates) {
                String state = "anwesend";
                _assessmentDetails.Add(new DateViewModel(date, state));
            }
        }

        public string Surname { get { return _member.Surname; } }
        public string Forename { get { return _member.Forename; } }
        public int MatNr { get { return _member.MatNr; } }
        public List<DateViewModel> AssessmentDetails { get { return _assessmentDetails; } }
    }

    public class AssessmentPageViewModel : NotifyModel
    {
        private PresencePageViewModel _presenceViewModel;
        private List<AssessmentMemberViewModel> _assessment;

        public AssessmentPageViewModel(Lecture lecture)
            : base() {
            _presenceViewModel = new PresencePageViewModel(lecture);

            _assessment = new List<AssessmentMemberViewModel>();
            foreach (var member in _presenceViewModel.CurrentLecture.Members) {
                _assessment.Add(new AssessmentMemberViewModel(member, _presenceViewModel.Dates));
            }
        }

        public List<AssessmentMemberViewModel> Assessment { get { return _assessment; } }
    }

    public partial class AssessmentPage : PhoneApplicationPage
    {
        private AssessmentPageViewModel _viewModel;

        public AssessmentPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (!App.Current.Resources.Contains("SelectedLecture")) {
                NavigationService.Navigate(new Uri("/Pages/LectureSelectPage.xaml", UriKind.Relative));
            } else {
                var lecture = App.Current.Resources["SelectedLecture"] as Lecture;
                App.Current.Resources.Remove("SelectedLecture");

                _viewModel = new AssessmentPageViewModel(lecture);
                AssessmentList.DataContext = _viewModel.Assessment;
            }
            base.OnNavigatedTo(e);
        }
    }
}