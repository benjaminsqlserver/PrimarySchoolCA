using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("Students", Schema = "dbo")]
    public partial class Student
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string AdmissionNumber { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FirstName { get; set; }

        [ConcurrencyCheck]
        public string MiddleName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LastName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int GenderID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime DateOfBirth { get; set; }

        [ConcurrencyCheck]
        public string PassportPhoto { get; set; }

        public ICollection<Attendance> Attendances { get; set; }

        public ICollection<ClassRegisterStudent> ClassRegisterStudents { get; set; }

        public ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

        public ICollection<ParentsOrGuardian> ParentsOrGuardians { get; set; }

        public Gender Gender { get; set; }

    }
}