using Sdl3Sharp.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a lifetime of SDL3# TTF
/// </summary>
/// <remarks>
/// <para>
/// You must have at least one instance of this type alive in order to use most of the functionality provided by SDL3# TTF.
/// </para>
/// </remarks>
public sealed partial class Ttf : IDisposable
{
	private static uint mInstanceCount;

	private bool mAlive;

	/// <summary>
	/// Creates a new instance of <see cref="Ttf"/> and initializes SDL3# TTF if it hasn't been initialized yet
	/// </summary>
	/// <remarks>
	/// <para>
	/// It is totally fine to have multiple instances of this type alive at the same time, as long as they all get <see cref="Dispose">disposed</see> eventually.
	/// Once the last instance of this type gets disposed, SDL3# TTF will be deinitialized.
	/// </para>
	/// </remarks>
	public Ttf()
	{
		SdlErrorHelper.ThrowIfFailed(TTF_Init());

		// If the call to TTF_Init failed, it should throw before we get here, so the instance counter isn't unintentionally incremented.
		Interlocked.Increment(ref mInstanceCount);

		mAlive = true;
	}

	/// <inheritdoc/>
	~Ttf() => DisposeImpl();

	/// <summary>
	/// Disposes this instance of <see cref="Ttf"/> and deinitializes SDL3# TTF if this is the last instance of <see cref="Ttf"/> to be disposed
	/// </summary>
	/// <remarks>
	/// <para>
	/// It is generally recommended that you dispose all resources that depend on SDL3# TTF before disposing the last instance of <see cref="Ttf"/>,
	/// to avoid potential memory leaks or other issues that might arise.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		DisposeImpl();
	}

	private void DisposeImpl()
	{
		if (mAlive)
		{
			mAlive = false;

			// This whole shenanigan with the dispose receivers is just to overcome the limitation 
			// that native SDL_ttf might not automatically close TTF_Fonts when TTF_Quit is called and it's the last call that would cause SDL_ttf to be deinitialized.
			// It seems like TTF_Fonts are NOT safe to be closed after SDL_ttf is deinitialized.
			// So to make things a bit safer on the managed side, we should do something about that limitation.
			// The official documentation even warns about that, but doesn't say anything about other types of resources, just TTF_Fonts.
			// Anyway, this abstraction enables us to keep track of the initialization count of SDL_ttf, and inform any objects that might not get automatically cleaned up
			// that SDL_ttf is about to be deinitialized, and they should clean up themselves before that happens.

			try
			{
				if (Interlocked.Decrement(ref mInstanceCount) is not > 0)
				{
					try
					{
						var exceptions = new Queue<Exception>();

						foreach ((var reference, _) in mDisposeReceivers)
						{
							if (reference.TryGetTarget(out var diposeReceiver))
							{
								try
								{
									diposeReceiver.DisposeFromTtf();
								}
								catch (Exception exception)
								{
									exceptions.Enqueue(exception);
								}
							}
						}

						if (exceptions.Count is > 0)
						{
							throw new AggregateException(exceptions);
						}
					}
					finally
					{
						mDisposeReceivers.Clear();
					}
				}
			}
			finally
			{
				TTF_Quit();
			}
		}
	}

	/// <summary>
	/// Gets the version of FreeType in use by the native SDL_ttf library
	/// </summary>
	/// <value>
	/// The version of FreeType in use by the native SDL_ttf library
	/// </value>
	/// <remarks>
	/// <para>
	/// You should at least have one <see cref="Ttf"/> instance alive when accessing this property,
	/// otherwise the result may not be accurate (SDL3# TTF must be initialized in order to query the FreeType version it is using).
	/// </para>
	/// </remarks>
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

	/// <summary>
	/// Gets the version of HarfBuzz in use by the native SDL_ttf library
	/// </summary>
	/// <value>
	/// The version of HarfBuzz in use by the native SDL_ttf library
	/// </value>
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

	/// <summary>
	/// Gets the version of the native SDL_ttf library in use
	/// </summary>
	/// <value>
	/// The version of the native SDL_ttf library in use
	/// </value>
	public static Version Version => TTF_Version();
}
