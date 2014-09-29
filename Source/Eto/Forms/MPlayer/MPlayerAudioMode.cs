using System;
using System.Collections.Generic;

namespace Eto.Forms
{
	/// <summary>
	/// Contains constraits for modes
	/// </summary>
	public class MPlayerAudioMode
	{
		/// <summary>
		/// The display modes.
		/// </summary>
		private readonly Dictionary<MPlayerAudioModeName, MPlayerModeCommand> _displayModes;

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MPlayerAudioMode"/> class.
		/// </summary>
		public MPlayerAudioMode()
		{
			_displayModes = new Dictionary<MPlayerAudioModeName, MPlayerModeCommand>();
			var modes = new[]
			{
				new MPlayerModeCommand(MPlayerAudioModeName.Alsa, "alsa", new[] { PlatformID.Unix }),
				new MPlayerModeCommand(MPlayerAudioModeName.DirectSound, "dsound", new[] { PlatformID.Xbox, PlatformID.Win32NT, PlatformID.Win32Windows }),
				new MPlayerModeCommand(MPlayerAudioModeName.Win32, "win32", new[] { PlatformID.Win32NT, PlatformID.Win32Windows }),
			};

			foreach (var mode in modes)
			{
				_displayModes.Add(mode.AudioMode, mode);
			}

			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.MacOSX:
					_mode = MPlayerAudioModeName.Alsa;
					break;
				case PlatformID.Unix:
					_mode = MPlayerAudioModeName.Alsa;
					break;
				default:
					_mode = MPlayerAudioModeName.DirectSound;
					break;
			}
		}

		/// <summary>
		/// The mode.
		/// </summary>
		private MPlayerAudioModeName _mode;

		/// <summary>
		/// Gets the mode command.
		/// </summary>
		/// <value>The mode command.</value>
		public string ModeCommand
		{
			get { return _displayModes[_mode].Command; }
		}

		/// <summary>
		/// Gets or sets the mode.
		/// </summary>
		/// <value>The mode.</value>
		public MPlayerAudioModeName Mode
		{
			get { return _mode; }
			set
			{
				if (_displayModes[value].Platroms.Contains(Environment.OSVersion.Platform))
					_mode = value;
			}
		}
	}
}

