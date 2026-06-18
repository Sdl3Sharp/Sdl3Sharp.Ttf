using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Ttf.TextEngineImplementation;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe CreateText = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Ttf.Text.TTF_Text*, Sdl3Sharp.Internal.Interop.CBool>;
using unsafe DestroyText = delegate* unmanaged[Cdecl]<void*, Sdl3Sharp.Ttf.Text.TTF_Text*, void>;

namespace Sdl3Sharp.Ttf;

partial class TextEngine
{
	[StructLayout(LayoutKind.Sequential)]
	internal readonly struct TTF_TextEngine
	{
		public readonly uint Version;
		public unsafe readonly void* Userdata;
		public unsafe readonly CreateText CreateText;
		public unsafe readonly DestroyText DestroyText;

		public unsafe TTF_TextEngine(void* userdata, CreateText createText, DestroyText destroyText)
		{
			this = default; // make sure we're zero'd out

			Version = unchecked((uint)Unsafe.SizeOf<TTF_TextEngine>());
			Userdata = userdata;
			CreateText = createText;
			DestroyText = destroyText;
		}

		public unsafe TTF_TextEngine(TextEngine engine, out GCHandle engineHandle) : this(
			userdata: unchecked((void*)GCHandle.ToIntPtr(engineHandle = GCHandle.Alloc(engine, GCHandleType.Normal))),
			createText: &CreateTextImpl,
			destroyText: &DestroyTextImpl
		)
		{ }

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static CBool CreateTextImpl(void* userdata, Text.TTF_Text* text)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: TextEngine engine })
			{
				if (!Text.TryGetOrCreate(text, engine, out var managedText))
				{
					// only really happens when text is null
					return false;
				}

				engine.InitializeText(managedText);

				return true;
			}

			// If we get here, then either our managed text engine was already disposed or collected,
			// or a non-managed text engine somehow managed to register this function as its CreateText implementation (which should be technically not possible).
			return false;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static void DestroyTextImpl(void* userdata, Text.TTF_Text* text)
		{
			if (userdata is not null && GCHandle.FromIntPtr(unchecked((IntPtr)userdata)) is { IsAllocated: true, Target: TextEngine engine })
			{
				if (Text.TryGetOrCreate(text, engine, out var managedText))
				{
					engine.CleanupText(managedText);

					// It is very important that we release the GCHandle allocated for the custom implementation-specific data here, if any.
					// We do make sure to advice users to reset the data in the CleanupText implementation, but we should also be defensive here in case they forget to do so.
					// Setting the EngineData property to null is enough to release potential GCHandles.
					managedText.Data.EngineData = null;
				}
			}
		}
	}
}
