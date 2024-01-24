using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("Subjects", Schema = "dbo")]
    public partial class Subject
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
        public long SubjectID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SubjectName { get; set; }

        public ICollection<AssessmentSetup> AssessmentSetups { get; set; }

        public ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

        public ICollection<SubjectSchoolType> SubjectSchoolTypes { get; set; }

    }
}