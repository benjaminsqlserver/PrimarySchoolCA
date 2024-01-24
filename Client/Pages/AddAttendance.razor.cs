using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PrimarySchoolCA.Client.Pages
{
    public partial class AddAttendance
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public ConDataService ConDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            attendance = new PrimarySchoolCA.Server.Models.ConData.Attendance();
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.Attendance attendance;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> academicSessionsForAcademicSessionID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> schoolClassesForSchoolClassID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Student> studentsForStudentID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Term> termsForTermID;


        protected int academicSessionsForAcademicSessionIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.AcademicSession academicSessionsForAcademicSessionIDValue;
        protected async Task academicSessionsForAcademicSessionIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetAcademicSessions(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AcademicSessionName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                academicSessionsForAcademicSessionID = result.Value.AsODataEnumerable();
                academicSessionsForAcademicSessionIDCount = result.Count;

                if (!object.Equals(attendance.AcademicSessionID, null))
                {
                    var valueResult = await ConDataService.GetAcademicSessions(filter: $"AcademicSessionID eq {attendance.AcademicSessionID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        academicSessionsForAcademicSessionIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AcademicSession" });
            }
        }

        protected int schoolClassesForSchoolClassIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolClassesForSchoolClassIDValue;
        protected async Task schoolClassesForSchoolClassIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetSchoolClasses(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SchoolClassName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                schoolClassesForSchoolClassID = result.Value.AsODataEnumerable();
                schoolClassesForSchoolClassIDCount = result.Count;

                if (!object.Equals(attendance.SchoolClassID, null))
                {
                    var valueResult = await ConDataService.GetSchoolClasses(filter: $"SchoolClassID eq {attendance.SchoolClassID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        schoolClassesForSchoolClassIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load SchoolClass" });
            }
        }

        protected int studentsForStudentIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.Student studentsForStudentIDValue;
        protected async Task studentsForStudentIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetStudents(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AdmissionNumber, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                studentsForStudentID = result.Value.AsODataEnumerable();
                studentsForStudentIDCount = result.Count;

                if (!object.Equals(attendance.StudentID, null))
                {
                    var valueResult = await ConDataService.GetStudents(filter: $"StudentID eq {attendance.StudentID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        studentsForStudentIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Student" });
            }
        }

        protected int termsForTermIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.Term termsForTermIDValue;
        protected async Task termsForTermIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetTerms(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(TermName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                termsForTermID = result.Value.AsODataEnumerable();
                termsForTermIDCount = result.Count;

                if (!object.Equals(attendance.TermID, null))
                {
                    var valueResult = await ConDataService.GetTerms(filter: $"TermID eq {attendance.TermID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        termsForTermIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Term" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.CreateAttendance(attendance);
                DialogService.Close(attendance);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}