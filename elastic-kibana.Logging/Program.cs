using elastic_kibana.Logging.Interface;
using elastic_kibana.Logging.Service;
using ElasticSearch.Core.Configuration;
using ElasticSearch.Logging.ElasticSearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ElasticConnectionSettings>(builder.Configuration.GetSection("ElasticConnectionSettings"));
builder.Services.AddTransient(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
builder.Services.AddSingleton<ElasticClientProvider>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
