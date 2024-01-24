using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("SubjectSchoolTypes", Schema = "dbo")]
    public partial class SubjectSchoolType
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
        public long ID { get; set; }

        [ConcurrencyCheck]
        public long? SubjectID { get; set; }

        [ConcurrencyCheck]
        public int? SchoolTypeID { get; set; }

        public SchoolType SchoolType { get; set; }

        public Subject Subject { get; set; }

    }
}