// API = receives/handles requests
// Database = persistently stores the data

using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Supabase");

var app = builder.Build();


// var countries = new List<Country>
// {
//     new Country { Id = 1, Name = "Algeria", Capital = "Algiers" },
//     new Country { Id = 2, Name = "Japan", Capital = "Tokyo" },
//     new Country { Id = 3, Name = "Canada", Capital = "Ottawa" }
// };

app.MapGet("/countries", async () =>
{
    var countriesFromDb = new List<Country>();

    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new NpgsqlCommand(
        "SELECT id, name, capital FROM countries ORDER BY id",
        connection
    );

    await using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        countriesFromDb.Add(new Country
        {
            Id = reader.GetInt64(0),
            Name = reader.GetString(1),
            Capital = reader.GetString(2)
        });
    }

    return Results.Ok(countriesFromDb);
});

//Will turn into JSON when returned from the endpoint (serialisation)
app.MapGet("/countries/{id}", async (long id) =>
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new NpgsqlCommand(
        "SELECT id, name, capital FROM countries WHERE id = @id",
        connection
    );

    command.Parameters.AddWithValue("id", id);

    await using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
    {
        return Results.NotFound();
    }

    var country = new Country
    {
        Id = reader.GetInt64(0),
        Name = reader.GetString(1),
        Capital = reader.GetString(2)
    };

    return Results.Ok(country);
});

//ASP.NET will automatically bind the JSON body of the request to the newCountry parameter
app.MapPost("/countries", async (Country newCountry) =>
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new NpgsqlCommand(
        """
        INSERT INTO countries (name, capital)
        VALUES (@name, @capital)
        RETURNING id, name, capital
        """,
        connection
    );

    command.Parameters.AddWithValue("name", newCountry.Name);
    command.Parameters.AddWithValue("capital", newCountry.Capital);

    await using var reader = await command.ExecuteReaderAsync();

    await reader.ReadAsync();

    var createdCountry = new Country
    {
        Id = reader.GetInt64(0),
        Name = reader.GetString(1),
        Capital = reader.GetString(2)
    };

    return Results.Created($"/countries/{createdCountry.Id}", createdCountry);
});

app.MapPut("/countries/{id}", async (long id, Country updatedCountry) =>
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new NpgsqlCommand(
        """
        UPDATE countries
        SET name = @name, capital = @capital
        WHERE id = @id
        RETURNING id, name, capital
        """,
        connection
    );

    command.Parameters.AddWithValue("id", id);
    command.Parameters.AddWithValue("name", updatedCountry.Name);
    command.Parameters.AddWithValue("capital", updatedCountry.Capital);

    await using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
    {
        return Results.NotFound();
    }

    var country = new Country
    {
        Id = reader.GetInt64(0),
        Name = reader.GetString(1),
        Capital = reader.GetString(2)
    };

    return Results.Ok(country);
});

app.MapDelete("/countries/{id}", async (long id) =>
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new NpgsqlCommand(
        """
        DELETE FROM countries
        WHERE id = @id
        """,
        connection
    );

    command.Parameters.AddWithValue("id", id);

    var rowsDeleted = await command.ExecuteNonQueryAsync();

    if (rowsDeleted == 0)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});


app.Run();


class Country
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Capital { get; set; }
}