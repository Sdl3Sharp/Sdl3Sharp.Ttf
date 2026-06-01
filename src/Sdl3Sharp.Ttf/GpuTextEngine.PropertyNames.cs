namespace Sdl3Sharp.Ttf;

partial class GpuTextEngine
{
	public new sealed class PropertyNames : TextEngine.PropertyNames
	{
		public const string GpuDevicePointer = "SDL_ttf.gpu_text_engine.create.device";
		public const string AtlasTextureSizeNumber = "SDL_ttf.gpu_text_engine.create.atlas_texture_size";

		private PropertyNames() { }
	}
}
