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
    public partial class NewTutorView : PhoneApplicationPage
    {
        private Tutor _newTutor = new Tutor { Surname="Nachname", Forename="Vorname" };

        public NewTutorView() {
            InitializeComponent();
            DataContext = _newTutor;
        }

        private void OnClick_CheckButton(object sender, EventArgs e) {
            int count = App.ViewModel.Tutors.Count(
                t => t.Surname.Equals(_newTutor.Surname)
                    && t.Forename.Equals(_newTutor.Forename));
            if (count == 0 && !_newTutor.Surname.Equals("Nachname") && !_newTutor.Forename.Equals("Vorname")) {
                App.ViewModel.Tutors.Add(_newTutor);
                NavigationService.GoBack();
            } else {
                // TODO show a message that new tutor data is invalid
            }
        }

        private void OnClick_CancelButton(object sender, EventArgs e) {
            NavigationService.GoBack();
        }
    }
}