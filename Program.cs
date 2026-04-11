var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<Blog.Services.EmailService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: React'ýn çalýþacaðý adrese izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5176",
            "https://personal-site-taupe-omega-52.vercel.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowReact"); 
app.UseAuthorization();
app.MapControllers();

app.Run();