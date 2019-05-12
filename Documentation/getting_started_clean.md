---
title: Getting started clean
description: Get started with Dolittle WebAssembly support - clean
keywords: WebAssembly, Getting Started
author: einari
weight: 2
---
This section describes how you can get started with the Dolittle WebAssembly support, cleanest possible way.
If you're looking for a finished end to end sample with everything set up, you can go [here](https://github.com/dolittle-samples/ToDolittle).
Also, look the the other [getting started]({{< relref getting_started >}}) guide for a more end to end guide.

## .NET Core

At the root of it sits .NET Core. All our support is built around this with both the code for it to run
and the tooling around it. So to get started you'll need to do create a `classlib` representing the
starting point. Create a folder for your client e.g. `Client`.

```shell
$ dotnet new classlib
```

The Mono SDK for WebAssembly has been packaged into a single NuGet package, called [Dolittle.Interaction.WebAssembly.Core](https://www.nuget.org/packages/Dolittle.Interaction.WebAssembly.Core/).
This is a heavy package with everything needed for Monos WebAssembly support inside it.

Dolittle has a specific build pipeline that we need to get the expected WebAssembly output.
After creating the `classlib`, Add the following dependencies to your `.csproj` file:

```xml
<ItemGroup>
    <PackageReference Include="Dolittle.Build.MSBuild" Version="3.*"/>
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Build" Version="3.*"/>
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Core" Version="3.*"/> 
</ItemGroup>
```

## EntryPoint

The next thing we need is a class holding the entrypoint of the application.
Add a C# file called `Program.cs` and make it look like the following:

```csharp
using System;

namespace Client
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello from WebAssembly");
    }
}
```

## Build & Run

Doing a ``dotnet build`` will now generate a `publish` folder with all that is necessary to run.
We then need an HTML file to start it all. Add a file called `index.html` to the `publish` folder.

```html
<html>

<head>
</head>

<body>
    <script src="mono-config.js"></script>
    <script src="mono.js" async></script>
    <script>
        window.Module = {};

        window.Module.onRuntimeInitialized = function() {
            MONO.mono_load_runtime_and_bcl(
                'managed',
                'managed',
                0,
                config.file_list,
                function() {
                    Module.mono_bindings_init("[WebAssembly.Bindings]WebAssembly.Runtime");
                    BINDING.call_static_method("[Client] Client.Program:Main", []);
                }
            );
        };
    </script>
</body>

</html>
```

In order to run it, we need to have a web server that runs it all. This can basically be anything,
for instance [http-serve](https://www.npmjs.com/package/http-serve) or similar.

Run the web server from the `publish` folder and navigate to the URL the WebServer set up.
Open the developer tools of the browser and navigate to the console, you should be seeing something
like the following:
![Hello from WebAssembly](../hello_from_webassembly.png).
