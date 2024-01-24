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
    public partial class EditLocalGovtArea
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

        [Parameter]
        public int LgaID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            localGovtArea = await ConDataService.GetLocalGovtAreaByLgaId(lgaId:LgaID);
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localGovtArea;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.State> statesForStateID;


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
                var result = await ConDataService.UpdateLocalGovtArea(lgaId:LgaID, localGovtArea);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(localGovtArea);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            localGovtArea = await ConDataService.GetLocalGovtAreaByLgaId(lgaId:LgaID);
        }
    }
}