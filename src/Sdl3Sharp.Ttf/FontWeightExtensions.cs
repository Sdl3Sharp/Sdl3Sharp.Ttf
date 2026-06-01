#if SDL_TTF3_2_2_OR_GREATER

using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Provides extension methods for <see cref="FontWeight"/>
/// </summary>
public static class FontWeightExtensions
{
	extension(FontWeight)
	{
		/// <summary>
		/// Creates a custom <see cref="FontWeight"/> from an integer value
		/// </summary>
		/// <param name="weight">The weight value, typically in the range of 100 (<see cref="FontWeight.Thin"/>) to 950 (<see cref="FontWeight.ExtraBlack"/>)</param>
		/// <returns>A <see cref="FontWeight"/> representing the specified weight value</returns>
		/// <remarks>
		/// <para>
		/// This method does not perform any validation on the input <paramref name="weight"/> value.
		/// If the value is outside of the allowed range, it might lead to unexpected or undefined behavior when using the resulting <see cref="FontWeight"/>.
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static FontWeight Custom(int weight) => unchecked((FontWeight)weight);
	}
}

#endif
