using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MISA.QuanLiTaiSan.Api.Middleware;
using MISA.QuanLiTaiSan.BL.AccountService;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.BudgetCategoryService;
using MISA.QuanLiTaiSan.BL.BudgetDetailService;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.BL.FixedAssetCategoryBL;
using MISA.QuanLiTaiSan.BL.TableImportService;
using MISA.QuanLiTaiSan.BL.VoucherDetailService;
using MISA.QuanLiTaiSan.BL.VoucherService;
using MISA.QuanLiTaiSan.Common.Jwt;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.DL.AccountRepository;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.BudgetCategoryRepository;
using MISA.QuanLiTaiSan.DL.BudgetDetailRepository;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using MISA.QuanLiTaiSan.DL.TableImportRepository;
using MISA.QuanLiTaiSan.DL.VoucherDetailRepository;
using MISA.QuanLiTaiSan.DL.VoucherRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new Exception("Jwt key is not configured");
    }
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();
builder.Services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


builder.Services.AddScoped<IFixedAssetImportService, FixedAssetImportService>();
builder.Services.AddScoped<IFixedAssetImportRepository, FixedAssetImportRepository>();

builder.Services.AddScoped<IFixedAssetCategoryService, FixedAssetCategoryService>();
builder.Services.AddScoped<IFixedAssetCategoryRepository, FixedAssetCategoryRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

builder.Services.AddScoped<IVoucherDetailService, VoucherDetailService>();
builder.Services.AddScoped<IVoucherDetailRepository, VoucherDetailRepository>();

builder.Services.AddScoped<IBudgetCategoryService, BudgetCategoryService>();
builder.Services.AddScoped<IBudgetCategoryRepository, BudgetCategoryRepository>();

builder.Services.AddScoped<IBudgetDetailService, BudgetDetailService>();
builder.Services.AddScoped<IBudgetDetailRepository, BudgetDetailRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>)); // DI BaseService vào IBaseService thông qua constructor
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // DI BaseRepository vào IBaseRepository thông qua constructor

builder.Services.AddSingleton<IJwtHelper, JwtHelper>();


var app = builder.Build();
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseMiddleware(typeof(ErrorMiddleware));
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
