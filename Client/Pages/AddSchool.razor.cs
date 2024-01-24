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
    public partial class AddSchool
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
            school = new PrimarySchoolCA.Server.Models.ConData.School();
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.School school;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> localGovtAreasForLGAID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.State> statesForStateID;


        protected int localGovtAreasForLGAIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localGovtAreasForLGAIDValue;
        protected async Task localGovtAreasForLGAIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetLocalGovtAreas(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{args.Filter}", orderby: $"{args.OrderBy}");
                localGovtAreasForLGAID = result.Value.AsODataEnumerable();
                localGovtAreasForLGAIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load LocalGovtArea" });
            }
        }

        protected int statesForStateIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.State statesForStateIDValue;
        protected async Task statesForStateIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetStates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"{args.Filter}", orderby: $"{args.OrderBy}");
                statesForStateID = result.Value.AsODataEnumerable();
                statesForStateIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load State" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.CreateSchool(school);
                DialogService.Close(school);
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