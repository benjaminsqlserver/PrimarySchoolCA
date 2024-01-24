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
    [Route("odata/ConData/ClassRegisterStudents")]
    public partial class ClassRegisterStudentsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public ClassRegisterStudentsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> GetClassRegisterStudents()
        {
            var items = this.context.ClassRegisterStudents.AsQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>();
            this.OnClassRegisterStudentsRead(ref items);

            return items;
        }

        partial void OnClassRegisterStudentsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> items);

        partial void OnClassRegisterStudentGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/ClassRegisterStudents(ID={ID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> GetClassRegisterStudent(long key)
        {
            var items = this.context.ClassRegisterStudents.Where(i => i.ID == key);
            var result = SingleResult.Create(items);

            OnClassRegisterStudentGet(ref result);

            return result;
        }
        partial void OnClassRegisterStudentDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        [HttpDelete("/odata/ConData/ClassRegisterStudents(ID={ID})")]
        public IActionResult DeleteClassRegisterStudent(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ClassRegisterStudents
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnClassRegisterStudentDeleted(item);
                this.context.ClassRegisterStudents.Remove(item);
                this.context.SaveChanges();
                this.OnAfterClassRegisterStudentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClassRegisterStudentUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        [HttpPut("/odata/ConData/ClassRegisterStudents(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutClassRegisterStudent(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ClassRegisterStudents
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnClassRegisterStudentUpdated(item);
                this.context.ClassRegisterStudents.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisterStudents.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ClassRegister,Student");
                this.OnAfterClassRegisterStudentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/ClassRegisterStudents(ID={ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchClassRegisterStudent(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ClassRegisterStudents
                    .Where(i => i.ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnClassRegisterStudentUpdated(item);
                this.context.ClassRegisterStudents.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisterStudents.Where(i => i.ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ClassRegister,Student");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClassRegisterStudentCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item)
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

                this.OnClassRegisterStudentCreated(item);
                this.context.ClassRegisterStudents.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClassRegisterStudents.Where(i => i.ID == item.ID);

                Request.QueryString = Request.QueryString.Add("$expand", "ClassRegister,Student");

                this.OnAfterClassRegisterStudentCreated(item);

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
