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
    #region db context

    public class DataContextFactory
    {
        private static string _connectionString = "Data Source=isostore:/PhoneApp1.sdf";
        private static PhoneAppContext _dbContext;

        public static PhoneAppContext GetDataContext() {
            if (_dbContext == null) {
                _dbContext = new PhoneAppContext(_connectionString);
                if (!_dbContext.DatabaseExists()) {
                    _dbContext.CreateDatabase();
                }
            }
            return _dbContext;
        }
    }

    public class PhoneAppContext : DataContext
    {
        public Table<Lecture> Lectures;
        public Table<ExceptionDate> ExceptionDates;
        public Table<Tutor> Tutors;
        public Table<Member> Members;

        private Table<LectureTutor> LectureTutors;
        private Table<LectureMember> LectureMembers;

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

    #endregion

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

    #region Tables

    [Table(Name="Lectures")]
    public class Lecture : NotifyingModel
    {
        private int _id;
        [Column(Name="Id", Storage="_id", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id { get { return _id; } }

        private string _name;
        [Column(Name="Name", Storage="_name")]
        public string Name {
            get { return _name; }
            set {
                if (value!=_name) {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private DateTime _beginDate;
        [Column(Name="BeginDate", Storage="_beginDate")]
        public DateTime BeginDate {
            get { return _beginDate; }
            set {
                if (_beginDate!=value) {
                    NotifyPropertyChanging("BeginDate");
                    _beginDate=value;
                    NotifyPropertyChanged("BeginDate");
                }
            }
        }

        private DateTime _endDate;
        [Column(Name="EndDate", Storage="_endDate")]
        public DateTime EndDate {
            get { return _endDate; }
            set {
                if (_endDate!=value) {
                    NotifyPropertyChanging("EndDate");
                    _endDate=value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        private EntitySet<ExceptionDate> _exceptionDates;
        [Association(Name="FK_ExceptionDates_Lectures", Storage="_exceptionDates", OtherKey="Id", DeleteRule="CASCADE")]
        public ICollection<ExceptionDate> ExceptionDates {
            get { return _exceptionDates; }
            set {
                NotifyPropertyChanging("ExceptionDates");
                _exceptionDates.Assign(value);
                NotifyPropertyChanged("ExceptionDates");
            }
        }

        private EntitySet<LectureTime> _lectureTimes;
        [Association(Name="FK_LectureTimes_Lectures", Storage="_lectureTimes", OtherKey="Id", DeleteRule="CASCADE")]
        public ICollection<LectureTime> LectureTimes {
            get { return _lectureTimes; }
            set {
                NotifyPropertyChanging("LectureTimes");
                _lectureTimes.Assign(value);
                NotifyPropertyChanged("LectureTimes");
            }
        }

        private EntitySet<LectureTutor> _lectureTutor;
        [Association(Name="FK_LectureTutors_Lectures", Storage="_lectureTutor", OtherKey="_lectureId", DeleteRule="CASCADE")]
        private ICollection<LectureTutor> LectureTutors {
            get { return _lectureTutor; }
            set {
                _lectureTutor.Assign(value);
            }
        }

        public ICollection<Tutor> Tutors {
            get {
                var tutors = new ObservableCollection<Tutor>(from lt in LectureTutors select lt.Tutor);
                tutors.CollectionChanged += OnTutorsCollectionChanged;
                return tutors;
            }
        }

        private void OnTutorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                NotifyPropertyChanging("Tutors");
                foreach (Tutor addedTutor in e.NewItems) {
                    LectureTutors.Add(new LectureTutor { Lecture=this, Tutor=addedTutor });
                }
                NotifyPropertyChanged("Tutors");
            }

            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Tutor removedTutor in e.OldItems) {
                    var affectedLectureTutors = from lt in LectureTutors
                                                where lt.Tutor==removedTutor
                                                select lt;
                    NotifyPropertyChanging("Tutors");
                    foreach (LectureTutor lectureTutorToRemove in affectedLectureTutors) {
                        lectureTutorToRemove.Tutor = null;
                    }
                    NotifyPropertyChanged("Tutors");
                }
            }
        }

        private EntitySet<LectureMember> _lectureMembers;
        [Association(Name="FK_LectureMembers_Lectures", Storage="_lectureMembers", OtherKey="_lectureId", DeleteRule="CASCADE")]
        private ICollection<LectureMember> LectureMembers {
            get { return _lectureMembers; }
            set { _lectureMembers.Assign(value); }
        }

        public ICollection<Member> Members {
            get {
                var members = new ObservableCollection<Member>(from lm in LectureMembers select lm.Member);
                members.CollectionChanged += OnMembersCollectionChanged;
                return members;
            }
        }

        private void OnMembersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                NotifyPropertyChanging("Members");
                foreach (Member addedMember in e.NewItems) {
                    LectureMembers.Add(new LectureMember { Lecture = this, Member = addedMember });
                }
                NotifyPropertyChanged("Members");
            }

            if (e.Action == NotifyCollectionChangedAction.Remove) {
                foreach (Member removedMember in e.OldItems) {
                    var affectedLectureMembers = from lm in LectureMembers
                                                 where lm.Member == removedMember
                                                 select lm;
                    NotifyPropertyChanging("Members");
                    foreach (LectureMember affectedLectureMember in affectedLectureMembers) {
                        affectedLectureMember.Lecture = null;
                    }
                    NotifyPropertyChanged("Members");
                }
            }
        }

        public Lecture() {
            _exceptionDates = new EntitySet<ExceptionDate>(new Action<ExceptionDate>(OnAddExceptionDate), new Action<ExceptionDate>(OnRemoveExceptionDate));
            _lectureTimes = new EntitySet<LectureTime>(new Action<LectureTime>(OnAddLectureTime), new Action<LectureTime>(OnRemoveLectureTime));
            _lectureTutor = new EntitySet<LectureTutor>();
            _lectureMembers = new EntitySet<LectureMember>();
        }

        private void OnRemoveLectureTime(LectureTime obj) {
            obj.Lecture = null;
        }

        private void OnAddLectureTime(LectureTime obj) {
            obj.Lecture = this;
        }

        private void OnRemoveExceptionDate(ExceptionDate date) {
            date.Lecture = null;
        }

        private void OnAddExceptionDate(ExceptionDate date) {
            date.Lecture = this;
        }

    }

    [Table(Name="ExceptionDates")]
    public class ExceptionDate
    {
        private int _id;
        [Column(Name="Id", Storage="_id", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id { get { return _id; } }

        private DateTime _date;
        [Column(Name="Date", Storage="_date")]
        public DateTime Date {
            get { return _date; }
            set {
                if (value!=_date) {
                    _date = value;
                }
            }
        }

        private EntityRef<Lecture> _lecture;
        [Association(Name="FK_ExceptionDates_Lectures", Storage="_lecture", IsForeignKey=true, ThisKey="Id", DeleteOnNull=true)]
        public Lecture Lecture {
            get { return _lecture.Entity; }
            set {
                if (value!=_lecture.Entity) {
                    _lecture.Entity = value;
                }
            }
        }
    }

    [Table(Name="LectureTimes")]
    public class LectureTime : NotifyingModel
    {
        private int _id;
        [Column(Name="Id", Storage="_id", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id { get { return _id; } }

        private int _hours;
        [Column(Name="Hours", Storage="_hours")]
        public int Hours {
            get { return _hours; }
            set {
                if (Hours!=value) {
                    NotifyPropertyChanging("Hours");
                    _hours=value;
                    NotifyPropertyChanged("Hours");
                }
            }
        }

        private int _minutes;
        [Column(Name="Minutes", Storage="_minutes")]
        public int Minutes {
            get { return _minutes; }
            set {
                if (_minutes!=value) {
                    NotifyPropertyChanging("Minutes");
                    _minutes=value;
                    NotifyPropertyChanged("Minutes");
                }
            }
        }

        private int _duration;
        [Column(Name="Duration", Storage="_duration")]
        public int Duration {
            get { return _duration; }
            set {
                if (_duration!=value) {
                    NotifyPropertyChanging("Duration");
                    _duration=value;
                    NotifyPropertyChanged("Duration");
                }
            }
        }

        private int _weekday;
        [Column(Name="Weekday", Storage="_weekday")]
        public int Weekday {
            get { return _weekday; }
            set {
                if (_weekday!=value) {
                    NotifyPropertyChanging("Weekday");
                    _weekday=value;
                    NotifyPropertyChanged("Weekday");
                }
            }
        }

        private EntityRef<Lecture> _lecture;
        [Association(Name="FK_LectureTimes_Lectures", Storage="_lecture", IsForeignKey=true, ThisKey="Id", DeleteOnNull=true)]
        public Lecture Lecture {
            get { return _lecture.Entity; }
            set {
                if (_lecture.Entity!=value) {
                    NotifyPropertyChanging("Lecture");
                    _lecture.Entity=value;
                    NotifyPropertyChanged("Lecture");
                }
            }
        }
    }

    [Table(Name="Tutors")]
    public class Tutor : NotifyingModel
    {
        private int _id;
        [Column(Name="Id", Storage="_id", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id { get { return _id; } }

        private string _surname;
        [Column(Name="Surname", Storage="_surname")]
        public string Surname {
            get { return _surname; }
            set {
                if (_surname!=value) {
                    NotifyPropertyChanging("Surname");
                    _surname=value;
                    NotifyPropertyChanged("Surname");
                }
            }
        }

        private string _forename;
        [Column(Name="Forename", Storage="_forename")]
        public string Forename {
            get { return _forename; }
            set {
                if (_forename!=value) {
                    NotifyPropertyChanging("Forename");
                    _forename=value;
                    NotifyPropertyChanged("Forename");
                }
            }
        }

        private EntitySet<LectureTutor> _lectureTutors;
        [Association(Name="FK_LectureTutors_Tutors", Storage="_lectureTutors", OtherKey="_tutorId", DeleteRule="CASCADE")]
        private ICollection<LectureTutor> LectureTutors {
            get { return _lectureTutors; }
            set {
                _lectureTutors.Assign(value);
            }
        }

        public ICollection<Lecture> Lectures {
            get {
                var lectures = new ObservableCollection<Lecture>(from lt in LectureTutors select lt.Lecture);
                lectures.CollectionChanged += OnLecturesCollectionChanged;
                return lectures;
            }
        }

        private void OnLecturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action==NotifyCollectionChangedAction.Add) {
                NotifyPropertyChanging("Lectures");
                foreach (Lecture addedLecture in e.NewItems) {
                    LectureTutors.Add(new LectureTutor { Lecture=addedLecture, Tutor=this });
                }
                NotifyPropertyChanged("Lectures");
            }
            if (e.Action==NotifyCollectionChangedAction.Remove) {
                foreach (Lecture removedLecture in e.OldItems) {
                    var affectedLectureTutors = from lt in LectureTutors
                                                where lt.Lecture==removedLecture
                                                select lt;
                    NotifyPropertyChanging("Lectures");
                    foreach (LectureTutor lectureTutorToRemove in affectedLectureTutors) {
                        lectureTutorToRemove.Lecture = null;
                    }
                    NotifyPropertyChanging("Lectures");
                }
            }
        }

        public Tutor() {
            _lectureTutors = new EntitySet<LectureTutor>();
        }
    }

    [Table(Name="LectureTutors")]
    public class LectureTutor
    {
        [Column(Name="LectureId", IsPrimaryKey=true)]
        private int _lectureId;

        private EntityRef<Lecture> _lecture;
        [Association(Name="FK_LectureTutors_Lectures", Storage="_lecture", IsForeignKey=true, ThisKey="_lectureId", DeleteOnNull=true)]
        public Lecture Lecture {
            get { return _lecture.Entity; }
            set {
                if (_lecture.Entity!=value) {
                    _lecture.Entity = value;
                }
            }
        }

        [Column(Name="TutorId", IsPrimaryKey=true)]
        private int _tutorId;

        private EntityRef<Tutor> _tutor;
        [Association(Name="FK_LectureTutors_Tutors", Storage="_tutor", IsForeignKey=true, ThisKey="_tutorId", DeleteOnNull=true)]
        public Tutor Tutor {
            get { return _tutor.Entity; }
            set {
                if (_tutor.Entity!=value) {
                    _tutor.Entity=value;
                }
            }
        }
    }

    [Table(Name="LectureMembers")]
    public class LectureMember
    {
        [Column(Name="LectureId", IsPrimaryKey=true)]
        private int _lectureId;

        private EntityRef<Lecture> _lecture;
        [Association(Name="FK_LectureMembers_Lectures", Storage="_lecture", IsForeignKey=true, ThisKey="_lectureId", DeleteOnNull=true)]
        public Lecture Lecture {
            get { return _lecture.Entity; }
            set {
                if (_lecture.Entity != value) {
                    _lecture.Entity = value;
                }
            }
        }

        [Column(Name="MemberId", IsPrimaryKey=true)]
        private int _memberId;

        private EntityRef<Member> _member;
        [Association(Name="FK_LectureMembers_Members", Storage="_member", IsForeignKey=true, ThisKey="_memberId", DeleteOnNull=true)]
        public Member Member {
            get { return _member.Entity; }
            set {
                if (_member.Entity != value) {
                    _member.Entity = value;
                }
            }
        }
    }

    [Table(Name="Members")]
    public class Member
    {
        private int _id;
        [Column(Name="Id", Storage="_id", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id { get { return _id; } }

        private int _matNr;
        [Column(Name="MatNr", Storage="_matNr", CanBeNull=false)]
        public int MatNr {
            get { return _matNr; }
            set {
                if (_matNr!=value) {
                    _matNr=value;
                }
            }
        }

        private string _surname;
        [Column(Name="Surname", Storage="_surname", CanBeNull=false)]
        public string Surname {
            get { return _surname; }
            set {
                if (_surname!=value) {
                    _surname=value;
                }
            }
        }

        private string _forename;
        [Column(Name="Forename", Storage="_forename", CanBeNull=false)]
        public string Forename {
            get { return _forename; }
            set {
                if (_forename!=value) {
                    _forename=value;
                }
            }
        }

        private DateTime _birthday;
        [Column(Name="Birthday", Storage="_birthday", CanBeNull=false)]
        public DateTime Birthday {
            get { return _birthday; }
            set {
                if (_birthday!=value) {
                    _birthday=value;
                }
            }
        }

        private EntitySet<LectureMember> _lectureMembers;
        [Association(Name="FK_LectureMembers_Members", Storage="_lectureMembers", OtherKey="_memberId", DeleteRule="CASCADE")]
        private ICollection<LectureMember> LectureMembers {
            get { return _lectureMembers; }
            set {
                _lectureMembers.Assign(value);
            }
        }

        public ICollection<Lecture> Lectures {
            get {
                var lectures = new ObservableCollection<Lecture>(from lm in LectureMembers select lm.Lecture);
                lectures.CollectionChanged += OnLecturesCollectionChanged;
                return lectures;
            }
        }

        private void OnLecturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                foreach (Lecture addedLecture in e.NewItems) {
                    LectureMembers.Add(new LectureMember { Lecture = addedLecture, Member = this });
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                foreach (Lecture removedLecture in e.OldItems) {
                    var affectedLectureMembers = from lm in LectureMembers
                                                 where lm.Lecture == removedLecture
                                                 select lm;
                    foreach (LectureMember affectedLectureMember in affectedLectureMembers) {
                        affectedLectureMember.Lecture = null;
                    }
                }
            }
        }

        public Member() {
            _lectureMembers = new EntitySet<LectureMember>();
        }
    }

    #endregion
}