using Sdl3Sharp.Video.Coloring;
using Sdl3Sharp.Video.Drawing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

/// <summary>
/// Represents the internal data of a <see cref="Text"/> instance
/// </summary>
/// <remarks>
/// <para>
/// You can use the <see cref="TextData"/> associated with a <see cref="Text"/> instance in order to render the text when it's associated with a custom text engine.
/// </para>
/// <para>
/// You can access the <see cref="TextData"/> of a <see cref="Text"/> instance via the <see cref="TextExtensions.get_Data(Text)"/> property.
/// </para>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public sealed partial class TextData
{
	[NotNull]
	private readonly Text mText;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal TextData([NotNull] Text text) => mText = text;

	/// <exception cref="ObjectDisposedException">The <see cref="Text"/> associated with this <see cref="TextData"/> has already been disposed</exception>
	[NotNull]
	private unsafe TTF_TextData* Target
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		[return: NotNull]
		get
		{
			if ((mText.Pointer is var textPtr && textPtr is null)
				|| (textPtr->Internal is var dataPtr && dataPtr is null))
			{
				[DoesNotReturn]
				static void failTextDisposed() => throw new ObjectDisposedException(nameof(Text), $"The {nameof(Text)} associated with this {nameof(TextData)} has already been disposed.");

				failTextDisposed();
			}

			return dataPtr;
		}
	}

	/// <summary>
	/// Gets the <see cref="Font"/> associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, if any
	/// </summary>
	/// <value>
	/// Gets the <see cref="Font"/> associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, or <see langword="null"/> if the <see cref="Text"/> has no font associated with it
	/// </value>
	/// <inheritdoc cref="Target"/>
	public Font? Font
	{
		get
		{
			unsafe
			{
				Font.TryGetOrCreate(Target->Font, out var font);
				return font;
			}
		}
	}

	/// <summary>
	/// Gets the <see cref="Color"/> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </summary>
	/// <value>
	/// The <see cref="Color"/> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </value>
	/// <inheritdoc cref="Target"/>
	public Color<float> Color { get { unsafe { return Target->Color; } } }

	/// <summary>
	/// Gets or sets a value indicating whether the layout of the <see cref="Text"/> that this <see cref="TextData"/> belongs to needs to be updated
	/// </summary>
	/// <value>
	/// A value indicating whether the layout of the <see cref="Text"/> that this <see cref="TextData"/> belongs to needs to be updated
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <see langword="true"/>, the layout of the <see cref="Text"/> will be updated the next time the <see cref="Text"/> is updated.
	/// </para>
	/// <para>
	/// Like <see cref="NeedsEngineUpdate"/>, if the value of this property is <see langword="true"/>,
	/// it will cause the <see cref="Text"/> to be <see cref="TextEngine.InitializeText(Text)">initialized</see> again by the <see cref="TextEngine"/>
	/// the next time the <see cref="Text"/> is updated, after the layout update.
	/// Unlike <see cref="NeedsEngineUpdate"/>, it will also cause the <see cref="Text"/> to be <see cref="TextEngine.CleanupText(Text)">deinitialized</see> by the <see cref="TextEngine"/>
	/// the next time the <see cref="Text"/> is updated, before the layout update.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Target"/>
	public bool NeedsLayoutUpdate
	{
		get { unsafe { return Target->NeedsLayoutUpdate; } }
		set { unsafe { Target->NeedsLayoutUpdate = value; } }
	}

	/// <summary>
	/// Gets the text area of the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </summary>
	/// <value>
	/// The text area of the <see cref="Text"/> that this <see cref="TextData"/> belongs to, in pixels
	/// </value>
	/// <remarks>
	/// <para>
	/// The <see cref="Rect{T}.Left"/> coordinate of the result specifies the horizontal offset of the upper-left corner of the <see cref="Text"/>,
	/// the <see cref="Rect{T}.Top"/> coordinate of the result specifies the vertical offset of the upper-left corner of the <see cref="Text"/>,
	/// the <see cref="Rect{T}.Width"/> of the result specifies the width of the <see cref="Text"/>,
	/// and the <see cref="Rect{T}.Height"/> of the result specifies the height of the <see cref="Text"/>.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Target"/>
	public Rect<int> Rect
	{
		get
		{
			unsafe
			{
				// X, Y, W, and H (in that order) are laid out in TTF_TextData in a way that lets us just reinterpret the memory directly as a Rect<int>, which is very convenient and efficient.
				return *unchecked((Rect<int>*)&Target->X);
			}
		}
	}

	/// <summary>
	/// Gets the collection of the <see cref="DrawOperation"/>s used to render the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </summary>
	/// <value>
	/// The collection of the <see cref="DrawOperation"/>s used to render the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </value>
	/// <remarks>
	/// <para>
	/// If you use a custom text engine to render the <see cref="Text"/> that this <see cref="TextData"/> belongs to,
	/// these are the general operations the custom text engine should perform in order to render the <see cref="Text"/>.
	/// </para>
	/// </remarks>
	public DrawOperationCollection DrawOperations { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(this); }

	/// <summary>
	/// Gets the collection of clusters of glyphs, represented as <see cref="SubString"/>s, in the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </summary>
	/// <value>
	/// The collection of clusters of glyphs, represented as <see cref="SubString"/>s, in the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property essentially partitions the text of the <see cref="Text"/> that this <see cref="TextData"/> belongs to into clusters of glyphs, represented as <see cref="SubString"/>s.
	/// </para>
	/// </remarks>
	public ClusterCollection Clusters { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(this); }

	/// <summary>
	/// Get the <see cref="Text.Properties">properties</see> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to
	/// </summary>
	/// <value>
	/// The <see cref="Text.Properties">properties</see> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to, or <see langword="null"/> if the <see cref="Text"/> has no properties associated with it yet
	/// </value>
	/// <remarks>
	/// <para>
	/// The value of this property will be the same properties as <see cref="Text.Properties"/> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to.
	/// If the value of this property is <see langword="null"/>, it means that the <see cref="Text"/> that this <see cref="TextData"/> belongs to has no properties associated with it yet,
	/// as they would be created automatically the first time <see cref="Text.Properties"/> on the <see cref="Text"/> instance is accessed.
	/// Unlike <see cref="Text.Properties"/>, accessing this property will not cause the properties to be created if they don't exist yet, and will simply return <see langword="null"/> in that case.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Target"/>
	public Properties? Properties
	{
		get
		{
			unsafe
			{
				return Target->Props switch
				{
					0 => null,
					var id => Properties.GetOrCreate(sdl: null, id)
				};
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether the <see cref="Text"/> that this <see cref="TextData"/> belongs to needs to be updated by the associated <see cref="TextEngine"/>
	/// </summary>
	/// <value>
	/// A value indicating whether the <see cref="Text"/> that this <see cref="TextData"/> belongs to needs to be updated by the associated <see cref="TextEngine"/>
	/// </value>
	/// <remarks>
	/// <para>
	/// Like <see cref="NeedsLayoutUpdate"/>, if the value of this property is <see langword="true"/>,
	/// it will cause the <see cref="Text"/> to be <see cref="TextEngine.InitializeText(Text)">initialized</see> again by the <see cref="TextEngine"/>
	/// the next time the <see cref="Text"/> is updated, after the layout update if <see cref="NeedsLayoutUpdate"/> is also <see langword="true"/>.
	/// Unlike <see cref="NeedsLayoutUpdate"/>, it will not cause the <see cref="Text"/> to be <see cref="TextEngine.CleanupText(Text)">deinitialized</see> by the <see cref="TextEngine"/>,
	/// as that should already have happened when the <see cref="Text.Engine"/> of the <see cref="Text"/> that this <see cref="TextData"/> belongs to was set or changed
	/// (which will in turn cause the value of this property to be set to <see langword="true"/>).
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="Target"/>
	public bool NeedsEngineUpdate
	{
		get { unsafe { return Target->NeedsEngineUpdate; } }
		set { unsafe { Target->NeedsEngineUpdate = value; } }
	}

	/// <summary>
	/// Gets the current <see cref="TextEngine"/> associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, if any
	/// </summary>
	/// <value>
	/// The current <see cref="TextEngine"/> associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, or <see langword="null"/> if the <see cref="Text"/> has no text engine associated with it
	/// </value>
	/// <inheritdoc cref="Target"/>
	public TextEngine? Engine
	{
		get
		{
			unsafe
			{
				TextEngine.TryGetOrCreate(Target->Engine, out var engine);
				return engine;
			}
		}
	}

	// We rename "TTF_TextData.EngineText" to "EngineData" in the public API, because it's mostly used as a way to associate additional data with a Text object for use in a custom text engine implementation.
	// SDL calls it "EngineText" because it was their way of allowing custom text implementations for custom text engines, lacking OOP capabilities.
	/// <summary>
	/// Gets or sets custom additional data associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, for use in custom text engine implementations
	/// </summary>
	/// <value>
	/// Custom additional data associated with the <see cref="Text"/> that this <see cref="TextData"/> belongs to, for use in custom text engine implementations
	/// </value>
	/// <remarks>
	/// <para>
	/// If you use a custom text engine to render the <see cref="Text"/> that this <see cref="TextData"/> belongs to,
	/// you can use this property to store additional, text engine-specific data to be associated with the <see cref="Text"/>.
	/// This data can include information that the text engine needs to render the <see cref="Text"/>.
	/// </para>
	/// <para>
	/// To render the <see cref="Text"/>s, text engines can access this property anytime as long as they are the current text engine associated with the <see cref="Text"/>.
	/// Text engines should initialize the data in <see cref="TextEngine.InitializeText(Text)"/> and perform deinitialization with the data in <see cref="TextEngine.CleanupText(Text)"/>.
	/// </para>
	/// </remarks>
	public object? EngineData
	{
		get
		{
			unsafe
			{
				var dataPtr = Target->EngineText;

				if (dataPtr is not null && GCHandle.FromIntPtr((IntPtr)dataPtr) is { IsAllocated: true, Target: var data })
				{
					// If we set the data (i.e., the data was set from managed code and therefore represents a valid GCHandle),
					// we just return the managed object it is pointing to.
					return data;
				}

				// Either the data pointer is null, in which case its sensible to return null,
				// or the data pointer points to unmanaged memory (or an once valid GCHandle died somehow in the meantime), in which case we decide to return null as well,
				// to prevent exposing arbitrary memory to the user.
				// In either case, it should be the reasonable choice to simply return null.
				return null;
			}
		}

		set
		{
			unsafe
			{
				var target = Target; // cache the target pointer to avoid multiple dereferences, which is more efficient and also makes the code cleaner
				var dataPtr = target->EngineText;

				if (dataPtr is not null && GCHandle.FromIntPtr((IntPtr)dataPtr) is { IsAllocated: true, Target: var data } gcHandle)
				{
					if (ReferenceEquals(value, data))
					{
						// We're already set and there's nothing to do
						return;
					}

					// First, we must free the existing GCHandle to prevent memory leaks or GC-stalling with the old data object
					gcHandle.Free();
				}

				if (value is null)
				{
					// If the value is null, we set the data pointer to null as well, which should be valid
					target->EngineText = null;
				}
				else
				{
					// If the value is not null, we allocate a GCHandle for it and set the data pointer to the handle's value.
					// We allocate the handle with GCHandleType.Normal to prevent the GC from collecting the object in the case it's no longer referenced from managed code,
					// but the user might want to retrieve it later via a TextData instance.
					target->EngineText = unchecked((void*)GCHandle.ToIntPtr(GCHandle.Alloc(value, GCHandleType.Normal)));

					// Note that it is imperative that we must make sure to free an existing GCHandle in our base implementation of "DestroyText"
					// for a "TTF_TextEngine" that's represented by a TextEngine instance!
				}
			}
		}
	}
}
