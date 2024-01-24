using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("States", Schema = "dbo")]
    public partial class State
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
        public int StateID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StateName { get; set; }

        public ICollection<LocalGovtArea> LocalGovtAreas { get; set; }

        public ICollection<ParentsOrGuardian> ParentsOrGuardians { get; set; }

        public ICollection<School> Schools { get; set; }

    }
}