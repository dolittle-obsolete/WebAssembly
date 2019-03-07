# Dolittle WebAssembly Interaction Layer

## TEMPORARY

When releasing package from locally:

```shell
$ export APPVEYOR_BUILD_NUMBER=<build number>
```


## Cloning

This repository has sub modules, clone it with:

```shell
$ git clone --recursive <repository url>
```

If you've already cloned it, you can get the submodules by doing the following:

```shell
$ git submodule update --init --recursive
```

## Building

To build the .NET part of this, you have use the `Build` submodule
To build, run one of the following:

Windows:

```shell
$ Build\build.cmd
```

Linux / macOS

```shell
$ Build\build.sh
```

For the JavaScript client parts, you need to first of all restore all packages.
This project relies on [yarn](https://yarnpkg.com) as a pre-requisite. You simply
then run 

```shell
$ yarn
```

At the root to restore packages.

To build all the different JavaScript packages locally for use in for instance the sample,
you simply run:

```shell
$ yarn build
```

## Packages

| Production   | From CI  |
| ------- | ------ |
| [![NuGet](https://img.shields.io/nuget/v/dolittle.svg)](https://www.nuget.org/packages?q=dolittle) | [![MyGet](https://img.shields.io/myget/dolittle/vpre/dolittle.svg)](https://www.myget.org/gallery/dolittle) |

## Build Status

[![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/it6m104jmt5lr57g?svg=true)](https://ci.appveyor.com/project/Dolittle/webassembly)

## Visual Studio

You can open the `.sln` file in the root of the repository and just build directly.

## VSCode

From the `Build` submdoule there is also a .vscode folder that gets a symbolic link for the root. This means you can open the
root of the repository directly in Visual Studio Code and start building. There are quite a few build tasks, so click F1 and type "Run Tasks" and select the "Tasks: Run Tasks"
option and then select the build task you want to run. It is folder sensitive and will look for the nearest `.csproj` file based on the file you have open.
If it doesn't find it, it will pick the `.sln` file instead.

## Pre-requisites

If you're packaging the solution on Windows, you'll need to have [7-Zip](http://www.7-zip.org/) installed.

## Usage

To use this, you'll need to have a .NET Standard 2.0 class library project and reference the NuGet package called `Dolittle.Interaction.WebAssembly.Build`. This will build and output the necessary WebAssembly files to get you started.
It also outputs an `index.html` file.

### Output

The default output points to `./publish`, this can be changed by adding a variable to a `<PropertyGroup>` in your `.csproj` file for your project.

```xml
<PropertyGroup>
    <WasmOutput>../Web/wwwroot/wasm</WasmOutput>
</PropertyGroup>
```

This will then output all artifacts needed to run your application.

## Mono Wasm

This solution is built on top of the [Mono Wasm](https://github.com/mono/mono/tree/master/sdks/wasm) solution.
The package creation is the one responsible for packaging this project and including the necessary dependencies.
To keep this repository as lightweight as possible the Mono Wasm dependency is downloaded and included at packaging time.

### Updating the Mono Wasm reference to a greater version

To update the Mono Wasm build to a greater version - one needs to update the [Core.csproj](./Source/Core/Core.csproj).
The `DownloadFile` task points to a `SourceUrl` - this is the attribute that needs to be updated:

```xml
<DownloadFile DestinationFolder="."
              DestinationFileName="mono-wasm.zip"
              SourceUrl="https://jenkins.mono-project.com/job/test-mono-mainline-wasm/label=ubuntu-1804-amd64/2016/Azure/processDownloadRequest/2016/ubuntu-1804-amd64/sdks/wasm/mono-wasm-33189ef31f3.zip"/>
```

You update it by finding the URL to for instance the last successful build artifacts from [here](https://jenkins.mono-project.com/job/test-mono-mainline-wasm/label=ubuntu-1804-amd64/lastSuccessfulBuild/Azure/).

Navigate to it and get the link:
![](./GetLink.gif)

## Debugging

https://hackernoon.com/introducing-uno-webassembly-projects-and-debugging-f360d4776df3
https://docs.microsoft.com/en-us/aspnet/core/razor-components/debugging?view=aspnetcore-3.0
https://www.hanselman.com/blog/CompilingCToWASMWithMonoAndBlazorThenDebuggingNETSourceWithRemoteDebuggingInChromeDevTools.aspx

Mono Issue:
https://github.com/mono/mono/issues/8378

## More details

To learn more about the projects of Dolittle and how to contribute, please go [here](https://github.com/dolittle/Home).

## Getting Started

Go to our [documentation site](http://www.dolittle.io) and learn more about the project and how to get started.
Samples can also be found [here](https://github.com/Dolittle-Samples).
You can find entropy projects [here](https://github.com/Dolittle-Entropy).
