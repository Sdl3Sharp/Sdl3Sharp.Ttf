#if SDL_TTF3_3_0_OR_GREATER

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a text engine that can be used to render <see cref="Text"/> using OpenGL
/// </summary>
/// <remarks>
/// <para>
/// You are responsible for ensuring that the OpenGL context is properly set up and alive while using <see cref="GLTextEngine"/>s.
/// </para>
/// <para>
/// You can set the <see cref="Text.Engine"/> property of a <see cref="Text"/> to an instance of this type,
/// and then call the <see cref="Text.TryGetGLDrawData(out Sdl3Sharp.Ttf.GLAtlasDrawSequenceEnumerable)"/> method on the same <see cref="Text"/> to get OpenGL geometry data representing the text.
/// You then have to render the resulting geometry yourself using the appropriate OpenGL APIs.
/// </para>
/// </remarks>
public sealed partial class GLTextEngine : TextEngine
{
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="SdlException">The <see cref="GLTextEngine"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe GLTextEngine(TTF_TextEngine* engine) : base(engine)
	{
		if (Pointer is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateGLTextEngine() => throw new SdlException($"Could not create the {nameof(GLTextEngine)}");

			failCouldNotCreateGLTextEngine();
		}
	}

	/// <inheritdoc cref="GLTextEngine(TTF_TextEngine*)"/>
	private unsafe GLTextEngine(IUnsafeConstructorDispatch? _ = default) :
		this(TTF_CreateGLTextEngine())
	{ }

	/// <summary>
	/// Creates a new <see cref="GLTextEngine"/>
	/// </summary>
	/// <remarks>
	/// <para>
	/// You are responsible for ensuring that the OpenGL context is properly set up and alive while using the resulting text engine.
	/// </para>
	/// <para>
	/// This constructor should be only called from the thread that created the OpenGL context.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="GLTextEngine(IUnsafeConstructorDispatch?)"/>
	public GLTextEngine() :
		this(default(IUnsafeConstructorDispatch?))
	{ }

	private unsafe static TTF_TextEngine* CreateWithProperties(int? atlasTextureSize, Properties? properties)
	{
		Properties propertiesUsed;
		Unsafe.SkipInit(out int? atlasTextureSizeBackup);

		if (properties is null)
		{
			propertiesUsed = [];

			if (atlasTextureSize is int atlasTextureSizeValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, atlasTextureSizeValue);
			}
		}
		else
		{
			propertiesUsed = properties;

			if (atlasTextureSize is int atlasTextureSizeValue)
			{
				atlasTextureSizeBackup = propertiesUsed.TryGetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, out var existingAtlasTextureSize)
					? unchecked((int)existingAtlasTextureSize)
					: null;

				propertiesUsed.TrySetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, atlasTextureSizeValue);
			}
		}

		try
		{
			return TTF_CreateGLTextEngineWithProperties(propertiesUsed.Id);
		}
		finally
		{
			if (properties is null)
			{
				// propertiesUsed was just a temporary instance we created for this call, so we need to dispose it now

				propertiesUsed.Dispose();
			}
			else
			{
				// we restore the original properties values from the given properties instance

				if (atlasTextureSize.HasValue)
				{
					if (atlasTextureSizeBackup is int atlasTextureSizeValue)
					{
						propertiesUsed.TrySetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, atlasTextureSizeValue);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateAtlasTextureSizeNumber);
					}
				}
			}
		}
	}

	/// <inheritdoc cref="GLTextEngine(TTF_TextEngine*)"/>
	private unsafe GLTextEngine(int? atlasTextureSize, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		this(CreateWithProperties(atlasTextureSize, properties))
	{ }

	/// <summary>
	/// Creates a new <see cref="GLTextEngine"/> with additional properties
	/// </summary>
	/// <param name="atlasTextureSize">The size of the texture atlas</param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// You are responsible for ensuring that the OpenGL context is properly set up and alive while using the resulting text engine.
	/// </para>
	/// <para>
	/// This constructor should be only called from the thread that created the OpenGL context.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="GLTextEngine(int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public GLTextEngine(int? atlasTextureSize = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(atlasTextureSize, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Gets or sets the winding order of the vertices returned by <see cref="Text.TryGetGLDrawData(out GLAtlasDrawSequenceEnumerable)"/> when used with this <see cref="GLTextEngine"/>
	/// </summary>
	/// <value>
	/// The winding order of the vertices returned by <see cref="Text.TryGetGLDrawData(out GLAtlasDrawSequenceEnumerable)"/> when used with this <see cref="GLTextEngine"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the thread that created the text engine.
	/// </para>
	/// </remarks>
	public GLTextEngineWinding Winding
	{
		get
		{
			unsafe
			{
				return TTF_GetGLTextEngineWinding(Pointer);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetGLTextEngineWinding(Pointer, value);
			}
		}
	}

	/// <inheritdoc/>
	protected sealed override void Dispose(bool disposing)
	{
		unsafe
		{
			var engine = Pointer;

			if (engine is not null)
			{
				TTF_DestroyGLTextEngine(engine);
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

#endif
