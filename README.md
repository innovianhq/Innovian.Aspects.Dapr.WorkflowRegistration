<a href="https://innovian.net">
	<p align="center">
		<img src="https://innovian.net/img/bluelogo.svg" width="100px"/>
	</p>
</a>

#### Innovian.Aspets.Dapr.WorkflowRegistration
[![NuGet](https://img.shields.io/nuget/v/Innovian.Aspects.Dapr.WorkflowRegistration.svg)](https://www.nuget.org/packages/Innovian.Aspects.Dapr.WorkflowRegistration/)

#### Innovian.Aspects.Dapr.WorkflowRegistration.Fabric
[![Build Status](https://dev.azure.com/innovian/Innovian%20Open%20Source/_apis/build/status%2FMetalama%20Aspects%2FInnovian.Aspects.Dapr.WorkflowRegistration.Fabric?branchName=main)](https://dev.azure.com/innovian/Innovian%20Open%20Source/_build/latest?definitionId=337&branchName=main)  [![NuGet](https://img.shields.io/nuget/v/Innovian.Aspects.Dapr.WorkflowRegistration.Fabric.svg)](https://www.nuget.org/packages/Innovian.Aspects.Dapr.WorkflowRegistration.Fabric/)

This project is provided to the larger open-source community by [Innovian](https://innovian.net).

This solution implements an aspect using [Metalama](https://github.com/postsharp/Metalama) targeting .NET 8 that provides a utility intended for use with [Dapr Workflows](https://docs.dapr.io/developing-applications/building-blocks/workflow/workflow-overview/).
When using .NET, the expected approach is to register the various Workflow and Workflow Activities in the DI registration process, but this can be cumbersome and time-consuming to maintain, especially
in projects surfacing a great many such types. Rather than rely on a reflection-based approach to locate each of these types during startup, this aspect provides the same capability but at compilation
time, ensuring that all Workflow and Workflow Activities in the project are properly registered in DI as expected, limiting the potential for developer error and bugs that show up online at runtime.

## What does it do?

The attribute performs the following:
- Introduces a new class into the project with the namespace `<Program namespace>.DaprUtilities` where "<Program namespace>" is the namespace used in your Program.cs file within a public, partial, static class called `DaprRegistrationHelper`.
- Introduces a new method into this type named "DaprRegistrationHelper".
- When provided with a list of Workflows and Workflow Activities from the Fabric, this will introduce a registration for each into the introduced method.


## Usage
While a package is available on NuGet for the attribute itself, it's intended that the target developer only utilize the Fabric to apply this attribute. This is because it's the Fabric that locates the 
Workflow and Workfow Activity types in the project and compiles them for the attribute to build the registration method in the target type. Applying the attribute directly to a `DaprRegistrationHelper` class
will only introduce the `RegisterAllEntities` method, but will not actually register any of the Workflows or Workflow Activities.

The Fabric identifies Workflows by looking for those types in the project that implement an abstract base type called "Workflow" and identifies Workflow Activities by looking for those types 
in the project that implement an abstract base type called "WorkflowActivity".

### Installation

Using the .NET CLI tools:
```sh
dotnet add package Innovian.Aspects.Dapr.WorkflowRegistration.Fabric
```

Using the Package Manager Console:
```powershell
Install-Package Innovian.Aspects.Dapr.WorkflowRegistration.Fabric
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on the project within your solution you wish to add the attribute to.
3. Click on "Manage NuGet Packages...".
4. Click on the "Browse" tab and search for "Innovian.Aspects.Dapr.WorkflowRegistration.Fabric".
5. Click on the "Innovian.Aspects.Dapr.WorkflowRegistration.Fabric" package, select the appropriate version in the right-tab and click *Install*.

### Usage
No additional effort is necessary beyond installation of the `Innovian.Aspects.Dapr.WorkflowRegistration.Fabric` package on the project to configure the fabric or the applied aspect. It will automatically identify all each of the 
Workflow and Workflow Activities in the project, introduce the static type `DaprRegistrationHelper` and a static method called `RegisterAllEntities`. Because it's not yet possible to modify statements within a method at this time, registration is
left as an exercise to the use of both the Dapr Workflow client and the Dapr Workflows (via the introduced static method).

For example, let's say you've got a simple Program.cs and you want to register everything for Dapr. You'd start off with the following:
```cs
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

And to register your Dapr workflows, simply update to reflect the following (note your namespace will likely vary based on your own Program.cs namespace):
```cs
using Dapr.Workflow;
using DaprUtilities;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprWorkflowClient();
builder.Services.AddDaprWorkflow(DaprRegistrationHelper.RegisterAllEntities);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

The `builder.Services.AddDaprWorkflowClient();` statement is required for typical Dapr Workflow client registration and is not impacted by this attribute, but is included for completeness.
However, the following line `builder.Services.AddDaprWorkflow(DaprRegistrationHelper.RegisterAllEntities);` will not work until the aspect has been applied to your project as neither
the `DaprRegistrationHelper` static class nor its internal method will necessarily already exist in your project. Rather, these will be introduced by the fabric and aspect and are
invoked precisely as you see above as though you'd written them yourself. And that's it!

Happy coding!