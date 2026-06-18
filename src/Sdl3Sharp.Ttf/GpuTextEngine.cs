using Sdl3Sharp.Video.Gpu;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a text engine that can be used to render <see cref="Text"/> using a <see cref="GpuDevice"/>
/// </summary>
/// <remarks>
/// <para>
/// This text engine enables you to draw text as part of your GPU rendering pipeline.
/// </para>
/// <para>
/// You can set the <see cref="Text.Engine"/> property of a <see cref="Text"/> to an instance of this type,
/// and then call the <see cref="Text.TryGetGpuDrawData(out Sdl3Sharp.Ttf.GpuAtlasDrawSequenceEnumerable)"/> method on the same <see cref="Text"/> to get GPU geometry data representing the text.
/// You then have to render the resulting geometry yourself using the appropriate GPU APIs.
/// </para>
/// </remarks>
public sealed partial class GpuTextEngine : TextEngine
{
	private interface IUnsafeConstructorDispatch;

	/// <exception cref="SdlException">The <see cref="GpuTextEngine"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe GpuTextEngine(TTF_TextEngine* engine) : base(engine)
	{
		if (Pointer is null)
		{
			[DoesNotReturn]
			static void failCouldNotCreateGpuTextEngine() => throw new SdlException($"Could not create the {nameof(GpuTextEngine)}");

			failCouldNotCreateGpuTextEngine();
		}
	}

	/// <inheritdoc cref="GpuTextEngine(TTF_TextEngine*)"/>
	private unsafe GpuTextEngine(GpuDevice device, IUnsafeConstructorDispatch? _ = default) :
		this(TTF_CreateGPUTextEngine(device is not null ? device.Pointer : null))
	{ }

	/// <summary>
	/// Creates a new <see cref="GpuTextEngine"/> using the specified <see cref="GpuDevice"/>
	/// </summary>
	/// <param name="device">The <see cref="GpuDevice"/> to use for the text engine</param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called from the thread that created the GPU device.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="GpuTextEngine(GpuDevice, IUnsafeConstructorDispatch?)"/>
	public GpuTextEngine(GpuDevice device) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(device, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	private unsafe static TTF_TextEngine* CreateWithProperties(GpuDevice? device, int? atlasTextureSize, Properties? properties)
	{
		Properties propertiesUsed;
		Unsafe.SkipInit(out IntPtr? deviceBackup);
		Unsafe.SkipInit(out int? atlasTextureSizeBackup);

		if (properties is null)
		{
			propertiesUsed = [];

			if (device is not null)
			{
				propertiesUsed.TrySetPointerValue(PropertyNames.CreateGpuDevicePointer, unchecked((IntPtr)device.Pointer));
			}

			if (atlasTextureSize is int atlasTextureSizeValue)
			{
				propertiesUsed.TrySetNumberValue(PropertyNames.CreateAtlasTextureSizeNumber, atlasTextureSizeValue);
			}
		}
		else
		{
			propertiesUsed = properties;

			if (device is not null)
			{
				deviceBackup = propertiesUsed.TryGetPointerValue(PropertyNames.CreateGpuDevicePointer, out var existingDevicePtr)
					? existingDevicePtr
					: null;

				propertiesUsed.TrySetPointerValue(PropertyNames.CreateGpuDevicePointer, unchecked((IntPtr)device.Pointer));
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
			return TTF_CreateGPUTextEngineWithProperties(propertiesUsed.Id);
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

				if (device is not null)
				{
					if (deviceBackup is IntPtr devicePtr)
					{
						propertiesUsed.TrySetPointerValue(PropertyNames.CreateGpuDevicePointer, devicePtr);
					}
					else
					{
						propertiesUsed.TryRemove(PropertyNames.CreateGpuDevicePointer);
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

	/// <inheritdoc cref="GpuTextEngine(TTF_TextEngine*)"/>
	private unsafe GpuTextEngine(GpuDevice device, int? atlasTextureSize, Properties? properties, IUnsafeConstructorDispatch? _ = default) :
		this(CreateWithProperties(device, atlasTextureSize, properties))
	{ }

	/// <summary>
	/// Creates a new <see cref="GpuTextEngine"/> using the specified <see cref="GpuDevice"/> and additional properties
	/// </summary>
	/// <param name="device">The <see cref="GpuDevice"/> to use for the text engine</param>
	/// <param name="atlasTextureSize">The size of the texture atlas</param>
	/// <param name="properties">Additional properties</param>
	/// <remarks>
	/// <para>
	/// This constructor should be only called from the thread that created the GPU device.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="GpuTextEngine(GpuDevice, int?, Properties?, IUnsafeConstructorDispatch?)"/>
	public GpuTextEngine(GpuDevice device, int? atlasTextureSize = default, Properties? properties = default) :
#pragma warning disable IDE0034 // For the sake of explicitness
		this(device, atlasTextureSize, properties, default(IUnsafeConstructorDispatch?))
#pragma warning restore IDE0034
	{ }

	/// <summary>
	/// Gets or sets the winding order of the vertices returned by <see cref="Text.TryGetGpuDrawData(out GpuAtlasDrawSequenceEnumerable)"/> when used with this <see cref="GpuTextEngine"/>
	/// </summary>
	/// <value>
	/// The winding order of the vertices returned by <see cref="Text.TryGetGpuDrawData(out GpuAtlasDrawSequenceEnumerable)"/> when used with this <see cref="GpuTextEngine"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// This property should only be accessed from the thread that created the text engine.
	/// </para>
	/// </remarks>
	public GpuTextEngineWinding Winding
	{
		get
		{
			unsafe
			{
				return TTF_GetGPUTextEngineWinding(Pointer);
			}
		}

		set
		{
			unsafe
			{
				TTF_SetGPUTextEngineWinding(Pointer, value);
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
				TTF_DestroyGPUTextEngine(engine);
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
