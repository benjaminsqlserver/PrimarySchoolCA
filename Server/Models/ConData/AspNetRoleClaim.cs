using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("AspNetRoleClaims", Schema = "dbo")]
    public partial class AspNetRoleClaim
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
        public int Id { get; set; }

        [ConcurrencyCheck]
        public string ClaimType { get; set; }

        [ConcurrencyCheck]
        public string ClaimValue { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string RoleId { get; set; }

        public AspNetRole AspNetRole { get; set; }

    }
}