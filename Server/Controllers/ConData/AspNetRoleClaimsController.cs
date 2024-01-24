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
    [Route("odata/ConData/AspNetRoleClaims")]
    public partial class AspNetRoleClaimsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AspNetRoleClaimsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> GetAspNetRoleClaims()
        {
            var items = this.context.AspNetRoleClaims.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>();
            this.OnAspNetRoleClaimsRead(ref items);

            return items;
        }

        partial void OnAspNetRoleClaimsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> items);

        partial void OnAspNetRoleClaimGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AspNetRoleClaims(Id={Id})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> GetAspNetRoleClaim(int key)
        {
            var items = this.context.AspNetRoleClaims.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnAspNetRoleClaimGet(ref result);

            return result;
        }
        partial void OnAspNetRoleClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        [HttpDelete("/odata/ConData/AspNetRoleClaims(Id={Id})")]
        public IActionResult DeleteAspNetRoleClaim(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AspNetRoleClaims
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetRoleClaimDeleted(item);
                this.context.AspNetRoleClaims.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAspNetRoleClaimDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetRoleClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        [HttpPut("/odata/ConData/AspNetRoleClaims(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAspNetRoleClaim(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetRoleClaims
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetRoleClaimUpdated(item);
                this.context.AspNetRoleClaims.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoleClaims.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole");
                this.OnAfterAspNetRoleClaimUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AspNetRoleClaims(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAspNetRoleClaim(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetRoleClaims
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAspNetRoleClaimUpdated(item);
                this.context.AspNetRoleClaims.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoleClaims.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetRoleClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item)
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

                this.OnAspNetRoleClaimCreated(item);
                this.context.AspNetRoleClaims.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoleClaims.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole");

                this.OnAfterAspNetRoleClaimCreated(item);

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
