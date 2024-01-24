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
    public partial class StudentListForParentGuardianDropdownsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public StudentListForParentGuardianDropdownsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [Route("odata/ConData/StudentListForParentGuardianDropdownsFunc()")]
        public IActionResult StudentListForParentGuardianDropdownsFunc()
        {
            this.OnStudentListForParentGuardianDropdownsDefaultParams();

            var items = this.context.StudentListForParentGuardianDropdowns.FromSqlInterpolated($"EXEC [dbo].[StudentListForParentGuardianDropdowns] ").ToList().AsQueryable();

            this.OnStudentListForParentGuardianDropdownsInvoke(ref items);

            return Ok(items);
        }

        partial void OnStudentListForParentGuardianDropdownsDefaultParams();

        partial void OnStudentListForParentGuardianDropdownsInvoke(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown> items);
    }
}
