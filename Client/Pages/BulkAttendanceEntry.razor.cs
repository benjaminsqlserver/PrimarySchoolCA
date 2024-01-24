using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using PrimarySchoolCA.Server.Models.ConData;

namespace PrimarySchoolCA.Client.Pages
{
    public partial class BulkAttendanceEntry
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
        protected SecurityService Security { get; set; }

        protected int academicSessionID;

        protected int termID;

        protected int schoolClassID;

        [Inject]
        protected PrimarySchoolCA.Client.ConDataService ConDataService { get; set; }

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> academicSessions;

        protected int academicSessionsCount;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Term> terms;

        protected int termsCount;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> schoolClasses;

        protected int schoolClassesCount;

       protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.AttendanceViewModel> students;

       protected PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegister;


       protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
           academicSessionID = 0;

            termID = 0;

            schoolClassID = 0;

            classRegister=new PrimarySchoolCA.Server.Models.ConData.ClassRegister(){};

            students = new List<PrimarySchoolCA.Server.Models.ConData.AttendanceViewModel>(){};
        }


        protected async Task academicSessionsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetAcademicSessions(new Query { Top = args.Top, Skip = args.Skip, Filter = args.Filter, OrderBy = args.OrderBy });

                academicSessions = result.Value.AsODataEnumerable();
                academicSessionsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }


        protected async Task termsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetTerms(new Query { Top = args.Top, Skip = args.Skip, Filter = args.Filter, OrderBy = args.OrderBy });

                terms = result.Value.AsODataEnumerable();
                termsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }


        protected async Task schoolClassesLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetSchoolClasses(new Query { Top = args.Top, Skip = args.Skip, Filter = args.Filter, OrderBy = args.OrderBy });

                schoolClasses = result.Value.AsODataEnumerable();
                schoolClassesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        

    

        protected async System.Threading.Tasks.Task FetchRegisterClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await FetchClassRegister();
        }

        private async Task FetchClassRegister()
        {
            try
            {
                //get class register from database
                classRegister = await ConDataService.FetchClassRegister(academicSessionID, termID, schoolClassID);
                if (classRegister.ClassRegisterID > 0)//if class register is successfully fetched from database
                {
                    //get the list of students contained in class register
                    List<ClassRegisterStudent> classRegisterStudents = await ConDataService.FetchClassRegisterStudents(classRegister.ClassRegisterID);

                    if (classRegisterStudents.Any())//if at least one student is found in class register
                    {
                        var studentList = new List<AttendanceViewModel>();//create a new list based on AttendanceViewModel
                        foreach (var item in classRegisterStudents) 
                        {
                            //get student full details from Student table using the studentID(Primary key)
                            Student studentRecord = await ConDataService.GetStudentByStudentId(null, item.StudentID);
                            if (studentRecord != null)
                            {
                                //add each student found in class register to view model list
                                studentList.Add(new AttendanceViewModel { AcademicSessionID = academicSessionID, TermID = termID, SchoolClassID = schoolClassID, StudentID = studentRecord.StudentID, AdmissionNumber = studentRecord.AdmissionNumber, FirstName = studentRecord.FirstName, LastName = studentRecord.LastName, Present = false, AttendanceDate = DateTime.Today });
                            }
                        }

                        students = studentList;
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Register Fetch Error!", "An Error Occurred While Fetching Class Register", 5000);
            }
        }

        protected async System.Threading.Tasks.Task CancelFetchRegisterClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await ResetFormAsync();
        }

        private async Task ResetFormAsync()
        {
            ResetForm();
        }

        private void ResetForm()
        {
            try
            {
                academicSessionID = 0;
                termID = 0;
                schoolClassID = 0;
                students = new List<AttendanceViewModel>();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error!", "An Error Occurred!", 5000);
            }
        }
        protected async System.Threading.Tasks.Task TakeAttendanceClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await SaveAttendanceAsync();
        }
        private async Task SaveAttendanceAsync()
        {
            try
            {

                if (!students.Any())//if zero number of student is displayed on datagrid
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Register Fetch Error!", "Class Register Is Either Empty Empty Or You May Not Have Fetched It.", 5000);
                    return;
                }
                else//if at least one student is displayed on datagrid
                {
                    foreach (AttendanceViewModel student in students)
                    {
                        //save attendance record into database
                        await ConDataService.SaveAttendance(student);
                        
                    }

                    ResetForm();

                    NotificationService.Notify(NotificationSeverity.Success, "Class Attendance Marking Success!", "You Have Successfully Concluded Registering The Class Attendance", 5000);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Attendance Save Error!", "An Error Occurred While Saving Attendance", 5000);
            }
        }
    }
}