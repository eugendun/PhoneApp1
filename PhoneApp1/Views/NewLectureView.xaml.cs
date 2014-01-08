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
            DataContext = _newLecture;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
        }

        private void OnClick_CheckNewMemberAppBarButton(object sender, EventArgs e) {
            if (!App.ViewModel.Lectures.Contains(_newLecture) && _newLecture.Name!="Veranstaltungsname") {
                App.ViewModel.Lectures.Add(_newLecture);
                NavigationService.GoBack();
            } else {
                //TODO show as Message
            }
        }

        private void OnClick_CancelNewMemberAppBarButton(object sender, EventArgs e) {
            NavigationService.GoBack();
        }
    }
}