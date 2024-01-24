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
    [Route("odata/ConData/AspNetUserRoles")]
    public partial class AspNetUserRolesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AspNetUserRolesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> GetAspNetUserRoles()
        {
            var items = this.context.AspNetUserRoles.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>();
            this.OnAspNetUserRolesRead(ref items);

            return items;
        }

        partial void OnAspNetUserRolesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> items);

        partial void OnAspNetUserRoleGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AspNetUserRoles(UserId={keyUserId},RoleId={keyRoleId})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> GetAspNetUserRole([FromODataUri] string keyUserId, [FromODataUri] string keyRoleId)
        {
            var items = this.context.AspNetUserRoles.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId));
            var result = SingleResult.Create(items);

            OnAspNetUserRoleGet(ref result);

            return result;
        }
        partial void OnAspNetUserRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        [HttpDelete("/odata/ConData/AspNetUserRoles(UserId={keyUserId},RoleId={keyRoleId})")]
        public IActionResult DeleteAspNetUserRole([FromODataUri] string keyUserId, [FromODataUri] string keyRoleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AspNetUserRoles
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserRoleDeleted(item);
                this.context.AspNetUserRoles.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAspNetUserRoleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        [HttpPut("/odata/ConData/AspNetUserRoles(UserId={keyUserId},RoleId={keyRoleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAspNetUserRole([FromODataUri] string keyUserId, [FromODataUri] string keyRoleId, [FromBody]PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserRoles
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserRoleUpdated(item);
                this.context.AspNetUserRoles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserRoles.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole,AspNetUser");
                this.OnAfterAspNetUserRoleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AspNetUserRoles(UserId={keyUserId},RoleId={keyRoleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAspNetUserRole([FromODataUri] string keyUserId, [FromODataUri] string keyRoleId, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserRoles
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAspNetUserRoleUpdated(item);
                this.context.AspNetUserRoles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserRoles.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.RoleId == Uri.UnescapeDataString(keyRoleId));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole,AspNetUser");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item)
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

                this.OnAspNetUserRoleCreated(item);
                this.context.AspNetUserRoles.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserRoles.Where(i => i.UserId == item.UserId && i.RoleId == item.RoleId);

                Request.QueryString = Request.QueryString.Add("$expand", "AspNetRole,AspNetUser");

                this.OnAfterAspNetUserRoleCreated(item);

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
