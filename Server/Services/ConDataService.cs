using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using PrimarySchoolCA.Server.Data;

namespace PrimarySchoolCA.Server
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAcademicSessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/academicsessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/academicsessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAcademicSessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/academicsessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/academicsessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAcademicSessionsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AcademicSession>> GetAcademicSessions(Query query = null)
        {
            var items = Context.AcademicSessions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAcademicSessionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAcademicSessionGet(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnGetAcademicSessionByAcademicSessionId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AcademicSession> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> GetAcademicSessionByAcademicSessionId(int academicsessionid)
        {
            var items = Context.AcademicSessions
                              .AsNoTracking()
                              .Where(i => i.AcademicSessionID == academicsessionid);

 
            OnGetAcademicSessionByAcademicSessionId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAcademicSessionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAcademicSessionCreated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionCreated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> CreateAcademicSession(PrimarySchoolCA.Server.Models.ConData.AcademicSession academicsession)
        {
            OnAcademicSessionCreated(academicsession);

            var existingItem = Context.AcademicSessions
                              .Where(i => i.AcademicSessionID == academicsession.AcademicSessionID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AcademicSessions.Add(academicsession);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(academicsession).State = EntityState.Detached;
                throw;
            }

            OnAfterAcademicSessionCreated(academicsession);

            return academicsession;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> CancelAcademicSessionChanges(PrimarySchoolCA.Server.Models.ConData.AcademicSession item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAcademicSessionUpdated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionUpdated(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> UpdateAcademicSession(int academicsessionid, PrimarySchoolCA.Server.Models.ConData.AcademicSession academicsession)
        {
            OnAcademicSessionUpdated(academicsession);

            var itemToUpdate = Context.AcademicSessions
                              .Where(i => i.AcademicSessionID == academicsession.AcademicSessionID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(academicsession);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAcademicSessionUpdated(academicsession);

            return academicsession;
        }

        partial void OnAcademicSessionDeleted(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);
        partial void OnAfterAcademicSessionDeleted(PrimarySchoolCA.Server.Models.ConData.AcademicSession item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AcademicSession> DeleteAcademicSession(int academicsessionid)
        {
            var itemToDelete = Context.AcademicSessions
                              .Where(i => i.AcademicSessionID == academicsessionid)
                              .Include(i => i.AssessmentSetups)
                              .Include(i => i.Attendances)
                              .Include(i => i.ClassRegisters)
                              .Include(i => i.ContinuousAssessments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAcademicSessionDeleted(itemToDelete);


            Context.AcademicSessions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAcademicSessionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetRoleClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroleclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetRoleClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroleclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetRoleClaimsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>> GetAspNetRoleClaims(Query query = null)
        {
            var items = Context.AspNetRoleClaims.AsQueryable();

            items = items.Include(i => i.AspNetRole);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetRoleClaimsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetRoleClaimGet(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnGetAspNetRoleClaimById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> GetAspNetRoleClaimById(int id)
        {
            var items = Context.AspNetRoleClaims
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AspNetRole);
 
            OnGetAspNetRoleClaimById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetRoleClaimGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetRoleClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> CreateAspNetRoleClaim(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim aspnetroleclaim)
        {
            OnAspNetRoleClaimCreated(aspnetroleclaim);

            var existingItem = Context.AspNetRoleClaims
                              .Where(i => i.Id == aspnetroleclaim.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetRoleClaims.Add(aspnetroleclaim);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetroleclaim).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetRoleClaimCreated(aspnetroleclaim);

            return aspnetroleclaim;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> CancelAspNetRoleClaimChanges(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetRoleClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> UpdateAspNetRoleClaim(int id, PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim aspnetroleclaim)
        {
            OnAspNetRoleClaimUpdated(aspnetroleclaim);

            var itemToUpdate = Context.AspNetRoleClaims
                              .Where(i => i.Id == aspnetroleclaim.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetroleclaim);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetRoleClaimUpdated(aspnetroleclaim);

            return aspnetroleclaim;
        }

        partial void OnAspNetRoleClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);
        partial void OnAfterAspNetRoleClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim> DeleteAspNetRoleClaim(int id)
        {
            var itemToDelete = Context.AspNetRoleClaims
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetRoleClaimDeleted(itemToDelete);


            Context.AspNetRoleClaims.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetRoleClaimDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetRolesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRole> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRole>> GetAspNetRoles(Query query = null)
        {
            var items = Context.AspNetRoles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetRoleGet(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnGetAspNetRoleById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetRole> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> GetAspNetRoleById(string id)
        {
            var items = Context.AspNetRoles
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAspNetRoleById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> CreateAspNetRole(PrimarySchoolCA.Server.Models.ConData.AspNetRole aspnetrole)
        {
            OnAspNetRoleCreated(aspnetrole);

            var existingItem = Context.AspNetRoles
                              .Where(i => i.Id == aspnetrole.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetRoles.Add(aspnetrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetrole).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetRoleCreated(aspnetrole);

            return aspnetrole;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> CancelAspNetRoleChanges(PrimarySchoolCA.Server.Models.ConData.AspNetRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> UpdateAspNetRole(string id, PrimarySchoolCA.Server.Models.ConData.AspNetRole aspnetrole)
        {
            OnAspNetRoleUpdated(aspnetrole);

            var itemToUpdate = Context.AspNetRoles
                              .Where(i => i.Id == aspnetrole.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetRoleUpdated(aspnetrole);

            return aspnetrole;
        }

        partial void OnAspNetRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);
        partial void OnAfterAspNetRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetRole> DeleteAspNetRole(string id)
        {
            var itemToDelete = Context.AspNetRoles
                              .Where(i => i.Id == id)
                              .Include(i => i.AspNetRoleClaims)
                              .Include(i => i.AspNetUserRoles)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetRoleDeleted(itemToDelete);


            Context.AspNetRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserClaimsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserclaims/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserClaimsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserclaims/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserClaimsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>> GetAspNetUserClaims(Query query = null)
        {
            var items = Context.AspNetUserClaims.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserClaimsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserClaimGet(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);
        partial void OnGetAspNetUserClaimById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> GetAspNetUserClaimById(int id)
        {
            var items = Context.AspNetUserClaims
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserClaimById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserClaimGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> CreateAspNetUserClaim(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim aspnetuserclaim)
        {
            OnAspNetUserClaimCreated(aspnetuserclaim);

            var existingItem = Context.AspNetUserClaims
                              .Where(i => i.Id == aspnetuserclaim.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserClaims.Add(aspnetuserclaim);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserclaim).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserClaimCreated(aspnetuserclaim);

            return aspnetuserclaim;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> CancelAspNetUserClaimChanges(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> UpdateAspNetUserClaim(int id, PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim aspnetuserclaim)
        {
            OnAspNetUserClaimUpdated(aspnetuserclaim);

            var itemToUpdate = Context.AspNetUserClaims
                              .Where(i => i.Id == aspnetuserclaim.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserclaim);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserClaimUpdated(aspnetuserclaim);

            return aspnetuserclaim;
        }

        partial void OnAspNetUserClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);
        partial void OnAfterAspNetUserClaimDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim> DeleteAspNetUserClaim(int id)
        {
            var itemToDelete = Context.AspNetUserClaims
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserClaimDeleted(itemToDelete);


            Context.AspNetUserClaims.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserClaimDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserLoginsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserlogins/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserLoginsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserlogins/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserLoginsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>> GetAspNetUserLogins(Query query = null)
        {
            var items = Context.AspNetUserLogins.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserLoginsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserLoginGet(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnGetAspNetUserLoginByLoginProviderAndProviderKey(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> GetAspNetUserLoginByLoginProviderAndProviderKey(string loginprovider, string providerkey)
        {
            var items = Context.AspNetUserLogins
                              .AsNoTracking()
                              .Where(i => i.LoginProvider == loginprovider && i.ProviderKey == providerkey);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserLoginByLoginProviderAndProviderKey(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserLoginGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserLoginCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> CreateAspNetUserLogin(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin aspnetuserlogin)
        {
            OnAspNetUserLoginCreated(aspnetuserlogin);

            var existingItem = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == aspnetuserlogin.LoginProvider && i.ProviderKey == aspnetuserlogin.ProviderKey)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserLogins.Add(aspnetuserlogin);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserlogin).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserLoginCreated(aspnetuserlogin);

            return aspnetuserlogin;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> CancelAspNetUserLoginChanges(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserLoginUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> UpdateAspNetUserLogin(string loginprovider, string providerkey, PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin aspnetuserlogin)
        {
            OnAspNetUserLoginUpdated(aspnetuserlogin);

            var itemToUpdate = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == aspnetuserlogin.LoginProvider && i.ProviderKey == aspnetuserlogin.ProviderKey)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserlogin);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserLoginUpdated(aspnetuserlogin);

            return aspnetuserlogin;
        }

        partial void OnAspNetUserLoginDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);
        partial void OnAfterAspNetUserLoginDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin> DeleteAspNetUserLogin(string loginprovider, string providerkey)
        {
            var itemToDelete = Context.AspNetUserLogins
                              .Where(i => i.LoginProvider == loginprovider && i.ProviderKey == providerkey)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserLoginDeleted(itemToDelete);


            Context.AspNetUserLogins.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserLoginDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserroles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetuserroles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserRolesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>> GetAspNetUserRoles(Query query = null)
        {
            var items = Context.AspNetUserRoles.AsQueryable();

            items = items.Include(i => i.AspNetRole);
            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserRoleGet(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnGetAspNetUserRoleByUserIdAndRoleId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> GetAspNetUserRoleByUserIdAndRoleId(string userid, string roleid)
        {
            var items = Context.AspNetUserRoles
                              .AsNoTracking()
                              .Where(i => i.UserId == userid && i.RoleId == roleid);

            items = items.Include(i => i.AspNetRole);
            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserRoleByUserIdAndRoleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> CreateAspNetUserRole(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole aspnetuserrole)
        {
            OnAspNetUserRoleCreated(aspnetuserrole);

            var existingItem = Context.AspNetUserRoles
                              .Where(i => i.UserId == aspnetuserrole.UserId && i.RoleId == aspnetuserrole.RoleId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserRoles.Add(aspnetuserrole);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuserrole).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserRoleCreated(aspnetuserrole);

            return aspnetuserrole;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> CancelAspNetUserRoleChanges(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> UpdateAspNetUserRole(string userid, string roleid, PrimarySchoolCA.Server.Models.ConData.AspNetUserRole aspnetuserrole)
        {
            OnAspNetUserRoleUpdated(aspnetuserrole);

            var itemToUpdate = Context.AspNetUserRoles
                              .Where(i => i.UserId == aspnetuserrole.UserId && i.RoleId == aspnetuserrole.RoleId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuserrole);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserRoleUpdated(aspnetuserrole);

            return aspnetuserrole;
        }

        partial void OnAspNetUserRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);
        partial void OnAfterAspNetUserRoleDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserRole item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole> DeleteAspNetUserRole(string userid, string roleid)
        {
            var itemToDelete = Context.AspNetUserRoles
                              .Where(i => i.UserId == userid && i.RoleId == roleid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserRoleDeleted(itemToDelete);


            Context.AspNetUserRoles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUsersRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUser> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUser>> GetAspNetUsers(Query query = null)
        {
            var items = Context.AspNetUsers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserGet(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);
        partial void OnGetAspNetUserById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUser> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> GetAspNetUserById(string id)
        {
            var items = Context.AspNetUsers
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAspNetUserById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);
        partial void OnAfterAspNetUserCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> CreateAspNetUser(PrimarySchoolCA.Server.Models.ConData.AspNetUser aspnetuser)
        {
            OnAspNetUserCreated(aspnetuser);

            var existingItem = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUsers.Add(aspnetuser);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetuser).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserCreated(aspnetuser);

            return aspnetuser;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> CancelAspNetUserChanges(PrimarySchoolCA.Server.Models.ConData.AspNetUser item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);
        partial void OnAfterAspNetUserUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> UpdateAspNetUser(string id, PrimarySchoolCA.Server.Models.ConData.AspNetUser aspnetuser)
        {
            OnAspNetUserUpdated(aspnetuser);

            var itemToUpdate = Context.AspNetUsers
                              .Where(i => i.Id == aspnetuser.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetuser);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserUpdated(aspnetuser);

            return aspnetuser;
        }

        partial void OnAspNetUserDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);
        partial void OnAfterAspNetUserDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUser item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUser> DeleteAspNetUser(string id)
        {
            var itemToDelete = Context.AspNetUsers
                              .Where(i => i.Id == id)
                              .Include(i => i.AspNetUserClaims)
                              .Include(i => i.AspNetUserLogins)
                              .Include(i => i.AspNetUserRoles)
                              .Include(i => i.AspNetUserTokens)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserDeleted(itemToDelete);


            Context.AspNetUsers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAspNetUserTokensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusertokens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAspNetUserTokensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/aspnetusertokens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAspNetUserTokensRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>> GetAspNetUserTokens(Query query = null)
        {
            var items = Context.AspNetUserTokens.AsQueryable();

            items = items.Include(i => i.AspNetUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAspNetUserTokensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAspNetUserTokenGet(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> GetAspNetUserTokenByUserIdAndLoginProviderAndName(string userid, string loginprovider, string name)
        {
            var items = Context.AspNetUserTokens
                              .AsNoTracking()
                              .Where(i => i.UserId == userid && i.LoginProvider == loginprovider && i.Name == name);

            items = items.Include(i => i.AspNetUser);
 
            OnGetAspNetUserTokenByUserIdAndLoginProviderAndName(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAspNetUserTokenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAspNetUserTokenCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenCreated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> CreateAspNetUserToken(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken aspnetusertoken)
        {
            OnAspNetUserTokenCreated(aspnetusertoken);

            var existingItem = Context.AspNetUserTokens
                              .Where(i => i.UserId == aspnetusertoken.UserId && i.LoginProvider == aspnetusertoken.LoginProvider && i.Name == aspnetusertoken.Name)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AspNetUserTokens.Add(aspnetusertoken);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aspnetusertoken).State = EntityState.Detached;
                throw;
            }

            OnAfterAspNetUserTokenCreated(aspnetusertoken);

            return aspnetusertoken;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> CancelAspNetUserTokenChanges(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAspNetUserTokenUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenUpdated(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> UpdateAspNetUserToken(string userid, string loginprovider, string name, PrimarySchoolCA.Server.Models.ConData.AspNetUserToken aspnetusertoken)
        {
            OnAspNetUserTokenUpdated(aspnetusertoken);

            var itemToUpdate = Context.AspNetUserTokens
                              .Where(i => i.UserId == aspnetusertoken.UserId && i.LoginProvider == aspnetusertoken.LoginProvider && i.Name == aspnetusertoken.Name)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(aspnetusertoken);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAspNetUserTokenUpdated(aspnetusertoken);

            return aspnetusertoken;
        }

        partial void OnAspNetUserTokenDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);
        partial void OnAfterAspNetUserTokenDeleted(PrimarySchoolCA.Server.Models.ConData.AspNetUserToken item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken> DeleteAspNetUserToken(string userid, string loginprovider, string name)
        {
            var itemToDelete = Context.AspNetUserTokens
                              .Where(i => i.UserId == userid && i.LoginProvider == loginprovider && i.Name == name)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAspNetUserTokenDeleted(itemToDelete);


            Context.AspNetUserTokens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAspNetUserTokenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAssessmentSetupsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmentsetups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmentsetups/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAssessmentSetupsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmentsetups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmentsetups/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAssessmentSetupsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>> GetAssessmentSetups(Query query = null)
        {
            var items = Context.AssessmentSetups.AsQueryable();

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.AssessmentType);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Subject);
            items = items.Include(i => i.Term);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAssessmentSetupsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssessmentSetupGet(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnGetAssessmentSetupByAssessmentSetupId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> GetAssessmentSetupByAssessmentSetupId(long assessmentsetupid)
        {
            var items = Context.AssessmentSetups
                              .AsNoTracking()
                              .Where(i => i.AssessmentSetupID == assessmentsetupid);

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.AssessmentType);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Subject);
            items = items.Include(i => i.Term);
 
            OnGetAssessmentSetupByAssessmentSetupId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAssessmentSetupGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAssessmentSetupCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> CreateAssessmentSetup(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup assessmentsetup)
        {
            OnAssessmentSetupCreated(assessmentsetup);

            var existingItem = Context.AssessmentSetups
                              .Where(i => i.AssessmentSetupID == assessmentsetup.AssessmentSetupID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AssessmentSetups.Add(assessmentsetup);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(assessmentsetup).State = EntityState.Detached;
                throw;
            }

            OnAfterAssessmentSetupCreated(assessmentsetup);

            return assessmentsetup;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> CancelAssessmentSetupChanges(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAssessmentSetupUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> UpdateAssessmentSetup(long assessmentsetupid, PrimarySchoolCA.Server.Models.ConData.AssessmentSetup assessmentsetup)
        {
            OnAssessmentSetupUpdated(assessmentsetup);

            var itemToUpdate = Context.AssessmentSetups
                              .Where(i => i.AssessmentSetupID == assessmentsetup.AssessmentSetupID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(assessmentsetup);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAssessmentSetupUpdated(assessmentsetup);

            return assessmentsetup;
        }

        partial void OnAssessmentSetupDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);
        partial void OnAfterAssessmentSetupDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentSetup item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup> DeleteAssessmentSetup(long assessmentsetupid)
        {
            var itemToDelete = Context.AssessmentSetups
                              .Where(i => i.AssessmentSetupID == assessmentsetupid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAssessmentSetupDeleted(itemToDelete);


            Context.AssessmentSetups.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAssessmentSetupDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAssessmentTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmenttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmenttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAssessmentTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/assessmenttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/assessmenttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAssessmentTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentType> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentType>> GetAssessmentTypes(Query query = null)
        {
            var items = Context.AssessmentTypes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAssessmentTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssessmentTypeGet(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnGetAssessmentTypeByAssessmentTypeId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.AssessmentType> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> GetAssessmentTypeByAssessmentTypeId(int assessmenttypeid)
        {
            var items = Context.AssessmentTypes
                              .AsNoTracking()
                              .Where(i => i.AssessmentTypeID == assessmenttypeid);

 
            OnGetAssessmentTypeByAssessmentTypeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAssessmentTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAssessmentTypeCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeCreated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> CreateAssessmentType(PrimarySchoolCA.Server.Models.ConData.AssessmentType assessmenttype)
        {
            OnAssessmentTypeCreated(assessmenttype);

            var existingItem = Context.AssessmentTypes
                              .Where(i => i.AssessmentTypeID == assessmenttype.AssessmentTypeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AssessmentTypes.Add(assessmenttype);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(assessmenttype).State = EntityState.Detached;
                throw;
            }

            OnAfterAssessmentTypeCreated(assessmenttype);

            return assessmenttype;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> CancelAssessmentTypeChanges(PrimarySchoolCA.Server.Models.ConData.AssessmentType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAssessmentTypeUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeUpdated(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> UpdateAssessmentType(int assessmenttypeid, PrimarySchoolCA.Server.Models.ConData.AssessmentType assessmenttype)
        {
            OnAssessmentTypeUpdated(assessmenttype);

            var itemToUpdate = Context.AssessmentTypes
                              .Where(i => i.AssessmentTypeID == assessmenttype.AssessmentTypeID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(assessmenttype);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAssessmentTypeUpdated(assessmenttype);

            return assessmenttype;
        }

        partial void OnAssessmentTypeDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);
        partial void OnAfterAssessmentTypeDeleted(PrimarySchoolCA.Server.Models.ConData.AssessmentType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.AssessmentType> DeleteAssessmentType(int assessmenttypeid)
        {
            var itemToDelete = Context.AssessmentTypes
                              .Where(i => i.AssessmentTypeID == assessmenttypeid)
                              .Include(i => i.AssessmentSetups)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAssessmentTypeDeleted(itemToDelete);


            Context.AssessmentTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAssessmentTypeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAttendancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/attendances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/attendances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAttendancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/attendances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/attendances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAttendancesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Attendance> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.Attendance>> GetAttendances(Query query = null)
        {
            var items = Context.Attendances.AsQueryable();

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Student);
            items = items.Include(i => i.Term);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAttendancesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAttendanceGet(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnGetAttendanceByAttendanceId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Attendance> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> GetAttendanceByAttendanceId(long attendanceid)
        {
            var items = Context.Attendances
                              .AsNoTracking()
                              .Where(i => i.AttendanceID == attendanceid);

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Student);
            items = items.Include(i => i.Term);
 
            OnGetAttendanceByAttendanceId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAttendanceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAttendanceCreated(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceCreated(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> CreateAttendance(PrimarySchoolCA.Server.Models.ConData.Attendance attendance)
        {
            OnAttendanceCreated(attendance);

            var existingItem = Context.Attendances
                              .Where(i => i.AttendanceID == attendance.AttendanceID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Attendances.Add(attendance);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(attendance).State = EntityState.Detached;
                throw;
            }

            OnAfterAttendanceCreated(attendance);

            return attendance;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> CancelAttendanceChanges(PrimarySchoolCA.Server.Models.ConData.Attendance item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAttendanceUpdated(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceUpdated(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> UpdateAttendance(long attendanceid, PrimarySchoolCA.Server.Models.ConData.Attendance attendance)
        {
            OnAttendanceUpdated(attendance);

            var itemToUpdate = Context.Attendances
                              .Where(i => i.AttendanceID == attendance.AttendanceID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(attendance);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAttendanceUpdated(attendance);

            return attendance;
        }

        partial void OnAttendanceDeleted(PrimarySchoolCA.Server.Models.ConData.Attendance item);
        partial void OnAfterAttendanceDeleted(PrimarySchoolCA.Server.Models.ConData.Attendance item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Attendance> DeleteAttendance(long attendanceid)
        {
            var itemToDelete = Context.Attendances
                              .Where(i => i.AttendanceID == attendanceid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAttendanceDeleted(itemToDelete);


            Context.Attendances.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAttendanceDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportClassRegistersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisters/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClassRegistersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisters/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClassRegistersRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegister>> GetClassRegisters(Query query = null)
        {
            var items = Context.ClassRegisters.AsQueryable();

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Term);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnClassRegistersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClassRegisterGet(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnGetClassRegisterByClassRegisterId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegister> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> GetClassRegisterByClassRegisterId(long classregisterid)
        {
            var items = Context.ClassRegisters
                              .AsNoTracking()
                              .Where(i => i.ClassRegisterID == classregisterid);

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Term);
 
            OnGetClassRegisterByClassRegisterId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnClassRegisterGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClassRegisterCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> CreateClassRegister(PrimarySchoolCA.Server.Models.ConData.ClassRegister classregister)
        {
            OnClassRegisterCreated(classregister);

            var existingItem = Context.ClassRegisters
                              .Where(i => i.ClassRegisterID == classregister.ClassRegisterID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ClassRegisters.Add(classregister);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(classregister).State = EntityState.Detached;
                throw;
            }

            OnAfterClassRegisterCreated(classregister);

            return classregister;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> CancelClassRegisterChanges(PrimarySchoolCA.Server.Models.ConData.ClassRegister item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClassRegisterUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> UpdateClassRegister(long classregisterid, PrimarySchoolCA.Server.Models.ConData.ClassRegister classregister)
        {
            OnClassRegisterUpdated(classregister);

            var itemToUpdate = Context.ClassRegisters
                              .Where(i => i.ClassRegisterID == classregister.ClassRegisterID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(classregister);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClassRegisterUpdated(classregister);

            return classregister;
        }

        partial void OnClassRegisterDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);
        partial void OnAfterClassRegisterDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegister item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegister> DeleteClassRegister(long classregisterid)
        {
            var itemToDelete = Context.ClassRegisters
                              .Where(i => i.ClassRegisterID == classregisterid)
                              .Include(i => i.ClassRegisterStudents)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClassRegisterDeleted(itemToDelete);


            Context.ClassRegisters.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClassRegisterDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportClassRegisterStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisterstudents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisterstudents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClassRegisterStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/classregisterstudents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/classregisterstudents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClassRegisterStudentsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>> GetClassRegisterStudents(Query query = null)
        {
            var items = Context.ClassRegisterStudents.AsQueryable();

            items = items.Include(i => i.ClassRegister);
            items = items.Include(i => i.Student);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnClassRegisterStudentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClassRegisterStudentGet(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnGetClassRegisterStudentById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> GetClassRegisterStudentById(long id)
        {
            var items = Context.ClassRegisterStudents
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.ClassRegister);
            items = items.Include(i => i.Student);
 
            OnGetClassRegisterStudentById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnClassRegisterStudentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClassRegisterStudentCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentCreated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> CreateClassRegisterStudent(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classregisterstudent)
        {
            OnClassRegisterStudentCreated(classregisterstudent);

            var existingItem = Context.ClassRegisterStudents
                              .Where(i => i.ID == classregisterstudent.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ClassRegisterStudents.Add(classregisterstudent);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(classregisterstudent).State = EntityState.Detached;
                throw;
            }

            OnAfterClassRegisterStudentCreated(classregisterstudent);

            return classregisterstudent;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> CancelClassRegisterStudentChanges(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClassRegisterStudentUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentUpdated(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> UpdateClassRegisterStudent(long id, PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent classregisterstudent)
        {
            OnClassRegisterStudentUpdated(classregisterstudent);

            var itemToUpdate = Context.ClassRegisterStudents
                              .Where(i => i.ID == classregisterstudent.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(classregisterstudent);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClassRegisterStudentUpdated(classregisterstudent);

            return classregisterstudent;
        }

        partial void OnClassRegisterStudentDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);
        partial void OnAfterClassRegisterStudentDeleted(PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent> DeleteClassRegisterStudent(long id)
        {
            var itemToDelete = Context.ClassRegisterStudents
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClassRegisterStudentDeleted(itemToDelete);


            Context.ClassRegisterStudents.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClassRegisterStudentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportGendersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/genders/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGendersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/genders/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGendersRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Gender> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.Gender>> GetGenders(Query query = null)
        {
            var items = Context.Genders.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnGendersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGenderGet(PrimarySchoolCA.Server.Models.ConData.Gender item);
        partial void OnGetGenderByGenderId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Gender> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> GetGenderByGenderId(int genderid)
        {
            var items = Context.Genders
                              .AsNoTracking()
                              .Where(i => i.GenderID == genderid);

 
            OnGetGenderByGenderId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnGenderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGenderCreated(PrimarySchoolCA.Server.Models.ConData.Gender item);
        partial void OnAfterGenderCreated(PrimarySchoolCA.Server.Models.ConData.Gender item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> CreateGender(PrimarySchoolCA.Server.Models.ConData.Gender gender)
        {
            OnGenderCreated(gender);

            var existingItem = Context.Genders
                              .Where(i => i.GenderID == gender.GenderID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Genders.Add(gender);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gender).State = EntityState.Detached;
                throw;
            }

            OnAfterGenderCreated(gender);

            return gender;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> CancelGenderChanges(PrimarySchoolCA.Server.Models.ConData.Gender item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGenderUpdated(PrimarySchoolCA.Server.Models.ConData.Gender item);
        partial void OnAfterGenderUpdated(PrimarySchoolCA.Server.Models.ConData.Gender item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> UpdateGender(int genderid, PrimarySchoolCA.Server.Models.ConData.Gender gender)
        {
            OnGenderUpdated(gender);

            var itemToUpdate = Context.Genders
                              .Where(i => i.GenderID == gender.GenderID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gender);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGenderUpdated(gender);

            return gender;
        }

        partial void OnGenderDeleted(PrimarySchoolCA.Server.Models.ConData.Gender item);
        partial void OnAfterGenderDeleted(PrimarySchoolCA.Server.Models.ConData.Gender item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Gender> DeleteGender(int genderid)
        {
            var itemToDelete = Context.Genders
                              .Where(i => i.GenderID == genderid)
                              .Include(i => i.ParentsOrGuardians)
                              .Include(i => i.Students)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGenderDeleted(itemToDelete);


            Context.Genders.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGenderDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLocalGovtAreasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/localgovtareas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/localgovtareas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLocalGovtAreasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/localgovtareas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/localgovtareas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLocalGovtAreasRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>> GetLocalGovtAreas(Query query = null)
        {
            var items = Context.LocalGovtAreas.AsQueryable();

            items = items.Include(i => i.State);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLocalGovtAreasRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLocalGovtAreaGet(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnGetLocalGovtAreaByLgaId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> GetLocalGovtAreaByLgaId(int lgaid)
        {
            var items = Context.LocalGovtAreas
                              .AsNoTracking()
                              .Where(i => i.LgaID == lgaid);

            items = items.Include(i => i.State);
 
            OnGetLocalGovtAreaByLgaId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLocalGovtAreaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLocalGovtAreaCreated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaCreated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> CreateLocalGovtArea(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localgovtarea)
        {
            OnLocalGovtAreaCreated(localgovtarea);

            var existingItem = Context.LocalGovtAreas
                              .Where(i => i.LgaID == localgovtarea.LgaID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LocalGovtAreas.Add(localgovtarea);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(localgovtarea).State = EntityState.Detached;
                throw;
            }

            OnAfterLocalGovtAreaCreated(localgovtarea);

            return localgovtarea;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> CancelLocalGovtAreaChanges(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLocalGovtAreaUpdated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaUpdated(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> UpdateLocalGovtArea(int lgaid, PrimarySchoolCA.Server.Models.ConData.LocalGovtArea localgovtarea)
        {
            OnLocalGovtAreaUpdated(localgovtarea);

            var itemToUpdate = Context.LocalGovtAreas
                              .Where(i => i.LgaID == localgovtarea.LgaID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(localgovtarea);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLocalGovtAreaUpdated(localgovtarea);

            return localgovtarea;
        }

        partial void OnLocalGovtAreaDeleted(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);
        partial void OnAfterLocalGovtAreaDeleted(PrimarySchoolCA.Server.Models.ConData.LocalGovtArea item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea> DeleteLocalGovtArea(int lgaid)
        {
            var itemToDelete = Context.LocalGovtAreas
                              .Where(i => i.LgaID == lgaid)
                              .Include(i => i.ParentsOrGuardians)
                              .Include(i => i.Schools)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLocalGovtAreaDeleted(itemToDelete);


            Context.LocalGovtAreas.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLocalGovtAreaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportParentsOrGuardiansToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/parentsorguardians/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/parentsorguardians/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportParentsOrGuardiansToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/parentsorguardians/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/parentsorguardians/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnParentsOrGuardiansRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>> GetParentsOrGuardians(Query query = null)
        {
            var items = Context.ParentsOrGuardians.AsQueryable();

            items = items.Include(i => i.Gender);
            items = items.Include(i => i.LocalGovtArea);
            items = items.Include(i => i.State);
            items = items.Include(i => i.Student);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnParentsOrGuardiansRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnParentsOrGuardianGet(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnGetParentsOrGuardianByParentOrGuardianId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> GetParentsOrGuardianByParentOrGuardianId(long parentorguardianid)
        {
            var items = Context.ParentsOrGuardians
                              .AsNoTracking()
                              .Where(i => i.ParentOrGuardianID == parentorguardianid);

            items = items.Include(i => i.Gender);
            items = items.Include(i => i.LocalGovtArea);
            items = items.Include(i => i.State);
            items = items.Include(i => i.Student);
 
            OnGetParentsOrGuardianByParentOrGuardianId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnParentsOrGuardianGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnParentsOrGuardianCreated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianCreated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> CreateParentsOrGuardian(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsorguardian)
        {
            OnParentsOrGuardianCreated(parentsorguardian);

            var existingItem = Context.ParentsOrGuardians
                              .Where(i => i.ParentOrGuardianID == parentsorguardian.ParentOrGuardianID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ParentsOrGuardians.Add(parentsorguardian);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(parentsorguardian).State = EntityState.Detached;
                throw;
            }

            OnAfterParentsOrGuardianCreated(parentsorguardian);

            return parentsorguardian;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> CancelParentsOrGuardianChanges(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnParentsOrGuardianUpdated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianUpdated(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> UpdateParentsOrGuardian(long parentorguardianid, PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian parentsorguardian)
        {
            OnParentsOrGuardianUpdated(parentsorguardian);

            var itemToUpdate = Context.ParentsOrGuardians
                              .Where(i => i.ParentOrGuardianID == parentsorguardian.ParentOrGuardianID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(parentsorguardian);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterParentsOrGuardianUpdated(parentsorguardian);

            return parentsorguardian;
        }

        partial void OnParentsOrGuardianDeleted(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);
        partial void OnAfterParentsOrGuardianDeleted(PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian> DeleteParentsOrGuardian(long parentorguardianid)
        {
            var itemToDelete = Context.ParentsOrGuardians
                              .Where(i => i.ParentOrGuardianID == parentorguardianid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnParentsOrGuardianDeleted(itemToDelete);


            Context.ParentsOrGuardians.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterParentsOrGuardianDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchoolClassesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schoolclasses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schoolclasses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchoolClassesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schoolclasses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schoolclasses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchoolClassesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolClass>> GetSchoolClasses(Query query = null)
        {
            var items = Context.SchoolClasses.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchoolClassesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSchoolClassGet(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnGetSchoolClassBySchoolClassId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolClass> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> GetSchoolClassBySchoolClassId(int schoolclassid)
        {
            var items = Context.SchoolClasses
                              .AsNoTracking()
                              .Where(i => i.SchoolClassID == schoolclassid);

 
            OnGetSchoolClassBySchoolClassId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSchoolClassGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSchoolClassCreated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassCreated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> CreateSchoolClass(PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolclass)
        {
            OnSchoolClassCreated(schoolclass);

            var existingItem = Context.SchoolClasses
                              .Where(i => i.SchoolClassID == schoolclass.SchoolClassID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SchoolClasses.Add(schoolclass);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(schoolclass).State = EntityState.Detached;
                throw;
            }

            OnAfterSchoolClassCreated(schoolclass);

            return schoolclass;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> CancelSchoolClassChanges(PrimarySchoolCA.Server.Models.ConData.SchoolClass item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSchoolClassUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> UpdateSchoolClass(int schoolclassid, PrimarySchoolCA.Server.Models.ConData.SchoolClass schoolclass)
        {
            OnSchoolClassUpdated(schoolclass);

            var itemToUpdate = Context.SchoolClasses
                              .Where(i => i.SchoolClassID == schoolclass.SchoolClassID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(schoolclass);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSchoolClassUpdated(schoolclass);

            return schoolclass;
        }

        partial void OnSchoolClassDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);
        partial void OnAfterSchoolClassDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolClass item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolClass> DeleteSchoolClass(int schoolclassid)
        {
            var itemToDelete = Context.SchoolClasses
                              .Where(i => i.SchoolClassID == schoolclassid)
                              .Include(i => i.AssessmentSetups)
                              .Include(i => i.Attendances)
                              .Include(i => i.ClassRegisters)
                              .Include(i => i.ContinuousAssessments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSchoolClassDeleted(itemToDelete);


            Context.SchoolClasses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSchoolClassDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchoolsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchoolsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchoolsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.School> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.School>> GetSchools(Query query = null)
        {
            var items = Context.Schools.AsQueryable();

            items = items.Include(i => i.LocalGovtArea);
            items = items.Include(i => i.State);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchoolsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSchoolGet(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnGetSchoolBySchoolId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.School> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.School> GetSchoolBySchoolId(long schoolid)
        {
            var items = Context.Schools
                              .AsNoTracking()
                              .Where(i => i.SchoolID == schoolid);

            items = items.Include(i => i.LocalGovtArea);
            items = items.Include(i => i.State);
 
            OnGetSchoolBySchoolId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSchoolGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSchoolCreated(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolCreated(PrimarySchoolCA.Server.Models.ConData.School item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> CreateSchool(PrimarySchoolCA.Server.Models.ConData.School school)
        {
            OnSchoolCreated(school);

            var existingItem = Context.Schools
                              .Where(i => i.SchoolID == school.SchoolID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Schools.Add(school);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(school).State = EntityState.Detached;
                throw;
            }

            OnAfterSchoolCreated(school);

            return school;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> CancelSchoolChanges(PrimarySchoolCA.Server.Models.ConData.School item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSchoolUpdated(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolUpdated(PrimarySchoolCA.Server.Models.ConData.School item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> UpdateSchool(long schoolid, PrimarySchoolCA.Server.Models.ConData.School school)
        {
            OnSchoolUpdated(school);

            var itemToUpdate = Context.Schools
                              .Where(i => i.SchoolID == school.SchoolID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(school);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSchoolUpdated(school);

            return school;
        }

        partial void OnSchoolDeleted(PrimarySchoolCA.Server.Models.ConData.School item);
        partial void OnAfterSchoolDeleted(PrimarySchoolCA.Server.Models.ConData.School item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.School> DeleteSchool(long schoolid)
        {
            var itemToDelete = Context.Schools
                              .Where(i => i.SchoolID == schoolid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSchoolDeleted(itemToDelete);


            Context.Schools.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSchoolDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchoolTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchoolTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/schooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/schooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchoolTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolType> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolType>> GetSchoolTypes(Query query = null)
        {
            var items = Context.SchoolTypes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchoolTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSchoolTypeGet(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnGetSchoolTypeBySchoolTypeId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SchoolType> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> GetSchoolTypeBySchoolTypeId(int schooltypeid)
        {
            var items = Context.SchoolTypes
                              .AsNoTracking()
                              .Where(i => i.SchoolTypeID == schooltypeid);

 
            OnGetSchoolTypeBySchoolTypeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSchoolTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> CreateSchoolType(PrimarySchoolCA.Server.Models.ConData.SchoolType schooltype)
        {
            OnSchoolTypeCreated(schooltype);

            var existingItem = Context.SchoolTypes
                              .Where(i => i.SchoolTypeID == schooltype.SchoolTypeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SchoolTypes.Add(schooltype);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(schooltype).State = EntityState.Detached;
                throw;
            }

            OnAfterSchoolTypeCreated(schooltype);

            return schooltype;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> CancelSchoolTypeChanges(PrimarySchoolCA.Server.Models.ConData.SchoolType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> UpdateSchoolType(int schooltypeid, PrimarySchoolCA.Server.Models.ConData.SchoolType schooltype)
        {
            OnSchoolTypeUpdated(schooltype);

            var itemToUpdate = Context.SchoolTypes
                              .Where(i => i.SchoolTypeID == schooltype.SchoolTypeID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(schooltype);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSchoolTypeUpdated(schooltype);

            return schooltype;
        }

        partial void OnSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolType item);
        partial void OnAfterSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SchoolType> DeleteSchoolType(int schooltypeid)
        {
            var itemToDelete = Context.SchoolTypes
                              .Where(i => i.SchoolTypeID == schooltypeid)
                              .Include(i => i.SubjectSchoolTypes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSchoolTypeDeleted(itemToDelete);


            Context.SchoolTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSchoolTypeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/states/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/states/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStatesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.State> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.State>> GetStates(Query query = null)
        {
            var items = Context.States.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStateGet(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnGetStateByStateId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.State> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.State> GetStateByStateId(int stateid)
        {
            var items = Context.States
                              .AsNoTracking()
                              .Where(i => i.StateID == stateid);

 
            OnGetStateByStateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStateCreated(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateCreated(PrimarySchoolCA.Server.Models.ConData.State item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> CreateState(PrimarySchoolCA.Server.Models.ConData.State state)
        {
            OnStateCreated(state);

            var existingItem = Context.States
                              .Where(i => i.StateID == state.StateID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.States.Add(state);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(state).State = EntityState.Detached;
                throw;
            }

            OnAfterStateCreated(state);

            return state;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> CancelStateChanges(PrimarySchoolCA.Server.Models.ConData.State item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStateUpdated(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateUpdated(PrimarySchoolCA.Server.Models.ConData.State item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> UpdateState(int stateid, PrimarySchoolCA.Server.Models.ConData.State state)
        {
            OnStateUpdated(state);

            var itemToUpdate = Context.States
                              .Where(i => i.StateID == state.StateID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(state);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStateUpdated(state);

            return state;
        }

        partial void OnStateDeleted(PrimarySchoolCA.Server.Models.ConData.State item);
        partial void OnAfterStateDeleted(PrimarySchoolCA.Server.Models.ConData.State item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.State> DeleteState(int stateid)
        {
            var itemToDelete = Context.States
                              .Where(i => i.StateID == stateid)
                              .Include(i => i.LocalGovtAreas)
                              .Include(i => i.ParentsOrGuardians)
                              .Include(i => i.Schools)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStateDeleted(itemToDelete);


            Context.States.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Student> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.Student>> GetStudents(Query query = null)
        {
            var items = Context.Students.AsQueryable();

            items = items.Include(i => i.Gender);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentGet(PrimarySchoolCA.Server.Models.ConData.Student item);
        partial void OnGetStudentByStudentId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Student> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> GetStudentByStudentId(long studentid)
        {
            var items = Context.Students
                              .AsNoTracking()
                              .Where(i => i.StudentID == studentid);

            items = items.Include(i => i.Gender);
 
            OnGetStudentByStudentId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentCreated(PrimarySchoolCA.Server.Models.ConData.Student item);
        partial void OnAfterStudentCreated(PrimarySchoolCA.Server.Models.ConData.Student item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> CreateStudent(PrimarySchoolCA.Server.Models.ConData.Student student)
        {
            OnStudentCreated(student);

            var existingItem = Context.Students
                              .Where(i => i.StudentID == student.StudentID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Students.Add(student);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(student).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentCreated(student);

            return student;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> CancelStudentChanges(PrimarySchoolCA.Server.Models.ConData.Student item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentUpdated(PrimarySchoolCA.Server.Models.ConData.Student item);
        partial void OnAfterStudentUpdated(PrimarySchoolCA.Server.Models.ConData.Student item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> UpdateStudent(long studentid, PrimarySchoolCA.Server.Models.ConData.Student student)
        {
            OnStudentUpdated(student);

            var itemToUpdate = Context.Students
                              .Where(i => i.StudentID == student.StudentID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(student);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentUpdated(student);

            return student;
        }

        partial void OnStudentDeleted(PrimarySchoolCA.Server.Models.ConData.Student item);
        partial void OnAfterStudentDeleted(PrimarySchoolCA.Server.Models.ConData.Student item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Student> DeleteStudent(long studentid)
        {
            var itemToDelete = Context.Students
                              .Where(i => i.StudentID == studentid)
                              .Include(i => i.Attendances)
                              .Include(i => i.ClassRegisterStudents)
                              .Include(i => i.ContinuousAssessments)
                              .Include(i => i.ParentsOrGuardians)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentDeleted(itemToDelete);


            Context.Students.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSubjectsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSubjectsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSubjectsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Subject> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.Subject>> GetSubjects(Query query = null)
        {
            var items = Context.Subjects.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSubjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubjectGet(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnGetSubjectBySubjectId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Subject> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> GetSubjectBySubjectId(long subjectid)
        {
            var items = Context.Subjects
                              .AsNoTracking()
                              .Where(i => i.SubjectID == subjectid);

 
            OnGetSubjectBySubjectId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSubjectGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSubjectCreated(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectCreated(PrimarySchoolCA.Server.Models.ConData.Subject item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> CreateSubject(PrimarySchoolCA.Server.Models.ConData.Subject subject)
        {
            OnSubjectCreated(subject);

            var existingItem = Context.Subjects
                              .Where(i => i.SubjectID == subject.SubjectID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Subjects.Add(subject);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(subject).State = EntityState.Detached;
                throw;
            }

            OnAfterSubjectCreated(subject);

            return subject;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> CancelSubjectChanges(PrimarySchoolCA.Server.Models.ConData.Subject item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSubjectUpdated(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectUpdated(PrimarySchoolCA.Server.Models.ConData.Subject item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> UpdateSubject(long subjectid, PrimarySchoolCA.Server.Models.ConData.Subject subject)
        {
            OnSubjectUpdated(subject);

            var itemToUpdate = Context.Subjects
                              .Where(i => i.SubjectID == subject.SubjectID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(subject);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSubjectUpdated(subject);

            return subject;
        }

        partial void OnSubjectDeleted(PrimarySchoolCA.Server.Models.ConData.Subject item);
        partial void OnAfterSubjectDeleted(PrimarySchoolCA.Server.Models.ConData.Subject item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Subject> DeleteSubject(long subjectid)
        {
            var itemToDelete = Context.Subjects
                              .Where(i => i.SubjectID == subjectid)
                              .Include(i => i.AssessmentSetups)
                              .Include(i => i.ContinuousAssessments)
                              .Include(i => i.SubjectSchoolTypes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSubjectDeleted(itemToDelete);


            Context.Subjects.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSubjectDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSubjectSchoolTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjectschooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjectschooltypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSubjectSchoolTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/subjectschooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/subjectschooltypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSubjectSchoolTypesRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>> GetSubjectSchoolTypes(Query query = null)
        {
            var items = Context.SubjectSchoolTypes.AsQueryable();

            items = items.Include(i => i.SchoolType);
            items = items.Include(i => i.Subject);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSubjectSchoolTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubjectSchoolTypeGet(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnGetSubjectSchoolTypeById(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> GetSubjectSchoolTypeById(long id)
        {
            var items = Context.SubjectSchoolTypes
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            items = items.Include(i => i.SchoolType);
            items = items.Include(i => i.Subject);
 
            OnGetSubjectSchoolTypeById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSubjectSchoolTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSubjectSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeCreated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> CreateSubjectSchoolType(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectschooltype)
        {
            OnSubjectSchoolTypeCreated(subjectschooltype);

            var existingItem = Context.SubjectSchoolTypes
                              .Where(i => i.ID == subjectschooltype.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SubjectSchoolTypes.Add(subjectschooltype);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(subjectschooltype).State = EntityState.Detached;
                throw;
            }

            OnAfterSubjectSchoolTypeCreated(subjectschooltype);

            return subjectschooltype;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> CancelSubjectSchoolTypeChanges(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSubjectSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeUpdated(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> UpdateSubjectSchoolType(long id, PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType subjectschooltype)
        {
            OnSubjectSchoolTypeUpdated(subjectschooltype);

            var itemToUpdate = Context.SubjectSchoolTypes
                              .Where(i => i.ID == subjectschooltype.ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(subjectschooltype);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSubjectSchoolTypeUpdated(subjectschooltype);

            return subjectschooltype;
        }

        partial void OnSubjectSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);
        partial void OnAfterSubjectSchoolTypeDeleted(PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType> DeleteSubjectSchoolType(long id)
        {
            var itemToDelete = Context.SubjectSchoolTypes
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSubjectSchoolTypeDeleted(itemToDelete);


            Context.SubjectSchoolTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSubjectSchoolTypeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTermsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/terms/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/terms/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTermsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/terms/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/terms/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTermsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Term> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.Term>> GetTerms(Query query = null)
        {
            var items = Context.Terms.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTermsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTermGet(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnGetTermByTermId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.Term> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> GetTermByTermId(int termid)
        {
            var items = Context.Terms
                              .AsNoTracking()
                              .Where(i => i.TermID == termid);

 
            OnGetTermByTermId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTermGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTermCreated(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermCreated(PrimarySchoolCA.Server.Models.ConData.Term item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> CreateTerm(PrimarySchoolCA.Server.Models.ConData.Term term)
        {
            OnTermCreated(term);

            var existingItem = Context.Terms
                              .Where(i => i.TermID == term.TermID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Terms.Add(term);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(term).State = EntityState.Detached;
                throw;
            }

            OnAfterTermCreated(term);

            return term;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> CancelTermChanges(PrimarySchoolCA.Server.Models.ConData.Term item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTermUpdated(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermUpdated(PrimarySchoolCA.Server.Models.ConData.Term item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> UpdateTerm(int termid, PrimarySchoolCA.Server.Models.ConData.Term term)
        {
            OnTermUpdated(term);

            var itemToUpdate = Context.Terms
                              .Where(i => i.TermID == term.TermID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(term);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTermUpdated(term);

            return term;
        }

        partial void OnTermDeleted(PrimarySchoolCA.Server.Models.ConData.Term item);
        partial void OnAfterTermDeleted(PrimarySchoolCA.Server.Models.ConData.Term item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.Term> DeleteTerm(int termid)
        {
            var itemToDelete = Context.Terms
                              .Where(i => i.TermID == termid)
                              .Include(i => i.AssessmentSetups)
                              .Include(i => i.Attendances)
                              .Include(i => i.ClassRegisters)
                              .Include(i => i.ContinuousAssessments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTermDeleted(itemToDelete);


            Context.Terms.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTermDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportContinuousAssessmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/continuousassessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/continuousassessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportContinuousAssessmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/continuousassessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/continuousassessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnContinuousAssessmentsRead(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> items);

        public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>> GetContinuousAssessments(Query query = null)
        {
            var items = Context.ContinuousAssessments.AsQueryable();

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Student);
            items = items.Include(i => i.Subject);
            items = items.Include(i => i.Term);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnContinuousAssessmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnContinuousAssessmentGet(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnGetContinuousAssessmentByRecordId(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> items);


        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> GetContinuousAssessmentByRecordId(long recordid)
        {
            var items = Context.ContinuousAssessments
                              .AsNoTracking()
                              .Where(i => i.RecordID == recordid);

            items = items.Include(i => i.AcademicSession);
            items = items.Include(i => i.SchoolClass);
            items = items.Include(i => i.Student);
            items = items.Include(i => i.Subject);
            items = items.Include(i => i.Term);
 
            OnGetContinuousAssessmentByRecordId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnContinuousAssessmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnContinuousAssessmentCreated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentCreated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> CreateContinuousAssessment(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment continuousassessment)
        {
            OnContinuousAssessmentCreated(continuousassessment);

            var existingItem = Context.ContinuousAssessments
                              .Where(i => i.RecordID == continuousassessment.RecordID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ContinuousAssessments.Add(continuousassessment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(continuousassessment).State = EntityState.Detached;
                throw;
            }

            OnAfterContinuousAssessmentCreated(continuousassessment);

            return continuousassessment;
        }

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> CancelContinuousAssessmentChanges(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnContinuousAssessmentUpdated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentUpdated(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> UpdateContinuousAssessment(long recordid, PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment continuousassessment)
        {
            OnContinuousAssessmentUpdated(continuousassessment);

            var itemToUpdate = Context.ContinuousAssessments
                              .Where(i => i.RecordID == continuousassessment.RecordID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(continuousassessment);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterContinuousAssessmentUpdated(continuousassessment);

            return continuousassessment;
        }

        partial void OnContinuousAssessmentDeleted(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);
        partial void OnAfterContinuousAssessmentDeleted(PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment item);

        public async Task<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment> DeleteContinuousAssessment(long recordid)
        {
            var itemToDelete = Context.ContinuousAssessments
                              .Where(i => i.RecordID == recordid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnContinuousAssessmentDeleted(itemToDelete);


            Context.ContinuousAssessments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterContinuousAssessmentDeleted(itemToDelete);

            return itemToDelete;
        }
    
      public async Task ExportStudentListForParentGuardianDropdownsToExcel(Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/studentlistforparentguardiandropdowns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/studentlistforparentguardiandropdowns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task ExportStudentListForParentGuardianDropdownsToCSV(Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/studentlistforparentguardiandropdowns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/studentlistforparentguardiandropdowns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task<IQueryable<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown>> GetStudentListForParentGuardianDropdowns(Query query = null)
      {
          OnStudentListForParentGuardianDropdownsDefaultParams();

          var items = Context.StudentListForParentGuardianDropdowns.FromSqlInterpolated($"EXEC [dbo].[StudentListForParentGuardianDropdowns] ").ToList().AsQueryable();

          if (query != null)
          {
              if (!string.IsNullOrEmpty(query.Expand))
              {
                  var propertiesToExpand = query.Expand.Split(',');
                  foreach(var p in propertiesToExpand)
                  {
                      items = items.Include(p.Trim());
                  }
              }

              ApplyQuery(ref items, query);
          }
          
          OnStudentListForParentGuardianDropdownsInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnStudentListForParentGuardianDropdownsDefaultParams();

      partial void OnStudentListForParentGuardianDropdownsInvoke(ref IQueryable<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown> items);  
      public async Task<int> InsertSingleCaRecords(long? StudentID, int? AcademicSessionID, int? TermID, int? SchoolClassID, long? SubjectID, int? CAMarkObtainable, int? CAMarkObtained, string EntryDate, string InsertedBy)
      {
          OnInsertSingleCaRecordsDefaultParams(ref StudentID, ref AcademicSessionID, ref TermID, ref SchoolClassID, ref SubjectID, ref CAMarkObtainable, ref CAMarkObtained, ref EntryDate, ref InsertedBy);

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
              if((_p.Direction == ParameterDirection.Output || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
              {
                  _p.Value = DBNull.Value;
              }
          }

          Context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[InsertSingleCARecord] @StudentID, @AcademicSessionID, @TermID, @SchoolClassID, @SubjectID, @CAMarkObtainable, @CAMarkObtained, @EntryDate, @InsertedBy", @params);

          int result = Convert.ToInt32(@params[0].Value);


          OnInsertSingleCaRecordsInvoke(ref result);

          return await Task.FromResult(result);
      }

      partial void OnInsertSingleCaRecordsDefaultParams(ref long? StudentID, ref int? AcademicSessionID, ref int? TermID, ref int? SchoolClassID, ref long? SubjectID, ref int? CAMarkObtainable, ref int? CAMarkObtained, ref string EntryDate, ref string InsertedBy);
      partial void OnInsertSingleCaRecordsInvoke(ref int result);
    }
}