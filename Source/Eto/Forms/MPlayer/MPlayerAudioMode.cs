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
				new MPlayerModeCommand(MPlayerAudioModeName.MacCoreAudio, "coreaudio" , new [] { Eto.Platforms.Mac, Eto.Platforms.Mac64, Eto.Platforms.XamMac}),
				new MPlayerModeCommand(MPlayerAudioModeName.DirectSound, "dsound", new[] { Eto.Platforms.Wpf, Eto.Platforms.WinForms }),
				new MPlayerModeCommand(MPlayerAudioModeName.Win32, "win32", new[] { Eto.Platforms.Wpf, Eto.Platforms.WinForms }),
				};

			foreach (var mode in modes)
			{
				_displayModes.Add(mode.AudioMode, mode);
			}

			if(Eto.Platform.Instance.IsMac)
				_mode = MPlayerAudioModeName.MacCoreAudio;
			else 
				_mode = MPlayerAudioModeName.DirectSound;

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
		}
	}
}

