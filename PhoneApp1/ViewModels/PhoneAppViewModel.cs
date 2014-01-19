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
    public class NotifyModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public PhoneAppContext phoneAppDB;

        public NotifyModel() {
            phoneAppDB = DataContextFactory.GetDataContext();
        }
    }

    public class PhoneAppViewModel : NotifyModel
    {
        public PhoneAppViewModel() {
            phoneAppDB = DataContextFactory.GetDataContext();
            Lectures = new ObservableCollection<Lecture>(phoneAppDB.Lectures);
            Lectures.CollectionChanged += Lectures_CollectionChanged;

            Tutors = new ObservableCollection<Tutor>(phoneAppDB.Tutors);
            Tutors.CollectionChanged += Tutors_CollectionChanged;

            Members = new ObservableCollection<Member>(phoneAppDB.Members);
            Members.CollectionChanged += Members_CollectionChanged;
        }

        private void Lectures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (Lecture addedLecture in e.NewItems) {
                    phoneAppDB.Lectures.InsertOnSubmit(addedLecture);
                }
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Lecture removedLecture in e.OldItems) {
                    var affectedTutors = from t in Tutors
                                         where t.Lectures.Contains(removedLecture)
                                         select t;
                    foreach (Tutor tutor in affectedTutors) {
                        tutor.Lectures.Remove(removedLecture);
                    }

                    var affectedMembers = from m in Members
                                          where m.Lectures.Contains(removedLecture)
                                          select m;
                    foreach (Member member in affectedMembers) {
                        member.Lectures.Remove(removedLecture);
                    }

                    phoneAppDB.Lectures.DeleteOnSubmit(removedLecture);
                }
            }
            SaveChangesToDB();
        }

        private void Tutors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (Tutor addedTutor in e.NewItems) {
                    phoneAppDB.Tutors.InsertOnSubmit(addedTutor);
                }
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Tutor removedTutor in e.OldItems) {
                    var affectedLectures = from l in Lectures
                                           where l.Tutors.Contains(removedTutor)
                                           select l;
                    foreach (Lecture lecture in affectedLectures) {
                        lecture.Tutors.Remove(removedTutor);
                    }

                    phoneAppDB.Tutors.DeleteOnSubmit(removedTutor);
                }
            }
            SaveChangesToDB();
        }

        private void Members_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                foreach (Member addedMember in e.NewItems) {
                    phoneAppDB.Members.InsertOnSubmit(addedMember);
                }
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Member removedMember in e.OldItems) {
                    var affectedLectures = from l in Lectures
                                           where l.Members.Contains(removedMember)
                                           select l;
                    foreach (Lecture lecture in affectedLectures) {
                        lecture.Members.Remove(removedMember);
                    }

                    phoneAppDB.Members.DeleteOnSubmit(removedMember);
                }
            }
            SaveChangesToDB();
        }

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

        private ObservableCollection<Member> _Members;
        public ObservableCollection<Member> Members {
            get { return _Members; }
            set {
                _Members = value;
                NotifyPropertyChanged("Members");
            }
        }

        public void SaveChangesToDB() {
            phoneAppDB.SubmitChanges();
        }
    }
}