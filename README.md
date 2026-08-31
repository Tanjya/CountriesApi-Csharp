# Countries API 🌍

A small full-stack project I built while learning C# and ASP.NET Core.

The main purpose of this project wasn't to build something overly complex. I wanted to take concepts I had worked with before, such as APIs and CRUD operations, and understand how they worked within a C#/.NET environment.

I built the project gradually, adding one layer at a time so I could understand what was happening in my code rather than simply getting to a finished result.

## How I Built It

### 1. Learning the C# foundations

I started by getting comfortable with the C# concepts I would need for the project, including classes, objects, methods, lists and asynchronous programming.

Rather than trying to learn the whole language first, I focused on the concepts I needed to start building.

### 2. Building my first ASP.NET API

I then created a Minimal API using ASP.NET Core.

I started with an in-memory `List<Country>` and gradually implemented:

- GET
- POST
- PUT
- DELETE

Starting with an in-memory list allowed me to focus on understanding how requests, endpoints and responses worked before introducing a database.

### 3. Adding persistent storage

Once the CRUD endpoints were working, I wanted to understand how the same API would work with data stored outside the application.

I created a PostgreSQL database using Supabase and connected it to the C# application using Npgsql.

I then adapted each endpoint so that it communicated with PostgreSQL rather than the original in-memory list.

This was probably the most useful stage of the project because it helped me understand the journey from:

```text
HTTP request
    ↓
ASP.NET endpoint
    ↓
SQL query
    ↓
PostgreSQL
    ↓
C# object
    ↓
JSON response
```

### 4. Adding a simple frontend

My main focus was the backend, but I also have previous frontend experience.

Once the API was working, I created a small HTML, CSS and JavaScript interface so that the project could be demonstrated visually and understood by someone who isn't looking directly at the API.

The frontend currently allows users to view countries and add new ones.

It communicates with my ASP.NET API using JavaScript `fetch()` requests.

## What the API Can Do

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/countries` | Get all countries |
| GET | `/countries/{id}` | Get one country |
| POST | `/countries` | Add a country |
| PUT | `/countries/{id}` | Update a country |
| DELETE | `/countries/{id}` | Delete a country |

## Technologies I Used

- C# and .NET 10
- ASP.NET Core Minimal API
- PostgreSQL
- Supabase
- Npgsql
- HTML, CSS and JavaScript
- Git and GitHub

## A Challenge I Worked Through

One of the main challenges was moving from an in-memory list to a real database.

My endpoints were initially working with a `List<Country>`, but I learned that this data only existed while the application was running.

I wanted to understand how persistent storage changed the application, so I connected the API to PostgreSQL and worked through each CRUD operation again.

Doing this helped me understand much more clearly how the different parts of a backend application connect rather than just knowing how to write an endpoint.

## What I Took From the Project

The biggest thing I gained from this project was a better understanding of how the different layers of an application work together.

I became more comfortable with C# and ASP.NET, but more importantly I practised approaching an unfamiliar technology by breaking it into smaller pieces, building incrementally, testing what I had built and making sure I understood each stage before moving on.

There is still plenty I could develop further, but I now have a foundation that I can continue building on.

## Next Steps

If I continued developing the project, some of my next steps would be:

- Connect the frontend to the existing PUT and DELETE endpoints
- Add stronger validation and user feedback
- Add automated tests
- Explore a more structured application architecture as the project grows
- Deploy the application
