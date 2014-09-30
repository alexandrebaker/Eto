using System;

namespace Eto.Forms
{
	/// <summary>
	/// Video output modes
	/// </summary>
	public enum MPlayerAudioModeName
	{
		/// <summary>
		/// Win32.
		/// </summary>
		Win32,

		/// <summary>
		/// DirectSound.
		/// </summary>
		DirectSound,

		/// <summary>
		/// Alsa.
		/// </summary>
		Alsa,

		/// <summary>
		/// Darwin/Mac OS X native audio output.
		/// </summary>
		MacCoreAudio,

		/// <summary>
		/// OpenAL audio output.
		/// </summary>
		OpenAL,

		/// <summary>
		/// MPEG-PES audio output
		/// </summary>
		MpegPES
	}
}

