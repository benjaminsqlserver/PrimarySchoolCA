using PrimarySchoolCA.Server.Models.ConData;
using Radzen;

namespace PrimarySchoolCA.Client
{
    public partial class ConDataService
    {
        public async Task<IEnumerable<LocalGovtArea>> GetLGAsByStateID(int selectedStateID)
        {
            var lgs=new List<LocalGovtArea>();

            try
            {
                var lgs1 = await GetLocalGovtAreas();
                IEnumerable<Server.Models.ConData.LocalGovtArea> lg1Result = lgs1.Value.AsODataEnumerable();

                lgs=lg1Result.Where(p=>p.StateID==selectedStateID).ToList();
            }
            catch(Exception ex)
            {
                
            }


            return await Task.FromResult(lgs);
        }

        //This method has just been moved from the RADZEN BLAZOR Studio generated partial class to this partial class
        // which is handcrafted by BENJAMIN FADINA
        public async Task<ClassRegister> FetchClassRegister(int academicSessionID, int termID, int schoolClassID)
        {
            var register = new ClassRegister();
            // Construct the filter expression based on academicSessionID, termID, and schoolClassID
            string filter = $"AcademicSessionID eq {academicSessionID} and TermID eq {termID} and SchoolClassID eq {schoolClassID}";

            var uri = new Uri(baseUri, $"ClassRegisters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            Radzen.ODataServiceResult<ClassRegister> result = await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister>>(response);

            IEnumerable<ClassRegister> classRegisters = result.Value.AsODataEnumerable();
            if (classRegisters.Any())
            {
                register = classRegisters.ElementAt(0);
            }
            return await Task.FromResult(register);
        }

        public async Task<List<ClassRegisterStudent>> FetchClassRegisterStudents(long classRegisterID)
        {
            var students = new List<ClassRegisterStudent>();
            // Construct the filter expression based on classRegisterID
            string filter = $"ClassRegisterID eq {classRegisterID}";

            var uri = new Uri(baseUri, $"ClassRegisterStudents");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisterStudents(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            Radzen.ODataServiceResult<ClassRegisterStudent> result = await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>>(response);

            IEnumerable<ClassRegisterStudent> registerStudents = result.Value.AsODataEnumerable();

            if (registerStudents.Any())
            {
                students = registerStudents.ToList();
            }

            return await Task.FromResult(students);
        }

        public async Task SaveAttendance(AttendanceViewModel student)
        {
            //create attendance object from attendanceviewmodel object
            Attendance newAttendance = new Attendance { AcademicSessionID = student.AcademicSessionID, AttendanceDate = student.AttendanceDate, Present = student.Present, SchoolClassID = student.SchoolClassID, StudentID = student.StudentID, TermID = student.TermID };
            // Construct the filter expression based on academicSessionID, termID, and schoolClassID
            string filter = $"AcademicSessionID eq {student.AcademicSessionID} and TermID eq {student.TermID} and SchoolClassID eq {student.SchoolClassID} and StudentID eq {student.StudentID}";
            var uri = new Uri(baseUri, $"Attendances");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: filter);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAttendances(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            Radzen.ODataServiceResult<Attendance> result = await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Attendance>>(response);

            IEnumerable<Attendance> attendances = result.Value.AsODataEnumerable();

            if (attendances.Any())//this implies that student's attendance has been taken at least once 
            {
                Attendance matchingAttendanceRecord = attendances.FirstOrDefault(p => p.AttendanceDate == student.AttendanceDate);
                if (matchingAttendanceRecord != null)//student's attendance is already taken on a particular date
                {
                    newAttendance.AttendanceID = matchingAttendanceRecord.AttendanceID;
                    await UpdateAttendance(newAttendance.AttendanceID, newAttendance);//update previously attendance record
                }
                else//otherwise
                {
                    //create new student attendance record
                    await CreateAttendance(newAttendance);
                }

            }
            else
            {
                await CreateAttendance(newAttendance);
            }
        }
    }
}
