using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using PhoneApp1.Models;
using System.Diagnostics;

namespace PhoneApp1.ViewModel
{
    public class PhoneAppViewModel : INotifyPropertyChanged
    {
        private PhoneAppContext phoneAppDB;

        public PhoneAppViewModel() {
            phoneAppDB = DataContextFactory.GetDataContext();
        }

        public void SaveChangesToDB() {
            phoneAppDB.SubmitChanges();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}