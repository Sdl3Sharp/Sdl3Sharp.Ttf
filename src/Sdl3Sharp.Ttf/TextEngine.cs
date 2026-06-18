using Sdl3Sharp.Ttf.TextEngineImplementation;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

/// <summary>
/// Represents a text engine that can be used to render <see cref="Text"/> instances and manage implementation-specific data associated with <see cref="Text"/>
/// </summary>
/// <remarks>
/// <para>
/// Before a text engine can be used to render <see cref="Text"/> instances, it needs to be associated with those <see cref="Text"/> instances.
/// You can associate a <see cref="TextEngine"/> with a <see cref="Text"/> instance by setting the <see cref="Text.Engine"/> property of the <see cref="Text"/> instance.
/// </para>
/// <para>
/// Make sure that you don't <see cref="Dispose()">dispose</see> <see cref="TextEngine"/>s that are currently in use by <see cref="Text"/> instances.
/// Always <see cref="Text.Dispose()">dispose</see> the <see cref="Text"/> first before disposing the <see cref="TextEngine"/> that it uses.
/// </para>
/// </remarks>
public abstract partial class TextEngine : IDisposable
{
	private interface IUnsafeConstructorDispatch;

	private static readonly ConcurrentDictionary<IntPtr, WeakReference<TextEngine>> mKnownInstances = [];

	private unsafe TTF_TextEngine* mEngine;
	private GCHandle mSelfHandle;
	private readonly bool mFree;

	private unsafe TextEngine(TTF_TextEngine* engine, bool register, bool free)
	{
		mEngine = engine;
		mSelfHandle = default;
		mFree = free;

		if (register)
		{
			if (mEngine is not null)
			{
				mKnownInstances.AddOrUpdate(unchecked((IntPtr)mEngine), addRef, updateRef, this);
			}

			static WeakReference<TextEngine> addRef(IntPtr engine, TextEngine newEngine) => new(newEngine);

			static WeakReference<TextEngine> updateRef(IntPtr engine, WeakReference<TextEngine> existingEngineRef, TextEngine newEngine)
			{
				if (existingEngineRef.TryGetTarget(out var existingEngine))
				{
#pragma warning disable IDE0079
#pragma warning disable CA1816
					GC.SuppressFinalize(existingEngine);
#pragma warning restore CA1816
#pragma warning restore IDE0079

					existingEngine.Dispose(disposing: false, forget: false); // Is disposing: false really correct here? I believe that that's closely related to what a finalizer is supposed to do, so I think the answer is yes.
				}

				existingEngineRef.SetTarget(newEngine);

				return existingEngineRef;
			}
		}
	}

	private protected unsafe TextEngine(TTF_TextEngine* engine) :
		this(engine, register: true, free: false) // free: false because the SDL provided text engines provide their own destroy functions
	{ }

	/// <exception cref="SdlException">The <see cref="TextEngine"/> could not be created (check <see cref="Error.TryGet(out string?)"/> for more information)</exception>
	private unsafe TextEngine(IUnsafeConstructorDispatch? _  = default) :
		// There's no magic and no special constructor for creating a native text engine instance.
		// It's just allocating the memory for it and filling out the interface data (which the constructor of TTF_TextEngine does).
		this(unchecked((TTF_TextEngine*)Utilities.NativeMemory.Malloc((nuint)Unsafe.SizeOf<TTF_TextEngine>())), register: true, free: true)
	{
		unsafe
		{
			if (mEngine is null)
			{
				[DoesNotReturn]
				static void failCouldNotCreateTextEngine() => throw new SdlException($"Could not create the {nameof(TextEngine)}");

				failCouldNotCreateTextEngine();
			}

			*mEngine = new(this, out mSelfHandle);
		}
	}

	/// <summary>
	/// A base constructor for creating new custom <see cref="TextEngine"/> instances
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this constructor as a base constructor when implementing a custom <see cref="TextEngine"/> implementation.
	/// </para>
	/// </remarks>
	/// <inheritdoc cref="TextEngine(IUnsafeConstructorDispatch?)"/>
	protected TextEngine() :
		this(default(IUnsafeConstructorDispatch?))
	{ }

	/// <inheritdoc/>
	~TextEngine() => Dispose(disposing: false, forget: true);

