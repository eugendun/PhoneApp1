using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;

//disable neved used/assigned warnings for fields that are being used by LINQ
#pragma warning disable 0169, 0649

namespace PhoneApp1.Models
{
    public class DataContextFactory
    {
        private static string _connectionString = "Data Source=isostore:/PhoneApp1.sdf";

        public static PhoneAppContext GetDataContext() {
            return new PhoneAppContext(_connectionString); ;
        }
    }

    public class PhoneAppContext : DataContext
    {
        public PhoneAppContext(string connectionString) : base(connectionString) { }

        public static void RemoveRecord<T>(T recordToRemove) where T : class {
            PhoneAppContext dataContext = DataContextFactory.GetDataContext();

            Table<T> tableData = dataContext.GetTable<T>();
            var deleteRecord = tableData.SingleOrDefault(record => record == recordToRemove);
            if (deleteRecord != null) {
                tableData.DeleteOnSubmit(deleteRecord);
            }

            dataContext.SubmitChanges();
        }
    }

    public class NotifyingModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        protected void NotifyPropertyChanging(string propertyName) {
            if (PropertyChanging != null) {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

}