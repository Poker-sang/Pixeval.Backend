using Pixeval.Backend;
using Pixeval.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSqlite<PixevalDbContext>("Data Source=pixeval-database.db");

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (await TempService.InitDbContextAsync(app.Services))
{
    await TempService.InitIllustrationsUsersAsync(app.Services);
    await TempService.InitFavoriteAsync(app.Services);
    await TempService.InitFollowAsync(app.Services);
}

app.Run();
