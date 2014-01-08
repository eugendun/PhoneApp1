using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using PhoneApp1.Models;
using System.Diagnostics;
using System.Collections.Specialized;
using System;

namespace PhoneApp1.ViewModel
{
    public class PhoneAppViewModel : INotifyPropertyChanged
    {
        private PhoneAppContext phoneAppDB;

        public PhoneAppViewModel() {
            phoneAppDB = DataContextFactory.GetDataContext();
            Lectures = new ObservableCollection<Lecture>(phoneAppDB.Lectures.ToList());
            Lectures.CollectionChanged += OnCollectionChanged;

            Tutors = new ObservableCollection<Tutor>(phoneAppDB.Tutors.ToList());
            Tutors.CollectionChanged += OnCollectionChanged;

            Followers = new ObservableCollection<Follower>(phoneAppDB.Followers.ToList());
            Followers.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (var addedItem in e.NewItems) {
                    phoneAppDB.GetTable(addedItem.GetType()).InsertOnSubmit(addedItem);
                }
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (var removedItem in e.OldItems) {
                    phoneAppDB.GetTable(removedItem.GetType()).DeleteOnSubmit(removedItem);
                }
            }
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

        private ObservableCollection<Lecture> _lectures;
        public ObservableCollection<Lecture> Lectures {
            get { return _lectures; }
            set {
                _lectures = value;
                NotifyPropertyChanged("Lectures");
            }
        }

        private ObservableCollection<Tutor> _tutors;
        public ObservableCollection<Tutor> Tutors {
            get { return _tutors; }
            set {
                _tutors = value;
                NotifyPropertyChanged("Tutors");
            }
        }

        private ObservableCollection<Follower> _followers;
        public ObservableCollection<Follower> Followers {
            get { return _followers; }
            set {
                _followers = value;
                NotifyPropertyChanged("Followers");
            }
        }
    }
}