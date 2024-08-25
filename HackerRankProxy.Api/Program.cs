using HackerRankProxy.App;
using System.Diagnostics.CodeAnalysis;

namespace HackerRankProxy.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var docs = Directory.EnumerateFiles(baseDirectory, "*.xml");
                foreach (var doc in docs)
                {
                    s.IncludeXmlComments(doc, includeControllerXmlComments: true);
                }
            });

            builder.Services.BootstrapHackerRankApp(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
