using dbfirst.Data;
using dbfirst.Middlewares;
using dbfirst.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var MyCors = "_myCors";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(MyCors, p =>
        p.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ThorDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CategoriaService>();

var app = builder.Build();

app.MapOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors(MyCors);
app.UseRouting();
app.UseMiddleware<Auth>();


app.UseAuthorization();
app.MapControllers();
app.Run();