using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using PrimarySchoolCA.Server.Data;

namespace PrimarySchoolCA.Server.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/academicsessions/csv")]
        [HttpGet("/export/ConData/academicsessions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAcademicSessionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAcademicSessions(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/academicsessions/excel")]
        [HttpGet("/export/ConData/academicsessions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAcademicSessionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAcademicSessions(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetroleclaims/csv")]
        [HttpGet("/export/ConData/aspnetroleclaims/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRoleClaimsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetRoleClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetroleclaims/excel")]
        [HttpGet("/export/ConData/aspnetroleclaims/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRoleClaimsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetRoleClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetroles/csv")]
        [HttpGet("/export/ConData/aspnetroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetroles/excel")]
        [HttpGet("/export/ConData/aspnetroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserclaims/csv")]
        [HttpGet("/export/ConData/aspnetuserclaims/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserClaimsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserclaims/excel")]
        [HttpGet("/export/ConData/aspnetuserclaims/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserClaimsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserClaims(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserlogins/csv")]
        [HttpGet("/export/ConData/aspnetuserlogins/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserLoginsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserLogins(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserlogins/excel")]
        [HttpGet("/export/ConData/aspnetuserlogins/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserLoginsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserLogins(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserroles/csv")]
        [HttpGet("/export/ConData/aspnetuserroles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetuserroles/excel")]
        [HttpGet("/export/ConData/aspnetuserroles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserRoles(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetusers/csv")]
        [HttpGet("/export/ConData/aspnetusers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetusers/excel")]
        [HttpGet("/export/ConData/aspnetusers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUsersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUsers(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetusertokens/csv")]
        [HttpGet("/export/ConData/aspnetusertokens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserTokensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAspNetUserTokens(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/aspnetusertokens/excel")]
        [HttpGet("/export/ConData/aspnetusertokens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAspNetUserTokensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAspNetUserTokens(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/assessmentsetups/csv")]
        [HttpGet("/export/ConData/assessmentsetups/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentSetupsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAssessmentSetups(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/assessmentsetups/excel")]
        [HttpGet("/export/ConData/assessmentsetups/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentSetupsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAssessmentSetups(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/assessmenttypes/csv")]
        [HttpGet("/export/ConData/assessmenttypes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAssessmentTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/assessmenttypes/excel")]
        [HttpGet("/export/ConData/assessmenttypes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAssessmentTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/attendances/csv")]
        [HttpGet("/export/ConData/attendances/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAttendancesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAttendances(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/attendances/excel")]
        [HttpGet("/export/ConData/attendances/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAttendancesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAttendances(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/classregisters/csv")]
        [HttpGet("/export/ConData/classregisters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClassRegistersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClassRegisters(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/classregisters/excel")]
        [HttpGet("/export/ConData/classregisters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClassRegistersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClassRegisters(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/classregisterstudents/csv")]
        [HttpGet("/export/ConData/classregisterstudents/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClassRegisterStudentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetClassRegisterStudents(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/classregisterstudents/excel")]
        [HttpGet("/export/ConData/classregisterstudents/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportClassRegisterStudentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetClassRegisterStudents(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/genders/csv")]
        [HttpGet("/export/ConData/genders/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGendersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGenders(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/genders/excel")]
        [HttpGet("/export/ConData/genders/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGendersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGenders(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/localgovtareas/csv")]
        [HttpGet("/export/ConData/localgovtareas/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLocalGovtAreasToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLocalGovtAreas(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/localgovtareas/excel")]
        [HttpGet("/export/ConData/localgovtareas/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLocalGovtAreasToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLocalGovtAreas(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/parentsorguardians/csv")]
        [HttpGet("/export/ConData/parentsorguardians/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParentsOrGuardiansToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetParentsOrGuardians(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/parentsorguardians/excel")]
        [HttpGet("/export/ConData/parentsorguardians/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParentsOrGuardiansToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetParentsOrGuardians(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schoolclasses/csv")]
        [HttpGet("/export/ConData/schoolclasses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolClassesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchoolClasses(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schoolclasses/excel")]
        [HttpGet("/export/ConData/schoolclasses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolClassesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchoolClasses(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schools/csv")]
        [HttpGet("/export/ConData/schools/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchools(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schools/excel")]
        [HttpGet("/export/ConData/schools/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchools(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schooltypes/csv")]
        [HttpGet("/export/ConData/schooltypes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchoolTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/schooltypes/excel")]
        [HttpGet("/export/ConData/schooltypes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchoolTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/states/csv")]
        [HttpGet("/export/ConData/states/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStates(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/states/excel")]
        [HttpGet("/export/ConData/states/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStates(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/students/csv")]
        [HttpGet("/export/ConData/students/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudents(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/students/excel")]
        [HttpGet("/export/ConData/students/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudents(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/subjects/csv")]
        [HttpGet("/export/ConData/subjects/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSubjects(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/subjects/excel")]
        [HttpGet("/export/ConData/subjects/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSubjects(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/subjectschooltypes/csv")]
        [HttpGet("/export/ConData/subjectschooltypes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectSchoolTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSubjectSchoolTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/subjectschooltypes/excel")]
        [HttpGet("/export/ConData/subjectschooltypes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectSchoolTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSubjectSchoolTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/terms/csv")]
        [HttpGet("/export/ConData/terms/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTermsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTerms(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/terms/excel")]
        [HttpGet("/export/ConData/terms/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTermsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTerms(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/continuousassessments/csv")]
        [HttpGet("/export/ConData/continuousassessments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContinuousAssessmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetContinuousAssessments(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/continuousassessments/excel")]
        [HttpGet("/export/ConData/continuousassessments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContinuousAssessmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetContinuousAssessments(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/studentlistforparentguardiandropdowns/csv(, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentListForParentGuardianDropdownsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudentListForParentGuardianDropdowns(), Request.Query), fileName);
        }

        [HttpGet("/export/ConData/studentlistforparentguardiandropdowns/excel(, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentListForParentGuardianDropdownsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudentListForParentGuardianDropdowns(), Request.Query), fileName);
        }
    }
}
