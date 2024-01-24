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
    [Route("odata/ConData/ContinuousAssessments")]
    public partial class ContinuousAssessmentsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public ContinuousAssessmentsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> GetContinuousAssessments()
        {
            var items = this.context.ContinuousAssessments.AsQueryable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>();
            this.OnContinuousAssessmentsRead(ref items);

            return items;
        }

        partial void OnContinuousAssessmentsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> items);

        partial void OnContinuousAssessmentGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/ContinuousAssessments(RecordID={RecordID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> GetContinuousAssessment(long key)
        {
            var items = this.context.ContinuousAssessments.Where(i => i.RecordID == key);
            var result = SingleResult.Create(items);

            OnContinuousAssessmentGet(ref result);

            return result;
        }
        partial void OnContinuousAssessmentDeleted(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentDeleted(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        [HttpDelete("/odata/ConData/ContinuousAssessments(RecordID={RecordID})")]
        public IActionResult DeleteContinuousAssessment(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ContinuousAssessments
                    .Where(i => i.RecordID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnContinuousAssessmentDeleted(item);
                this.context.ContinuousAssessments.Remove(item);
                this.context.SaveChanges();
                this.OnAfterContinuousAssessmentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnContinuousAssessmentUpdated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentUpdated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        [HttpPut("/odata/ConData/ContinuousAssessments(RecordID={RecordID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutContinuousAssessment(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ContinuousAssessments
                    .Where(i => i.RecordID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnContinuousAssessmentUpdated(item);
                this.context.ContinuousAssessments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContinuousAssessments.Where(i => i.RecordID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Subject,Term");
                this.OnAfterContinuousAssessmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/ContinuousAssessments(RecordID={RecordID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchContinuousAssessment(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ContinuousAssessments
                    .Where(i => i.RecordID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnContinuousAssessmentUpdated(item);
                this.context.ContinuousAssessments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContinuousAssessments.Where(i => i.RecordID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Subject,Term");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnContinuousAssessmentCreated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentCreated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item)
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

                this.OnContinuousAssessmentCreated(item);
                this.context.ContinuousAssessments.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContinuousAssessments.Where(i => i.RecordID == item.RecordID);

                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Subject,Term");

                this.OnAfterContinuousAssessmentCreated(item);

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
