using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("Terms", Schema = "dbo")]
    public partial class Term
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
        public int TermID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string TermName { get; set; }

        public ICollection<AssessmentSetup> AssessmentSetups { get; set; }

        public ICollection<Attendance> Attendances { get; set; }

        public ICollection<ClassRegister> ClassRegisters { get; set; }

        public ICollection<ContinuousAssessment> ContinuousAssessments { get; set; }

    }
}