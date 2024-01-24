using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("Schools", Schema = "dbo")]
    public partial class School
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
        public long SchoolID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SchoolName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Town { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int StateID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int LGAID { get; set; }

        [ConcurrencyCheck]
        public string SchoolCodeNo { get; set; }

        [ConcurrencyCheck]
        public string MainPhoto { get; set; }

        public LocalGovtArea LocalGovtArea { get; set; }

        public State State { get; set; }

    }
}