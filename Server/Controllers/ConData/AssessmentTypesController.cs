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
    [Route("odata/ConData/AssessmentTypes")]
    public partial class AssessmentTypesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AssessmentTypesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.AssessmentType> GetAssessmentTypes()
        {
            var items = this.context.AssessmentTypes.AsQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentType>();
            this.OnAssessmentTypesRead(ref items);

            return items;
        }

        partial void OnAssessmentTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentType> items);

        partial void OnAssessmentTypeGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.AssessmentType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/AssessmentTypes(AssessmentTypeID={AssessmentTypeID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.AssessmentType> GetAssessmentType(int key)
        {
            var items = this.context.AssessmentTypes.Where(i => i.AssessmentTypeID == key);
            var result = SingleResult.Create(items);

            OnAssessmentTypeGet(ref result);

            return result;
        }
        partial void OnAssessmentTypeDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        [HttpDelete("/odata/ConData/AssessmentTypes(AssessmentTypeID={AssessmentTypeID})")]
        public IActionResult DeleteAssessmentType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AssessmentTypes
                    .Where(i => i.AssessmentTypeID == key)
                    .Include(i => i.AssessmentSetups)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAssessmentTypeDeleted(item);
                this.context.AssessmentTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAssessmentTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAssessmentTypeUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        [HttpPut("/odata/ConData/AssessmentTypes(AssessmentTypeID={AssessmentTypeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAssessmentType(int key, [FromBody]PrimarySchoolCA.Server.Models.ConData.AssessmentType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AssessmentTypes
                    .Where(i => i.AssessmentTypeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentType>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAssessmentTypeUpdated(item);
                this.context.AssessmentTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentTypes.Where(i => i.AssessmentTypeID == key);
                
                this.OnAfterAssessmentTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/AssessmentTypes(AssessmentTypeID={AssessmentTypeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAssessmentType(int key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.AssessmentType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AssessmentTypes
                    .Where(i => i.AssessmentTypeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.AssessmentType>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAssessmentTypeUpdated(item);
                this.context.AssessmentTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentTypes.Where(i => i.AssessmentTypeID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAssessmentTypeCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.AssessmentType item)
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

                this.OnAssessmentTypeCreated(item);
                this.context.AssessmentTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AssessmentTypes.Where(i => i.AssessmentTypeID == item.AssessmentTypeID);

                

                this.OnAfterAssessmentTypeCreated(item);

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
