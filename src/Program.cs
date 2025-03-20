using Microsoft.EntityFrameworkCore;
using BuildCalc.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "vue/dist";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:8888")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddDbContext<MyContext>(options => options.UseMySql(
  builder.Configuration.GetConnectionString("MyContext"), new MySqlServerVersion(new Version(8, 4, 2))));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(app.Environment.IsDevelopment())
{
    var vueDevServer = new ProcessStartInfo
    {
        FileName = "npm",
        Arguments = "run serve",
        WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "vue/"),
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    Process.Start(vueDevServer);
}

// app.UseHttpsRedirection();

//SAP静的ファイルの設定
app.UseStaticFiles();
app.UseSpaStaticFiles();

// ルーティングの設定
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response Status: {context.Response.StatusCode}");
});

// SPAミドルウェア設定
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "vue/dist";

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8888");
    }
});


app.Run();