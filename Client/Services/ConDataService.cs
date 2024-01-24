
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace PrimarySchoolCA.Client
{
    public partial class ConDataService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ConDataService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ConData/");
        }


        public async System.Threading.Tasks.Task ExportAcademicSessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/academicsessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/academicsessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAcademicSessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/academicsessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/academicsessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAcademicSessions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AcademicSession>> GetAcademicSessions(Query query)
        {
            return await GetAcademicSessions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AcademicSession>> GetAcademicSessions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AcademicSessions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAcademicSessions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AcademicSession>>(response);
        }

        partial void OnCreateAcademicSession(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> CreateAcademicSession(PrimarySchoolCA.Server.Models.ConData.AcademicSession academicSession = default(PrimarySchoolCA.Server.Models.ConData.AcademicSession))
        {
            var uri = new Uri(baseUri, $"AcademicSessions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(academicSession), Encoding.UTF8, "application/json");

            OnCreateAcademicSession(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AcademicSession>(response);
        }

        partial void OnDeleteAcademicSession(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAcademicSession(int academicSessionId = default(int))
        {
            var uri = new Uri(baseUri, $"AcademicSessions({academicSessionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAcademicSession(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAcademicSessionByAcademicSessionId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> GetAcademicSessionByAcademicSessionId(string expand = default(string), int academicSessionId = default(int))
        {
            var uri = new Uri(baseUri, $"AcademicSessions({academicSessionId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAcademicSessionByAcademicSessionId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AcademicSession>(response);
        }

        partial void OnUpdateAcademicSession(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAcademicSession(int academicSessionId = default(int), PrimarySchoolCA.Server.Models.ConData.AcademicSession academicSession = default(PrimarySchoolCA.Server.Models.ConData.AcademicSession))
        {
            var uri = new Uri(baseUri, $"AcademicSessions({academicSessionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", academicSession.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(academicSession), Encoding.UTF8, "application/json");

            OnUpdateAcademicSession(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetRoleClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetRoleClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetRoleClaims(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>> GetAspNetRoleClaims(Query query)
        {
            return await GetAspNetRoleClaims(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>> GetAspNetRoleClaims(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetRoleClaims");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetRoleClaims(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>>(response);
        }

        partial void OnCreateAspNetRoleClaim(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> CreateAspNetRoleClaim(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim aspNetRoleClaim = default(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim))
        {
            var uri = new Uri(baseUri, $"AspNetRoleClaims");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetRoleClaim), Encoding.UTF8, "application/json");

            OnCreateAspNetRoleClaim(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>(response);
        }

        partial void OnDeleteAspNetRoleClaim(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetRoleClaim(int id = default(int))
        {
            var uri = new Uri(baseUri, $"AspNetRoleClaims({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetRoleClaim(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetRoleClaimById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> GetAspNetRoleClaimById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"AspNetRoleClaims({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetRoleClaimById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>(response);
        }

        partial void OnUpdateAspNetRoleClaim(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetRoleClaim(int id = default(int), PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim aspNetRoleClaim = default(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim))
        {
            var uri = new Uri(baseUri, $"AspNetRoleClaims({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetRoleClaim.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetRoleClaim), Encoding.UTF8, "application/json");

            OnUpdateAspNetRoleClaim(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetRoles(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRole>> GetAspNetRoles(Query query)
        {
            return await GetAspNetRoles(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRole>> GetAspNetRoles(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetRoles");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetRoles(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetRole>>(response);
        }

        partial void OnCreateAspNetRole(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> CreateAspNetRole(PrimarySchoolCA.Server.Models.ConData.AspNetRole aspNetRole = default(PrimarySchoolCA.Server.Models.ConData.AspNetRole))
        {
            var uri = new Uri(baseUri, $"AspNetRoles");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetRole), Encoding.UTF8, "application/json");

            OnCreateAspNetRole(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetRole>(response);
        }

        partial void OnDeleteAspNetRole(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetRole(string id = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetRoles('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetRoleById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> GetAspNetRoleById(string expand = default(string), string id = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetRoles('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetRoleById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetRole>(response);
        }

        partial void OnUpdateAspNetRole(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetRole(string id = default(string), PrimarySchoolCA.Server.Models.ConData.AspNetRole aspNetRole = default(PrimarySchoolCA.Server.Models.ConData.AspNetRole))
        {
            var uri = new Uri(baseUri, $"AspNetRoles('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetRole.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetRole), Encoding.UTF8, "application/json");

            OnUpdateAspNetRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetUserClaims(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>> GetAspNetUserClaims(Query query)
        {
            return await GetAspNetUserClaims(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>> GetAspNetUserClaims(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserClaims");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserClaims(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>>(response);
        }

        partial void OnCreateAspNetUserClaim(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> CreateAspNetUserClaim(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim aspNetUserClaim = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim))
        {
            var uri = new Uri(baseUri, $"AspNetUserClaims");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserClaim), Encoding.UTF8, "application/json");

            OnCreateAspNetUserClaim(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>(response);
        }

        partial void OnDeleteAspNetUserClaim(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetUserClaim(int id = default(int))
        {
            var uri = new Uri(baseUri, $"AspNetUserClaims({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetUserClaim(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetUserClaimById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> GetAspNetUserClaimById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"AspNetUserClaims({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserClaimById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>(response);
        }

        partial void OnUpdateAspNetUserClaim(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetUserClaim(int id = default(int), PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim aspNetUserClaim = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim))
        {
            var uri = new Uri(baseUri, $"AspNetUserClaims({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetUserClaim.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserClaim), Encoding.UTF8, "application/json");

            OnUpdateAspNetUserClaim(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserLoginsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserLoginsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetUserLogins(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>> GetAspNetUserLogins(Query query)
        {
            return await GetAspNetUserLogins(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>> GetAspNetUserLogins(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserLogins");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserLogins(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>>(response);
        }

        partial void OnCreateAspNetUserLogin(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> CreateAspNetUserLogin(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin aspNetUserLogin = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin))
        {
            var uri = new Uri(baseUri, $"AspNetUserLogins");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserLogin), Encoding.UTF8, "application/json");

            OnCreateAspNetUserLogin(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>(response);
        }

        partial void OnDeleteAspNetUserLogin(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetUserLogin(string loginProvider = default(string), string providerKey = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserLogins(LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',ProviderKey='{Uri.EscapeDataString(providerKey.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetUserLogin(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetUserLoginByLoginProviderAndProviderKey(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> GetAspNetUserLoginByLoginProviderAndProviderKey(string expand = default(string), string loginProvider = default(string), string providerKey = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserLogins(LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',ProviderKey='{Uri.EscapeDataString(providerKey.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserLoginByLoginProviderAndProviderKey(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>(response);
        }

        partial void OnUpdateAspNetUserLogin(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetUserLogin(string loginProvider = default(string), string providerKey = default(string), PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin aspNetUserLogin = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin))
        {
            var uri = new Uri(baseUri, $"AspNetUserLogins(LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',ProviderKey='{Uri.EscapeDataString(providerKey.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetUserLogin.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserLogin), Encoding.UTF8, "application/json");

            OnUpdateAspNetUserLogin(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetUserRoles(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>> GetAspNetUserRoles(Query query)
        {
            return await GetAspNetUserRoles(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>> GetAspNetUserRoles(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserRoles");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserRoles(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>>(response);
        }

        partial void OnCreateAspNetUserRole(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> CreateAspNetUserRole(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole aspNetUserRole = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole))
        {
            var uri = new Uri(baseUri, $"AspNetUserRoles");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserRole), Encoding.UTF8, "application/json");

            OnCreateAspNetUserRole(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>(response);
        }

        partial void OnDeleteAspNetUserRole(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetUserRole(string userId = default(string), string roleId = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserRoles(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',RoleId='{Uri.EscapeDataString(roleId.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetUserRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetUserRoleByUserIdAndRoleId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> GetAspNetUserRoleByUserIdAndRoleId(string expand = default(string), string userId = default(string), string roleId = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserRoles(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',RoleId='{Uri.EscapeDataString(roleId.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserRoleByUserIdAndRoleId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>(response);
        }

        partial void OnUpdateAspNetUserRole(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetUserRole(string userId = default(string), string roleId = default(string), PrimarySchoolCA.Server.Models.ConData.AspNetUserRole aspNetUserRole = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole))
        {
            var uri = new Uri(baseUri, $"AspNetUserRoles(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',RoleId='{Uri.EscapeDataString(roleId.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetUserRole.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserRole), Encoding.UTF8, "application/json");

            OnUpdateAspNetUserRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetUsers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUser>> GetAspNetUsers(Query query)
        {
            return await GetAspNetUsers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUser>> GetAspNetUsers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUsers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUsers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUser>>(response);
        }

        partial void OnCreateAspNetUser(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> CreateAspNetUser(PrimarySchoolCA.Server.Models.ConData.AspNetUser aspNetUser = default(PrimarySchoolCA.Server.Models.ConData.AspNetUser))
        {
            var uri = new Uri(baseUri, $"AspNetUsers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUser), Encoding.UTF8, "application/json");

            OnCreateAspNetUser(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUser>(response);
        }

        partial void OnDeleteAspNetUser(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetUser(string id = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUsers('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetUser(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetUserById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> GetAspNetUserById(string expand = default(string), string id = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUsers('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUser>(response);
        }

        partial void OnUpdateAspNetUser(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetUser(string id = default(string), PrimarySchoolCA.Server.Models.ConData.AspNetUser aspNetUser = default(PrimarySchoolCA.Server.Models.ConData.AspNetUser))
        {
            var uri = new Uri(baseUri, $"AspNetUsers('{Uri.EscapeDataString(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetUser.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUser), Encoding.UTF8, "application/json");

            OnUpdateAspNetUser(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserTokensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAspNetUserTokensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAspNetUserTokens(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>> GetAspNetUserTokens(Query query)
        {
            return await GetAspNetUserTokens(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>> GetAspNetUserTokens(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserTokens");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserTokens(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>>(response);
        }

        partial void OnCreateAspNetUserToken(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> CreateAspNetUserToken(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken aspNetUserToken = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken))
        {
            var uri = new Uri(baseUri, $"AspNetUserTokens");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserToken), Encoding.UTF8, "application/json");

            OnCreateAspNetUserToken(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>(response);
        }

        partial void OnDeleteAspNetUserToken(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAspNetUserToken(string userId = default(string), string loginProvider = default(string), string name = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserTokens(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',Name='{Uri.EscapeDataString(name.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAspNetUserToken(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> GetAspNetUserTokenByUserIdAndLoginProviderAndName(string expand = default(string), string userId = default(string), string loginProvider = default(string), string name = default(string))
        {
            var uri = new Uri(baseUri, $"AspNetUserTokens(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',Name='{Uri.EscapeDataString(name.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>(response);
        }

        partial void OnUpdateAspNetUserToken(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAspNetUserToken(string userId = default(string), string loginProvider = default(string), string name = default(string), PrimarySchoolCA.Server.Models.ConData.AspNetUserToken aspNetUserToken = default(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken))
        {
            var uri = new Uri(baseUri, $"AspNetUserTokens(UserId='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',LoginProvider='{Uri.EscapeDataString(loginProvider.Trim().Replace("'", "''"))}',Name='{Uri.EscapeDataString(name.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", aspNetUserToken.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(aspNetUserToken), Encoding.UTF8, "application/json");

            OnUpdateAspNetUserToken(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAssessmentSetupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmentsetups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmentsetups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAssessmentSetupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmentsetups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmentsetups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAssessmentSetups(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>> GetAssessmentSetups(Query query)
        {
            return await GetAssessmentSetups(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>> GetAssessmentSetups(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AssessmentSetups");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAssessmentSetups(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>>(response);
        }

        partial void OnCreateAssessmentSetup(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> CreateAssessmentSetup(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup assessmentSetup = default(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup))
        {
            var uri = new Uri(baseUri, $"AssessmentSetups");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(assessmentSetup), Encoding.UTF8, "application/json");

            OnCreateAssessmentSetup(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>(response);
        }

        partial void OnDeleteAssessmentSetup(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAssessmentSetup(long assessmentSetupId = default(long))
        {
            var uri = new Uri(baseUri, $"AssessmentSetups({assessmentSetupId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAssessmentSetup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAssessmentSetupByAssessmentSetupId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> GetAssessmentSetupByAssessmentSetupId(string expand = default(string), long assessmentSetupId = default(long))
        {
            var uri = new Uri(baseUri, $"AssessmentSetups({assessmentSetupId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAssessmentSetupByAssessmentSetupId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>(response);
        }

        partial void OnUpdateAssessmentSetup(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAssessmentSetup(long assessmentSetupId = default(long), PrimarySchoolCA.Server.Models.ConData.AssessmentSetup assessmentSetup = default(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup))
        {
            var uri = new Uri(baseUri, $"AssessmentSetups({assessmentSetupId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", assessmentSetup.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(assessmentSetup), Encoding.UTF8, "application/json");

            OnUpdateAssessmentSetup(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAssessmentTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmenttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmenttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAssessmentTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmenttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmenttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAssessmentTypes(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentType>> GetAssessmentTypes(Query query)
        {
            return await GetAssessmentTypes(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentType>> GetAssessmentTypes(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AssessmentTypes");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAssessmentTypes(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.AssessmentType>>(response);
        }

        partial void OnCreateAssessmentType(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> CreateAssessmentType(PrimarySchoolCA.Server.Models.ConData.AssessmentType assessmentType = default(PrimarySchoolCA.Server.Models.ConData.AssessmentType))
        {
            var uri = new Uri(baseUri, $"AssessmentTypes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(assessmentType), Encoding.UTF8, "application/json");

            OnCreateAssessmentType(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AssessmentType>(response);
        }

        partial void OnDeleteAssessmentType(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAssessmentType(int assessmentTypeId = default(int))
        {
            var uri = new Uri(baseUri, $"AssessmentTypes({assessmentTypeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAssessmentType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAssessmentTypeByAssessmentTypeId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> GetAssessmentTypeByAssessmentTypeId(string expand = default(string), int assessmentTypeId = default(int))
        {
            var uri = new Uri(baseUri, $"AssessmentTypes({assessmentTypeId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAssessmentTypeByAssessmentTypeId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.AssessmentType>(response);
        }

        partial void OnUpdateAssessmentType(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAssessmentType(int assessmentTypeId = default(int), PrimarySchoolCA.Server.Models.ConData.AssessmentType assessmentType = default(PrimarySchoolCA.Server.Models.ConData.AssessmentType))
        {
            var uri = new Uri(baseUri, $"AssessmentTypes({assessmentTypeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", assessmentType.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(assessmentType), Encoding.UTF8, "application/json");

            OnUpdateAssessmentType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAttendancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/attendances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/attendances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAttendancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/attendances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/attendances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAttendances(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Attendance>> GetAttendances(Query query)
        {
            return await GetAttendances(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Attendance>> GetAttendances(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Attendances");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAttendances(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Attendance>>(response);
        }

        partial void OnCreateAttendance(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> CreateAttendance(PrimarySchoolCA.Server.Models.ConData.Attendance attendance = default(PrimarySchoolCA.Server.Models.ConData.Attendance))
        {
            var uri = new Uri(baseUri, $"Attendances");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(attendance), Encoding.UTF8, "application/json");

            OnCreateAttendance(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Attendance>(response);
        }

        partial void OnDeleteAttendance(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAttendance(long attendanceId = default(long))
        {
            var uri = new Uri(baseUri, $"Attendances({attendanceId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAttendance(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAttendanceByAttendanceId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> GetAttendanceByAttendanceId(string expand = default(string), long attendanceId = default(long))
        {
            var uri = new Uri(baseUri, $"Attendances({attendanceId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAttendanceByAttendanceId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Attendance>(response);
        }

        partial void OnUpdateAttendance(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAttendance(long attendanceId = default(long), PrimarySchoolCA.Server.Models.ConData.Attendance attendance = default(PrimarySchoolCA.Server.Models.ConData.Attendance))
        {
            var uri = new Uri(baseUri, $"Attendances({attendanceId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", attendance.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(attendance), Encoding.UTF8, "application/json");

            OnUpdateAttendance(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportClassRegistersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportClassRegistersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetClassRegisters(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister>> GetClassRegisters(Query query)
        {
            return await GetClassRegisters(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister>> GetClassRegisters(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ClassRegisters");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisters(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegister>>(response);
        }

        partial void OnCreateClassRegister(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> CreateClassRegister(PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegister = default(PrimarySchoolCA.Server.Models.ConData.ClassRegister))
        {
            var uri = new Uri(baseUri, $"ClassRegisters");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(classRegister), Encoding.UTF8, "application/json");

            OnCreateClassRegister(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ClassRegister>(response);
        }

        partial void OnDeleteClassRegister(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteClassRegister(long classRegisterId = default(long))
        {
            var uri = new Uri(baseUri, $"ClassRegisters({classRegisterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteClassRegister(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetClassRegisterByClassRegisterId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> GetClassRegisterByClassRegisterId(string expand = default(string), long classRegisterId = default(long))
        {
            var uri = new Uri(baseUri, $"ClassRegisters({classRegisterId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisterByClassRegisterId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ClassRegister>(response);
        }

        partial void OnUpdateClassRegister(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateClassRegister(long classRegisterId = default(long), PrimarySchoolCA.Server.Models.ConData.ClassRegister classRegister = default(PrimarySchoolCA.Server.Models.ConData.ClassRegister))
        {
            var uri = new Uri(baseUri, $"ClassRegisters({classRegisterId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", classRegister.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(classRegister), Encoding.UTF8, "application/json");

            OnUpdateClassRegister(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportClassRegisterStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisterstudents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisterstudents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportClassRegisterStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisterstudents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisterstudents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetClassRegisterStudents(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>> GetClassRegisterStudents(Query query)
        {
            return await GetClassRegisterStudents(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>> GetClassRegisterStudents(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ClassRegisterStudents");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisterStudents(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>>(response);
        }

        partial void OnCreateClassRegisterStudent(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> CreateClassRegisterStudent(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classRegisterStudent = default(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent))
        {
            var uri = new Uri(baseUri, $"ClassRegisterStudents");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(classRegisterStudent), Encoding.UTF8, "application/json");

            OnCreateClassRegisterStudent(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>(response);
        }

        partial void OnDeleteClassRegisterStudent(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteClassRegisterStudent(long id = default(long))
        {
            var uri = new Uri(baseUri, $"ClassRegisterStudents({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteClassRegisterStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetClassRegisterStudentById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> GetClassRegisterStudentById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"ClassRegisterStudents({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetClassRegisterStudentById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>(response);
        }

        partial void OnUpdateClassRegisterStudent(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateClassRegisterStudent(long id = default(long), PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classRegisterStudent = default(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent))
        {
            var uri = new Uri(baseUri, $"ClassRegisterStudents({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", classRegisterStudent.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(classRegisterStudent), Encoding.UTF8, "application/json");

            OnUpdateClassRegisterStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportGendersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGendersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGenders(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Gender>> GetGenders(Query query)
        {
            return await GetGenders(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Gender>> GetGenders(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Genders");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGenders(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Gender>>(response);
        }

        partial void OnCreateGender(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> CreateGender(PrimarySchoolCA.Server.Models.ConData.Gender gender = default(PrimarySchoolCA.Server.Models.ConData.Gender))
        {
            var uri = new Uri(baseUri, $"Genders");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");

            OnCreateGender(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Gender>(response);
        }

        partial void OnDeleteGender(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteGender(int genderId = default(int))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteGender(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetGenderByGenderId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> GetGenderByGenderId(string expand = default(string), int genderId = default(int))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGenderByGenderId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Gender>(response);
        }

        partial void OnUpdateGender(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateGender(int genderId = default(int), PrimarySchoolCA.Server.Models.ConData.Gender gender = default(PrimarySchoolCA.Server.Models.ConData.Gender))
        {
            var uri = new Uri(baseUri, $"Genders({genderId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", gender.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gender), Encoding.UTF8, "application/json");

            OnUpdateGender(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportLocalGovtAreasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/localgovtareas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/localgovtareas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLocalGovtAreasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/localgovtareas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/localgovtareas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLocalGovtAreas(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>> GetLocalGovtAreas(Query query)
        {
            return await GetLocalGovtAreas(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>> GetLocalGovtAreas(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"LocalGovtAreas");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLocalGovtAreas(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>>(response);
        }

        partial void OnCreateLocalGovtArea(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> CreateLocalGovtArea(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localGovtArea = default(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea))
        {
            var uri = new Uri(baseUri, $"LocalGovtAreas");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(localGovtArea), Encoding.UTF8, "application/json");

            OnCreateLocalGovtArea(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>(response);
        }

        partial void OnDeleteLocalGovtArea(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteLocalGovtArea(int lgaId = default(int))
        {
            var uri = new Uri(baseUri, $"LocalGovtAreas({lgaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteLocalGovtArea(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetLocalGovtAreaByLgaId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> GetLocalGovtAreaByLgaId(string expand = default(string), int lgaId = default(int))
        {
            var uri = new Uri(baseUri, $"LocalGovtAreas({lgaId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLocalGovtAreaByLgaId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>(response);
        }

        partial void OnUpdateLocalGovtArea(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateLocalGovtArea(int lgaId = default(int), PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localGovtArea = default(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea))
        {
            var uri = new Uri(baseUri, $"LocalGovtAreas({lgaId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", localGovtArea.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(localGovtArea), Encoding.UTF8, "application/json");

            OnUpdateLocalGovtArea(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportParentsOrGuardiansToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/parentsorguardians/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/parentsorguardians/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportParentsOrGuardiansToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/parentsorguardians/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/parentsorguardians/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetParentsOrGuardians(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>> GetParentsOrGuardians(Query query)
        {
            return await GetParentsOrGuardians(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>> GetParentsOrGuardians(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ParentsOrGuardians");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetParentsOrGuardians(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>>(response);
        }

        partial void OnCreateParentsOrGuardian(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> CreateParentsOrGuardian(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsOrGuardian = default(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian))
        {
            var uri = new Uri(baseUri, $"ParentsOrGuardians");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(parentsOrGuardian), Encoding.UTF8, "application/json");

            OnCreateParentsOrGuardian(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>(response);
        }

        partial void OnDeleteParentsOrGuardian(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteParentsOrGuardian(long parentOrGuardianId = default(long))
        {
            var uri = new Uri(baseUri, $"ParentsOrGuardians({parentOrGuardianId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteParentsOrGuardian(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetParentsOrGuardianByParentOrGuardianId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> GetParentsOrGuardianByParentOrGuardianId(string expand = default(string), long parentOrGuardianId = default(long))
        {
            var uri = new Uri(baseUri, $"ParentsOrGuardians({parentOrGuardianId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetParentsOrGuardianByParentOrGuardianId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>(response);
        }

        partial void OnUpdateParentsOrGuardian(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateParentsOrGuardian(long parentOrGuardianId = default(long), PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsOrGuardian = default(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian))
        {
            var uri = new Uri(baseUri, $"ParentsOrGuardians({parentOrGuardianId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", parentsOrGuardian.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(parentsOrGuardian), Encoding.UTF8, "application/json");

            OnUpdateParentsOrGuardian(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSchoolClassesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schoolclasses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schoolclasses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSchoolClassesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schoolclasses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schoolclasses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSchoolClasses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolClass>> GetSchoolClasses(Query query)
        {
            return await GetSchoolClasses(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolClass>> GetSchoolClasses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SchoolClasses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchoolClasses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolClass>>(response);
        }

        partial void OnCreateSchoolClass(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> CreateSchoolClass(PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolClass = default(PrimarySchoolCA.Server.Models.ConData.SchoolClass))
        {
            var uri = new Uri(baseUri, $"SchoolClasses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schoolClass), Encoding.UTF8, "application/json");

            OnCreateSchoolClass(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SchoolClass>(response);
        }

        partial void OnDeleteSchoolClass(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSchoolClass(int schoolClassId = default(int))
        {
            var uri = new Uri(baseUri, $"SchoolClasses({schoolClassId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSchoolClass(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSchoolClassBySchoolClassId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> GetSchoolClassBySchoolClassId(string expand = default(string), int schoolClassId = default(int))
        {
            var uri = new Uri(baseUri, $"SchoolClasses({schoolClassId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchoolClassBySchoolClassId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SchoolClass>(response);
        }

        partial void OnUpdateSchoolClass(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSchoolClass(int schoolClassId = default(int), PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolClass = default(PrimarySchoolCA.Server.Models.ConData.SchoolClass))
        {
            var uri = new Uri(baseUri, $"SchoolClasses({schoolClassId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", schoolClass.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schoolClass), Encoding.UTF8, "application/json");

            OnUpdateSchoolClass(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSchoolsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSchoolsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSchools(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.School>> GetSchools(Query query)
        {
            return await GetSchools(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.School>> GetSchools(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Schools");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchools(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.School>>(response);
        }

        partial void OnCreateSchool(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> CreateSchool(PrimarySchoolCA.Server.Models.ConData.School school = default(PrimarySchoolCA.Server.Models.ConData.School))
        {
            var uri = new Uri(baseUri, $"Schools");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(school), Encoding.UTF8, "application/json");

            OnCreateSchool(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.School>(response);
        }

        partial void OnDeleteSchool(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSchool(long schoolId = default(long))
        {
            var uri = new Uri(baseUri, $"Schools({schoolId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSchool(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSchoolBySchoolId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> GetSchoolBySchoolId(string expand = default(string), long schoolId = default(long))
        {
            var uri = new Uri(baseUri, $"Schools({schoolId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchoolBySchoolId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.School>(response);
        }

        partial void OnUpdateSchool(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSchool(long schoolId = default(long), PrimarySchoolCA.Server.Models.ConData.School school = default(PrimarySchoolCA.Server.Models.ConData.School))
        {
            var uri = new Uri(baseUri, $"Schools({schoolId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", school.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(school), Encoding.UTF8, "application/json");

            OnUpdateSchool(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSchoolTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSchoolTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSchoolTypes(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolType>> GetSchoolTypes(Query query)
        {
            return await GetSchoolTypes(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolType>> GetSchoolTypes(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SchoolTypes");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchoolTypes(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SchoolType>>(response);
        }

        partial void OnCreateSchoolType(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> CreateSchoolType(PrimarySchoolCA.Server.Models.ConData.SchoolType schoolType = default(PrimarySchoolCA.Server.Models.ConData.SchoolType))
        {
            var uri = new Uri(baseUri, $"SchoolTypes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schoolType), Encoding.UTF8, "application/json");

            OnCreateSchoolType(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SchoolType>(response);
        }

        partial void OnDeleteSchoolType(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSchoolType(int schoolTypeId = default(int))
        {
            var uri = new Uri(baseUri, $"SchoolTypes({schoolTypeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSchoolType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSchoolTypeBySchoolTypeId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> GetSchoolTypeBySchoolTypeId(string expand = default(string), int schoolTypeId = default(int))
        {
            var uri = new Uri(baseUri, $"SchoolTypes({schoolTypeId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSchoolTypeBySchoolTypeId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SchoolType>(response);
        }

        partial void OnUpdateSchoolType(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSchoolType(int schoolTypeId = default(int), PrimarySchoolCA.Server.Models.ConData.SchoolType schoolType = default(PrimarySchoolCA.Server.Models.ConData.SchoolType))
        {
            var uri = new Uri(baseUri, $"SchoolTypes({schoolTypeId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", schoolType.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(schoolType), Encoding.UTF8, "application/json");

            OnUpdateSchoolType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStates(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.State>> GetStates(Query query)
        {
            return await GetStates(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.State>> GetStates(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"States");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStates(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.State>>(response);
        }

        partial void OnCreateState(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> CreateState(PrimarySchoolCA.Server.Models.ConData.State state = default(PrimarySchoolCA.Server.Models.ConData.State))
        {
            var uri = new Uri(baseUri, $"States");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(state), Encoding.UTF8, "application/json");

            OnCreateState(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.State>(response);
        }

        partial void OnDeleteState(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteState(int stateId = default(int))
        {
            var uri = new Uri(baseUri, $"States({stateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteState(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStateByStateId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> GetStateByStateId(string expand = default(string), int stateId = default(int))
        {
            var uri = new Uri(baseUri, $"States({stateId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStateByStateId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.State>(response);
        }

        partial void OnUpdateState(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateState(int stateId = default(int), PrimarySchoolCA.Server.Models.ConData.State state = default(PrimarySchoolCA.Server.Models.ConData.State))
        {
            var uri = new Uri(baseUri, $"States({stateId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", state.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(state), Encoding.UTF8, "application/json");

            OnUpdateState(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudents(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Student>> GetStudents(Query query)
        {
            return await GetStudents(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Student>> GetStudents(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Students");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudents(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Student>>(response);
        }

        partial void OnCreateStudent(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> CreateStudent(PrimarySchoolCA.Server.Models.ConData.Student student = default(PrimarySchoolCA.Server.Models.ConData.Student))
        {
            var uri = new Uri(baseUri, $"Students");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            OnCreateStudent(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Student>(response);
        }

        partial void OnDeleteStudent(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudent(long studentId = default(long))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentByStudentId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> GetStudentByStudentId(string expand = default(string), long studentId = default(long))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentByStudentId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Student>(response);
        }

        partial void OnUpdateStudent(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudent(long studentId = default(long), PrimarySchoolCA.Server.Models.ConData.Student student = default(PrimarySchoolCA.Server.Models.ConData.Student))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", student.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            OnUpdateStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSubjectsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSubjectsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSubjects(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Subject>> GetSubjects(Query query)
        {
            return await GetSubjects(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Subject>> GetSubjects(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Subjects");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjects(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Subject>>(response);
        }

        partial void OnCreateSubject(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> CreateSubject(PrimarySchoolCA.Server.Models.ConData.Subject subject = default(PrimarySchoolCA.Server.Models.ConData.Subject))
        {
            var uri = new Uri(baseUri, $"Subjects");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subject), Encoding.UTF8, "application/json");

            OnCreateSubject(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Subject>(response);
        }

        partial void OnDeleteSubject(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSubject(long subjectId = default(long))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSubject(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSubjectBySubjectId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> GetSubjectBySubjectId(string expand = default(string), long subjectId = default(long))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectBySubjectId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Subject>(response);
        }

        partial void OnUpdateSubject(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSubject(long subjectId = default(long), PrimarySchoolCA.Server.Models.ConData.Subject subject = default(PrimarySchoolCA.Server.Models.ConData.Subject))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", subject.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subject), Encoding.UTF8, "application/json");

            OnUpdateSubject(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSubjectSchoolTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjectschooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjectschooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSubjectSchoolTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjectschooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjectschooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSubjectSchoolTypes(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>> GetSubjectSchoolTypes(Query query)
        {
            return await GetSubjectSchoolTypes(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>> GetSubjectSchoolTypes(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SubjectSchoolTypes");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectSchoolTypes(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>>(response);
        }

        partial void OnCreateSubjectSchoolType(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> CreateSubjectSchoolType(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectSchoolType = default(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType))
        {
            var uri = new Uri(baseUri, $"SubjectSchoolTypes");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subjectSchoolType), Encoding.UTF8, "application/json");

            OnCreateSubjectSchoolType(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>(response);
        }

        partial void OnDeleteSubjectSchoolType(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSubjectSchoolType(long id = default(long))
        {
            var uri = new Uri(baseUri, $"SubjectSchoolTypes({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSubjectSchoolType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSubjectSchoolTypeById(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> GetSubjectSchoolTypeById(string expand = default(string), long id = default(long))
        {
            var uri = new Uri(baseUri, $"SubjectSchoolTypes({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectSchoolTypeById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>(response);
        }

        partial void OnUpdateSubjectSchoolType(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSubjectSchoolType(long id = default(long), PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectSchoolType = default(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType))
        {
            var uri = new Uri(baseUri, $"SubjectSchoolTypes({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", subjectSchoolType.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subjectSchoolType), Encoding.UTF8, "application/json");

            OnUpdateSubjectSchoolType(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTermsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/terms/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/terms/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTermsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/terms/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/terms/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTerms(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Term>> GetTerms(Query query)
        {
            return await GetTerms(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Term>> GetTerms(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Terms");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTerms(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.Term>>(response);
        }

        partial void OnCreateTerm(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> CreateTerm(PrimarySchoolCA.Server.Models.ConData.Term term = default(PrimarySchoolCA.Server.Models.ConData.Term))
        {
            var uri = new Uri(baseUri, $"Terms");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(term), Encoding.UTF8, "application/json");

            OnCreateTerm(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Term>(response);
        }

        partial void OnDeleteTerm(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTerm(int termId = default(int))
        {
            var uri = new Uri(baseUri, $"Terms({termId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTerm(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTermByTermId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> GetTermByTermId(string expand = default(string), int termId = default(int))
        {
            var uri = new Uri(baseUri, $"Terms({termId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTermByTermId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.Term>(response);
        }

        partial void OnUpdateTerm(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTerm(int termId = default(int), PrimarySchoolCA.Server.Models.ConData.Term term = default(PrimarySchoolCA.Server.Models.ConData.Term))
        {
            var uri = new Uri(baseUri, $"Terms({termId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", term.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(term), Encoding.UTF8, "application/json");

            OnUpdateTerm(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportContinuousAssessmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/continuousassessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/continuousassessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportContinuousAssessmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/continuousassessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/continuousassessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetContinuousAssessments(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>> GetContinuousAssessments(Query query)
        {
            return await GetContinuousAssessments(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>> GetContinuousAssessments(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ContinuousAssessments");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetContinuousAssessments(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>>(response);
        }

        partial void OnCreateContinuousAssessment(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> CreateContinuousAssessment(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment continuousAssessment = default(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment))
        {
            var uri = new Uri(baseUri, $"ContinuousAssessments");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(continuousAssessment), Encoding.UTF8, "application/json");

            OnCreateContinuousAssessment(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>(response);
        }

        partial void OnDeleteContinuousAssessment(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteContinuousAssessment(long recordId = default(long))
        {
            var uri = new Uri(baseUri, $"ContinuousAssessments({recordId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteContinuousAssessment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetContinuousAssessmentByRecordId(HttpRequestMessage requestMessage);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> GetContinuousAssessmentByRecordId(string expand = default(string), long recordId = default(long))
        {
            var uri = new Uri(baseUri, $"ContinuousAssessments({recordId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetContinuousAssessmentByRecordId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>(response);
        }

        partial void OnUpdateContinuousAssessment(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateContinuousAssessment(long recordId = default(long), PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment continuousAssessment = default(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment))
        {
            var uri = new Uri(baseUri, $"ContinuousAssessments({recordId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", continuousAssessment.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(continuousAssessment), Encoding.UTF8, "application/json");

            OnUpdateContinuousAssessment(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentListForParentGuardianDropdownsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/studentlistforparentguardiandropdowns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/studentlistforparentguardiandropdowns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentListForParentGuardianDropdownsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/studentlistforparentguardiandropdowns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/studentlistforparentguardiandropdowns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudentListForParentGuardianDropdowns(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown>> GetStudentListForParentGuardianDropdowns(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"StudentListForParentGuardianDropdownsFunc()");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentListForParentGuardianDropdowns(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown>>(response);
        }

        partial void OnInsertSingleCaRecords(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> InsertSingleCaRecords(long? studentId = default(long?), int? academicSessionId = default(int?), int? termId = default(int?), int? schoolClassId = default(int?), long? subjectId = default(long?), int? camarkObtainable = default(int?), int? camarkObtained = default(int?), string entryDate = default(string), string insertedBy = default(string))
        {
            var uri = new Uri(baseUri, $"InsertSingleCaRecordsFunc(StudentID={studentId},AcademicSessionID={academicSessionId},TermID={termId},SchoolClassID={schoolClassId},SubjectID={subjectId},CAMarkObtainable={camarkObtainable},CAMarkObtained={camarkObtained},EntryDate='{Uri.EscapeDataString(entryDate.Trim().Replace("'", "''"))}',InsertedBy='{Uri.EscapeDataString(insertedBy.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnInsertSingleCaRecords(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}