using System;

namespace Eto.Forms
{
	/// <summary>
	/// Video output modes
	/// </summary>
	public enum MPlayerVideoModeName
	{
		/// <summary>
		/// X11.
		/// </summary>
		X11,

		/// <summary>
		/// Direct3D.
		/// </summary>
		Direct3D,

		/// <summary>
		/// DirectX.
		/// </summary>
		DirectX,

		/// <summary>
		/// GL.
		/// </summary>
		Gl,

		/// <summary>
		/// GL2
		/// </summary>
		Gl2,

		/// <summary>
		/// Mac OS X Core Video.
		/// </summary>
		MacCoreVideo,

		/// <summary>
		/// MPEG-PES file
		/// </summary>
		MpegPES,

		/// <summary>
		/// yuv4mpeg output for mjpegtools.
		/// </summary>
		Yuv4Mpeg
	}
}

