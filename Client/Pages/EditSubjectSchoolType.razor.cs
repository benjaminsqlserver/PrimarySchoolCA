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
    public partial class EditSubjectSchoolType
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
        public long ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            subjectSchoolType = await ConDataService.GetSubjectSchoolTypeById(id:ID);
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectSchoolType;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolType> schoolTypesForSchoolTypeID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Subject> subjectsForSubjectID;


        protected int schoolTypesForSchoolTypeIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.SchoolType schoolTypesForSchoolTypeIDValue;
        protected async Task schoolTypesForSchoolTypeIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetSchoolTypes(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SchoolTypeName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                schoolTypesForSchoolTypeID = result.Value.AsODataEnumerable();
                schoolTypesForSchoolTypeIDCount = result.Count;

                if (!object.Equals(subjectSchoolType.SchoolTypeID, null))
                {
                    var valueResult = await ConDataService.GetSchoolTypes(filter: $"SchoolTypeID eq {subjectSchoolType.SchoolTypeID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        schoolTypesForSchoolTypeIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load SchoolType" });
            }
        }

        protected int subjectsForSubjectIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.Subject subjectsForSubjectIDValue;
        protected async Task subjectsForSubjectIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetSubjects(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SubjectName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                subjectsForSubjectID = result.Value.AsODataEnumerable();
                subjectsForSubjectIDCount = result.Count;

                if (!object.Equals(subjectSchoolType.SubjectID, null))
                {
                    var valueResult = await ConDataService.GetSubjects(filter: $"SubjectID eq {subjectSchoolType.SubjectID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        subjectsForSubjectIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Subject" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateSubjectSchoolType(id:ID, subjectSchoolType);
                DialogService.Close(subjectSchoolType);
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





        bool hasSchoolTypeIDValue;

        [Parameter]
        public int? SchoolTypeID { get; set; }

        bool hasSubjectIDValue;

        [Parameter]
        public long? SubjectID { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            subjectSchoolType = new PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType();

            hasSchoolTypeIDValue = parameters.TryGetValue<int?>("SchoolTypeID", out var hasSchoolTypeIDResult);

            if (hasSchoolTypeIDValue)
            {
                subjectSchoolType.SchoolTypeID = hasSchoolTypeIDResult;
            }

            hasSubjectIDValue = parameters.TryGetValue<long?>("SubjectID", out var hasSubjectIDResult);

            if (hasSubjectIDValue)
            {
                subjectSchoolType.SubjectID = hasSubjectIDResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}