using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PrimarySchoolCA.Server.Models.ConData;

namespace PrimarySchoolCA.Server.Data
{
    public partial class ConDataContext : DbContext
    {
        public ConDataContext()
        {
        }

        public ConDataContext(DbContextOptions<ConDataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.InsertSingleCaRecord>().HasNoKey();

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown>().HasNoKey();

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>().HasKey(table => new {
                table.LoginProvider, table.ProviderKey
            });

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>().HasKey(table => new {
                table.UserId, table.RoleId
            });

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>().HasKey(table => new {
                table.UserId, table.LoginProvider, table.Name
            });

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>()
              .HasOne(i => i.AspNetRole)
              .WithMany(i => i.AspNetRoleClaims)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserClaims)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserLogins)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>()
              .HasOne(i => i.AspNetRole)
              .WithMany(i => i.AspNetUserRoles)
              .HasForeignKey(i => i.RoleId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserRoles)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>()
              .HasOne(i => i.AspNetUser)
              .WithMany(i => i.AspNetUserTokens)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>()
              .HasOne(i => i.AcademicSession)
              .WithMany(i => i.AssessmentSetups)
              .HasForeignKey(i => i.AcademicSessionID)
              .HasPrincipalKey(i => i.AcademicSessionID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>()
              .HasOne(i => i.AssessmentType)
              .WithMany(i => i.AssessmentSetups)
              .HasForeignKey(i => i.AssessmentTypeID)
              .HasPrincipalKey(i => i.AssessmentTypeID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>()
              .HasOne(i => i.SchoolClass)
              .WithMany(i => i.AssessmentSetups)
              .HasForeignKey(i => i.SchoolClassID)
              .HasPrincipalKey(i => i.SchoolClassID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>()
              .HasOne(i => i.Subject)
              .WithMany(i => i.AssessmentSetups)
              .HasForeignKey(i => i.SubjectID)
              .HasPrincipalKey(i => i.SubjectID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>()
              .HasOne(i => i.Term)
              .WithMany(i => i.AssessmentSetups)
              .HasForeignKey(i => i.TermID)
              .HasPrincipalKey(i => i.TermID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Attendance>()
              .HasOne(i => i.AcademicSession)
              .WithMany(i => i.Attendances)
              .HasForeignKey(i => i.AcademicSessionID)
              .HasPrincipalKey(i => i.AcademicSessionID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Attendance>()
              .HasOne(i => i.SchoolClass)
              .WithMany(i => i.Attendances)
              .HasForeignKey(i => i.SchoolClassID)
              .HasPrincipalKey(i => i.SchoolClassID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Attendance>()
              .HasOne(i => i.Student)
              .WithMany(i => i.Attendances)
              .HasForeignKey(i => i.StudentID)
              .HasPrincipalKey(i => i.StudentID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Attendance>()
              .HasOne(i => i.Term)
              .WithMany(i => i.Attendances)
              .HasForeignKey(i => i.TermID)
              .HasPrincipalKey(i => i.TermID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegister>()
              .HasOne(i => i.AcademicSession)
              .WithMany(i => i.ClassRegisters)
              .HasForeignKey(i => i.AcademicSessionID)
              .HasPrincipalKey(i => i.AcademicSessionID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegister>()
              .HasOne(i => i.SchoolClass)
              .WithMany(i => i.ClassRegisters)
              .HasForeignKey(i => i.SchoolClassID)
              .HasPrincipalKey(i => i.SchoolClassID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegister>()
              .HasOne(i => i.Term)
              .WithMany(i => i.ClassRegisters)
              .HasForeignKey(i => i.TermID)
              .HasPrincipalKey(i => i.TermID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>()
              .HasOne(i => i.ClassRegister)
              .WithMany(i => i.ClassRegisterStudents)
              .HasForeignKey(i => i.ClassRegisterID)
              .HasPrincipalKey(i => i.ClassRegisterID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>()
              .HasOne(i => i.Student)
              .WithMany(i => i.ClassRegisterStudents)
              .HasForeignKey(i => i.StudentID)
              .HasPrincipalKey(i => i.StudentID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>()
              .HasOne(i => i.State)
              .WithMany(i => i.LocalGovtAreas)
              .HasForeignKey(i => i.StateID)
              .HasPrincipalKey(i => i.StateID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>()
              .HasOne(i => i.Gender)
              .WithMany(i => i.ParentsOrGuardians)
              .HasForeignKey(i => i.GenderID)
              .HasPrincipalKey(i => i.GenderID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>()
              .HasOne(i => i.LocalGovtArea)
              .WithMany(i => i.ParentsOrGuardians)
              .HasForeignKey(i => i.LgaID)
              .HasPrincipalKey(i => i.LgaID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>()
              .HasOne(i => i.State)
              .WithMany(i => i.ParentsOrGuardians)
              .HasForeignKey(i => i.StateID)
              .HasPrincipalKey(i => i.StateID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>()
              .HasOne(i => i.Student)
              .WithMany(i => i.ParentsOrGuardians)
              .HasForeignKey(i => i.StudentID)
              .HasPrincipalKey(i => i.StudentID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.School>()
              .HasOne(i => i.LocalGovtArea)
              .WithMany(i => i.Schools)
              .HasForeignKey(i => i.LGAID)
              .HasPrincipalKey(i => i.LgaID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.School>()
              .HasOne(i => i.State)
              .WithMany(i => i.Schools)
              .HasForeignKey(i => i.StateID)
              .HasPrincipalKey(i => i.StateID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Student>()
              .HasOne(i => i.Gender)
              .WithMany(i => i.Students)
              .HasForeignKey(i => i.GenderID)
              .HasPrincipalKey(i => i.GenderID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>()
              .HasOne(i => i.SchoolType)
              .WithMany(i => i.SubjectSchoolTypes)
              .HasForeignKey(i => i.SchoolTypeID)
              .HasPrincipalKey(i => i.SchoolTypeID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>()
              .HasOne(i => i.Subject)
              .WithMany(i => i.SubjectSchoolTypes)
              .HasForeignKey(i => i.SubjectID)
              .HasPrincipalKey(i => i.SubjectID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .HasOne(i => i.AcademicSession)
              .WithMany(i => i.ContinuousAssessments)
              .HasForeignKey(i => i.AcademicSessionID)
              .HasPrincipalKey(i => i.AcademicSessionID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .HasOne(i => i.SchoolClass)
              .WithMany(i => i.ContinuousAssessments)
              .HasForeignKey(i => i.SchoolClassID)
              .HasPrincipalKey(i => i.SchoolClassID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .HasOne(i => i.Student)
              .WithMany(i => i.ContinuousAssessments)
              .HasForeignKey(i => i.StudentID)
              .HasPrincipalKey(i => i.StudentID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .HasOne(i => i.Subject)
              .WithMany(i => i.ContinuousAssessments)
              .HasForeignKey(i => i.SubjectID)
              .HasPrincipalKey(i => i.SubjectID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .HasOne(i => i.Term)
              .WithMany(i => i.ContinuousAssessments)
              .HasForeignKey(i => i.TermID)
              .HasPrincipalKey(i => i.TermID);

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.AspNetUser>()
              .Property(p => p.LockoutEnd)
              .HasColumnType("datetimeoffset");

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Attendance>()
              .Property(p => p.AttendanceDate)
              .HasColumnType("datetime");

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>()
              .Property(p => p.DateAdded)
              .HasColumnType("datetime");

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.Student>()
              .Property(p => p.DateOfBirth)
              .HasColumnType("datetime");

            builder.Entity<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>()
              .Property(p => p.EntryDate)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AcademicSession> AcademicSessions { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetRole> AspNetRoles { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> AspNetUserClaims { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> AspNetUserLogins { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> AspNetUserRoles { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetUser> AspNetUsers { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> AspNetUserTokens { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> AssessmentSetups { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.AssessmentType> AssessmentTypes { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.Attendance> Attendances { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.ClassRegister> ClassRegisters { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> ClassRegisterStudents { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.Gender> Genders { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.InsertSingleCaRecord> InsertSingleCaRecords { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> LocalGovtAreas { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> ParentsOrGuardians { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.SchoolClass> SchoolClasses { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.School> Schools { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.SchoolType> SchoolTypes { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.State> States { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown> StudentListForParentGuardianDropdowns { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.Student> Students { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.Subject> Subjects { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> SubjectSchoolTypes { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.Term> Terms { get; set; }

        public DbSet<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> ContinuousAssessments { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}