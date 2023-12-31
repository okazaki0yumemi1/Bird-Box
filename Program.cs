using Bird_Box.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    var filePath = Path.Combine(
        System.Environment.CurrentDirectory,
        "Documentation/ApiDocumentation.xml"
    );
    o.IncludeXmlComments(filePath);
});
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Bird_Box.Data.BirdBoxContext>();

builder.Services.AddSingleton<
    Bird_Box.Services.RecordingService,
    Bird_Box.Services.RecordingService
>();

//Database operations
builder.Services.AddScoped<Bird_Box.Data.BirdRepository, Bird_Box.Data.BirdRepository>();

//Options for BirdNET Analyzer
builder.Services.AddScoped<
    Bird_Box.Controllers.OptionsController,
    Bird_Box.Controllers.OptionsController
>();

//Results
builder.Services.AddScoped<
    Bird_Box.Controllers.ResultsController,
    Bird_Box.Controllers.ResultsController
>();

builder.Services.AddScoped<
    Bird_Box.Controllers.ResultsAPIController,
    Bird_Box.Controllers.ResultsAPIController
>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Bird_Box.Data.BirdBoxContext>();
    dbContext.Database.Migrate();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Results/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger(o => o.RouteTemplate = "api/swagger/{documentname}/swagger.json");
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});
app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Results}/{action=Index}/{id?}");
app.Run();
