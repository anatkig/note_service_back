var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.Configure<StorageConfig>(builder.Configuration.GetSection("AzureTableStorage"));
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
