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
        public Table<Member> Members;
        public Table<Subject> Subjects;
        private Table<SubjectMember> SubjectMembers;

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

    [Table(Name = "Members")]
    public class Member : NotifyingModel
    {
        private int _matNr;
        private string _surname;
        private string _forename;
        private DateTime _birthday;

        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
        public int MatNr {
            get { return _matNr; }
            set {
                if (_matNr != value) {
                    NotifyPropertyChanging("MatNr");
                    _matNr = value;
                    NotifyPropertyChanged("MatNr");
                }
            }
        }

        [Column]
        public string Surname {
            get { return _surname; }
            set {
                if (_surname != value) {
                    NotifyPropertyChanging("Surname");
                    _surname = value;
                    NotifyPropertyChanged("Surname");
                }
            }
        }

        [Column]
        public string Forename {
            get {
                return _forename;
            }
            set {
                if (_forename != value) {
                    NotifyPropertyChanging("Forename");
                    _forename = value;
                    NotifyPropertyChanged("Forename");
                }
            }
        }

        [Column]
        public DateTime Birthday {
            get { return _birthday; }
            set {
                if (_birthday != value) {
                    NotifyPropertyChanging("Birthday");
                    _birthday = value;
                    NotifyPropertyChanged("Birthday");
                }
            }
        }

        private EntitySet<SubjectMember> _subjectMembers = new EntitySet<SubjectMember>();
        [Association(Name = "FK_SubjectMembers_Members", Storage = "_subjectMembers", OtherKey = "_memberMatNr", ThisKey = "MatNr", DeleteRule="CASCADE")]
        internal ICollection<SubjectMember> SubjectMembers {
            get { return _subjectMembers; }
            set { _subjectMembers.Assign(value); }
        }

        public ICollection<Subject> Subjects {
            get {
                var subjects = new ObservableCollection<Subject>(from sm in SubjectMembers select sm.Subject);
                subjects.CollectionChanged += SubjectCollectionChanged;
                return subjects;
            }
        }

        private void SubjectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (NotifyCollectionChangedAction.Add == e.Action) {
                foreach (Subject addedSubject in e.NewItems) {
                    OnSubjectAdded(addedSubject);
                }
            }

            if (NotifyCollectionChangedAction.Remove == e.Action) {
                foreach (Subject removedSubject in e.OldItems) {
                    OnSubjectRemoved(removedSubject);
                }
            }
        }

        private void OnSubjectRemoved(Subject removedSubject) {
            NotifyPropertyChanging("Subjects");
            SubjectMember sm = SubjectMembers.SingleOrDefault(record => record.Member == this && record.Subject == removedSubject);
            if (sm != null) {
                sm.Remove();
            }
        }

        private void OnSubjectAdded(Subject addedSubject) {
            NotifyPropertyChanging("Subjects");
            SubjectMember sm = new SubjectMember() { Member = this, Subject = addedSubject };
        }
    }

    [Table(Name = "Subjects")]
    public class Subject : NotifyingModel
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private int Id { get; set; }

        private string _name;
        [Column]
        public string Name {
            get { return _name; }
            set {
                if (_name != value) {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private DateTime _beginDate;
        [Column]
        public DateTime BeginDate {
            get { return _beginDate; }
            set {
                if (_beginDate != value) {
                    NotifyPropertyChanging("BeginDate");
                    _beginDate = value;
                    NotifyPropertyChanged("BeginDate");
                }
            }
        }

        private DateTime _endDate;
        [Column]
        public DateTime EndDate {
            get { return _endDate; }
            set {
                if (_endDate != value) {
                    NotifyPropertyChanging("EndDate");
                    _endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        private EntitySet<SubjectMember> _subjectMembers = new EntitySet<SubjectMember>();
        [Association(Name = "FK_SubjectMembers_Subjects", Storage = "_subjectMembers", OtherKey = "_subjectId", ThisKey = "Id", DeleteRule="CASCADE")]
        internal ICollection<SubjectMember> SubjectMembers {
            get { return _subjectMembers; }
            set { _subjectMembers.Assign(value); }
        }

        public ICollection<Member> Members {
            get {
                var members = new ObservableCollection<Member>(from sm in SubjectMembers select sm.Member);
                members.CollectionChanged += MembersCollectionChanged;
                return members;
            }
        }

        private void MembersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (NotifyCollectionChangedAction.Add == e.Action) {
                foreach (Member addedMember in e.NewItems) {
                    OnMemberAdded(addedMember);
                }
            }

            if (NotifyCollectionChangedAction.Remove == e.Action) {
                foreach (Member removedMember in e.OldItems) {
                    OnMemberRemoved(removedMember);
                }
            }
        }

        private void OnMemberAdded(Member newMember) {
            NotifyPropertyChanging("Members");
            SubjectMember newSubjectMember = new SubjectMember() { Member = newMember, Subject = this };
            NotifyPropertyChanged("Members");
        }

        private void OnMemberRemoved(Member memberToRemove) {
            NotifyPropertyChanging("Members");
            SubjectMember subjectMemberToRemove = SubjectMembers.SingleOrDefault(sm => sm.Member == memberToRemove && sm.Subject == this);
            if (subjectMemberToRemove != null) {
                subjectMemberToRemove.Remove();
            }
            NotifyPropertyChanged("Members");
        }
    }

    [Table(Name = "SubjectMembers")]
    internal class SubjectMember
    {
        [Column(IsPrimaryKey = true, Name = "Subject")]
        private int _subjectId;

        private EntityRef<Subject> _subject = new EntityRef<Subject>();
        [Association(Name = "FK_SubjectMembers_Subjects", IsForeignKey = true, Storage = "_subject", ThisKey = "_subjectId")]
        public Subject Subject {
            get { return _subject.Entity; }
            set {
                Subject priorSubject = _subject.Entity;
                Subject newSubject = value;

                if (newSubject != priorSubject) {
                    _subject.Entity = null;
                    if (priorSubject != null) {
                        priorSubject.SubjectMembers.Remove(this);
                    }

                    _subject.Entity = newSubject;
                    newSubject.SubjectMembers.Add(this);
                }

            }
        }

        [Column(IsPrimaryKey = true, Name = "Member")]
        private int _memberMatNr;

        private EntityRef<Member> _member = new EntityRef<Member>();
        [Association(Name = "FK_SubjectMembers_Members", IsForeignKey = true, Storage = "_member", ThisKey = "_memberMatNr")]
        public Member Member {
            get { return _member.Entity; }
            set {
                Member priorMemer = _member.Entity;
                Member newMember = value;

                if (newMember != priorMemer) {
                    _member.Entity = null;
                    if (priorMemer != null) {
                        priorMemer.SubjectMembers.Remove(this);
                    }

                    _member.Entity = newMember;
                    newMember.SubjectMembers.Add(this);
                }
            }
        }

        public void Remove() {
            PhoneAppContext.RemoveRecord(this);

            Subject priorSubject = Subject;
            priorSubject.SubjectMembers.Remove(this);

            Member priorMember = Member;
            priorSubject.SubjectMembers.Remove(this);
        }
    }
}