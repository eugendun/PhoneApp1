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
    public class FollowerViewModel
    {
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string MatNr { get; set; }
        public DateTime Birthday { get; set; }
    }

    public partial class NewFollowerView : PhoneApplicationPage
    {
        private FollowerViewModel _follower = new FollowerViewModel {
            Surname="Nachname",
            Forename="Vorname",
            MatNr="Matrikelnummer",
            Birthday=DateTime.Today
        };

        public NewFollowerView() {
            InitializeComponent();
            DataContext = _follower;
        }

        private void OnClick_CancelButton(object sender, System.EventArgs e) {
            NavigationService.GoBack();
        }

        private void OnClick_AddButton(object sender, System.EventArgs e) {
            try {
                Follower follower = new Follower {
                    Surname = _follower.Surname,
                    Forename = _follower.Forename,
                    Birthday = _follower.Birthday
                };
                follower.MatNr = Convert.ToInt32(_follower.MatNr);
                App.ViewModel.Followers.Add(follower);
                NavigationService.GoBack();
            } catch (Exception) {
                // TODO
            }
        }
    }
}