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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PhoneApp1.Views
{
    internal class LectureTimeViewModel : NotifyModel
    {
        private DayOfWeek _dayOfWeek;
        public DayOfWeek DayOfWeek {
            get { return _dayOfWeek; }
            set {
                _dayOfWeek = value;
                NotifyPropertyChanged("DayOfWeek");
            }
        }

        private Boolean _takesPlace;
        public Boolean TakesPlace {
            get { return _takesPlace; }
            set {
                _takesPlace = value;
                NotifyPropertyChanged("TakesPlace");
            }
        }

        private DateTime _beginTime;
        public DateTime BeginTime {
            get { return _beginTime; }
            set {
                _beginTime = value;
                NotifyPropertyChanged("BeginTime");
            }
        }

        public LectureTimeViewModel(DayOfWeek dayOfWeek)
            : base() {
            DayOfWeek = dayOfWeek;
            TakesPlace = false;
            BeginTime = new DateTime();
        }
    }

    public partial class LectureNewPage : PhoneApplicationPage
    {
        private Lecture _newLecture = new Lecture {
            Name="Veranstaltungsname",
            BeginDate=DateTime.Today,
            EndDate=DateTime.Today,
        };

        private ObservableCollection<LectureTimeViewModel> LectureTimeCollection;

        public LectureNewPage() {
            InitializeComponent();

            LectureTimeCollection = new ObservableCollection<LectureTimeViewModel>();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek))) {
                LectureTimeCollection.Add(new LectureTimeViewModel(day));
            }

            var selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
            if (selectedLecture!=null) {
                _newLecture = selectedLecture;
            }

            foreach (var item in LectureTimeCollection) {
                var lectureTime = (from sl in _newLecture.LectureTimes
                                   where sl.Weekday.Equals((int)item.DayOfWeek)
                                   select sl).FirstOrDefault();
                if (lectureTime != null) {
                    item.TakesPlace = true;
                    item.BeginTime = new DateTime(1, 1, 1, lectureTime.Hours, lectureTime.Minutes, 0, DateTimeKind.Utc);
                }
            }

            DataContext = _newLecture;
            LectureTimes.DataContext = LectureTimeCollection;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            foreach (var item in _newLecture.LectureTimes) {
                var time = (from l in LectureTimeCollection
                            where l.DayOfWeek.Equals((DayOfWeek)item.Weekday)
                            select l).FirstOrDefault();
                if (time == null) {
                    _newLecture.LectureTimes.Remove(item);
                }
            }

            foreach (var item in LectureTimeCollection) {
                var time = (from l in _newLecture.LectureTimes
                            where l.Weekday.Equals((int)item.DayOfWeek)
                            select l).FirstOrDefault();
                if (time == null && item.TakesPlace) {
                    var newLectureTime = new LectureTime {
                        Weekday = (int)item.DayOfWeek,
                        Hours = item.BeginTime.Hour,
                        Minutes = item.BeginTime.Minute
                    };
                    _newLecture.LectureTimes.Add(newLectureTime);
                }
            }

            base.OnNavigatedFrom(e);
        }

        private void OnClick_CheckNewMemberAppBarButton(object sender, EventArgs e) {
            if (_newLecture.Name.Equals("Veranstaltungsname")) {
                // TODO show message that invalid params given
            } else {
                if (!App.ViewModel.Lectures.Contains(_newLecture)) {
                    App.ViewModel.Lectures.Add(_newLecture);
                }
                NavigationService.GoBack();
            }
        }

        private void OnClick_CancelNewMemberAppBarButton(object sender, EventArgs e) {
            NavigationService.GoBack();
        }
    }
}