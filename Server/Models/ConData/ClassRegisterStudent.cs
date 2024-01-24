using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("ClassRegisterStudents", Schema = "dbo")]
    public partial class ClassRegisterStudent
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

        [Required]
        [ConcurrencyCheck]
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long ClassRegisterID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime DateAdded { get; set; }

        public ClassRegister ClassRegister { get; set; }

        public Student Student { get; set; }

    }
}