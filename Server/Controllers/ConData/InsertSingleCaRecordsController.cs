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
    public partial class InsertSingleCaRecordsController : ODataController
    {
        private PrimarySchoolCA.Server.Data.ConDataContext context;

        public InsertSingleCaRecordsController(PrimarySchoolCA.Server.Data.ConDataContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("odata/ConData/InsertSingleCaRecordsFunc(StudentID={StudentID},AcademicSessionID={AcademicSessionID},TermID={TermID},SchoolClassID={SchoolClassID},SubjectID={SubjectID},CAMarkObtainable={CAMarkObtainable},CAMarkObtained={CAMarkObtained},EntryDate={EntryDate},InsertedBy={InsertedBy})")]
        public IActionResult InsertSingleCaRecordsFunc([FromODataUri] long? StudentID, [FromODataUri] int? AcademicSessionID, [FromODataUri] int? TermID, [FromODataUri] int? SchoolClassID, [FromODataUri] long? SubjectID, [FromODataUri] int? CAMarkObtainable, [FromODataUri] int? CAMarkObtained, [FromODataUri] string EntryDate, [FromODataUri] string InsertedBy)
        {
            this.OnInsertSingleCaRecordsDefaultParams(ref StudentID, ref AcademicSessionID, ref TermID, ref SchoolClassID, ref SubjectID, ref CAMarkObtainable, ref CAMarkObtained, ref EntryDate, ref InsertedBy);


            SqlParameter[] @params =
            {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@StudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = StudentID},
              new SqlParameter("@AcademicSessionID", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = AcademicSessionID},
              new SqlParameter("@TermID", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = TermID},
              new SqlParameter("@SchoolClassID", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = SchoolClassID},
              new SqlParameter("@SubjectID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = SubjectID},
              new SqlParameter("@CAMarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = CAMarkObtainable},
              new SqlParameter("@CAMarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = CAMarkObtained},
              new SqlParameter("@EntryDate", SqlDbType.DateTime, -1) {Direction = ParameterDirection.Input, Value = string.IsNullOrEmpty(EntryDate) ? DBNull.Value : (object)DateTime.Parse(EntryDate, null, System.Globalization.DateTimeStyles.RoundtripKind)},
              new SqlParameter("@InsertedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = InsertedBy},

            };

            foreach(var _p in @params)
            {
                if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
                {
                    _p.Value = DBNull.Value;
                }
            }

            this.context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[InsertSingleCARecord] @StudentID, @AcademicSessionID, @TermID, @SchoolClassID, @SubjectID, @CAMarkObtainable, @CAMarkObtained, @EntryDate, @InsertedBy", @params);

            int result = Convert.ToInt32(@params[0].Value);

            this.OnInsertSingleCaRecordsInvoke(ref result);

            return Ok(result);
        }

        partial void OnInsertSingleCaRecordsDefaultParams(ref long? StudentID, ref int? AcademicSessionID, ref int? TermID, ref int? SchoolClassID, ref long? SubjectID, ref int? CAMarkObtainable, ref int? CAMarkObtained, ref string EntryDate, ref string InsertedBy);
      partial void OnInsertSingleCaRecordsInvoke(ref int result);
    }
}
