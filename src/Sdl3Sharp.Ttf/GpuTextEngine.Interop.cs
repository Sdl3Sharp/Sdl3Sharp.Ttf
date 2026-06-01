using Sdl3Sharp.SourceGeneration;
using Sdl3Sharp.Ttf.Internal.Interop;
using Sdl3Sharp.Video.Gpu;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

partial class GpuTextEngine
{
	/// <summary>
	/// Creates a text engine for drawing text with the SDL GPU API
	/// </summary>
	/// <param name="device">The SDL_GPUDevice to use for creating textures and drawing text</param>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the device.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngine">TTF_CreateGPUTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateGPUTextEngine(GpuDevice.SDL_GPUDevice* device);

	/// <summary>
	/// Creates a text engine for drawing text with the SDL GPU API, with the specified properties
	/// </summary>
	/// <param name="props">The properties to use</param>
	/// <returns>Returns a <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object or NULL on failure; call SDL_GetError() for more information</returns>
	/// <remarks>
	/// <para>
	/// These are the supported properties:
	/// <list type="bullet">
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_GPU_TEXT_ENGINE_DEVICE_POINTER"><c>TTF_PROP_GPU_TEXT_ENGINE_DEVICE_POINTER</c></see></term>
	///			<description>The SDL_GPUDevice to use for creating textures and drawing text</description>
	///		</item>
	///		<item>
	///			<term><see href="https://wiki.libsdl.org/SDL3_ttf/TTF_PROP_GPU_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER"><c>TTF_PROP_GPU_TEXT_ENGINE_ATLAS_TEXTURE_SIZE_NUMBER</c></see></term>
	///			<description>The size of the texture atlas</description>
	///		</item>
	/// </list>
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the device.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngineWithProperties">TTF_CreateGPUTextEngineWithProperties</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial TTF_TextEngine* TTF_CreateGPUTextEngineWithProperties(uint props);

	/// <summary>
	/// Destroys a text engine created for drawing text with the SDL GPU API
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngine">TTF_CreateGPUTextEngine</see>()</param>
	/// <remarks>
	/// <para>
	/// All text created by this engine should be destroyed before calling this function.
	/// </para>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_DestroyGPUTextEngine">TTF_DestroyGPUTextEngine</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_DestroyGPUTextEngine(TTF_TextEngine* engine);

	/// <summary>
	/// Gets the winding order of the vertices returned by <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGPUTextDrawData">TTF_GetGPUTextDrawData</see> for a particular GPU text engine
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngine">TTF_CreateGPUTextEngine</see>()</param>
	/// <returns>Returns the winding order used by the GPU text engine or <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GPU_TEXTENGINE_WINDING_INVALID">TTF_GPU_TEXTENGINE_WINDING_INVALID</see> in case of error</returns>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGPUTextEngineWinding">TTF_GetGPUTextEngineWinding</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial GpuTextEngineWinding TTF_GetGPUTextEngineWinding(TTF_TextEngine* engine);

	/// <summary>
	/// Sets the winding order of the vertices returned by <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_GetGPUTextDrawData">TTF_GetGPUTextDrawData</see> for a particular GPU text engine.
	/// </summary>
	/// <param name="engine">A <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_TextEngine">TTF_TextEngine</see> object created with <see href="https://wiki.libsdl.org/SDL3_ttf/TTF_CreateGPUTextEngine">TTF_CreateGPUTextEngine</see>()</param>
	/// <param name="winding">The new winding order option</param>
	/// <remarks>
	/// <para>
	/// This function should be called on the thread that created the engine.
	/// </para>
	/// </remarks>
	/// <seealso href="https://wiki.libsdl.org/SDL3_ttf/TTF_SetGPUTextEngineWinding">TTF_SetGPUTextEngineWinding</seealso>
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void TTF_SetGPUTextEngineWinding(TTF_TextEngine* engine, GpuTextEngineWinding winding);
}
