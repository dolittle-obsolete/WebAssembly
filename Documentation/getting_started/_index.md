---
title: Getting started
description: Get started with Dolittle WebAssembly support
keywords: WebAssembly, Getting Started
author: einari
weight: 2
---
This section describes how you can get started with WebAssembly support.
If you're looking for a finished sample with everything set up, you can go [here](https://github.com/dolittle-samples/ToDolittle).

{{% notice information %}}
If you're going to use all of Dolittle and not just the [Fundamentals](/fundamentals) part,
we recommend you get familiar with [Dolittle, its runtime, building blocks and SDK](/runtime).

You can use bits and pieces and you do not need to take in all of Dolittle to be able to
make use of our WebAssembly support.
{{% /notice %}}

## Introduction

The Dolittle WebAssembly support can be used from do different perspectives:

- Want to have a client / server version and be able to go to client and be offline, sharing code between them
- Client only - no server, or the code on the server is different from the client

Today, the WebAssembly support is oriented around .NET and leveraging the work of [the Mono Project](https://www.mono-project.com).
On top of this you'll need a Web based project based on the framework of your liking, be it a Single Page Framework or server side
rendering. The mechanics for the Web part is standard and totally agnostic. However, since we at Dolittle internally
uses Aurelia, we've added extra love for Aurelia to make it even simpler for hooking up things.

{{% notice information %}}
It is recommended that you have a look at our [Quickstart](https://dolittle.io/getting-started/quickstart/) and
our [walktrough](https://dolittle.io/getting-started/gettingstarted/), as this guide builds upon its shoulders.

For the time being, we don't have [boilerplates](http://github.com/dolittle-boilerplates) for WebAssembly, and
that means we need to do a bit more manual work to get started.
{{% /notice %}}

## .NET Core

At the root of it sits .NET Core. All our support is built around this with both the code for it to run
and the tooling around it. So to get started you'll need to do create a `classlib` representing the
starting point. Create a folder for your client e.g. `Client`.

```shell
$ dotnet new classlib
```

The Mono SDK for WebAssembly has been packaged into a single NuGet package, called [Dolittle.Interaction.WebAssembly.Core](https://www.nuget.org/packages/Dolittle.Interaction.WebAssembly.Core/). This is a heavy package with everything needed for Monos WebAssembly support
inside it.

Dolittle has a specific build pipeline, and we do need that as well. This walkthrough also assumes the Dolittle.SDK
and also assumes Autofac as the IoC container, MongoDB as database and our own development Event Store.
Add the following dependencies to your `.csproj` file:

```xml
<ItemGroup>
    <PackageReference Include="Dolittle.SDK" Version="3.*" />

    <PackageReference Include="Dolittle.DependencyInversion.Autofac" Version="3.*" />

    <PackageReference Include="Dolittle.Interaction.WebAssembly.Booting" Version="3.*" />
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Interop" Version="3.*" />
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Commands" Version="3.*" />
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Queries" Version="3.*" />
    <PackageReference Include="Dolittle.Events.WebAssembly.Dev" Version="3.*" />
    <PackageReference Include="Dolittle.ReadModels.MongoDB.WebAssembly" Version="3.*" />

    <PackageReference Include="Dolittle.Interaction.WebAssembly.Core" Version="3.*" />

    <PackageReference Include="Dolittle.Build.MSBuild" Version="3.*"/>
    <PackageReference Include="Dolittle.SDK.Build" Version="3.*" />
    <PackageReference Include="Dolittle.Interaction.WebAssembly.Build" Version="3.*" />
</ItemGroup>
```

Dolittle has a specific build pipeline that does a few things, amongst others it generates metadata
about your application in the form of a topology and all artifacts within that topology.
The topology is a map of the application in Dolittle terminology of Application, Bounded Context,
Modules and Features. Within these are [artifacts](https://dolittle.io/runtime/runtime/artifacts/).
The SDK build tool is responsible for creating this information and then an additional build tool for
our WebAssembly stack sits on top of this and embeds the information into the application being
built.

The SDK part can be configured, according to the documentation found [here](/runtime/dotnet-sdk/tooling/build_tool/).
For the most part we can probably leave it with the default values. All depending on what your starting
point is and what you're trying to achieve.

Below is an example of setting up the properties in the `.csproj`

```xml
<PropertyGroup>
    <!-- The relative path from this .csproj file to the bounded-context.json configuration file -->
    <DolittleBoundedContextConfigPath>./bounded-context.json</DolittleBoundedContextConfigPath>

    <!-- Whether or not to use modules or not when generating bounded context topology structure -->
    <DolittleUseModules>False</DolittleUseModules>

    <!--  A | separated Key/Value pair map of namespace segments to strip -->
    <DolittleNamespaceSegmentsToStrip/>

    <!-- Whether or not the build tool should generate proxies -->
    <DolittleGenerateProxies>True</DolittleGenerateProxies>

    <!-- The relative path to put proxies if generated-->
    <DolittleProxiesBasePath>../Web/Features</DolittleProxiesBasePath>

    <!-- The relative path where the .dolittle folder is located for generating artifacts, 
    this can be taken out if you're not having a side-by-side server scenario where you're sharing
    artifacts/topology -->
    <DolittleFolder>../Core/.dolittle</DolittleFolder>
</PropertyGroup>

```
{{% notice information %}}
The Dolittle tooling relies on a configuration file called `bounded-context.json`, this governs
the types of resources being used for things like Read Models and Event Store. This configuration
file does not have the dimension of types of runtime environments such as WebAssembly as an
interaction layer. You therefor need a copy of this file if you want to support both a server/client
scenario and a WebAssembly scenario side-by-side for the same codebase. We have an issue registered
to make this easier - found [here](https://github.com/dolittle-runtime/DotNET.SDK/issues/204).
{{% /notice %}}

### EntryPoint

The next thing we need is a class holding the entrypoint of the application.
Add a C# file called `Program.cs`.

We then need the following using statement:

```csharp
using Dolittle.Booting;
```

Add a static `Main()` method:

```csharp
static void Main(string[] args)
{
}
```

Inside this we can then configure Dolittle and the `Bootloader`.

```csharp
var bootResult = Bootloader.Configure(_ => _
    .WithEntryAssembly(typeof(Program).Assembly)
    .WithAssembliesSpecifiedIn(typeof(Program).Assembly)
    .SynchronousScheduling()
    .NoLogging()
).Start();
```

You should then have a file looking like something like this.

```csharp
using Dolittle.Booting;

namespace Client
{

    class Program
    {
        static void Main(string[] args)
        {
            var bootResult = Bootloader.Configure(_ => _
                .WithEntryAssembly(typeof(Program).Assembly)
                .WithAssembliesSpecifiedIn(typeof(Program).Assembly)
                .SynchronousScheduling()
                .NoLogging()
            ).Start();
       }
    }
}
```

{{% notice information %}}
Being early days, not all moving parts are in place. As you can notice, the `.NoLogging()` option
will give no logging. If you want to have logging on with the Dolittle logging infrastructure, you
will have to add your own `ILogAppender` implementation.

Logging has a noticeable impact on performance as well, so in production you really don't want to
have logging on.
{{% /notice %}}

To enable logging using the Dolittle logging infrastructure, add a `CustomLogAppender` file to your
project and add the following code:

```csharp
using Dolittle.Logging;

public class CustomLogAppender : ILogAppender
{
    public void Append(string filePath, int lineNumber, string member, LogLevel level, string message, Exception exception = null)
    {
        if (exception != null)
        {
            System.Console.WriteLine($"{System.DateTime.UtcNow} - ({filePath} - {member}[{lineNumber}]) - {level} - {message} - {exception.Message} - {exception.StackTrace}");
        }
        else
        {
            System.Console.WriteLine($"{System.DateTime.UtcNow} - ({filePath} - {member}[{lineNumber}]) - {level} - {message}");
        }
    }
}
```

Instead of the `.NoLogging()` you can then do `.UseLogAppender(new CustomLogAppender())`

You can now build using `dotnet build` from your shell and you should be seeing at the end of the build something similar:

```shell
Perform post build tasks
    Copying files for output (Post Task: 'Dolittle.Interaction.WebAssembly.Build.CopyFiles, Dolittle.Interaction.WebAssembly.Build, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')
      Copied 191 to output folder './publish/managed'
      Copied 104 to output folder './publish/managed'
      Copied 1 to output folder './publish'
      Copied 2 to output folder './publish'

  Time Elapsed 0:00:00:02.2163990 (Dolittle)

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:07.31
```

This means that all the artifacts from the WebAssembly side is now available in the `./publish` folder.
This is what we want to make use of in the Web solution.

## Web

### mono-config.js

In the `publish` folder you'll find a file called `mono-config.js`. This is an optional file, if you just
want to add a `<script/>` tag and get started without module loading or WebPack. You simply just include it
in your HTML file.

### assemblies.json

The `assemblies.json` file in the `publish` folder holds a list of all the assemblies to include in loading,
and if you built the .NET solution with the default `debug` configuration, it will also include the PDB - debugging
files. This file can be used directly in your JavaScript code by importing it and use the array directly
when configuring Mono to start.

### Service Worker

If you want to support an offline scenario, you want to make all the files
needed to run available for offline use. We use a [service worker](https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API)
for this purpose. Luckily, we have a [WebPack plugin](https://www.npmjs.com/package/@dolittle/webassembly.webpack) that takes information coming from the .NET build and generates the necessary file needed for this purpose.

All you need to do is add the package to your NPM based Web project:

```shell
$ npm install --save-dev @dolittle/webassembly.webpack
```

All you then need to do is import the plugin and add it to the plugins.

```javascript
const { ServiceWorkerGenerator } = require('@dolittle/webassembly.webpack');

// Assuming you have an instance of your WebPack config object
config.plugins.push(new ServiceWorkerGenerator());
```

### WebPack

In WebPack you can also leverage the [copy-webpack-plugin](https://www.npmjs.com/package/copy-webpack-plugin)
to copy the necessary files from the `Client` .NET projects output:

```javascript
const CopyWebpackPlugin = require('copy-webpack-plugin');

// Assuming you have an instance of your WebPack config object
config.plugins.push(new CopyWebpackPlugin([
    { from: 'publish/managed/**/*', to: 'managed', flatten: true },
    { from: 'publish/mono.js', to: 'mono.js', flatten: true },
    { from: 'publish/mono.wasm', to: 'mono.wasm', flatten: true },
    { from: 'manifest.json', to: 'manifest.json', flatten: true },
    { from: 'dolittle.png', to: 'dolittle.png', flatten: true }
]));
```

## Aurelia

As mentioned, we have a specific [Aurelia plugin](https://www.npmjs.com/package/@dolittle/webassembly.aurelia)
to make things easier for Aurelia developers. You simply add a reference to it in your project and configure
it as below:

```javascript
import * as assemblies from '../publish/assemblies.json';

export function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .plugin(PLATFORM.moduleName('@dolittle/webassembly.aurelia'), {
            entryPoint: "[Client] Client.Program:Main", // Method to call to start your application
                                                        // Format:
                                                        // [Assembly] Namespace.Class:Method
            assemblies: assemblies.default,
            offline: false
        });
}
```

### Non Aurelia

The plugin for Aurelia simplifies things for developers using Aurelia - what it does can be seen [here](https://github.com/dolittle-interaction/WebAssembly/blob/master/Source/Client.Aurelia/index.js).

Basically, it gets the Mono WASM runtime initialized and also sets up the Dolittle EventStore and gets the MongoDB/Minimongo initialized and ready to be used.

#### Configuring Mono in the browser

```javascript
import { EventProcessorOffsetRepository } from '@dolittle/runtime.events.webassembly.dev/Processing';
import { storage } from '@dolittle/runtime.events.webassembly.dev';
import * as assemblies from '../../Client/publish/assemblies.json';

storage.preload()
        .then(_ => EventProcessorOffsetRepository.preload())
        .then(_ => EventStore.preload()).catch(error => {
    console.error('Error preloading', error);
}).then(_ => {
    // Now the rest of the system should be ready for booting
    window.Module = {};

    window.Module.onRuntimeInitialized = () => {
        MONO.mono_load_runtime_and_bcl(
            'managed',          // Virtual filesystem prefix
            'managed',          // folder to look for managed DLLs
            0,                  // Wether or not to enable debugging (0: false, 1: true)
            assemblies,         // 
            () => {
                Module.mono_bindings_init("[WebAssembly.Bindings]WebAssembly.Runtime");
                BINDING.call_static_method("[Client] Client.Program:Main", []); // Method to call to start your application
                                                                                // Format:
                                                                                // [Assembly] Namespace.Class:Method
            }
        );
    };

    let monoScript = document.createElement('script');
    monoScript.async = true;
    monoScript.src = config.monoScript;
    document.body.appendChild(monoScript);

    if (config.offline === true) {
        navigator.serviceWorker.register('service-worker.js');
    }

    window._mongoDB = new MongoDB();
    window._eventStore = {
        eventProcessorOffsetRepository: new EventProcessorOffsetRepository(),
        eventStore: new EventStore()
    }
}
```

{{% notice information %}}
Right now, our interop uses global variables. This is something we're looking into improving.
As you can see in code above, it adds `_mongoDB` and `_eventStore` as global variables.
These are needed for the C# implementations to be able to work with the underlying data storage
mechanism in the browser; [IndexedDB](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API).
{{% /notice %}}
