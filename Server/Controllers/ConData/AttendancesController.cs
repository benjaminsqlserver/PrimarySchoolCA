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
    [Route("odata/ConData/Attendances")]
    public partial class AttendancesController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public AttendancesController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<PrimarySchoolCA.Server.Models.ConData.Attendance> GetAttendances()
        {
            var items = this.context.Attendances.AsQueryable<PrimarySchoolCA.Server.Models.ConData.Attendance>();
            this.OnAttendancesRead(ref items);

            return items;
        }

        partial void OnAttendancesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Attendance> items);

        partial void OnAttendanceGet(ref SingleResult<PrimarySchoolCA.Server.Models.ConData.Attendance> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Attendances(AttendanceID={AttendanceID})")]
        public SingleResult<PrimarySchoolCA.Server.Models.ConData.Attendance> GetAttendance(long key)
        {
            var items = this.context.Attendances.Where(i => i.AttendanceID == key);
            var result = SingleResult.Create(items);

            OnAttendanceGet(ref result);

            return result;
        }
        partial void OnAttendanceDeleted(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceDeleted(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        [HttpDelete("/odata/ConData/Attendances(AttendanceID={AttendanceID})")]
        public IActionResult DeleteAttendance(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Attendances
                    .Where(i => i.AttendanceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Attendance>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAttendanceDeleted(item);
                this.context.Attendances.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAttendanceDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAttendanceUpdated(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceUpdated(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        [HttpPut("/odata/ConData/Attendances(AttendanceID={AttendanceID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAttendance(long key, [FromBody]PrimarySchoolCA.Server.Models.ConData.Attendance item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Attendances
                    .Where(i => i.AttendanceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Attendance>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAttendanceUpdated(item);
                this.context.Attendances.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attendances.Where(i => i.AttendanceID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Term");
                this.OnAfterAttendanceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Attendances(AttendanceID={AttendanceID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAttendance(long key, [FromBody]Delta<PrimarySchoolCA.Server.Models.ConData.Attendance> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Attendances
                    .Where(i => i.AttendanceID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<PrimarySchoolCA.Server.Models.ConData.Attendance>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAttendanceUpdated(item);
                this.context.Attendances.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attendances.Where(i => i.AttendanceID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Term");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAttendanceCreated(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceCreated(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] PrimarySchoolCA.Server.Models.ConData.Attendance item)
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

                this.OnAttendanceCreated(item);
                this.context.Attendances.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attendances.Where(i => i.AttendanceID == item.AttendanceID);

                Request.QueryString = Request.QueryString.Add("$expand", "AcademicSession,SchoolClass,Student,Term");

                this.OnAfterAttendanceCreated(item);

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
