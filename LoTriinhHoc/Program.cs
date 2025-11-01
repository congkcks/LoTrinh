using LoTriinhHoc.Data;
using LoTriinhHoc.Mappings;       // AutoMapper MappingProfile
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Add services to the container
// -----------------------------
builder.Services.AddControllers();

// ✅ Thêm DbContext (PostgreSQL)
builder.Services.AddDbContext<LotrinhhocDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Thêm AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ✅ Cấu hình CORS (cho phép tất cả)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()   // Cho phép mọi domain
            .AllowAnyMethod()   // Cho phép mọi phương thức (GET, POST, PUT, DELETE)
            .AllowAnyHeader();  // Cho phép mọi header
    });
});

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -----------------------------
// Configure the HTTP pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Áp dụng CORS trước MapControllers
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
