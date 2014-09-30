using System;
using System.Collections.Generic;

namespace Eto.Forms
{
	/// <summary>
	/// Keep information about video or audio mode
	/// </summary>
	public class MPlayerModeCommand
	{
		/// <summary>
		/// Gets or sets the video mode.
		/// </summary>
		/// <value>The video mode.</value>
		public MPlayerVideoModeName VideoMode { get; internal set; }

		/// <summary>
		/// Gets or sets the audio mode.
		/// </summary>
		/// <value>The audio mode.</value>
		public MPlayerAudioModeName AudioMode { get; internal set; }

		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>The command.</value>
		public string Command { get; internal set; }

		/// <summary>
		/// The platroms.
		/// </summary>
		internal List<string> Platroms = new List<string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MPlayerModeCommand"/> class.
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="commandName">Command name.</param>
		/// <param name="platformIds">Platform identifiers.</param>
		public MPlayerModeCommand(MPlayerVideoModeName mode, string commandName, IEnumerable<string> platformIds)
		{
			VideoMode = mode;
			Platroms.AddRange(platformIds);
			Command = commandName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MPlayerModeCommand"/> class.
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="commandName">Command name.</param>
		/// <param name="platformIds">Platform identifiers.</param>
		public MPlayerModeCommand(MPlayerAudioModeName mode, string commandName, IEnumerable<string> platformIds)
		{
			AudioMode = mode;
			Platroms.AddRange(platformIds);
			Command = commandName;
		}
	}
}

