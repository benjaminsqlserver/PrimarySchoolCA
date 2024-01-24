using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PrimarySchoolCA.Server.Controllers.ConData
{
    [Route("odata/ConData/AssessmentSetups")]
    public partial class AssessmentSetupsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AssessmentSetupsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> GetAssessmentSetups()
        {
            var items = this.context.AssessmentSetups.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>();
            this.OnAssessmentSetupsRead(ref items);

            return items;
        }

        partial void OnAssessmentSetupsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> items);

        partial void OnAssessmentSetupGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AssessmentSetups(AssessmentSetupID={AssessmentSetupID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> GetAssessmentSetup(long key)
        {
            var items = this.context.AssessmentSetups.Where(i => i.AssessmentSetupID == key);
            var result = SingleResult.Create(items);

            OnAssessmentSetupGet(ref result);

            return result;
        }
        partial void OnAssessmentSetupDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        [HttpDelete("/odata/ConData/AssessmentSetups(AssessmentSetupID={AssessmentSetupID})")]
        public IActionResult DeleteAssessmentSetup(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AssessmentSetups
                    .Where(i => i.AssessmentSetupID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAssessmentSetupDeleted(item);
                this.context.AssessmentSetups.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAssessmentSetupDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAssessmentSetupUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        [HttpPut("/odata/ConData/AssessmentSetups(AssessmentSetupID={AssessmentSetupID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAssessmentSetup(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AssessmentSetups
                    .Where(i => i.AssessmentSetupID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAssessmentSetupUpdated(item);
                this.context.AssessmentSetups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentSetups.Where(i => i.AssessmentSetupID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,AssessmentType,SchoolClass,Subject,Term");
                this.OnAfterAssessmentSetupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AssessmentSetups(AssessmentSetupID={AssessmentSetupID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAssessmentSetup(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AssessmentSetups
                    .Where(i => i.AssessmentSetupID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAssessmentSetupUpdated(item);
                this.context.AssessmentSetups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentSetups.Where(i => i.AssessmentSetupID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,AssessmentType,SchoolClass,Subject,Term");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAssessmentSetupCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnAssessmentSetupCreated(item);
                this.context.AssessmentSetups.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentSetups.Where(i => i.AssessmentSetupID == item.AssessmentSetupID);

                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,AssessmentType,SchoolClass,Subject,Term");

                this.OnAfterAssessmentSetupCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
