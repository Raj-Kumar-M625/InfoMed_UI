using InfoMed.Security;
using InfoMed.Services.Implementation;
using InfoMed.Services.Interface;
using InfoMed.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IFeesService, FeesService>();
builder.Services.AddScoped<ITextContentAreasService, TextContentAreasService>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();
builder.Services.AddScoped<ILastYearMemoriesService, LastYearMemoriesService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(365 * 100);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false;
});

builder.Services.AddScoped<Session>();

// Add DataProtectionPurposeStrings
builder.Services.AddSingleton<DataProtectionConnectionString>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");
app.UseExceptionHandler("/Error/500");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "custom",
        pattern: "EventCrafter",
        defaults: new { controller = "Account", action = "Login" });

    endpoints.MapControllerRoute(
        name: "customFallback",
        pattern: "{e}",
        defaults: new { controller = "Event", action = "Home" }
     );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Event}/{action=Home}/{id?}");

app.Run();
