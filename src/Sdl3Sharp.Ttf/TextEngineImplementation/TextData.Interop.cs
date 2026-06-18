using Sdl3Sharp.Internal.Interop;
using Sdl3Sharp.Video.Coloring;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ttf.TextEngineImplementation;

partial class TextData
{
	// Used for opaque pointers
	[StructLayout(LayoutKind.Sequential, Size = 0)]
	internal readonly struct TTF_TextLayout;

	[StructLayout(LayoutKind.Sequential)]
	internal struct TTF_TextData
	{
		public unsafe readonly Font.TTF_Font* Font;
		public readonly Color<float> Color;

		public CBool NeedsLayoutUpdate;
		private unsafe readonly TTF_TextLayout* mLayout;
		public readonly int X;
		public readonly int Y;
		public readonly int W;
		public readonly int H;
		public readonly int NumOps;
		public unsafe readonly DrawOperation.TTF_DrawOperation* Ops;
		public readonly int NumClusters;
		public unsafe readonly SubString* Clusters;

		public readonly uint Props;

		public CBool NeedsEngineUpdate;
		public unsafe readonly TextEngine.TTF_TextEngine* Engine;
		public unsafe void* EngineText;
	}
}
