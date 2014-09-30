using System;
using System.Collections.Generic;

namespace Eto.Forms
{
	/// <summary>
	/// Contains constraits for modes
	/// </summary>
	public class MPlayerVideoMode
	{
		/// <summary>
		/// The display modes.
		/// </summary>
		private readonly Dictionary<MPlayerVideoModeName, MPlayerModeCommand> _displayModes;

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MPlayerVideoMode"/> class.
		/// </summary>
		public MPlayerVideoMode()
		{
			_displayModes = new Dictionary<MPlayerVideoModeName, MPlayerModeCommand>();
			var modes = new[]
			{
				new MPlayerModeCommand(MPlayerVideoModeName.MacCoreVideo, "corevideo" , new [] { Eto.Platforms.Mac, Eto.Platforms.Mac64, Eto.Platforms.XamMac}),
				new MPlayerModeCommand(MPlayerVideoModeName.Direct3D, "direct3d", new[] { Eto.Platforms.Wpf, Eto.Platforms.WinForms }),
				new MPlayerModeCommand(MPlayerVideoModeName.DirectX, "directx", new[] { Eto.Platforms.Wpf, Eto.Platforms.WinForms }),
			};

			foreach (var mode in modes)
			{
				_displayModes.Add(mode.VideoMode, mode);
			}

			if (Eto.Platform.Detect.IsMac)
				_mode = MPlayerVideoModeName.MacCoreVideo;
			else
				_mode = MPlayerVideoModeName.Direct3D;

		}

		/// <summary>
		/// The mode.
		/// </summary>
		private MPlayerVideoModeName _mode;

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
		public MPlayerVideoModeName Mode
		{
			get { return _mode; }
		}
	}
}

