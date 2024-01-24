using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("SchoolTypes", Schema = "dbo")]
    public partial class SchoolType
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
        public int SchoolTypeID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SchoolTypeName { get; set; }

        public ICollection<SubjectSchoolType> SubjectSchoolTypes { get; set; }

    }
}