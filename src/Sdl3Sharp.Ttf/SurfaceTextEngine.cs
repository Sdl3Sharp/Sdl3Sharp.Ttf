using Sdl3Sharp.Video;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a text engine that can be used to render <see cref="Text"/> to a <see cref="Surface"/>
/// </summary>
/// <remarks>
/// <para>
/// You can set the <see cref="Text.Engine"/> property of a <see cref="Text"/> to an instance of this type,
/// and then call the <see cref="Text.TryDrawToSurface(Surface, int, int)"/> method on the same <see cref="Text"/> to draw the text to a surface.
/// </para>
/// </remarks>
public sealed partial class SurfaceTextEngine : TextEngine
{
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="SdlException">The <see cref="SurfaceTextEngine"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe SurfaceTextEngine(IUnsafeConstructorDispatch? _ = default) :
		base(TTF_CreateSurfaceTextEngine())
	{
		if (Pointer is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateSurfaceTextEngine() => throw new SdlException($"Could not create the {nameof(SurfaceTextEngine)}");

			failCouldNotCreateSurfaceTextEngine();
		}
	}

	/// <summary>
	/// Creates a new <see cref="SurfaceTextEngine"/>
	/// </summary>
	/// <inheritdoc cref="SurfaceTextEngine(IUnsafeConstructorDispatch?)"/>
	public SurfaceTextEngine() :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <inheritdoc/>
	protected sealed override void Dispose(bool disposing)
	{
		unsafe
		{
			var engine = Pointer;

			if (engine is not null)
			{
				TTF_DestroySurfaceTextEngine(engine);
			}

			base.Dispose(disposing);
		}
	}

	/// <summary>Not supported. Do not use this method. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete($"Not supported. Do not use this method. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override void CleanupText(Text text) => throw new NotSupportedException("Calling this method is not supported.");
#pragma warning restore CS0809

	/// <summary>Not supported. Do not use this method. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
	/// <exception cref="NotSupportedException">Always</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete($"Not supported. Do not use this method. This method will always throw a {nameof(NotSupportedException)} if called.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	protected sealed override void InitializeText(Text text) => throw new NotSupportedException("Calling this method is not supported.");
#pragma warning restore CS0809
}
