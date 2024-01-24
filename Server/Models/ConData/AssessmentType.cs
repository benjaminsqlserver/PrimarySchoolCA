using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("AssessmentTypes", Schema = "dbo")]
    public partial class AssessmentType
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
        public int AssessmentTypeID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string AssessmentTypeName { get; set; }

        public ICollection<AssessmentSetup> AssessmentSetups { get; set; }

    }
}