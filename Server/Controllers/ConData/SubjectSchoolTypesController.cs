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
    [Route("odata/ConData/SubjectSchoolTypes")]
    public partial class SubjectSchoolTypesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public SubjectSchoolTypesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> GetSubjectSchoolTypes()
        {
            var items = this.context.SubjectSchoolTypes.AsQueryable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>();
            this.OnSubjectSchoolTypesRead(ref items);

            return items;
        }

        partial void OnSubjectSchoolTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> items);

        partial void OnSubjectSchoolTypeGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/SubjectSchoolTypes(ID={ID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> GetSubjectSchoolType(long key)
        {
            var items = this.context.SubjectSchoolTypes.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnSubjectSchoolTypeGet(ref result);

            return result;
        }
        partial void OnSubjectSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        [HttpDelete("/odata/ConData/SubjectSchoolTypes(ID={ID})")]
        public IActionResult DeleteSubjectSchoolType(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SubjectSchoolTypes
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectSchoolTypeDeleted(item);
                this.context.SubjectSchoolTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSubjectSchoolTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        [HttpPut("/odata/ConData/SubjectSchoolTypes(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSubjectSchoolType(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SubjectSchoolTypes
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectSchoolTypeUpdated(item);
                this.context.SubjectSchoolTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectSchoolTypes.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "SchoolType,Subject");
                this.OnAfterSubjectSchoolTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/SubjectSchoolTypes(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSubjectSchoolType(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SubjectSchoolTypes
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSubjectSchoolTypeUpdated(item);
                this.context.SubjectSchoolTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectSchoolTypes.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "SchoolType,Subject");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item)
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

                this.OnSubjectSchoolTypeCreated(item);
                this.context.SubjectSchoolTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectSchoolTypes.Where(i => i.ID == item.ID);

                Request.QueryString = Request.QueryString.Add("$expand", "SchoolType,Subject");

                this.OnAfterSubjectSchoolTypeCreated(item);

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
