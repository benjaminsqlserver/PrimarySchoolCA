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
    public partial class EditClassRegisterStudent
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
            classRegisterStudent = await ConDataService.GetClassRegisterStudentById(id:ID);
        }
        protected bool errorVisible;
        protected PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classRegisterStudent;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> classRegistersForClassRegisterID;

        protected IEnumerable<PrimarySchoolCA.Server.Models.ConData.Student> studentsForStudentID;


        protected int classRegistersForClassRegisterIDCount;
        protected PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegistersForClassRegisterIDValue;
        protected async Task classRegistersForClassRegisterIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetClassRegisters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                classRegistersForClassRegisterID = result.Value.AsODataEnumerable();
                classRegistersForClassRegisterIDCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ClassRegister" });
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
                await ConDataService.UpdateClassRegisterStudent(id:ID, classRegisterStudent);
                DialogService.Close(classRegisterStudent);
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





        bool hasClassRegisterIDValue;

        [Parameter]
        public long ClassRegisterID { get; set; }

        bool hasStudentIDValue;

        [Parameter]
        public long StudentID { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            classRegisterStudent = new PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent();

            hasClassRegisterIDValue = parameters.TryGetValue<long>("ClassRegisterID", out var hasClassRegisterIDResult);

            if (hasClassRegisterIDValue)
            {
                classRegisterStudent.ClassRegisterID = hasClassRegisterIDResult;
            }

            hasStudentIDValue = parameters.TryGetValue<long>("StudentID", out var hasStudentIDResult);

            if (hasStudentIDValue)
            {
                classRegisterStudent.StudentID = hasStudentIDResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}