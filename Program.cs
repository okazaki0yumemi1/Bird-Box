using Bird_Box.Authentication;
using Bird_Box.Data;
using Bird_Box.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Bird_Box.Data.BirdBoxContext>();
builder.Services.AddDbContext<Bird_Box.Authentication.BirdBoxAuthContext>();


builder.Services.AddSingleton<
    Bird_Box.Services.RecordingService,
    Bird_Box.Services.RecordingService
>();

//Database operations
builder.Services.AddScoped<BirdRepository, BirdRepository>();
builder.Services.AddScoped<MicrophoneRepository, MicrophoneRepository>();
builder.Services.AddScoped<ListeningTasksRepository, ListeningTasksRepository>();

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

var _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BirdBoxAuthContext>()
                .AddDefaultTokenProviders();


builder.Services.AddSwaggerGen(o =>
{
    var filePath = Path.Combine(
        System.Environment.CurrentDirectory,
        "Documentation/ApiDocumentation.xml"
    );
    o.IncludeXmlComments(filePath);
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
//         In = ParameterLocation.Header, 
//         Description = "Please insert JWT with Bearer into field",
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey 
//     });
//         o.AddSecurityRequirement(new OpenApiSecurityRequirement {
//     { 
//         new OpenApiSecurityScheme 
//         { 
//         Reference = new OpenApiReference 
//         { 
//             Type = ReferenceType.SecurityScheme,
//             Id = "Bearer" 
//         } 
//         },
//       new string[] { } 
//     } 
//   });
});
// builder.Services.AddAuthentication(options =>
//             {
//                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//             })

//             // Adding Jwt Bearer
//             .AddJwtBearer(options =>
//             {
//                 options.SaveToken = true;
//                 options.RequireHttpsMetadata = false;
//                 options.TokenValidationParameters = new TokenValidationParameters()
//                 {
//                     ValidateIssuer = true,
//                     ValidateAudience = true,
//                     ValidAudience = _config["JWT:ValidAudience"],
//                     ValidIssuer = _config["JWT:ValidIssuer"],
//                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]))
//                 };
//             });
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _config["JWT:ValidAudience"],
            ValidIssuer = _config["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BirdBoxContext>();
    dbContext.Database.Migrate();
    dbContext.Database.EnsureCreated();

    var authContext = scope.ServiceProvider.GetRequiredService<BirdBoxAuthContext>();
    authContext.Database.Migrate();
    authContext.Database.EnsureCreated();
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

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger(o => o.RouteTemplate = "api/swagger/{documentname}/swagger.json");
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});
app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Results}/{action=Index}/{id?}");

await app.StartAsync();

//Make GET request to API to restore tasks
var requestApiToRestoreTasks = CommandLine.ExecuteCommand(
    $"curl -X GET localhost:5001/api/recordings/"
);
Console.WriteLine(
    $"Cached tasks have been restored. There are {requestApiToRestoreTasks} tasks running right now."
);

await app.WaitForShutdownAsync();
//app.Run();
// using (var host = app)
// {
//     host.Start();
//     var requestApiToRestoreTasks = CommandLine.ExecuteCommand($"curl -X GET localhost:5001/api/recordings/");
//     Console.WriteLine($"Cached tasks have been restored. There are {requestApiToRestoreTasks} tasks running right now.");
//     host.
// }
// //app.Start();
