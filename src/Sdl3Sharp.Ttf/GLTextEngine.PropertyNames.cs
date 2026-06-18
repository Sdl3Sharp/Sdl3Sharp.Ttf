#if SDL_TTF3_3_0_OR_GREATER

namespace Sdl3Sharp.Ttf;

partial class GLTextEngine
{
	/// <summary>
	/// Provides property names for <see cref="GLTextEngine"/> properties
	/// </summary>
	public new sealed class PropertyNames : TextEngine.PropertyNames
	{
		/// <summary>
		/// The name of a <see cref="GLTextEngine(int?, Properties?)">property used when creating a <see cref="GLTextEngine"/></see>
		/// that holds the size of the texture atlas to use for the text engine
		/// </summary>
		public const string CreateAtlasTextureSizeNumber = "SDL_ttf.gl_text_engine.create.atlas_texture_size";

		private PropertyNames() { }
	}
}

#endif
