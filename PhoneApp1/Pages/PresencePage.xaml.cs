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
using System.Diagnostics;
using PhoneApp1.ViewModel;

namespace PhoneApp1
{
    public class PresencePageViewModel : NotifyModel
    {
        private Lecture _currentLecture;
        public Lecture CurrentLecture {
            get { return _currentLecture; }
        }

        private List<DateTime> _dates;
        public List<DateTime> Dates {
            get { return _dates; }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate {
            get { return _currentDate; }
            set {
                _currentDate = value;
                NotifyPropertyChanged("CurrentDate");
            }
        }

        public PresencePageViewModel(Lecture lecture)
            : base() {
            _currentLecture = lecture;

            var daysOfWeek = (from l in _currentLecture.LectureTimes
                              orderby l.Weekday ascending
                              select (DayOfWeek)l.Weekday).ToList();

            var startdate = DateTime.Today.Date;                // TODO lecture begin date
            var enddate = DateTime.Today.AddMonths(1).Date;     // TODO lecture end date
            var itDate = startdate;
            _dates = new List<DateTime>();
            while (itDate < enddate) {
                if (daysOfWeek.Contains(itDate.DayOfWeek)) {
                    _dates.Add(itDate.Date);
                }
                itDate = itDate.AddDays(1);
            }
            _currentDate = _dates.FirstOrDefault();
        }

        public void PreviousDay() {
            var index = Dates.IndexOf(CurrentDate);
            if (index > 0) {
                CurrentDate = Dates.ElementAt(index-1);
            }
        }

        public void NextDay() {
            var index = Dates.IndexOf(CurrentDate);
            if (index < Dates.Count-1) {
                CurrentDate = Dates.ElementAt(index+1);
            }
        }
    }

    public partial class PresencePage : PhoneApplicationPage
    {
        private PresencePageViewModel _viewModel;

        public PresencePage() {
            InitializeComponent();
            if (App.Current.Resources.Contains("SelectedLecture")) {
                App.Current.Resources.Remove("SelectedLecture");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (!App.Current.Resources.Contains("SelectedLecture")) {
                NavigationService.Navigate(new Uri("/Pages/LectureSelectPage.xaml", UriKind.Relative));
            } else {
                var lecture  = App.Current.Resources["SelectedLecture"] as Lecture;

                _viewModel = new PresencePageViewModel(lecture);

                DataContext = _viewModel;
                MemberList.DataContext = _viewModel.CurrentLecture.Members;
            }

            base.OnNavigatedTo(e);
        }

        private void PreviousDate_Click(object sender, RoutedEventArgs e) {
            _viewModel.PreviousDay();
        }

        private void NextDate_Click(object sender, RoutedEventArgs e) {
            _viewModel.NextDay();
        }
    }
}