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
    public partial class AddParentsOrGuardian
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
            await base.OnInitializedAsync();
            parentsOrGuardian.StudentID = Convert.ToInt64(StudentID);
            //initialize local government items as empty list
            localGovtAreasForLgaID = new List<Server.Models.ConData.LocalGovtArea>();
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsOrGuardian;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Gender> gendersForGenderID;
        // local government items
        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> localGovtAreasForLgaID;
        //states items
        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.State> statesForStateID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Student> studentsForStudentID;


        protected int gendersForGenderIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.Gender gendersForGenderIDValue;
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

        protected int localGovtAreasForLgaIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localGovtAreasForLgaIDValue;
        //method to load local government areas
        protected async Task localGovtAreasForLgaIDLoadData(LoadDataArgs args)
        {
            try
            {
                //var result = await ConDataService.GetLocalGovtAreas(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(LgaName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                //localGovtAreasForLgaID = result.Value.AsODataEnumerable();
                //localGovtAreasForLgaIDCount = result.Count;
                localGovtAreasForLgaID = new List<Server.Models.ConData.LocalGovtArea>();
                localGovtAreasForLgaIDCount = localGovtAreasForLgaID.Count();

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
                var result = await ConDataService.GetStates(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(StateName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statesForStateID = result.Value.AsODataEnumerable();
                statesForStateIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load State" });
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

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Student" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.CreateParentsOrGuardian(parentsOrGuardian);
                DialogService.Close(parentsOrGuardian);
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





        bool hasGenderIDValue;

        [Parameter]
        public int GenderID { get; set; }

        bool hasLgaIDValue;

        [Parameter]
        public int LgaID { get; set; }

        bool hasStateIDValue;

        [Parameter]
        public int StateID { get; set; }

        bool hasStudentIDValue;

        [Parameter]
        public long StudentID { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        //SetParametersAsync Is called before oninitializedasync
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            parentsOrGuardian = new PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian();

            hasGenderIDValue = parameters.TryGetValue<int>("GenderID", out var hasGenderIDResult);

            if (hasGenderIDValue)
            {
                parentsOrGuardian.GenderID = hasGenderIDResult;
            }

            hasLgaIDValue = parameters.TryGetValue<int>("LgaID", out var hasLgaIDResult);

            if (hasLgaIDValue)
            {
                parentsOrGuardian.LgaID = hasLgaIDResult;
            }

            hasStateIDValue = parameters.TryGetValue<int>("StateID", out var hasStateIDResult);

            if (hasStateIDValue)
            {
                parentsOrGuardian.StateID = hasStateIDResult;
            }

            hasStudentIDValue = parameters.TryGetValue<long>("StudentID", out var hasStudentIDResult);

            if (hasStudentIDValue)
            {
                parentsOrGuardian.StudentID = hasStudentIDResult;
            }
            await base.SetParametersAsync(parameters);
        }

       

        protected async System.Threading.Tasks.Task StateIDChange(System.Object args)
        {
            try
            {
                int selectedStateID = parentsOrGuardian.StateID;
                IEnumerable<Server.Models.ConData.LocalGovtArea> lgas = await ConDataService.GetLGAsByStateID(selectedStateID);
                localGovtAreasForLgaID = lgas;
            }
            catch(Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error!", "An Error Occurred While Changing State Value!", 7000);
            }
            
        }
    }
}