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
				new MPlayerModeCommand(MPlayerVideoModeName.X11, "x11", new[] { PlatformID.Unix }),
				new MPlayerModeCommand(MPlayerVideoModeName.Direct3D, "direct3d", new[] { PlatformID.Xbox, PlatformID.Win32NT, PlatformID.Win32Windows }),
				new MPlayerModeCommand(MPlayerVideoModeName.DirectX, "directx", new[] { PlatformID.Xbox, PlatformID.Win32NT, PlatformID.Win32Windows }),
				new MPlayerModeCommand(MPlayerVideoModeName.Gl, "gl", new[] { PlatformID.Unix, PlatformID.MacOSX, PlatformID.Win32NT, PlatformID.Win32Windows }),
				new MPlayerModeCommand(MPlayerVideoModeName.Gl2, "gl2", new[] { PlatformID.Unix, PlatformID.MacOSX, PlatformID.Win32NT, PlatformID.Win32Windows }),
			};

			foreach (var mode in modes)
			{
				_displayModes.Add(mode.VideoMode, mode);
			}

			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.MacOSX:
					_mode = MPlayerVideoModeName.Gl;
					break;
				case PlatformID.Unix:
					_mode = MPlayerVideoModeName.X11;
					break;
				default:
					_mode = MPlayerVideoModeName.Direct3D;
					break;
			}
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
			set
			{
				if (_displayModes[value].Platroms.Contains(Environment.OSVersion.Platform))
					_mode = value;
			}
		}
	}
}

