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

        public PhoneAppViewModel()
        {
            phoneAppDB = DataContextFactory.GetDataContext();
        }

        public void SaveChangesToDB()
        {
            phoneAppDB.SubmitChanges();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public void LoadCollectionsFromDatabase()
        {
            var membersInDB = from Member m in phoneAppDB.Members
                              select m;

            Members = new ObservableCollection<Member>(membersInDB);

            var subjectsInDB = from Subject s in phoneAppDB.Subjects
                               select s;

            Subjects = new ObservableCollection<Subject>(subjectsInDB);
        }

        #region Members

        private ObservableCollection<Member> _members;
        public ObservableCollection<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                NotifyPropertyChanged("Members");
            }
        }

        public void AddMember(Member newMember)
        {
            // add member to the database
            phoneAppDB.Members.InsertOnSubmit(newMember);
            phoneAppDB.SubmitChanges();

            // add member to the observable collection
            Members.Add(newMember);
        }

        public void DeleteMember(Member memberForDelete)
        {
            // remove member from the observable collection
            Members.Remove(memberForDelete);

            // remove member from the database
            phoneAppDB.Members.DeleteOnSubmit(memberForDelete);
            phoneAppDB.SubmitChanges();
        }

        #endregion

        #region Subjects

        private ObservableCollection<Subject> _subjects;
        public ObservableCollection<Subject> Subjects
        {
            get { return _subjects; }
            set
            {
                _subjects = value;
                NotifyPropertyChanged("Subjects");
            }
        }

        public void AddSubject(Subject newSubject)
        {
            phoneAppDB.Subjects.InsertOnSubmit(newSubject);
            phoneAppDB.SubmitChanges();

            Subjects.Add(newSubject);
        }

        public void DeleteSubject(Subject subjectToDelete)
        {
            Subjects.Remove(subjectToDelete);

            phoneAppDB.Subjects.DeleteOnSubmit(subjectToDelete);
            phoneAppDB.SubmitChanges();
        }

        #endregion
    }
}