using System;
namespace PrimarySchoolCA.Server.Models.ConData
{
    public partial class ContinuousAssessmentViewModel
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

        public long SubjectID
        {
            get;
            set;
        }

         public int MarkObtained
        {
            get;
            set;
        }


        public int MarkObtainable
        {
            get;
            set;
        }

        public DateTime EntryDate
        {
            get;
            set;
        }
        public string InsertedBy
        {
            get;
            set;
        }
    }
}
