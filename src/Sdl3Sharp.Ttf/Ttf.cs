using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

public sealed partial class Ttf
{
	public static Version FreeTypeVersion
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out int major);
				Unsafe.SkipInit(out int minor);
				Unsafe.SkipInit(out int patch);

				TTF_GetFreeTypeVersion(&major, &minor, &patch);

				// we just assume that the version number of FreeType in use will fit into a SDL version struct, which is a pretty safe assumption considering that major, minor, and patch should be in the [0, 1000) range
				return new Version(major, minor, patch);
			}
		}
	}

	public static Version HarfBuzzVersion
	{
		get
		{
			unsafe
			{
				Unsafe.SkipInit(out int major);
				Unsafe.SkipInit(out int minor);
				Unsafe.SkipInit(out int patch);

				TTF_GetHarfBuzzVersion(&major, &minor, &patch);

				// we just assume that the version number of HarfBuzz in use will fit into a SDL version struct, which is a pretty safe assumption considering that major, minor, and patch should be in the [0, 1000) range
				return new Version(major, minor, patch);
			}
		}
	}

	public static Version Version => TTF_Version();
}
