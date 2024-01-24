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
    [Route("odata/ConData/States")]
    public partial class StatesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public StatesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.State> GetStates()
        {
            var items = this.context.States.AsQueryable<PrimarySchoolCA.Server.Models.ConData.State>();
            this.OnStatesRead(ref items);

            return items;
        }

        partial void OnStatesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.State> items);

        partial void OnStateGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.State> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/States(StateID={StateID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.State> GetState(int key)
        {
            var items = this.context.States.Where(i => i.StateID == key);
            var result = SingleResult.Create(items);

            OnStateGet(ref result);

            return result;
        }
        partial void OnStateDeleted(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateDeleted(PrimarySchoolCA.Server.Models.ConData.State item);

        [HttpDelete("/odata/ConData/States(StateID={StateID})")]
        public IActionResult DeleteState(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.States
                    .Where(i => i.StateID == key)
                    .Include(i => i.LocalGovtAreas)
                    .Include(i => i.ParentsOrGuardians)
                    .Include(i => i.Schools)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.State>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStateDeleted(item);
                this.context.States.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStateUpdated(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateUpdated(PrimarySchoolCA.Server.Models.ConData.State item);

        [HttpPut("/odata/ConData/States(StateID={StateID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutState(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.State item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.States
                    .Where(i => i.StateID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.State>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStateUpdated(item);
                this.context.States.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateID == key);
                
                this.OnAfterStateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/States(StateID={StateID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchState(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.State> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.States
                    .Where(i => i.StateID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.State>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStateUpdated(item);
                this.context.States.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStateCreated(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateCreated(PrimarySchoolCA.Server.Models.ConData.State item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.State item)
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

                this.OnStateCreated(item);
                this.context.States.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateID == item.StateID);

                

                this.OnAfterStateCreated(item);

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
