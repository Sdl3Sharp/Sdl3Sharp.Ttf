using Sdl3Sharp.SourceGeneration;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.Internal.Interop;

internal sealed class Library : INativeImportLibrary
{
	static (string? libraryName, DllImportSearchPath? searchPath) INativeImportLibrary.GetLibraryNameAndSearchPath() => (
		"SDL3_ttf",
		DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.UseDllDirectoryForDependencies | DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories
	);
}
