using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("AssessmentSetups", Schema = "dbo")]
    public partial class AssessmentSetup
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
        public long AssessmentSetupID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AssessmentTypeID { get; set; }

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
        public int MarksObtainable { get; set; }

        public AcademicSession AcademicSession { get; set; }

        public AssessmentType AssessmentType { get; set; }

        public SchoolClass SchoolClass { get; set; }

        public Subject Subject { get; set; }

        public Term Term { get; set; }

    }
}