	internal unsafe TTF_TextEngine* Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mEngine; }

	/// <summary>
	/// Initializes a <see cref="Text"/> that's associated or about to be associated with this <see cref="TextEngine"/>
	/// </summary>
	/// <param name="text">The <see cref="Text"/> to initialize</param>
	/// <remarks>
	/// <para>
	/// You can and should use this method to initialize the provided <see cref="Text"/> instance with any implementation-specific data that you need your custom text engine implementation
	/// to associate with the <see cref="Text"/>.
	/// For that you should use the the <see cref="TextData"/> of the provided <see cref="Text"/> instance, which is accessible through the <see cref="TextEngineImplementation.TextExtensions.get_Data(Text)"/> property.
	/// The <see cref="TextData"/> of the provided <see cref="Text"/> instance has a <see cref="TextData.EngineData"/> property that you can use to store and associate any implementation-specific data that you need with the <see cref="Text"/> instance.
	/// </para>
	/// <para>
	/// Note that <see cref="InitializeText(Text)"/> and <see cref="CleanupText(Text)"/> might not only be called when the <see cref="Text"/> instance is associated or disassociated with this <see cref="TextEngine"/> respectively
	/// (e.g., the <see cref="Text"/> instance changes its <see cref="Text.Engine"/>).
	/// There are other reasons when implementation-specific data might be reinitialized (<see cref="InitializeText(Text)"/> being called again for the same <see cref="Text"/> instance, <em>with or without</em> a preceding call to <see cref="CleanupText(Text)"/>),
	/// for example when layout updates with the <see cref="Text"/> happen.
	/// You should be cautious about that and make sure that your implementation handles existing <see cref="TextData.EngineData"/> appropriately, and is fine with frequent reinitialization and cleanup of the same data instance.
	/// </para>
	/// </remarks>
	protected abstract void InitializeText(Text text);

	/// <summary>
	/// Cleans up a <see cref="Text"/> that's associated with this <see cref="TextEngine"/>
	/// </summary>
	/// <param name="text">The <see cref="Text"/> to clean up</param>
	/// <remarks>
	/// <para>
	/// You can and should use this method to clean up any implementation-specific data of your custom text engine implementation that's associated the provided <see cref="Text"/> instance.
	/// For that you should use the the <see cref="TextData"/> of the provided <see cref="Text"/> instance, which is accessible through the <see cref="TextEngineImplementation.TextExtensions.get_Data(Text)"/> property.
	/// The <see cref="TextData"/> of the provided <see cref="Text"/> instance has a <see cref="TextData.EngineData"/> property which you should clean up if you stored any implementation-specific data there before.
	/// It's recommended that you set the <see cref="TextData.EngineData"/> property to <c><see langword="null"/></c> after cleaning up and regardless of whether you actually cleaned up any data or not.
	/// </para>
	/// <para>
	/// Note that <see cref="InitializeText(Text)"/> and <see cref="CleanupText(Text)"/> might not only be called when the <see cref="Text"/> instance is associated or disassociated with this <see cref="TextEngine"/> respectively
	/// (e.g., the <see cref="Text"/> instance changes its <see cref="Text.Engine"/>).
	/// There are other reasons when implementation-specific data might be reinitialized (<see cref="CleanupText(Text)"/> being called in the middle of the lifetime of the same <see cref="Text"/> instance, most probably with <see cref="InitializeText(Text)"/> being called subsequently),
	/// for example when layout updates with the <see cref="Text"/> happen.
	/// You should be cautious about that and make sure that your implementation is fine with frequent reinitialization and cleanup of the same data instance.
	/// </para>
	/// </remarks>
	protected abstract void CleanupText(Text text);

	/// <summary>
	/// Disposes this <see cref="TextEngine"/>
	/// </summary>
	/// <remarks>
	/// <para>
	/// This method should only be called from the thread that created the text engine.
	/// </para>
	/// </remarks>
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Dispose(disposing: true, forget: true);
	}

	private void Dispose(bool disposing, bool forget)
	{
		unsafe
		{
			if (forget)
			{
				mKnownInstances.TryRemove(unchecked((IntPtr)mEngine), out _);
			}

			Dispose(disposing);

			// We always perform out freeing and cleanup logic, regardless of whether types inheriting from this type
			// override the Dispose(bool) method and call base.Dispose(bool) from it or not.
			// It's just the much cleaner thing to do. Imagine leaving a dangling GCHandle behind...

			if (mSelfHandle.IsAllocated)
			{
				mSelfHandle.Free();
				mSelfHandle = default;
			}

			if (mEngine is not null)
			{
				if (mFree)
				{
					Utilities.NativeMemory.Free(mEngine);
				}

				// If mFree is false, then we rely on the inheriting type to destroy and free the native text engine before that point.
				// That's because we set the native pointer to null here regardless.
				mEngine = null;
			}
		}
	}

	/// <summary>
	/// Disposes this <see cref="TextEngine"/>
	/// </summary>
	/// <param name="disposing">A value indicating whether the call came from a call to <see cref="Dispose()"/> or from the finalizer</param>
	/// <remarks>
	/// <para>
	/// This method should only be called from the thread that created the text engine.
	/// </para>
	/// </remarks>
	protected virtual void Dispose(bool disposing) { }

	internal unsafe static bool TryGetOrCreate(TTF_TextEngine* engine, [NotNullWhen(true)] out TextEngine? result)
	{
		// Always keep in mind, that we heavily rely on the fact that managed Text instances should keep their managed TextEngines alive,
		// and for that matter, actually all managed objects that can retrieve a managed TextEngine instance that's associated with them,
		// should do the same (except for TextData where that's handle in the associated Text instance).
		// If this invariant changes at any point, this code is at risk to being broken.

		if (engine is null)
		{
			result = null;
			return false;
		}

		var engineRef = mKnownInstances.GetOrAdd(unchecked((IntPtr)engine), createRef);

		if (!engineRef.TryGetTarget(out result))
		{
			engineRef.SetTarget(result = create(engine));
		}

		return true;

		static WeakReference<TextEngine> createRef(IntPtr engine) => new(create(unchecked((TTF_TextEngine*)engine)));

		static TextEngine create(TTF_TextEngine* engine) => new GenericFallback(engine); // There's no good way for us to figure out what the actual type of a native text engine is.
																						 // Yes, we could match it's native CreateText field against the various native CreateText implementations,
																						 // that's what SDL does internally, but we can't really get the symbols for those implementations as they're not exported.
																						 // The only way to do that would be to retrieve and cache them once the user created a specific text engine on the managed side.
																						 // But that would be too inconsistent behavior for my liking.
																						 // That's why we always just create a GenericFallback instance, in case we have to create a new managed wrapper for an existing native text engine.
																						 // It should be good enough for most use cases, and, considering that managed Text instances shoudl keep their managed TextEngines alive,
																						 // it shouldn't happen that often and we will rarely have to create a new managed wrapper,
																						 // rather than just retrieving it from the instance cache where it should still have the appropriate type.
	}
}
