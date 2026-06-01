using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using System.Runtime.CompilerServices;
using CBool = Sdl3Sharp.Internal.Interop.CBool;

namespace Sdl3Sharp.Ttf;

partial class Ttf
{
	/// <summary>
	/// Queries the version of the FreeType library in use
	/// </summary>
	/// <param name="major">To be filled in with the major version number. Can be NULL.</param>
	/// <param name="minor">To be filled in with the minor version number. Can be NULL.</param>
	/// <param name="patch">To be filled in with the patch version number. Can be NULL.</param>
	/// <remarks>
	/// <para>
	/// <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Init">TTF_Init</see>() should be called before calling this function.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetFreeTypeVersion">TTF_GetFreeTypeVersion</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_GetFreeTypeVersion(int* major, int* minor, int* patch);

	/// <summary>
	/// Queries the version of the HarfBuzz library in use.
	/// </summary>
	/// <param name="major">To be filled in with the major version number. Can be NULL.</param>
	/// <param name="minor">To be filled in with the minor version number. Can be NULL.</param>
	/// <param name="patch">To be filled in with the patch version number. Can be NULL.</param>
	/// <remarks>
	/// <para>
	/// If HarfBuzz is not available, the version reported is 0.0.0.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetHarfBuzzVersion">TTF_GetHarfBuzzVersion</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_GetHarfBuzzVersion(int* major, int* minor, int* patch);

	/// <summary>
	/// Initializes SDL_ttf
	/// </summary>
	/// <returns>Returns true on success or false on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// You must successfully call this function before it is safe to call any other function in this library.
	/// </para>
	/// <para>
	/// It is safe to call this more than once, and each successful <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Init">TTF_Init</see>() call should be paired with a matching <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Quit">TTF_Quit</see>() call.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_Init">TTF_Init</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial CBool TTF_Init();

	/// <summary>
	/// Deinitializes SDL_ttf
	/// </summary>
	/// <remarks>
	/// <para>
	/// You must call this when done with the library, to free internal resources. It is safe to call this when the library isn't initialized, as it will just return immediately.
	/// </para>
	/// <para>
	/// Once you have as many quit calls as you have had successful calls to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Init">TTF_Init</see>, the library will actually deinitialize.
	/// </para>
	/// <para>
	/// Please note that this does not automatically close any fonts that are still open at the time of deinitialization, and it is possibly not safe to close them afterwards, as parts of the library will no longer be initialized to deal with it.
	/// A well-written program should call <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CloseFont">TTF_CloseFont</see>() on any open fonts before calling this function!
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_Quit">TTF_Quit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial void TTF_Quit();

	/// <summary>
	/// This function gets the version of the dynamically linked SDL_ttf library
	/// </summary>
	/// <returns>Returns SDL_ttf version</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_Version">TTF_Version</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial Version TTF_Version();

	/// <summary>
	/// Checks if SDL_ttf is initialized
	/// </summary>
	/// <returns>Returns the current number of initialization calls, that need to eventually be paired with this many calls to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Quit">TTF_Quit</see>()</returns>
	/// <remarks>
	/// <para>
	/// This reports the number of times the library has been initialized by a call to <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Init">TTF_Init</see>(),
	/// without a paired deinitialization request from <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_Quit">TTF_Quit</see>().
	/// </para>
	/// <para>
	/// In short: if it's greater than zero, the library is currently initialized and ready to work. If zero, it is not initialized.
	/// </para>
	/// <para>
	/// Despite the return value being a signed integer, this function should not return a negative number.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_WasInit">TTF_WasInit</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal static partial int TTF_WasInit();
}
