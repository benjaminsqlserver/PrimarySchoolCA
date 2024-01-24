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
    public partial class ClassRegisters
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

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> classRegisters;

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.ClassRegister> grid0;
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
                var result = await ConDataService.GetClassRegisters(filter: $"{args.Filter}", expand: "AcademicSession,SchoolClass,Term", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                classRegisters = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ClassRegisters" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddClassRegister>("Add ClassRegister", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.ClassRegister> args)
        {
            await DialogService.OpenAsync<EditClassRegister>("Edit ClassRegister", new Dictionary<string, object> { {"ClassRegisterID", args.Data.ClassRegisterID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegister)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteClassRegister(classRegisterId:classRegister.ClassRegisterID);

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
                    Detail = $"Unable to delete ClassRegister" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportClassRegistersToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "AcademicSession,SchoolClass,Term", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "ClassRegisters");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportClassRegistersToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "AcademicSession,SchoolClass,Term", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "ClassRegisters");
            }
        }

        protected PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegister;
        protected async Task GetChildData(PrimarySchoolCA.Server.Models.ConData.ClassRegister args)
        {
            classRegister = args;
            var ClassRegisterStudentsResult = await ConDataService.GetClassRegisterStudents(filter:$"ClassRegisterID eq {args.ClassRegisterID}", expand: "ClassRegister,Student");
            if (ClassRegisterStudentsResult != null)
            {
                args.ClassRegisterStudents = ClassRegisterStudentsResult.Value.ToList();
            }
        }

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> ClassRegisterStudentsDataGrid;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task ClassRegisterStudentsAddButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.ClassRegister data)
        {
            var dialogResult = await DialogService.OpenAsync<AddClassRegisterStudent>("Add ClassRegisterStudents", new Dictionary<string, object> { {"ClassRegisterID" , data.ClassRegisterID} });
            await GetChildData(data);
            await ClassRegisterStudentsDataGrid.Reload();
        }

        protected async Task ClassRegisterStudentsRowSelect(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> args, PrimarySchoolCA.Server.Models.ConData.ClassRegister data)
        {
            var dialogResult = await DialogService.OpenAsync<EditClassRegisterStudent>("Edit ClassRegisterStudents", new Dictionary<string, object> { {"ID", args.Data.ID} });
            await GetChildData(data);
            await ClassRegisterStudentsDataGrid.Reload();
        }

        protected async Task ClassRegisterStudentsDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classRegisterStudent)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteClassRegisterStudent(id:classRegisterStudent.ID);

                    await GetChildData(classRegister);

                    if (deleteResult != null)
                    {
                        await ClassRegisterStudentsDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete ClassRegisterStudent" 
                });
            }
        }
    }
}