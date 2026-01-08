using SanitizationAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/people", (People people) =>
{
    var normalizedPeople = PeopleNormalizer.Normalize(people);

    return Results.Ok(normalizedPeople);
});


app.Run();


public class People
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

}