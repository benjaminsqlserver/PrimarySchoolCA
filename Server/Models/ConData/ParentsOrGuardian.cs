using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("ParentsOrGuardians", Schema = "dbo")]
    public partial class ParentsOrGuardian
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
        public long ParentOrGuardianID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string FirstName { get; set; }

        [ConcurrencyCheck]
        public string MiddleName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LastName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int GenderID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ResidentialAddress { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Town { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int StateID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int LgaID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string PhoneNumber1 { get; set; }

        [ConcurrencyCheck]
        public string PhoneNumber2 { get; set; }

        [ConcurrencyCheck]
        public string EmailAddress { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long StudentID { get; set; }

        public Gender Gender { get; set; }

        public LocalGovtArea LocalGovtArea { get; set; }

        public State State { get; set; }

        public Student Student { get; set; }

    }
}