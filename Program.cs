using DotK_TechShop.Models.Db;
using DotK_TechShop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);
// builder.Configuration => Properties that can get data from appsettings.json
// Injected naturally

var imgDirectory = builder.Configuration["ImgPath"]?? "\\imgs\\products\\";
var fullImgDirectory = builder.Environment.WebRootPath + imgDirectory;

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Session for using session
builder.Services.AddSession(options =>
{
    // Config session timeout => 30s
    options.IdleTimeout = TimeSpan.FromSeconds(30);
});


// Add DbContexts
builder.Services.AddDbContext<MyDbContext>(options => 
{
    // Read connections tring from Configuration
    var connectString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Choose connect type -> sqlserver
    if (connectString != null)
        options.UseSqlServer(connectString);
});

// Add Identity -> support authorize and authenticate
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AuthDbContext>();
// Add DbContext for using identity
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    var connectString = builder.Configuration.GetConnectionString("AuthConnection");
    if (connectString != null)
        options.UseSqlServer(connectString);
});

// Config login url -> when user not login, they'll redirect to this url
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Login";
    // options.LogoutPath = "/demo/logout";
    // configs other cookie options here
});


// Config Services
builder.Services.AddSingleton<FileUploader>(new FileUploader(fullImgDirectory));
builder.Services.AddSingleton<ImageUploader>(new ImageUploader(imgDirectory));
builder.Services.AddSingleton<EmailSender>(options =>
{
    string username = builder.Configuration["MailInfo:Mail"]?? "";
    string password = builder.Configuration["MailInfo:Password"] ?? "";
    return new EmailSender(username, password);
});

// Build application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Use wwwroot
app.UseStaticFiles();

// Use sestion
app.UseSession();

app.UseRouting();

// Use identity
app.UseAuthentication();
app.UseAuthorization();

// Config how the application work with urls
// example: https://localhost:3045/Demo/GetAction
// => go to DemoController and run GetAction()
app.MapControllerRoute
(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Run
app.Run();

