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
    [Route("odata/ConData/ParentsOrGuardians")]
    public partial class ParentsOrGuardiansController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public ParentsOrGuardiansController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> GetParentsOrGuardians()
        {
            var items = this.context.ParentsOrGuardians.AsQueryable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>();
            this.OnParentsOrGuardiansRead(ref items);

            return items;
        }

        partial void OnParentsOrGuardiansRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> items);

        partial void OnParentsOrGuardianGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/ParentsOrGuardians(ParentOrGuardianID={ParentOrGuardianID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> GetParentsOrGuardian(long key)
        {
            var items = this.context.ParentsOrGuardians.Where(i => i.ParentOrGuardianID == key);
            var result = SingleResult.Create(items);

            OnParentsOrGuardianGet(ref result);

            return result;
        }
        partial void OnParentsOrGuardianDeleted(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianDeleted(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        [HttpDelete("/odata/ConData/ParentsOrGuardians(ParentOrGuardianID={ParentOrGuardianID})")]
        public IActionResult DeleteParentsOrGuardian(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ParentsOrGuardians
                    .Where(i => i.ParentOrGuardianID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnParentsOrGuardianDeleted(item);
                this.context.ParentsOrGuardians.Remove(item);
                this.context.SaveChanges();
                this.OnAfterParentsOrGuardianDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnParentsOrGuardianUpdated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianUpdated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        [HttpPut("/odata/ConData/ParentsOrGuardians(ParentOrGuardianID={ParentOrGuardianID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutParentsOrGuardian(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ParentsOrGuardians
                    .Where(i => i.ParentOrGuardianID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnParentsOrGuardianUpdated(item);
                this.context.ParentsOrGuardians.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ParentsOrGuardians.Where(i => i.ParentOrGuardianID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Gender,LocalGovtArea,State,Student");
                this.OnAfterParentsOrGuardianUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/ParentsOrGuardians(ParentOrGuardianID={ParentOrGuardianID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchParentsOrGuardian(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ParentsOrGuardians
                    .Where(i => i.ParentOrGuardianID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnParentsOrGuardianUpdated(item);
                this.context.ParentsOrGuardians.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ParentsOrGuardians.Where(i => i.ParentOrGuardianID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Gender,LocalGovtArea,State,Student");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnParentsOrGuardianCreated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianCreated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item)
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

                this.OnParentsOrGuardianCreated(item);
                this.context.ParentsOrGuardians.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ParentsOrGuardians.Where(i => i.ParentOrGuardianID == item.ParentOrGuardianID);

                Request.QueryString = Request.QueryString.Add("$expand", "Gender,LocalGovtArea,State,Student");

                this.OnAfterParentsOrGuardianCreated(item);

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
