using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Eto.Forms
{
	/// <summary>
	/// Class for managing relations with mplayer application
	/// </summary>
	public interface IMPlayerContext
	{
		/// <summary>
		/// Gets or sets a location of mplayer
		/// </summary>
		string ExecutablePath { get; set; }

		/// <summary>
		/// Gets or sets a filename of mplayer
		/// </summary>
		string ExecutableName { get; set; }

		/// <summary>
		/// Gets a value indicating whether context initialized
		/// </summary>
		bool IsInitialized { get; }

		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		string FileName { get; }

		/// <summary>
		/// List of startup player commandline options
		/// </summary>
		StringCollection Options { get; }

		/// <summary>
		/// Video mode
		/// </summary>
		MPlayerVideoModeName VideoMode { get; set; }

		/// <summary>
		/// Audio mode
		/// </summary>
		MPlayerAudioModeName AudioMode{ get; set; }

		/// <summary>
		/// Destructor
		/// </summary>
		void CloseAll();

		/// <summary>
		/// Add startup option for player
		/// </summary>
		/// <param name="option">MPlayer commandline option</param>
		void AddOption(string option);

		/// <summary>
		/// Remove all options
		/// </summary>
		void ClearOptions();

		/// <summary>
		/// Initialize mplayer instance
		/// </summary>
		/// <param name="windowHandle">Window handle for video output</param>
		void Initialize(int windowHandle);

		/// <summary>
		/// Open media file
		/// </summary>
		/// <param name="fileName">Media file name</param>
		/// <returns>Result</returns>
		bool OpenFile(string fileName);

		/// <summary>
		/// Processes the progress.
		/// </summary>
		/// <param name="time">Time.</param>
		void ProcessProgress(string time);

		/// <summary>
		/// Process details
		/// </summary>
		/// <param name="key">Key string</param>
		/// <param name="value">Value string</param>
		void ProcessDetails(string key, string value);

		/// <summary>
		/// Send command to player
		/// </summary>
		/// <param name="command">Command</param>
		/// <returns>Result</returns>
		bool SendCommand(string command);
	}
}

