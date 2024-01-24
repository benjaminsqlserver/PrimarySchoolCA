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
    [Route("odata/ConData/SchoolClasses")]
    public partial class SchoolClassesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public SchoolClassesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> GetSchoolClasses()
        {
            var items = this.context.SchoolClasses.AsQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolClass>();
            this.OnSchoolClassesRead(ref items);

            return items;
        }

        partial void OnSchoolClassesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> items);

        partial void OnSchoolClassGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.SchoolClass> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/SchoolClasses(SchoolClassID={SchoolClassID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.SchoolClass> GetSchoolClass(int key)
        {
            var items = this.context.SchoolClasses.Where(i => i.SchoolClassID == key);
            var result = SingleResult.Create(items);

            OnSchoolClassGet(ref result);

            return result;
        }
        partial void OnSchoolClassDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        [HttpDelete("/odata/ConData/SchoolClasses(SchoolClassID={SchoolClassID})")]
        public IActionResult DeleteSchoolClass(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SchoolClasses
                    .Where(i => i.SchoolClassID == key)
                    .Include(i => i.AssessmentSetups)
                    .Include(i => i.Attendances)
                    .Include(i => i.ClassRegisters)
                    .Include(i => i.ContinuousAssessments)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolClass>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolClassDeleted(item);
                this.context.SchoolClasses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSchoolClassDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolClassUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        [HttpPut("/odata/ConData/SchoolClasses(SchoolClassID={SchoolClassID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSchoolClass(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.SchoolClass item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SchoolClasses
                    .Where(i => i.SchoolClassID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolClass>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolClassUpdated(item);
                this.context.SchoolClasses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolClasses.Where(i => i.SchoolClassID == key);
                
                this.OnAfterSchoolClassUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/SchoolClasses(SchoolClassID={SchoolClassID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSchoolClass(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.SchoolClass> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SchoolClasses
                    .Where(i => i.SchoolClassID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolClass>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSchoolClassUpdated(item);
                this.context.SchoolClasses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolClasses.Where(i => i.SchoolClassID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolClassCreated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassCreated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.SchoolClass item)
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

                this.OnSchoolClassCreated(item);
                this.context.SchoolClasses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolClasses.Where(i => i.SchoolClassID == item.SchoolClassID);

                

                this.OnAfterSchoolClassCreated(item);

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
