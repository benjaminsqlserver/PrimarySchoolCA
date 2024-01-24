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
    [Route("odata/ConData/SchoolTypes")]
    public partial class SchoolTypesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public SchoolTypesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.SchoolType> GetSchoolTypes()
        {
            var items = this.context.SchoolTypes.AsQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolType>();
            this.OnSchoolTypesRead(ref items);

            return items;
        }

        partial void OnSchoolTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolType> items);

        partial void OnSchoolTypeGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.SchoolType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/SchoolTypes(SchoolTypeID={SchoolTypeID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.SchoolType> GetSchoolType(int key)
        {
            var items = this.context.SchoolTypes.Where(i => i.SchoolTypeID == key);
            var result = SingleResult.Create(items);

            OnSchoolTypeGet(ref result);

            return result;
        }
        partial void OnSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        [HttpDelete("/odata/ConData/SchoolTypes(SchoolTypeID={SchoolTypeID})")]
        public IActionResult DeleteSchoolType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SchoolTypes
                    .Where(i => i.SchoolTypeID == key)
                    .Include(i => i.SubjectSchoolTypes)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolTypeDeleted(item);
                this.context.SchoolTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSchoolTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        [HttpPut("/odata/ConData/SchoolTypes(SchoolTypeID={SchoolTypeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSchoolType(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.SchoolType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SchoolTypes
                    .Where(i => i.SchoolTypeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolType>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSchoolTypeUpdated(item);
                this.context.SchoolTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolTypes.Where(i => i.SchoolTypeID == key);
                
                this.OnAfterSchoolTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/SchoolTypes(SchoolTypeID={SchoolTypeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSchoolType(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.SchoolType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SchoolTypes
                    .Where(i => i.SchoolTypeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SchoolType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSchoolTypeUpdated(item);
                this.context.SchoolTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolTypes.Where(i => i.SchoolTypeID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.SchoolType item)
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

                this.OnSchoolTypeCreated(item);
                this.context.SchoolTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SchoolTypes.Where(i => i.SchoolTypeID == item.SchoolTypeID);

                

                this.OnAfterSchoolTypeCreated(item);

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
