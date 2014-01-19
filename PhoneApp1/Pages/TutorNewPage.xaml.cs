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
    public partial class TutorNewPage : PhoneApplicationPage
    {
        private Tutor _newTutor = new Tutor { Surname="Nachname", Forename="Vorname" };

        public TutorNewPage() {
            InitializeComponent();
            DataContext = _newTutor;
        }

        private void OnClick_CheckButton(object sender, EventArgs e) {
            App.ViewModel.Tutors.Add(_newTutor);
            NavigationService.GoBack();
        }

        private void OnClick_CancelButton(object sender, EventArgs e) {
            NavigationService.GoBack();
        }
    }
}