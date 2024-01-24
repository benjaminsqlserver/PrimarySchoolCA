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
    [Route("odata/ConData/LocalGovtAreas")]
    public partial class LocalGovtAreasController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public LocalGovtAreasController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> GetLocalGovtAreas()
        {
            var items = this.context.LocalGovtAreas.AsQueryable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>();
            this.OnLocalGovtAreasRead(ref items);

            return items;
        }

        partial void OnLocalGovtAreasRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> items);

        partial void OnLocalGovtAreaGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/LocalGovtAreas(LgaID={LgaID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> GetLocalGovtArea(int key)
        {
            var items = this.context.LocalGovtAreas.Where(i => i.LgaID == key);
            var result = SingleResult.Create(items);

            OnLocalGovtAreaGet(ref result);

            return result;
        }
        partial void OnLocalGovtAreaDeleted(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaDeleted(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        [HttpDelete("/odata/ConData/LocalGovtAreas(LgaID={LgaID})")]
        public IActionResult DeleteLocalGovtArea(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.LocalGovtAreas
                    .Where(i => i.LgaID == key)
                    .Include(i => i.ParentsOrGuardians)
                    .Include(i => i.Schools)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnLocalGovtAreaDeleted(item);
                this.context.LocalGovtAreas.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLocalGovtAreaDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLocalGovtAreaUpdated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaUpdated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        [HttpPut("/odata/ConData/LocalGovtAreas(LgaID={LgaID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLocalGovtArea(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.LocalGovtAreas
                    .Where(i => i.LgaID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnLocalGovtAreaUpdated(item);
                this.context.LocalGovtAreas.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocalGovtAreas.Where(i => i.LgaID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "State");
                this.OnAfterLocalGovtAreaUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/LocalGovtAreas(LgaID={LgaID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLocalGovtArea(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.LocalGovtAreas
                    .Where(i => i.LgaID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnLocalGovtAreaUpdated(item);
                this.context.LocalGovtAreas.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocalGovtAreas.Where(i => i.LgaID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "State");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLocalGovtAreaCreated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaCreated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item)
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

                this.OnLocalGovtAreaCreated(item);
                this.context.LocalGovtAreas.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocalGovtAreas.Where(i => i.LgaID == item.LgaID);

                Request.QueryString = Request.QueryString.Add("$expand", "State");

                this.OnAfterLocalGovtAreaCreated(item);

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
