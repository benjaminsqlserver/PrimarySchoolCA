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
    [Route("odata/ConData/AspNetUserLogins")]
    public partial class AspNetUserLoginsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AspNetUserLoginsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> GetAspNetUserLogins()
        {
            var items = this.context.AspNetUserLogins.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>();
            this.OnAspNetUserLoginsRead(ref items);

            return items;
        }

        partial void OnAspNetUserLoginsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> items);

        partial void OnAspNetUserLoginGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AspNetUserLogins(LoginProvider={keyLoginProvider},ProviderKey={keyProviderKey})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> GetAspNetUserLogin([FromODataUri] string keyLoginProvider, [FromODataUri] string keyProviderKey)
        {
            var items = this.context.AspNetUserLogins.Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey));
            var result = SingleResult.Create(items);

            OnAspNetUserLoginGet(ref result);

            return result;
        }
        partial void OnAspNetUserLoginDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        [HttpDelete("/odata/ConData/AspNetUserLogins(LoginProvider={keyLoginProvider},ProviderKey={keyProviderKey})")]
        public IActionResult DeleteAspNetUserLogin([FromODataUri] string keyLoginProvider, [FromODataUri] string keyProviderKey)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AspNetUserLogins
                    .Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserLoginDeleted(item);
                this.context.AspNetUserLogins.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAspNetUserLoginDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserLoginUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        [HttpPut("/odata/ConData/AspNetUserLogins(LoginProvider={keyLoginProvider},ProviderKey={keyProviderKey})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAspNetUserLogin([FromODataUri] string keyLoginProvider, [FromODataUri] string keyProviderKey, [FromBody]PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserLogins
                    .Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserLoginUpdated(item);
                this.context.AspNetUserLogins.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserLogins.Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                this.OnAfterAspNetUserLoginUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AspNetUserLogins(LoginProvider={keyLoginProvider},ProviderKey={keyProviderKey})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAspNetUserLogin([FromODataUri] string keyLoginProvider, [FromODataUri] string keyProviderKey, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserLogins
                    .Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAspNetUserLoginUpdated(item);
                this.context.AspNetUserLogins.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserLogins.Where(i => i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.ProviderKey == Uri.UnescapeDataString(keyProviderKey));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserLoginCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item)
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

                this.OnAspNetUserLoginCreated(item);
                this.context.AspNetUserLogins.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserLogins.Where(i => i.LoginProvider == item.LoginProvider && i.ProviderKey == item.ProviderKey);

                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");

                this.OnAfterAspNetUserLoginCreated(item);

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
