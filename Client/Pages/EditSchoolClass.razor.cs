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
    public partial class EditSchoolClass
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
        public int SchoolClassID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            schoolClass = await ConDataService.GetSchoolClassBySchoolClassId(schoolClassId:SchoolClassID);
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolClass;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.UpdateSchoolClass(schoolClassId:SchoolClassID, schoolClass);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(schoolClass);
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

            schoolClass = await ConDataService.GetSchoolClassBySchoolClassId(schoolClassId:SchoolClassID);
        }
    }
}