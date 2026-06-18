using System;
using System.Diagnostics.CodeAnalysis;

namespace Sdl3Sharp.Ttf;

partial class TextEngine
{
	private unsafe sealed class GenericFallback(TTF_TextEngine* engine) : TextEngine(engine, register: false, free: true) // register: false because the only point where this constructor is used is where we handle the registration ourselves
	{
		[DoesNotReturn]
		protected override void CleanupText(Text text) => throw new NotSupportedException($"{nameof(CleanupText)} should not be called from user code.");

		[DoesNotReturn]
		protected override void InitializeText(Text text) => throw new NotSupportedException($"{nameof(InitializeText)} should not be called from user code.");
	}
}
