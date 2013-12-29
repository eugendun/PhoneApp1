using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace PhoneApp1.Models
{
    public class PhoneAppContext : DataContext
    {
        public PhoneAppContext(string connectionString) : base(connectionString) { }

        public Table<Member> Members;
        //public Table<Subject> Subjects;
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
        // TODO find out what does it mean.
        //[Column(IsVersion = true)]
        //private Binary _version;

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

        #region relationships

        [Column]
        private EntitySet<SubjectMember> _membersSubjects;
        internal int _memberSubjectId;

        [Association(Storage = "_membersSubjects", ThisKey = "_memberSubjectId", OtherKey = "_memberId")]
        public EntitySet<SubjectMember> MembersSubjects
        {
            get { return _membersSubjects; }
            set { _membersSubjects.Assign(value); }
        }

        public void AddSubjectMember(SubjectMember subjectMember)
        {
            _membersSubjects.Add(subjectMember);
        }

        #endregion
    }

    //[Table]
    //public class Subject : NotifyingModel
    //{
    //    private int _subjectId;
    //    [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
    //    public int SubjectId
    //    {
    //        get { return _subjectId; }
    //        set
    //        {
    //            if (_subjectId != value)
    //            {
    //                NotifyPropertyChanging("Id");
    //                _subjectId = value;
    //                NotifyPropertyChanged("Id");
    //            }
    //        }
    //    }

    //    private string _name;
    //    [Column]
    //    public string Name
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            if (_name != value)
    //            {
    //                NotifyPropertyChanging("Name");
    //                _name = value;
    //                NotifyPropertyChanged("Name");
    //            }
    //        }
    //    }

    //    private DateTime _beginDate;
    //    [Column]
    //    public DateTime BeginDate
    //    {
    //        get { return _beginDate; }
    //        set
    //        {
    //            if (_beginDate != value)
    //            {
    //                NotifyPropertyChanging("BeginDate");
    //                _beginDate = value;
    //                NotifyPropertyChanged("BeginDate");
    //            }
    //        }
    //    }

    //    private DateTime _endDate;
    //    [Column]
    //    public DateTime EndDate
    //    {
    //        get { return _endDate; }
    //        set
    //        {
    //            if (_endDate != value)
    //            {
    //                NotifyPropertyChanging("EndDate");
    //                _endDate = value;
    //                NotifyPropertyChanged("EndDate");
    //            }
    //        }
    //    }

    //    private EntitySet<SubjectMember> _subjectMembers;
    //    [Association(Storage = "_subjectMembers", ThisKey = "SubjectId", OtherKey = "SubjectMemberId", DeleteOnNull = false)]
    //    public EntitySet<SubjectMember> SubjectMembers
    //    {
    //        get { return _subjectMembers; }
    //        set { _subjectMembers.Assign(value); }
    //    }
    //}

    [Table]
    public class SubjectMember : NotifyingModel
    {
        private int _subjectMemberId;
        //private EntityRef<Subject> _subject;

        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int SubjectMemberId
        {
            get { return _subjectMemberId; }
            set
            {
                if (_subjectMemberId != value)
                {
                    _subjectMemberId = value;
                }
            }
        }

        #region relationships

        [Column]
        internal int _memberId;
        private EntityRef<Member> _member;

        //[Association(OtherKey = "SubjectId", ThisKey = "SubjectMemberId", DeleteOnNull = true)]
        //public EntityRef<Subject> Subject
        //{
        //    get { return _subject; }
        //    set { _subject = value; }
        //}

        [Association(Storage = "_member", OtherKey = "MatNr", ThisKey = "_memberId", IsForeignKey = true)]
        public Member Member
        {
            get { return _member.Entity; }
            set
            {
                NotifyPropertyChanging("Member");
                _memberId = value.MatNr;
                NotifyPropertyChanged("Member");
            }

        }

        #endregion
    }
}