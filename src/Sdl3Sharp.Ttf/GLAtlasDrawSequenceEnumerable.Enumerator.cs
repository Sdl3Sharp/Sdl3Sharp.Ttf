#if SDL_TTF3_3_0_OR_GREATER

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GLAtlasDrawSequenceEnumerable
{
	/// <summary>
	/// Enumerates the <see cref="GLAtlasDrawSequence"/>s in a <see cref="GLAtlasDrawSequenceEnumerable"/>
	/// </summary>
	/// <param name="sequences">The <see cref="GLAtlasDrawSequenceEnumerable"/> to enumerate the <see cref="GLAtlasDrawSequence"/>s of</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ref struct Enumerator(GLAtlasDrawSequenceEnumerable sequences) : IEnumerator<GLAtlasDrawSequence>
	{
		private unsafe GLAtlasDrawSequence mCurrent;
		private unsafe GLAtlasDrawSequence.TTF_GLAtlasDrawSequence* mNext = sequences.mSequences;

		/// <inheritdoc/>
		public readonly GLAtlasDrawSequence	Current { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mCurrent; }

		/// <summary>Not supported. Do not access this property, use the <see cref="Current"/> property instead. This property will always throw a <see cref="NotSupportedException"/> if accessed.</summary>
		/// <exception cref="NotSupportedException">Always</exception>
		[Obsolete($"Not supported. Do not access this property, use the {nameof(Current)} property instead. This property will always throw a {nameof(NotSupportedException)} if accessed.")]
		readonly object IEnumerator.Current { [DoesNotReturn] get => throw new NotSupportedException(); }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool MoveNext()
		{
			unsafe
			{
				if (mNext is null)
				{
					return false;
				}

				mCurrent = new(ref Unsafe.AsRef<GLAtlasDrawSequence.TTF_GLAtlasDrawSequence>(mNext));
				mNext = mNext->Next;

				return true;
			}
		}

		/// <inheritdoc/>
		readonly void IDisposable.Dispose() { }

		/// <summary>Not supported. Do not use this method. This method will always throw a <see cref="NotSupportedException"/> if called.</summary>
		/// <exception cref="NotSupportedException">Always</exception>
		[Obsolete($"Not supported. Do not use this method. This method will always throw a {nameof(NotSupportedException)} if called.")]
		[DoesNotReturn]
		readonly void IEnumerator.Reset() => throw new NotSupportedException();
	}
}

#endif
