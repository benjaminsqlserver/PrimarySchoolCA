using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("LocalGovtAreas", Schema = "dbo")]
    public partial class LocalGovtArea
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
        public int LgaID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LgaName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int StateID { get; set; }

        public State State { get; set; }

        public ICollection<ParentsOrGuardian> ParentsOrGuardians { get; set; }

        public ICollection<School> Schools { get; set; }

    }
}