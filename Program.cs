// API = receives/handles requests
// Database = persistently stores the data


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var countries = new List<Country>
{
    new Country { Id = 1, Name = "Algeria", Capital = "Algiers" },
    new Country { Id = 2, Name = "Japan", Capital = "Tokyo" },
    new Country { Id = 3, Name = "Canada", Capital = "Ottawa" }
};

app.MapGet("/countries/", () => {
    return countries;
});

//Will turn into JSON when returned from the endpoint (serialisation)
app.MapGet("/countries/{id}", (int id) => {
    var country = countries.FirstOrDefault(country => country.Id == id);

    if (country == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(country);
});

//ASP.NET will automatically bind the JSON body of the request to the newCountry parameter
app.MapPost("/countries", (Country newCountry) =>
{
    newCountry.Id = countries.Max(country => country.Id) + 1;
    countries.Add(newCountry);

    return Results.Created($"/countries/{newCountry.Id}", newCountry);
});

//Update an existing country by ID
app.MapPut("/countries/{id}", (int id, Country updatedCountry) =>
{
    var country = countries.FirstOrDefault(country => country.Id == id);

    if (country == null)
    {
        return Results.NotFound();
    }

    country.Name = updatedCountry.Name;
    country.Capital = updatedCountry.Capital;

    return Results.Ok(country);
});


app.MapDelete("/countries/{id}", (int id) =>
{
    //Find the country with this ID
    var country = countries.FirstOrDefault(country => country.Id == id);

    //If it doesn't exist → 404
    if (country == null)
    {
        return Results.NotFound();
    }
    //If it exists → remove it
    countries.Remove(country);
    //Successful deletion → 204
    return Results.NoContent();
});


app.Run();


class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Capital { get; set; }
}