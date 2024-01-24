using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("AspNetUserLogins", Schema = "dbo")]
    public partial class AspNetUserLogin
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
        [Required]
        public string LoginProvider { get; set; }

        [Key]
        [Required]
        public string ProviderKey { get; set; }

        [ConcurrencyCheck]
        public string ProviderDisplayName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string UserId { get; set; }

        public AspNetUser AspNetUser { get; set; }

    }
}