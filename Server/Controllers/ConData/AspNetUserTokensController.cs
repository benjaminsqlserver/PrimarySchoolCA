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
    [Route("odata/ConData/AspNetUserTokens")]
    public partial class AspNetUserTokensController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AspNetUserTokensController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> GetAspNetUserTokens()
        {
            var items = this.context.AspNetUserTokens.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>();
            this.OnAspNetUserTokensRead(ref items);

            return items;
        }

        partial void OnAspNetUserTokensRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> items);

        partial void OnAspNetUserTokenGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AspNetUserTokens(UserId={keyUserId},LoginProvider={keyLoginProvider},Name={keyName})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> GetAspNetUserToken([FromODataUri] string keyUserId, [FromODataUri] string keyLoginProvider, [FromODataUri] string keyName)
        {
            var items = this.context.AspNetUserTokens.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName));
            var result = SingleResult.Create(items);

            OnAspNetUserTokenGet(ref result);

            return result;
        }
        partial void OnAspNetUserTokenDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        [HttpDelete("/odata/ConData/AspNetUserTokens(UserId={keyUserId},LoginProvider={keyLoginProvider},Name={keyName})")]
        public IActionResult DeleteAspNetUserToken([FromODataUri] string keyUserId, [FromODataUri] string keyLoginProvider, [FromODataUri] string keyName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AspNetUserTokens
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserTokenDeleted(item);
                this.context.AspNetUserTokens.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAspNetUserTokenDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserTokenUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        [HttpPut("/odata/ConData/AspNetUserTokens(UserId={keyUserId},LoginProvider={keyLoginProvider},Name={keyName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAspNetUserToken([FromODataUri] string keyUserId, [FromODataUri] string keyLoginProvider, [FromODataUri] string keyName, [FromBody]PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserTokens
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAspNetUserTokenUpdated(item);
                this.context.AspNetUserTokens.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserTokens.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                this.OnAfterAspNetUserTokenUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AspNetUserTokens(UserId={keyUserId},LoginProvider={keyLoginProvider},Name={keyName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAspNetUserToken([FromODataUri] string keyUserId, [FromODataUri] string keyLoginProvider, [FromODataUri] string keyName, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AspNetUserTokens
                    .Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAspNetUserTokenUpdated(item);
                this.context.AspNetUserTokens.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserTokens.Where(i => i.UserId == Uri.UnescapeDataString(keyUserId) && i.LoginProvider == Uri.UnescapeDataString(keyLoginProvider) && i.Name == Uri.UnescapeDataString(keyName));
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAspNetUserTokenCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item)
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

                this.OnAspNetUserTokenCreated(item);
                this.context.AspNetUserTokens.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AspNetUserTokens.Where(i => i.UserId == item.UserId && i.LoginProvider == item.LoginProvider && i.Name == item.Name);

                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");

                this.OnAfterAspNetUserTokenCreated(item);

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
