using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("SchoolClasses", Schema = "dbo")]
    public partial class SchoolClass
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
        public int SchoolClassID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SchoolClassName { get; set; }

        public ICollection<AssessmentSetup> AssessmentSetups { get; set; }

        public ICollection<Attendance> Attendances { get; set; }

        public ICollection<ClassRegister> ClassRegisters { get; set; }

        public ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

    }
}