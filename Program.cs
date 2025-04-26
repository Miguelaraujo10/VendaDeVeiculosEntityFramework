using Microsoft.EntityFrameworkCore;
using ProjetoCompletoLocadora.Data;
using ProjetoCompletoLocadora.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locadora API V1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionMiddleware>(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
