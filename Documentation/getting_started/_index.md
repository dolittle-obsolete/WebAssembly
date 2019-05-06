---
title: Getting started
description: Get started with Dolittle WebAssembly support
keywords: WebAssembly, Getting Started
author: einari
weight: 2
---
This section describes how you can get started with WebAssembly support.

{{% notice information %}}
We recommend you get familiar with [Dolittle, its runtime, building blocks and SDK](/runtime).
{{% /notice %}}

## Introduction

The Dolittle WebAssembly support can be used from do different perspectives:

- Want to have a client / server version and be able to go to client and be offline, sharing code between them
- Client only - no server, or the code on the server is different from the client

Today, the WebAssembly support is oriented around .NET and leveraging the work of 

## .NET

```csharp
using Dolittle.Booting;
using Dolittle.Interaction.WebAssembly.Interop;
```

```csharp
var bootResult = Bootloader.Configure(_ => _
    .WithEntryAssembly(typeof(Program).Assembly)
    .WithAssembliesSpecifiedIn(typeof(Program).Assembly)
    .SynchronousScheduling()
    .NoLogging()
).Start();
```

```csharp
using Dolittle.Booting;
using Dolittle.Interaction.WebAssembly.Interop;

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

Logging has a noticable impact on performance as well, so in production you really don't want to
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

### General

Packages

### Side by Side
