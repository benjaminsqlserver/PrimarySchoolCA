using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimarySchoolCA.Server.Models.ConData
{
    [Table("Attendances", Schema = "dbo")]
    public partial class Attendance
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
        public long AttendanceID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AcademicSessionID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int TermID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int SchoolClassID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime AttendanceDate { get; set; }

        [ConcurrencyCheck]
        public bool Present { get; set; }

        public AcademicSession AcademicSession { get; set; }

        public SchoolClass SchoolClass { get; set; }

        public Student Student { get; set; }

        public Term Term { get; set; }

    }
}