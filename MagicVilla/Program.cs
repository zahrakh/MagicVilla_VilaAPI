var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
// Add services to the container.

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllers();

app.Run();