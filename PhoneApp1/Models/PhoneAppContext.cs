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

        public static PhoneAppContext GetDataContext() {
            return new PhoneAppContext(_connectionString); ;
        }
    }

    public class PhoneAppContext : DataContext
    {
        public Table<Lecture> Lectures;
        public Table<ExceptionDate> ExceptionDates;

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
    public class Lecture
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
                    _name = value;
                }
            }
        }

        private DateTime _beginDate;
        [Column(Name="BeginDate", Storage="_beginDate")]
        public DateTime BeginDate {
            get { return _beginDate; }
            set {
                if (_beginDate!=value) {
                    _beginDate=value;
                }
            }
        }

        private DateTime _endDate;
        [Column(Name="EndDate", Storage="_endDate")]
        public DateTime EndDate {
            get { return _endDate; }
            set {
                if (_endDate!=value) {
                    _endDate=value;
                }
            }
        }

        private EntitySet<ExceptionDate> _exceptionDates;
        [Association(Name="FK_ExceptionDates_Lectures", Storage="_exceptionDates", OtherKey="Id", DeleteRule="CASCADE")]
        public ICollection<ExceptionDate> ExceptionDates {
            get { return _exceptionDates; }
            set { _exceptionDates.Assign(value); }
        }

        private EntitySet<LectureTime> _lectureTimes;
        [Association(Name="FK_LectureTimes_Lectures", Storage="_lectureTimes", OtherKey="Id", DeleteRule="CASCADE")]
        public ICollection<LectureTime> LectureTimes {
            get { return _lectureTimes; }
            set { _lectureTimes.Assign(value); }
        }

        public Lecture() {
            _exceptionDates=new EntitySet<ExceptionDate>(new Action<ExceptionDate>(OnAddExceptionDate), new Action<ExceptionDate>(OnRemoveExceptionDate));
            _lectureTimes=new EntitySet<LectureTime>(new Action<LectureTime>(OnAddLectureTime), new Action<LectureTime>(OnRemoveLectureTime));
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
    public class LectureTime
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
                    _hours=value;
                }
            }
        }

        private int _minutes;
        [Column(Name="Minutes", Storage="_minutes")]
        public int Minutes {
            get { return _minutes; }
            set {
                if (_minutes!=value) {
                    _minutes=value;
                }
            }
        }

        private int _duration;
        [Column(Name="Duration", Storage="_duration")]
        public int Duration {
            get { return _duration; }
            set {
                if (_duration!=value) {
                    _duration=value;
                }
            }
        }

        private int _weekday;
        [Column(Name="Weekday", Storage="_weekday")]
        public int Weekday {
            get { return _weekday; }
            set {
                if (_weekday!=value) {
                    _weekday=value;
                }
            }
        }

        private EntityRef<Lecture> _lecture;
        [Association(Name="FK_LectureTimes_Lectures", Storage="_lecture", IsForeignKey=true, ThisKey="Id", DeleteOnNull=true)]
        public Lecture Lecture {
            get { return _lecture.Entity; }
            set {
                if (_lecture.Entity!=value) {
                    _lecture.Entity=value;
                }
            }
        }
    }

    [Table(Name="Tutors")]
    public class Tutor
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
                    _surname=value;
                }
            }
        }

        private string _forename;
        [Column(Name="Forename", Storage="_forename")]
        public string Forename {
            get { return _forename; }
            set {
                if (_forename!=value) {
                    _forename=value;
                }
            }
        }
    }

    [Table(Name="Followers")]
    public class Follower
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
    }

    #endregion
}