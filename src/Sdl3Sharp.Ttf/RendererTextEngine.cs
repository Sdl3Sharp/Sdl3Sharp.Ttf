using Sdl3Sharp.Video.Rendering;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a text engine that can be used to render <see cref="Text"/> using a <see cref="Renderer"/>
/// </summary>
/// <remarks>
/// <para>
/// You can set the <see cref="Text.Engine"/> property of a <see cref="Text"/> to an instance of this type,
/// and then call the <see cref="Text.TryDrawToRenderer(float, float)"/> method on the same <see cref="Text"/> to draw the text to the render target of the associated <see cref="Renderer"/>.
/// </para>
/// </remarks>
public sealed partial class RendererTextEngine : TextEngine
{
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="SdlException">The <see cref="RendererTextEngine"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe RendererTextEngine(TTF_TextEngine* engine) : base(engine)
	{
		if (Pointer is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateRendererTextEngine() => throw new SdlException($"Could not create the {nameof(RendererTextEngine)}");

			failCouldNotCreateRendererTextEngine();
		}
	}

	/// <inheritdoc cref="RendererTextEngine(TTF_TextEngine*)"/>
	private unsafe RendererTextEngine(Renderer renderer, IUnsafeConstructorDispatch? _ = default) :
		this(TTF_CreateRendererTextEngine(renderer is not null ? renderer.Pointer : null))
	{ }

	/// <summary>
	/// Creates a new <see cref="RendererTextEngine"/> using the specified <see cref="Renderer"/>
	/// </summary>
	/// <param name="renderer">The <see cref="Renderer"/> to use for the text engine</param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called from the thread that created the renderer.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="RendererTextEngine(Renderer, IUnsafeConstructorDispatch?)"/>
	public RendererTextEngine(Renderer renderer) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(renderer, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe static TTF_TextEngine* CreateWithProperties(Renderer? renderer, int? atlasTextureSize, Properties? properties)
	{
		Properties propertiesUsed;
		Unsafe.SkipInit(out IntPtr? rendererBackup);
		Unsafe.SkipInit(out int? atlasTextureSizeBackup);

		if (properties is null)
		{
			propertiesUsed = [];

			if (renderer is not null)
			{
				propertiesUsed.TrySetPointerValue(PropertyNames.CreateRendererPointer, unchecked((IntPtr)renderer.Pointer));
			}

			if (atlasTextureSize is int atlasTextureSizeValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, atlasTextureSizeValue);
			}
		}
		else
		{
			propertiesUsed = properties;

			if (renderer is not null)
			{
				rendererBackup = propertiesUsed.TryGetPointerValue(PropertyNames.CreateRendererPointer, out var exisitingRendererPtr)
					? exisitingRendererPtr
					: null;

				propertiesUsed.TrySetPointerValue(PropertyNames.CreateRendererPointer, unchecked((IntPtr)renderer.Pointer));
			}

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
			return TTF_CreateRendererTextEngineWithProperties(propertiesUsed.Id);
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

				if (renderer is not null)
				{
					if (rendererBackup is IntPtr rendererPtr)
					{
						propertiesUsed.TrySetPointerValue(PropertyNames.CreateRendererPointer, rendererPtr);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateRendererPointer);
					}
				}

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

	/// <inheritdoc cref="RendererTextEngine(TTF_TextEngine*)"/>
	private unsafe RendererTextEngine(Renderer? renderer, int? atlasTextureSize, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		this(CreateWithProperties(renderer, atlasTextureSize, properties))
	{ }

	/// <summary>
	/// Creates a new <see cref="RendererTextEngine"/> using the specified <see cref="Renderer"/> and additional properties
	/// </summary>
	/// <param name="renderer">The <see cref="Renderer"/> to use for the text engine</param>
	/// <param name="atlasTextureSize">The size of the texture atlas</param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called from the thread that created the renderer.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="RendererTextEngine(Renderer?, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public RendererTextEngine(Renderer renderer, int? atlasTextureSize = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(renderer, atlasTextureSize, properties, default(IUnsafeConstructorDispatch?))
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
				TTF_DestroyRendererTextEngine(engine);
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
