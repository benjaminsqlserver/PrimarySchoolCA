using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Radzen;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using PrimarySchoolCA.Server.Data;
using Microsoft.AspNetCore.Identity;
using PrimarySchoolCA.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient
    {
        BaseAddress = new Uri(baseAddress)
    };
});
builder.Services.AddScoped<PrimarySchoolCA.Server.ConDataService>();
builder.Services.AddDbContext<PrimarySchoolCA.Server.Data.ConDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConDataConnection"));
});
builder.Services.AddScoped<PrimarySchoolCA.Client.ConDataService>();
builder.Services.AddLocalization();
builder.Services.AddHttpClient("PrimarySchoolCA.Server").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<PrimarySchoolCA.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConDataConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, PrimarySchoolCA.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderConData = new ODataConventionModelBuilder();
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AcademicSession>("AcademicSessions");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetRoleClaim>("AspNetRoleClaims");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetRole>("AspNetRoles");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetUserClaim>("AspNetUserClaims");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetUserLogin>("AspNetUserLogins").EntityType.HasKey(entity => new { entity.LoginProvider, entity.ProviderKey });
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetUserRole>("AspNetUserRoles").EntityType.HasKey(entity => new { entity.UserId, entity.RoleId });
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetUser>("AspNetUsers");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AspNetUserToken>("AspNetUserTokens").EntityType.HasKey(entity => new { entity.UserId, entity.LoginProvider, entity.Name });
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AssessmentSetup>("AssessmentSetups");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.AssessmentType>("AssessmentTypes");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.Attendance>("Attendances");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.ClassRegister>("ClassRegisters");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.ClassRegisterStudent>("ClassRegisterStudents");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.Gender>("Genders");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.LocalGovtArea>("LocalGovtAreas");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.ParentsOrGuardian>("ParentsOrGuardians");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.SchoolClass>("SchoolClasses");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.School>("Schools");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.SchoolType>("SchoolTypes");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.State>("States");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.Student>("Students");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.Subject>("Subjects");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.SubjectSchoolType>("SubjectSchoolTypes");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.Term>("Terms");
    oDataBuilderConData.EntitySet<PrimarySchoolCA.Server.Models.ConData.ContinuousAssessment>("ContinuousAssessments");
    var conDataStudentListForParentGuardianDropdown = oDataBuilderConData.Function("StudentListForParentGuardianDropdownsFunc");
    conDataStudentListForParentGuardianDropdown.Returns<PrimarySchoolCA.Server.Models.ConData.StudentListForParentGuardianDropdown>();
    var conDataInsertSingleCaRecord = oDataBuilderConData.Function("InsertSingleCaRecordsFunc");
    conDataInsertSingleCaRecord.Parameter<long?>("StudentID");
    conDataInsertSingleCaRecord.Parameter<int?>("AcademicSessionID");
    conDataInsertSingleCaRecord.Parameter<int?>("TermID");
    conDataInsertSingleCaRecord.Parameter<int?>("SchoolClassID");
    conDataInsertSingleCaRecord.Parameter<long?>("SubjectID");
    conDataInsertSingleCaRecord.Parameter<int?>("CAMarkObtainable");
    conDataInsertSingleCaRecord.Parameter<int?>("CAMarkObtained");
    conDataInsertSingleCaRecord.Parameter<string>("EntryDate");
    conDataInsertSingleCaRecord.Parameter<string>("InsertedBy");
    conDataInsertSingleCaRecord.Returns(typeof(int));
    opt.AddRouteComponents("odata/ConData", oDataBuilderConData.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRequestLocalization(options => options.AddSupportedCultures("en", "ar-SA", "fr-FR").AddSupportedUICultures("en", "ar-SA", "fr-FR").SetDefaultCulture("en"));
app.UseHeaderPropagation();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();