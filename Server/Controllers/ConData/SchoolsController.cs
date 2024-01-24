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
    [Route("odata/ConData/Schools")]
    public partial class SchoolsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public SchoolsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.School> GetSchools()
        {
            var items = this.context.Schools.AsQueryable<PrimarySchoolCA.Server.Models.ConData.School>();
            this.OnSchoolsRead(ref items);

            return items;
        }

        partial void OnSchoolsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.School> items);

        partial void OnSchoolGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.School> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Schools(SchoolID={SchoolID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.School> GetSchool(long key)
        {
            var items = this.context.Schools.Where(i => i.SchoolID == key);
            var result = SingleResult.Create(items);

            OnSchoolGet(ref result);

            return result;
        }
        partial void OnSchoolDeleted(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolDeleted(PrimarySchoolCA.Server.Models.ConData.School item);

        [HttpDelete("/odata/ConData/Schools(SchoolID={SchoolID})")]
        public IActionResult DeleteSchool(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Schools
                    .Where(i => i.SchoolID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.School>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolDeleted(item);
                this.context.Schools.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSchoolDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolUpdated(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolUpdated(PrimarySchoolCA.Server.Models.ConData.School item);

        [HttpPut("/odata/ConData/Schools(SchoolID={SchoolID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSchool(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.School item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Schools
                    .Where(i => i.SchoolID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.School>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolUpdated(item);
                this.context.Schools.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schools.Where(i => i.SchoolID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LocalGovtArea,State");
                this.OnAfterSchoolUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Schools(SchoolID={SchoolID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSchool(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.School> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Schools
                    .Where(i => i.SchoolID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.School>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSchoolUpdated(item);
                this.context.Schools.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schools.Where(i => i.SchoolID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LocalGovtArea,State");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolCreated(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolCreated(PrimarySchoolCA.Server.Models.ConData.School item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.School item)
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

                this.OnSchoolCreated(item);
                this.context.Schools.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schools.Where(i => i.SchoolID == item.SchoolID);

                Request.QueryString = Request.QueryString.Add("$expand", "LocalGovtArea,State");

                this.OnAfterSchoolCreated(item);

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
