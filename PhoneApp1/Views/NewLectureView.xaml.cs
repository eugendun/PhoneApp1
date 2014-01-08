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

namespace PhoneApp1.Views
{
    public partial class NewLectureView : PhoneApplicationPage
    {
        private Lecture _newLecture = new Lecture {
            Name="Veranstaltungsname",
            BeginDate=DateTime.Today,
            EndDate=DateTime.Today,
        };

        public NewLectureView() {
            InitializeComponent();
            var selectedLecture = App.Current.Resources["SelectedLecture"] as Lecture;
            if (selectedLecture!=null) {
                _newLecture = selectedLecture;
            }
            DataContext = _newLecture;
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