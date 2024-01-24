using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("AspNetUserRoles", Schema = "dbo")]
    public partial class AspNetUserRole
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
        public string UserId { get; set; }

        [Key]
        [Required]
        public string RoleId { get; set; }

        public AspNetRole AspNetRole { get; set; }

        public AspNetUser AspNetUser { get; set; }

    }
}