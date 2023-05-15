using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalJwt.Models;
using MinimalJwt.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
            {

            }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ISongService, SongService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();


app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (UserLogin user, IUserService service) => Login(user, service));

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "artist-seller, administrator")]
    (Song song, ISongService service) => Create(song, service));

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    (int id, ISongService service) => Get(id, service));

app.MapGet("/list", (ISongService service) => List(service));

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "artist-seller, administrator")]
    (Song newSong, ISongService service) => Update(newSong, service));

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "administrator")]
    (int id, ISongService service) => Delete(id, service));


IResult Login(UserLogin user, IUserService service)
{
    if (string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
    {
        return Results.BadRequest("Email and password fields cannot be left empty");
    }

    var loggedInUser = service.Get(user);
    if (loggedInUser == null) return Results.NotFound("User was not found");

    var claims = new[]
    {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.Email),
            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256)
        );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(tokenString);
}


IResult Create(Song song, ISongService service)
{
    var result = service.Create(song);

    // We use Results.Ok here and not just Ok because we want the Ok method to return a IResult object
    return Results.Ok(result);
}

IResult Get(int id, ISongService service)
{ 
    var result = service.Get(id);

    if (result == null) return Results.NotFound($"Song with ID of {id} was not found.");

    return Results.Ok(result); 
}

IResult List(ISongService service) => Results.Ok(service.GetAll());

IResult Update(Song song, ISongService service)
{
    var result = service.Update(song);

    if (result == null) return Results.NotFound($"Song with ID of {song.Id} was not found.");

    return Results.Ok(result);
}

IResult Delete(int id, ISongService service)
{
    var result = service.Delete(id);

    if (!result) Results.BadRequest("Error. Song could not be deleted");

    return Results.Ok(result);
}


app.UseSwaggerUI();

app.Run();
