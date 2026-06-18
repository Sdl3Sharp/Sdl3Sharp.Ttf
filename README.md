# SDL3\# TTF

[![GitHub Release](https://img.shields.io/github/v/release/Sdl3Sharp/Sdl3Sharp.Ttf?logo=github&label=GitHub%20Release
)](https://github.com/Sdl3Sharp/Sdl3Sharp.Ttf/releases/latest)
[![NuGet Version](https://img.shields.io/nuget/v/Sdl3Sharp.Ttf?logo=nuget&label=NuGet%20Package)](https://www.nuget.org/packages/Sdl3Sharp.Ttf/latest)
[![SDL Native Library](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2FSdl3Sharp%2FSdl3Sharp.Ttf%2Frefs%2Fheads%2Fmain%2Fmake.json&query=%24.runtimesVersion&prefix=v&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1%2BjfqAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm%2B48GgAAAB50RVh0VGl0bGUAU2ltcGxlIERpcmVjdE1lZGlhIExheWVy7JOkpQAAANdJREFUKM%2B1kTFLQmEARc8nBr0%2BkMzBGkQQFAuittDf0NAvaHVqEaJd8Cc0tYhNES0F1d5S0OAa4SZBY6FbnNfwKMihzXvXy%2BFyb5D%2FlWPhgTzQoEPCPU3W%2BOSSD7r0CASmGeGWlGuqdIhEnmhzQsKMdZqAeOXUG1uOPRRHnptaEvFYcsAR%2B3xxBkCBDd5%2BC9Qzwrt3vth37KsTB26bOvLZPS8EMXHLslixahSXrFlz16E7PwHmHDzw1E2RIOQpsUxghUiRVQoUeeAxqxH%2BfBGZzQ8VFn%2FWN3OLaWMFnvBEAAAAAElFTkSuQmCC&label=SDL_ttf%20Native%20Library)
](https://github.com/Sdl3Sharp/SDL_ttf)

## About

**SDL3# TTF provides hand-crafted C# language bindings for [SDL_ttf](https://www.libsdl.org/projects/SDL_ttf/)**.

SDL3# TTF is entirely hand-crafted, with no auto-generation of API code based on the native library.\
Every part of the API is deliberately designed to feel native to C#, translating SDL's functionality into idiomatic, well thought-out counterparts that existing C# users should feel right at home with.\
It makes heavy use of modern C# features to provide an API that is expressive and comfortable to work with from managed code, while still exposing the full breadth of SDL's functionality.

> [!WARNING]
> While this project is already in an usable state, its core dependency, [SDL3#](https://github.com/Sdl3Sharp/Sdl3Sharp), is not. So you should treat this project just as work in progress as SDL3# itself, at least for the time being.

## Documentation

Documentation will be coming soon. For the time being, you can have a look at the [Examples](#examples) section below for a quick overview of how to use the library.

## Building

For instructions on how to build the project from source, see [BUILDING.md](BUILDING.md).

## Usage

### Requirements

- C# 14 or later
- .NET 10 or later

### Adding SDL3# as a dependency

SDL3# TTF depends on SDL3#, so you don't necessarily need to explicitly add SDL3# as a dependency in your project, since it will be pulled in transitively.\
However, it is actually recommended to reference SDL3# directly in your project as well, and if only for the sake of clarity.

#### Via your IDE

*(Example: Visual Studio)*

Open the NuGet Package Manager, search for `Sdl3Sharp.Ttf`, check "Include prerelease", and install the latest version.

#### Via `.csproj`

```xml
<PackageReference Include="Sdl3Sharp.Ttf" Version="*-*" />
```

#### Via a file-based C# app

Add the following directive at the top of your file:

```csharp
#:package Sdl3Sharp.Ttf@*-*
```

### Choosing a package variant

The default `Sdl3Sharp.Ttf` package includes pre-built native SDL3_ttf binaries for all supported platforms and is the easiest way to get started.

If you only need to target specific platforms, you can reference the corresponding RID-specific packages instead, for example `Sdl3Sharp.Ttf.win-x64` or `Sdl3Sharp.Ttf.linux-x64`, rather than pulling in binaries for every platform.

If you do not want any native binaries bundled at all, reference `Sdl3Sharp.Ttf.Core` instead. In that case you are responsible for providing the native SDL3_ttf and libffi binaries yourself, placed alongside the resulting executable of your application.

### Examples

#### Hello World

```csharp
// Assuming SDL3# is already initialized (e.g., via `new Sdl(...)`)

// Initialize SDL3# TTF
using var ttf = new Ttf();

// Open a font from a file path, with a point size of 24
using var font = new Font("path/to/font.ttf", 24);

// Render some text to a surface, using blended rendering and white color
if (font.TryRenderBlendedString("Hello World!", foregroundColor: new(r: 255, g: 255, b: 255, a: 255), out var surfaceText))
{
    using (surfaceText)
    {
        // do something with the rendered text
    }
}
```

#### With a `Text` object and a `Renderer`

```csharp
// Assuming SDL3# is already initialized (e.g., via `new Sdl(...)`)
Renderer renderer = ...;

// Initialize SDL3# TTF
using var ttf = new Ttf();

// Open a font from a file path, with a point size of 24
using var font = new Font("path/to/font.ttf", 24);

// Create a `RendererTextEngine` for the renderer
using var textEngine = new RendererTextEngine(renderer);

// Create a `Text` object
using var text = new Text("Hello")
{
    Font = font,
    Engine = textEngine,
    Color = new(r: 255, g: 255, b: 255, a: 255),
};

// You can modify the `Text` object at any time
text.String += " World!";

// Render the text to the renderer at position (100, 100)
if (text.TryDrawToRenderer(x: 100, y: 100))
{
    // Since the `Text` object uses a `RendererTextEngine` as its text engine,
    // the previous call to `TryDrawToRenderer` should have succeeded
    // and the text should now be drawn to the render target of the renderer.
}
```

## A note on AI usage

In the spirit of transparency, and in line with the [contributing guidelines](CONTRIBUTING.md), here is an overview of how AI was used in this project:

- **Documentation.** AI was used to help write and improve documentation. The content itself comes from the author, but AI was used to clarify and clean up the writing.
- **API design.** AI was used as a sounding board for API design decisions. Sometimes the options to consider came from the author, sometimes the AI had useful proposals to offer. All decisions were ultimately made by the author.
- **Infrastructural documents.** AI was used to help write project documents such as this [README.md](README.md), the [BUILDING.md](BUILDING.md), the [CONTRIBUTING.md](CONTRIBUTING.md), and the [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).
- **Code review.** AI was used to review some of the author's code, and occasionally this turned out to be fruitful, catching bugs that might otherwise have been overlooked.
- **Functional code.** No AI was used to write any functional code. All code in this project was written by the author.

## License

SDL3# TTF is licensed under the [MIT License](./LICENSE.md). See [NOTICE.md](./NOTICE.md) for third-party notices.
