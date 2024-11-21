using System.Text;
using System.Text.Json.Serialization;
using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Middleware;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Services.artwork;
using Backend_Teamwork.src.Services.booking;
using Backend_Teamwork.src.Services.category;
using Backend_Teamwork.src.Services.order;
using Backend_Teamwork.src.Services.user;
using Backend_Teamwork.src.Services.workshop;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using static Backend_Teamwork.src.Entities.User;

var builder = WebApplication.CreateBuilder(args);

//connect to database
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("Local")
);
dataSourceBuilder.MapEnum<UserRole>();
dataSourceBuilder.MapEnum<Status>();

//add database connection
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(dataSourceBuilder.Build());
});

//add auto-mapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

//add DI services
builder.Services.AddScoped<ICategoryService, CategoryService>().AddScoped<CategoryRepository>();
builder.Services.AddScoped<IArtworkService, ArtworkService>().AddScoped<ArtworkRepository>();
builder.Services.AddScoped<IUserService, UserService>().AddScoped<UserRepository>();
builder.Services.AddScoped<IOrderService, OrderService>().AddScoped<OrderRepository>();
builder.Services.AddScoped<IWorkshopService, WorkshopService>().AddScoped<WorkshopRepository>();
builder.Services.AddScoped<IBookingService, BookingService>().AddScoped<BookingRepository>();

//builder.Services.AddScoped<IPaymentService, IPaymentService>().AddScoped<PaymentRepository>();


//add logic for authentication
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
        };
    });

//add logic for athorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
});

//  ***  add CORS settings ***
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder => policyBuilder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
});
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         builder => builder.WithOrigins("http://localhost:5173")
//                           .AllowAnyHeader()
//                           .AllowAnyMethod());
// });


//add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder
    .Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//add swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();
app.MapGet("/", () => "Server is running");

//Convert to Timestamp format
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//

//test database connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Database is connected");
             dbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("Unable to connect to the database.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}
app.UseHttpsRedirection();

//use middleware
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

//use controllers
app.MapControllers();

//use swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();
