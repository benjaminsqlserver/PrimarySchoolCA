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
    [Route("odata/ConData/ClassRegisters")]
    public partial class ClassRegistersController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public ClassRegistersController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> GetClassRegisters()
        {
            var items = this.context.ClassRegisters.AsQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegister>();
            this.OnClassRegistersRead(ref items);

            return items;
        }

        partial void OnClassRegistersRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> items);

        partial void OnClassRegisterGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/ClassRegisters(ClassRegisterID={ClassRegisterID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister> GetClassRegister(long key)
        {
            var items = this.context.ClassRegisters.Where(i => i.ClassRegisterID == key);
            var result = SingleResult.Create(items);

            OnClassRegisterGet(ref result);

            return result;
        }
        partial void OnClassRegisterDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        [HttpDelete("/odata/ConData/ClassRegisters(ClassRegisterID={ClassRegisterID})")]
        public IActionResult DeleteClassRegister(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ClassRegisters
                    .Where(i => i.ClassRegisterID == key)
                    .Include(i => i.ClassRegisterStudents)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegister>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnClassRegisterDeleted(item);
                this.context.ClassRegisters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterClassRegisterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClassRegisterUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        [HttpPut("/odata/ConData/ClassRegisters(ClassRegisterID={ClassRegisterID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutClassRegister(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.ClassRegister item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ClassRegisters
                    .Where(i => i.ClassRegisterID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegister>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnClassRegisterUpdated(item);
                this.context.ClassRegisters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisters.Where(i => i.ClassRegisterID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Term");
                this.OnAfterClassRegisterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/ClassRegisters(ClassRegisterID={ClassRegisterID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchClassRegister(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.ClassRegister> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ClassRegisters
                    .Where(i => i.ClassRegisterID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegister>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnClassRegisterUpdated(item);
                this.context.ClassRegisters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisters.Where(i => i.ClassRegisterID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Term");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClassRegisterCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.ClassRegister item)
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

                this.OnClassRegisterCreated(item);
                this.context.ClassRegisters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisters.Where(i => i.ClassRegisterID == item.ClassRegisterID);

                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Term");

                this.OnAfterClassRegisterCreated(item);

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
