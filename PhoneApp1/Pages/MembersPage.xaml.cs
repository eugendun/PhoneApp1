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
    public partial class MembersPage : PhoneApplicationPage
    {
        private Member selectedMember;

        public MembersPage() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddMemberButton(object sender, System.EventArgs e) {
            NavigationService.Navigate(new Uri("/Pages/MemberNewPage.xaml", UriKind.Relative));
        }

        private void DeleteMemberButton_Click(object sender, EventArgs e) {
            if (selectedMember != null) {
                App.ViewModel.Members.Remove(selectedMember);
                App.ViewModel.phoneAppDB.SubmitChanges();
            }
        }

        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selector = sender as LongListSelector;
            var selMember = selector.SelectedItem as Member;
            selectedMember = selMember;
        }
    }
}