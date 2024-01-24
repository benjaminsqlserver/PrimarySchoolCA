using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Authorization;

namespace PrimarySchoolCA.Client.Pages
{
    public partial class Students
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

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Student> students;

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.Student> grid0;
        protected int count;

        protected string search = "";

       
        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetStudents(filter: $@"(contains(AdmissionNumber,""{search}"") or contains(FirstName,""{search}"") or contains(MiddleName,""{search}"") or contains(LastName,""{search}"") or contains(PassportPhoto,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Gender", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                students = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Students" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddStudent>("Add Student", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.Student> args)
        {
            await DialogService.OpenAsync<EditStudent>("Edit Student", new Dictionary<string, object> { {"StudentID", args.Data.StudentID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.Student student)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteStudent(studentId:student.StudentID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Student" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportStudentsToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Gender", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Students");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportStudentsToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "Gender", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Students");
            }
        }

        protected PrimarySchoolCA.Server.Models.ConData.Student student;
        protected async Task GetChildData(PrimarySchoolCA.Server.Models.ConData.Student args)
        {
            student = args;
            var ParentsOrGuardiansResult = await ConDataService.GetParentsOrGuardians(filter:$"StudentID eq {args.StudentID}", expand: "Gender,LocalGovtArea,State,Student");
            if (ParentsOrGuardiansResult != null)
            {
                args.ParentsOrGuardians = ParentsOrGuardiansResult.Value.ToList();
            }
        }

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> ParentsOrGuardiansDataGrid;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task ParentsOrGuardiansAddButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.Student data)
        {
            var dialogResult = await DialogService.OpenAsync<AddParentsOrGuardian>("Add ParentsOrGuardians", new Dictionary<string, object> { {"StudentID" , data.StudentID} });
            await GetChildData(data);
            await ParentsOrGuardiansDataGrid.Reload();
        }

        protected async Task ParentsOrGuardiansRowSelect(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> args, PrimarySchoolCA.Server.Models.ConData.Student data)
        {
            var dialogResult = await DialogService.OpenAsync<EditParentsOrGuardian>("Edit ParentsOrGuardians", new Dictionary<string, object> { {"ParentOrGuardianID", args.Data.ParentOrGuardianID} });
            await GetChildData(data);
            await ParentsOrGuardiansDataGrid.Reload();
        }

        protected async Task ParentsOrGuardiansDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsOrGuardian)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteParentsOrGuardian(parentOrGuardianId:parentsOrGuardian.ParentOrGuardianID);

                    await GetChildData(student);

                    if (deleteResult != null)
                    {
                        await ParentsOrGuardiansDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete ParentsOrGuardian" 
                });
            }
        }
    }
}