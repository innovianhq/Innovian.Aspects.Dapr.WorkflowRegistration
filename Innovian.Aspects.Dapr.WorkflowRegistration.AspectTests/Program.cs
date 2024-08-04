using Dapr.Workflow;
using DaprUtilities;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprWorkflowClient();
builder.Services.AddDaprWorkflow(DaprRegistrationHelper.RegisterAllEntities);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();