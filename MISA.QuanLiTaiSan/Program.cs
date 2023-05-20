using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.DepartmentBL;
using MISA.QuanLiTaiSan.BL.FixedAssetBL;
using MISA.QuanLiTaiSan.BL.FixedAssetCategoryBL;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.Database;
using MISA.QuanLiTaiSan.DL.DepartmentDL;
using MISA.QuanLiTaiSan.DL.FixedAssetCategoryDL;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

////Xử lý dependency injection
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>)); // DI BaseService vào IBaseService thông qua constructor
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // DI BaseRepository vào IBaseRepository thông qua constructor

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // vì cần thêm các method không có trong base nên phải tiêm thêm
builder.Services.AddScoped<IDepartmentService, DepartmentService>();






var app = builder.Build();
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
