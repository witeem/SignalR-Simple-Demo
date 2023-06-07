using SignalR.Pro.Hubs;
using SignalR.Pro.Repository;
using SignalR.Pro.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IConnectionRepository, InMemoryConnectionRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapHub<RecevieHub>("/recevieHub");
    endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller=Home}/{action=Chat}/{id?}");
});
app.Run();
