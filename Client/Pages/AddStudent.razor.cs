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
    public partial class AddStudent
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
            student = new PrimarySchoolCA.Server.Models.ConData.Student();
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.Student student;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Gender> gendersForGenderID;


        protected int gendersForGenderIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.Gender gendersForGenderIDValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task gendersForGenderIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetGenders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GenderName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                gendersForGenderID = result.Value.AsODataEnumerable();
                gendersForGenderIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Gender" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.CreateStudent(student);
                DialogService.Close(student);
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
    }
}