using Magnum_Web_App.Services;
using Magnum_Web_App.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddHttpClient<ITrainingService, TrainingService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddHttpClient<IFeeService, FeeService>();
builder.Services.AddScoped<IFeeService, FeeService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
