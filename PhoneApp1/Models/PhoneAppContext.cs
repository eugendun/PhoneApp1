using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace PhoneApp1.Models
{
    public class PhoneAppContext : DataContext
    {
        public PhoneAppContext(string connectionString) : base(connectionString) { }

        public Table<Member> Members;
        public Table<Subject> Subjects;
        public Table<SubjectMember> SubjectMembers;
    }

    public class NotifyingModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        protected void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class Member : NotifyingModel
    {
        private int _matNr;
        private string _surname;
        private string _forename;
        private DateTime _birthday;

        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
        public int MatNr
        {
            get { return _matNr; }
            set
            {
                if (_matNr != value)
                {
                    NotifyPropertyChanging("MatNr");
                    _matNr = value;
                    NotifyPropertyChanged("MatNr");
                }
            }
        }

        [Column]
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (_surname != value)
                {
                    NotifyPropertyChanging("Surname");
                    _surname = value;
                    NotifyPropertyChanged("Surname");
                }
            }
        }

        [Column]
        public string Forename
        {
            get
            {
                return _forename;
            }
            set
            {
                if (_forename != value)
                {
                    NotifyPropertyChanging("Forename");
                    _forename = value;
                    NotifyPropertyChanged("Forename");
                }
            }
        }

        [Column]
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    NotifyPropertyChanging("Birthday");
                    _birthday = value;
                    NotifyPropertyChanged("Birthday");
                }
            }
        }

        private EntitySet<SubjectMember> _subjectMembers;
        [Association(Storage = "_subjectMembers", OtherKey = "MemberMatNr")]
        public EntitySet<SubjectMember> SubjectMembers
        {
            get { return _subjectMembers; }
            set { _subjectMembers = value; }
        }

        public Member()
        {
            _subjectMembers = new EntitySet<SubjectMember>(
                new Action<SubjectMember>(this.attach_SubjectMember),
                new Action<SubjectMember>(this.detach_SubjectMember));
        }

        private void attach_SubjectMember(SubjectMember newSubjectMember)
        {
            NotifyPropertyChanging("Member");
            newSubjectMember.Member = this;
        }

        private void detach_SubjectMember(SubjectMember subjectMemberToRemove)
        {
            NotifyPropertyChanging("Member");
            subjectMemberToRemove.Member = null;
        }
    }

    [Table]
    public class Subject : NotifyingModel
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        private int SubjectId { get; set; }

        private string _name;
        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private DateTime _beginDate;
        [Column]
        public DateTime BeginDate
        {
            get { return _beginDate; }
            set
            {
                if (_beginDate != value)
                {
                    NotifyPropertyChanging("BeginDate");
                    _beginDate = value;
                    NotifyPropertyChanged("BeginDate");
                }
            }
        }

        private DateTime _endDate;
        [Column]
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    NotifyPropertyChanging("EndDate");
                    _endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        private EntitySet<SubjectMember> _subjectMembers;
        [Association(Storage = "_subjectMembers", OtherKey = "SubjectId")]
        public EntitySet<SubjectMember> SubjectMembers
        {
            get { return _subjectMembers; }
            set { _subjectMembers.Assign(value); }
        }

        public Subject()
        {
            _subjectMembers = new EntitySet<SubjectMember>(
                new Action<SubjectMember>(this.attach_SubjectMember),
                new Action<SubjectMember>(this.detach_SubjectMember));
        }

        private void attach_SubjectMember(SubjectMember newSubjectMember)
        {
            NotifyPropertyChanging("Subject");
            newSubjectMember.Subject = this;
        }

        private void detach_SubjectMember(SubjectMember subjectMemberToRemove)
        {
            NotifyPropertyChanging("Subject");
            subjectMemberToRemove.Subject = null;
        }
    }

    [Table]
    public class SubjectMember
    {
        [Column(IsPrimaryKey = true)]
        private int SubjectId;

        [Column(IsPrimaryKey = true)]
        private int MemberMatNr;

        private EntityRef<Subject> _subject;
        [Association(Storage = "_subject", ThisKey = "SubjectId")]
        public Subject Subject
        {
            get { return _subject.Entity; }
            set { _subject.Entity = value; }
        }

        private EntityRef<Member> _member;
        [Association(Storage = "_member", ThisKey = "MemberMatNr")]
        public Member Member
        {
            get { return _member.Entity; }
            set { _member.Entity = value; }
        }
    }
}