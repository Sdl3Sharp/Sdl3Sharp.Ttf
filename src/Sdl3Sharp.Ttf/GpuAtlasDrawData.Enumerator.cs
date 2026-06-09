using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf;

partial struct GpuAtlasDrawData
{
	/// <summary>
	/// Enumerates the <see cref="GpuAtlasDrawSequence"/>s in a <see cref="GpuAtlasDrawData"/> instance
	/// </summary>
	/// <param name="data">The <see cref="GpuAtlasDrawData"/> instance to enumerate</param>
	[StructLayout(LayoutKind.Sequential)]
	[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ref struct Enumerator(GpuAtlasDrawData data) : IEnumerator<GpuAtlasDrawSequence>
	{
		private ref readonly GpuAtlasDrawSequence mCurrent;
		private unsafe Node* mNext = data.mNode;

		/// <inheritdoc cref="IEnumerator{GpuAtlasDrawSequence}.Current"/>
		public readonly ref readonly GpuAtlasDrawSequence Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref mCurrent;
		}

		/// <inheritdoc/>
		readonly GpuAtlasDrawSequence IEnumerator<GpuAtlasDrawSequence>.Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => Current;
		}

		/// <inheritdoc/>
		readonly object IEnumerator.Current => Current;

		/// <inheritdoc/>
		[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public bool MoveNext()
		{
			unsafe
			{
				if (mNext is null)
				{
					return false;
				}

				mCurrent = ref mNext->Sequence;
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
