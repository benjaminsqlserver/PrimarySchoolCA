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
    [Route("odata/ConData/Terms")]
    public partial class TermsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public TermsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.Term> GetTerms()
        {
            var items = this.context.Terms.AsQueryable<PrimarySchoolCA.Server.Models.ConData.Term>();
            this.OnTermsRead(ref items);

            return items;
        }

        partial void OnTermsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Term> items);

        partial void OnTermGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.Term> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Terms(TermID={TermID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.Term> GetTerm(int key)
        {
            var items = this.context.Terms.Where(i => i.TermID == key);
            var result = SingleResult.Create(items);

            OnTermGet(ref result);

            return result;
        }
        partial void OnTermDeleted(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermDeleted(PrimarySchoolCA.Server.Models.ConData.Term item);

        [HttpDelete("/odata/ConData/Terms(TermID={TermID})")]
        public IActionResult DeleteTerm(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Terms
                    .Where(i => i.TermID == key)
                    .Include(i => i.AssessmentSetups)
                    .Include(i => i.Attendances)
                    .Include(i => i.ClassRegisters)
                    .Include(i => i.ContinuousAssessments)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Term>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTermDeleted(item);
                this.context.Terms.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTermDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTermUpdated(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermUpdated(PrimarySchoolCA.Server.Models.ConData.Term item);

        [HttpPut("/odata/ConData/Terms(TermID={TermID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTerm(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.Term item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Terms
                    .Where(i => i.TermID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Term>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTermUpdated(item);
                this.context.Terms.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Terms.Where(i => i.TermID == key);
                
                this.OnAfterTermUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Terms(TermID={TermID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTerm(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.Term> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Terms
                    .Where(i => i.TermID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Term>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTermUpdated(item);
                this.context.Terms.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Terms.Where(i => i.TermID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTermCreated(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermCreated(PrimarySchoolCA.Server.Models.ConData.Term item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.Term item)
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

                this.OnTermCreated(item);
                this.context.Terms.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Terms.Where(i => i.TermID == item.TermID);

                

                this.OnAfterTermCreated(item);

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
