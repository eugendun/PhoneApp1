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
            Tutors = new ObservableCollection<Tutor>(phoneAppDB.Tutors.ToList());
            Lectures.CollectionChanged += OnLecturesCollectionChanged;
        }

        private void OnLecturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (Lecture addedLecture in e.NewItems) {
                    phoneAppDB.Lectures.InsertOnSubmit(addedLecture);
                }
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Lecture removedLecture in e.OldItems) {
                    phoneAppDB.Lectures.DeleteOnSubmit(removedLecture);
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
    }
}