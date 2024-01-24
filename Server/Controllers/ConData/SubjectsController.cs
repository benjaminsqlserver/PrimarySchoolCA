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
    [Route("odata/ConData/Subjects")]
    public partial class SubjectsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public SubjectsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.Subject> GetSubjects()
        {
            var items = this.context.Subjects.AsQueryable<PrimarySchoolCA.Server.Models.ConData.Subject>();
            this.OnSubjectsRead(ref items);

            return items;
        }

        partial void OnSubjectsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Subject> items);

        partial void OnSubjectGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.Subject> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Subjects(SubjectID={SubjectID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.Subject> GetSubject(long key)
        {
            var items = this.context.Subjects.Where(i => i.SubjectID == key);
            var result = SingleResult.Create(items);

            OnSubjectGet(ref result);

            return result;
        }
        partial void OnSubjectDeleted(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectDeleted(PrimarySchoolCA.Server.Models.ConData.Subject item);

        [HttpDelete("/odata/ConData/Subjects(SubjectID={SubjectID})")]
        public IActionResult DeleteSubject(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .Include(i => i.AssessmentSetups)
                    .Include(i => i.ContinuousAssessments)
                    .Include(i => i.SubjectSchoolTypes)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Subject>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectDeleted(item);
                this.context.Subjects.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSubjectDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectUpdated(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectUpdated(PrimarySchoolCA.Server.Models.ConData.Subject item);

        [HttpPut("/odata/ConData/Subjects(SubjectID={SubjectID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSubject(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.Subject item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Subject>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectUpdated(item);
                this.context.Subjects.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == key);
                
                this.OnAfterSubjectUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Subjects(SubjectID={SubjectID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSubject(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.Subject> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Subject>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSubjectUpdated(item);
                this.context.Subjects.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectCreated(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectCreated(PrimarySchoolCA.Server.Models.ConData.Subject item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.Subject item)
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

                this.OnSubjectCreated(item);
                this.context.Subjects.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == item.SubjectID);

                

                this.OnAfterSubjectCreated(item);

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
