class Program
{
     public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddCors();
        var app = builder.Build();
        app.UseCors(config => config.AllowAnyHeader()
          .AllowAnyMethod()
          .AllowAnyOrigin());
        app.UseFileServer();
        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}