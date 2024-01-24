using System;
namespace PrimarySchoolCA.Server.Models.ConData
{
    public partial class AttendanceViewModel
    {
        public long StudentID { get; set; }

        public int AcademicSessionID { get; set; }

        public int TermID { get; set; }

        public int SchoolClassID { get; set; }
        public string AdmissionNumber
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }

        public bool Present
        {
            get;
            set;
        }

        public DateTime AttendanceDate
        {
            get;
            set;
        }
    }
}
