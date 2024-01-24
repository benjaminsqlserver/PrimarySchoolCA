using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("ContinuousAssessments", Schema = "dbo")]
    public partial class ContinuousAssessment
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
        public long RecordID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AcademicSessionID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int TermID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int SchoolClassID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long SubjectID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int CAMarkObtainable { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int CAMarkObtained { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime EntryDate { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string InsertedBy { get; set; }

        public AcademicSession AcademicSession { get; set; }

        public SchoolClass SchoolClass { get; set; }

        public Student Student { get; set; }

        public Subject Subject { get; set; }

        public Term Term { get; set; }

    }
}