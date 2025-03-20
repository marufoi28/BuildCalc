using Microsoft.EntityFrameworkCore;
using BuildCalc.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// ホストのポート設定
builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // 8080ポートでリクエストを受け付け
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

// DBコンテキストの設定
builder.Services.AddDbContext<MyContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("MyContext"), new MySqlServerVersion(new Version(8, 4, 2))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SPAの設定
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "vue/dist"; // ビルドされたVueファイルを格納するパス
});

var app = builder.Build();

// CORS設定
app.UseCors("AllowVueApp");

// 開発環境の設定
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
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
    // 本番環境では静的ファイルハンドリングの前にAPIルーティングを設定
    app.UseStaticFiles(); // 基本的な静的ファイル
    app.UseSpaStaticFiles(); // SPA用静的ファイル
}

// 重要: APIルーティングを先に設定
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // APIコントローラーを明示的にマッピング
    endpoints.MapControllers();
    
    // 追加のルートも設定
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{controller=Home}/{action=Index}/{id?}");
});

// APIのルーティングを設定した後にSPAを設定
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "vue";

    if (app.Environment.IsDevelopment())
    {
        // Vue.js 開発サーバーのプロキシ設定
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8888");
    }
    // この場所で app.UseStaticFiles() を呼び出さない
});

app.Run();