using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using miniTaskAPI.Data;
using miniTaskAPI.Interface;
using miniTaskAPI.Models;
using miniTaskAPI.Repository;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
var services = builder.Services;
// Add services to the container.

//When Ready for Production We can Use this 

//services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(conf.GetConnectionString("DefaultConnection"));
//});

services.AddScoped<ITaskService, TaskService>();
services.AddScoped<ICategoryService, CategoryService >();


//For testing and development Only
services.AddDbContext<AuthDbContext>(options =>
{
    options.UseInMemoryDatabase("AuthDb");
});

services.AddAuthentication();
services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<AuthDbContext>();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "AuthDemo",
        Version = "v1"
    });
    options.AddSecurityDefinition("bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please Enter Token",
        Name = "authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearer"
            }
        },
        [] }

    });
});

var app = builder.Build();
app.MapIdentityApi<ApplicationUser>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();