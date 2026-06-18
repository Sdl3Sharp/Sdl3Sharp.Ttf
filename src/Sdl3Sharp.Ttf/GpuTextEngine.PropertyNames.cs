using Sdl3Sharp.Video.Gpu;

namespace Sdl3Sharp.Ttf;

partial class GpuTextEngine
{
	/// <summary>
	/// Provides property names for <see cref="GpuTextEngine"/> properties
	/// </summary>
	public new sealed class PropertyNames : TextEngine.PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="GpuTextEngine(GpuDevice, int?, Properties?)">property used when creating a <see cref="GpuTextEngine"/></see>
		/// that holds a pointer to a native <c>SDL_GPUDevice</c> instance to use for the text engine
		/// </summary>
		public const string CreateGpuDevicePointer = "SDL_ttf.gpu_text_engine.create.device";

		/// <summary>
		/// The name of a <see cref="GpuTextEngine(GpuDevice, int?, Properties?)">property used when creating a <see cref="GpuTextEngine"/></see>
		/// that holds the size of the texture atlas to use for the text engine
		/// </summary>
		public const string CreateAtlasTextureSizeNumber = "SDL_ttf.gpu_text_engine.create.atlas_texture_size";

		private PropertyNames() { }
	}
}
