using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

partial class SurfaceTextEngine
{
	/// <summary>
	/// Creates a text engine for drawing text on SDL surfaces
	/// </summary>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateSurfaceTextEngine">TTF_CreateSurfaceTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateSurfaceTextEngine();

	/// <summary>
	/// Destroys a text engine created for drawing text on SDL surfaces
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateSurfaceTextEngine">TTF_CreateSurfaceTextEngine</see>()</param>
	/// <remarks>
	/// <para>
	/// All text created by this engine should be destroyed before calling this function.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DestroySurfaceTextEngine">TTF_DestroySurfaceTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_DestroySurfaceTextEngine(TTF_TextEngine* engine);
}
