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

namespace PhoneApp1
{
    public class MemberViewModel
    {
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string MatNr { get; set; }
        public DateTime Birthday { get; set; }
    }

    public partial class MemberNewView : PhoneApplicationPage
    {
        private MemberViewModel _Member = new MemberViewModel {
            Surname="Nachname",
            Forename="Vorname",
            MatNr="Matrikelnummer",
            Birthday=DateTime.Today
        };

        public MemberNewView() {
            InitializeComponent();
            DataContext = _Member;
        }

        private void OnClick_CancelButton(object sender, System.EventArgs e) {
            NavigationService.GoBack();
        }

        private void OnClick_AddButton(object sender, System.EventArgs e) {
            try {
                Member Member = new Member {
                    Surname = _Member.Surname,
                    Forename = _Member.Forename,
                    Birthday = _Member.Birthday
                };
                Member.MatNr = Convert.ToInt32(_Member.MatNr);
                App.ViewModel.Members.Add(Member);
                NavigationService.GoBack();
            } catch (Exception) {
                // TODO
            }
        }
    }
}