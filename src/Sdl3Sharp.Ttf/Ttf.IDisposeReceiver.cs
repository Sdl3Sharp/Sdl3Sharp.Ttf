using Sdl3Sharp.Internal;
using System;
using System.Collections.Concurrent;

namespace Sdl3Sharp.Ttf;

partial class Ttf
{
	internal interface IDiposeReceiver
	{
		void DisposeFromTtf();
	}

	private static readonly ConcurrentDictionary<WeakReference<IDiposeReceiver>, byte> mDisposeReceivers = new(WeakReferenceEqualityComparer<IDiposeReceiver>.Instance);

	internal static bool TryDeregisterDisposable(IDiposeReceiver disposeReceiver)
		=> mDisposeReceivers.TryRemove(new(disposeReceiver), out _);

	internal static bool TryRegisterDisposable(IDiposeReceiver disposeReceiver)
		=> new WeakReference<IDiposeReceiver>(disposeReceiver) switch
		{
			var disposeReceiverRef
				=> mDisposeReceivers.TryAdd(disposeReceiverRef, default)
				|| mDisposeReceivers.ContainsKey(disposeReceiverRef)
		};
}
