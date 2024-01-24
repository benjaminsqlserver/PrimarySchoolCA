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
    public partial class SchoolTypes
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

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolType> schoolTypes;

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.SchoolType> grid0;
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
                var result = await ConDataService.GetSchoolTypes(filter: $@"(contains(SchoolTypeName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                schoolTypes = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load SchoolTypes" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSchoolType>("Add SchoolType", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.SchoolType> args)
        {
            await DialogService.OpenAsync<EditSchoolType>("Edit SchoolType", new Dictionary<string, object> { {"SchoolTypeID", args.Data.SchoolTypeID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.SchoolType schoolType)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteSchoolType(schoolTypeId:schoolType.SchoolTypeID);

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
                    Detail = $"Unable to delete SchoolType" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportSchoolTypesToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "SchoolTypes");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportSchoolTypesToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "SchoolTypes");
            }
        }

        protected PrimarySchoolCA.Server.Models.ConData.SchoolType schoolType;
        protected async Task GetChildData(PrimarySchoolCA.Server.Models.ConData.SchoolType args)
        {
            schoolType = args;
            var SubjectSchoolTypesResult = await ConDataService.GetSubjectSchoolTypes(filter:$"SchoolTypeID eq {args.SchoolTypeID}", expand: "SchoolType,Subject");
            if (SubjectSchoolTypesResult != null)
            {
                args.SubjectSchoolTypes = SubjectSchoolTypesResult.Value.ToList();
            }
        }

        protected RadzenDataGrid<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> SubjectSchoolTypesDataGrid;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task SubjectSchoolTypesAddButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.SchoolType data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSubjectSchoolType>("Add SubjectSchoolTypes", new Dictionary<string, object> { {"SchoolTypeID" , data.SchoolTypeID} });
            await GetChildData(data);
            await SubjectSchoolTypesDataGrid.Reload();
        }

        protected async Task SubjectSchoolTypesRowSelect(DataGridRowMouseEventArgs<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> args, PrimarySchoolCA.Server.Models.ConData.SchoolType data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSubjectSchoolType>("Edit SubjectSchoolTypes", new Dictionary<string, object> { {"ID", args.Data.ID} });
            await GetChildData(data);
            await SubjectSchoolTypesDataGrid.Reload();
        }

        protected async Task SubjectSchoolTypesDeleteButtonClick(MouseEventArgs args, PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectSchoolType)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteSubjectSchoolType(id:subjectSchoolType.ID);

                    await GetChildData(schoolType);

                    if (deleteResult != null)
                    {
                        await SubjectSchoolTypesDataGrid.Reload();
                    }
                }
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete SubjectSchoolType" 
                });
            }
        }
    }
}