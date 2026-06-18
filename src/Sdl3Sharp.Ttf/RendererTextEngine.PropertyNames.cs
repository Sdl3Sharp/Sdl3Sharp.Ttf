using Sdl3Sharp.Video.Rendering;

namespace Sdl3Sharp.Ttf;

partial class RendererTextEngine
{
	/// <summary>
	/// Provides property names for <see cref="RendererTextEngine"/> properties
	/// </summary>
	public new sealed class PropertyNames : TextEngine.PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="RendererTextEngine(Renderer, int?, Properties?)">property used when creating a <see cref="RendererTextEngine"/></see>
		/// that holds a pointer to a native <c>SDL_Renderer</c> instance to use for the text engine
		/// </summary>
		public const string CreateRendererPointer = "SDL_ttf.renderer_text_engine.create.renderer";

		/// <summary>
		/// The name of a <see cref="RendererTextEngine(Renderer, int?, Properties?)">property used when creating a <see cref="RendererTextEngine"/></see>
		/// that holds the size of the texture atlas to use for the text engine
		/// </summary>
		public const string CreateAtlasTextureSizeNumber = "SDL_ttf.renderer_text_engine.create.atlas_texture_size";

		private PropertyNames() { }
	}
}
