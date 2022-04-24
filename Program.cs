using UREC_api;
using Microsoft.EntityFrameworkCore;

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "urec-admin.json");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<UrecContext>(opt => opt.UseSqlite("Data Source=mydb.db;"));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapGet("/", () => "Hello world");

app.MapPost("/generateqr", async (UserFirebaseToken userToken) =>
{
    var invalidTokenResponse = Results.ValidationProblem(new Dictionary<string, string[]>() { { "invalid token", new string[] { "The provided token cannot be verified" } } });
    if (await userToken.CheckToken() == false)
    {
        //????
        return invalidTokenResponse;
    }

    // get user information
    var tokenInfo = await userToken.DecodeToken();
    if (tokenInfo == null)
        return invalidTokenResponse;

    var student = new Student()
    {
        Uid = tokenInfo.Uid,
        Name = tokenInfo.Claims["name"] as String,
        Email = tokenInfo.Claims["email"] as String,
    };
    // generate QR-token and tied that to the information
    var generatedToken = new QRToken(student);
    
    // save the QRtoken to the database
    await UrecFirestore.AddToken(generatedToken);
    // db.QRTokens.Add(generatedToken);
    // await db.SaveChangesAsync();

    //return QRtoken;
    return Results.Json(generatedToken);
    //return Results.ValidationProblem(new Dictionary<string, string[]>() { { "Error", new string[] { "Bruh" } } });
});


// verify token from the user
app.MapPost("/verifyqr", async (QRTokenData token) =>
{
    var invalidTokenResponse = Results.ValidationProblem(new Dictionary<string, string[]>() { { "invalid token", new string[] { "The provided token cannot be verified" } } });

    // verify the token against firebase
    QRToken? comingQrToken = await UrecFirestore.VerifyToken(token.Token);

    if (comingQrToken == null)
        return invalidTokenResponse;
    else
        return Results.Json(comingQrToken);

});

app.Run();

