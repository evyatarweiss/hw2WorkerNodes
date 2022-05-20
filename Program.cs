using WorkNode.WorkNode;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var firstWorkerThread = new WorkerNode();
var secondWorkerThread = new WorkerNode();
var CurrentVirtualMachine = new CurrentVirtualMachine(firstWorkerThread, secondWorkerThread);

Task.Run(() => firstWorkerThread.GetItemsFromQueue());
Task.Run(() => secondWorkerThread.GetItemsFromQueue());
