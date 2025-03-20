using Microsoft.EntityFrameworkCore;
using BuildCalc.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// ホストのポート設定
builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // 開発環境: Vueの開発サーバーを許可
            policy.WithOrigins("http://localhost:8888", "http://0.0.0.0:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            // 本番環境: 本番サーバーを許可
            policy.WithOrigins("https://dotnetvue-630537362311.us-central1.run.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    });
});

// Add services to the container.
builder.Services.AddDbContext<MyContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("MyContext"), new MySqlServerVersion(new Version(8, 4, 2))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SPAの設定
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "vue/dist";
});

var app = builder.Build();

// 本番環境でVue開発サーバーを起動しないように、条件を分ける
if (app.Environment.IsDevelopment())
{
    // 開発環境でVue開発サーバーを起動
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
else
{
    app.UseSpaStaticFiles();
}

// 重要：APIへのリクエストのためにCORSを使用
app.UseCors("AllowVueApp");

// デバッグ用のログ
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response Status: {context.Response.StatusCode}");
});

// ルーティングの設定
app.UseRouting();
app.UseAuthorization();

// APIルートを最初に処理するために、MapControllersをUseSpaより前に配置
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// SPA設定（APIルーティングの後に配置）
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "vue";

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8888");
    }
    else
    {
        app.UseStaticFiles();
    }
});

app.Run();
