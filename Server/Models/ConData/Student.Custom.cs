using System.ComponentModel.DataAnnotations.Schema;

namespace PrimarySchoolCA.Server.Models.ConData
{
    public partial class Student
    {

        [NotMapped]
        public string StudentDetails
        {
            get
            {
                return AdmissionNumber+" "+FirstName+" "+LastName;
            }
        }
    }
}