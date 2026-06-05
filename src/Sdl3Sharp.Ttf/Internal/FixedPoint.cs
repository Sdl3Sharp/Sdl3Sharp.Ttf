using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf.Internal;

internal static class FixedPoint
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double FromSigned16Dot16(uint value)
	{
		var result = value / (double)(1 << 16);

		if (value is >= 1u << 31)
		{
			result -= 1 << 16;
		}

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint ToSigned16Dot16(double value)
	{
		if (value is < 0)
		{
			value += 1 << 16;
		}

		return unchecked((uint)(value * (1 << 16)));
	}
}
