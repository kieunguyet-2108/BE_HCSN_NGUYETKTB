using Microsoft.OpenApi.Models;
using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.BL.FixedAssetCategoryBL;
using MISA.QuanLiTaiSan.BL.TableImportService;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using MISA.QuanLiTaiSan.DL.TableImportRepository;

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();
builder.Services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


builder.Services.AddScoped<IFixedAssetImportService, FixedAssetImportService>();
builder.Services.AddScoped<IFixedAssetImportRepository, FixedAssetImportRepository>();

builder.Services.AddScoped<IFixedAssetCategoryService, FixedAssetCategoryService>();
builder.Services.AddScoped<IFixedAssetCategoryRepository, FixedAssetCategoryRepository>();


builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>)); // DI BaseService vào IBaseService thông qua constructor
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // DI BaseRepository vào IBaseRepository thông qua constructor


//builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

var app = builder.Build();
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
