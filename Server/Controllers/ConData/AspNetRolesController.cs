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
    [Route("odata/ConData/AspNetRoles")]
    public partial class AspNetRolesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AspNetRolesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AspNetRole> GetAspNetRoles()
        {
            var items = this.context.AspNetRoles.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRole>();
            this.OnAspNetRolesRead(ref items);

            return items;
        }

        partial void OnAspNetRolesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRole> items);

        partial void OnAspNetRoleGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetRole> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AspNetRoles(Id={Id})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetRole> GetAspNetRole(string key)
        {
            var items = this.context.AspNetRoles.Where(i => i.Id == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnAspNetRoleGet(ref result);

            return result;
        }
        partial void OnAspNetRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        [HttpDelete("/odata/ConData/AspNetRoles(Id={Id})")]
        public IActionResult DeleteAspNetRole(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AspNetRoles
                    .Where(i => i.Id == Uri.UnescapeDataString(key))
                    .Include(i => i.AspNetRoleClaims)
                    .Include(i => i.AspNetUserRoles)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRole>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetRoleDeleted(item);
                this.context.AspNetRoles.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAspNetRoleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        [HttpPut("/odata/ConData/AspNetRoles(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAspNetRole(string key, [FromBody]PrimarySchoolCA.Server.Models.ConData.AspNetRole item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetRoles
                    .Where(i => i.Id == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRole>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetRoleUpdated(item);
                this.context.AspNetRoles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoles.Where(i => i.Id == Uri.UnescapeDataString(key));
                
                this.OnAfterAspNetRoleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AspNetRoles(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAspNetRole(string key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AspNetRole> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetRoles
                    .Where(i => i.Id == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetRole>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAspNetRoleUpdated(item);
                this.context.AspNetRoles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoles.Where(i => i.Id == Uri.UnescapeDataString(key));
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AspNetRole item)
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

                this.OnAspNetRoleCreated(item);
                this.context.AspNetRoles.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetRoles.Where(i => i.Id == item.Id);

                

                this.OnAfterAspNetRoleCreated(item);

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
