using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using Sdl3Sharp.Video.Rendering;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

partial class RendererTextEngine
{
	/// <summary>
	/// Creates a text engine for drawing text on an SDL renderer
	/// </summary>
	/// <param name="renderer">The renderer to use for creating textures and drawing text</param>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the renderer.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateRendererTextEngine">TTF_CreateRendererTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateRendererTextEngine(Renderer.SDL_Renderer* renderer);

	/// <summary>
	/// Creates a text engine for drawing text on an SDL renderer, with the specified properties
	/// </summary>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_RENDERER_TEXT_ENGINE_RENDERER_POINTER"><c>TTF_PROP_RENDERER_TEXT_ENGINE_RENDERER_POINTER</c></see></term>
	///			<description>The renderer to use for creating textures and drawing text</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_RENDERER_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER"><c>TTF_PROP_RENDERER_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER</c></see></term>
	///			<description>The size of the texture atlas</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the renderer.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateRendererTextEngineWithProperties">TTF_CreateRendererTextEngineWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateRendererTextEngineWithProperties(uint props);

	/// <summary>
	/// Destroys a text engine created for drawing text on an SDL renderer
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateRendererTextEngine">TTF_CreateRendererTextEngine</see>()</param>
	/// <remarks>
	/// <para>
	/// All text created by this engine should be destroyed before calling this function.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DestroyRendererTextEngine">TTF_DestroyRendererTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_DestroyRendererTextEngine(TTF_TextEngine* engine);
}
