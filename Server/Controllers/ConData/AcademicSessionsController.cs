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
    [Route("odata/ConData/AcademicSessions")]
    public partial class AcademicSessionsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AcademicSessionsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> GetAcademicSessions()
        {
            var items = this.context.AcademicSessions.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AcademicSession>();
            this.OnAcademicSessionsRead(ref items);

            return items;
        }

        partial void OnAcademicSessionsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> items);

        partial void OnAcademicSessionGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AcademicSession> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AcademicSessions(AcademicSessionID={AcademicSessionID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AcademicSession> GetAcademicSession(int key)
        {
            var items = this.context.AcademicSessions.Where(i => i.AcademicSessionID == key);
            var result = SingleResult.Create(items);

            OnAcademicSessionGet(ref result);

            return result;
        }
        partial void OnAcademicSessionDeleted(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionDeleted(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        [HttpDelete("/odata/ConData/AcademicSessions(AcademicSessionID={AcademicSessionID})")]
        public IActionResult DeleteAcademicSession(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AcademicSessions
                    .Where(i => i.AcademicSessionID == key)
                    .Include(i => i.AssessmentSetups)
                    .Include(i => i.Attendances)
                    .Include(i => i.ClassRegisters)
                    .Include(i => i.ContinuousAssessments)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AcademicSession>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAcademicSessionDeleted(item);
                this.context.AcademicSessions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAcademicSessionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAcademicSessionUpdated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionUpdated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        [HttpPut("/odata/ConData/AcademicSessions(AcademicSessionID={AcademicSessionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAcademicSession(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.AcademicSession item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AcademicSessions
                    .Where(i => i.AcademicSessionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AcademicSession>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAcademicSessionUpdated(item);
                this.context.AcademicSessions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AcademicSessions.Where(i => i.AcademicSessionID == key);
                
                this.OnAfterAcademicSessionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AcademicSessions(AcademicSessionID={AcademicSessionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAcademicSession(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AcademicSession> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AcademicSessions
                    .Where(i => i.AcademicSessionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AcademicSession>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAcademicSessionUpdated(item);
                this.context.AcademicSessions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AcademicSessions.Where(i => i.AcademicSessionID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAcademicSessionCreated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionCreated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AcademicSession item)
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

                this.OnAcademicSessionCreated(item);
                this.context.AcademicSessions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AcademicSessions.Where(i => i.AcademicSessionID == item.AcademicSessionID);

                

                this.OnAfterAcademicSessionCreated(item);

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
