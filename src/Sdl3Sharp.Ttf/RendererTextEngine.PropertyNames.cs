namespace Sdl3Sharp.Ttf;

partial class RendererTextEngine
{
	public new sealed class PropertyNames : TextEngine.PropertyNames
	{
		public const string RendererPointer = "SDL_ttf.renderer_text_engine.renderer";
		public const string AtlasTextureSizeNumber = "SDL_ttf.renderer_text_engine.create.atlas_texture_size";

		private PropertyNames() { }
	}
}
