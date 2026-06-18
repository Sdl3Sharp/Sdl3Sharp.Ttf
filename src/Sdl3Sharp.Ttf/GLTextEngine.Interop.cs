#if SDL_TTF3_3_0_OR_GREATER

using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

partial class GLTextEngine
{
	/// <summary>
	/// Creates a text engine for drawing text with OpenGL
	/// </summary>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The caller is responsible for ensuring the correct OpenGL context is current when calling this function and when using the resulting text engine.
	/// </para>
	/// <para>
	/// The GL text engine and all text created with it become invalid if the OpenGL context is destroyed. Destroy the engine before destroying the context.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the OpenGL context.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngine">TTF_CreateGLTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateGLTextEngine();

	/// <summary>
	/// Creates a text engine for drawing text with OpenGL, with extra properties
	/// </summary>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// The caller is responsible for ensuring the correct OpenGL context is current when calling this function and when using the resulting text engine.
	/// </para>
	/// <para>
	/// The following properties are supported:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_GL_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER"><c>TTF_PROP_GL_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER</c></see></term>
	///			<description>The size of the texture atlas</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the OpenGL context.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngineWithProperties">TTF_CreateGLTextEngineWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateGLTextEngineWithProperties(uint props);

	/// <summary>
	/// Destroy a text engine created for drawing text with OpenGL
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngine">TTF_CreateGLTextEngine</see>()</param>
	/// <remarks>
	/// <para>
	/// All text created by this engine should be destroyed before calling this function.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DestroyGLTextEngine">TTF_DestroyGLTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_DestroyGLTextEngine(TTF_TextEngine* engine);

	/// <summary>
	/// Get the winding order of the vertices returned by <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGLTextDrawData">TTF_GetGLTextDrawData</see> for a particular GL text engine
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngine">TTF_CreateGLTextEngine</see>()</param>
	/// <returns>Returns the winding order used by the GL text engine or <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GL_TEXTENGINE_WINDING_INVALID">TTF_GL_TEXTENGINE_WINDING_INVALID</see> in case of error</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGLTextEngineWinding">TTF_GetGLTextEngineWinding</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial GLTextEngineWinding TTF_GetGLTextEngineWinding(TTF_TextEngine* engine);

	/// <summary>
	/// Sets the winding order of the vertices returned by <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGLTextDrawData">TTF_GetGLTextDrawData</see> for a particular GL text engine
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGLTextEngine">TTF_CreateGLTextEngine</see>()</param>
	/// <param name="winding">The new winding order option</param>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetGLTextEngineWinding">TTF_SetGLTextEngineWinding</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetGLTextEngineWinding(TTF_TextEngine* engine, GLTextEngineWinding winding);
}

#endif
