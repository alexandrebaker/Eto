using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Eto.Forms;

namespace Eto.Wpf.Forms
{
	/// <summary>
	/// Class for managing relations with mplayer application
	/// </summary>
	public class MPlayerContext : IMPlayerContext
	{
		/// <summary>
		/// The output data received event.
		/// </summary>
		DataReceivedEventHandler outputDataReceived;

		/// <summary>
		/// Gets or sets a location of mplayer
		/// </summary>
		public string ExecutablePath { get; set; }

		/// <summary>
		/// Gets or sets a filename of mplayer
		/// </summary>
		public string ExecutableName { get; set; }

		/// <summary>
		/// Gets a value indicating whether context initialized
		/// </summary>
		public bool IsInitialized { get; internal set; }

		public string FileName { get; internal set; }

		/// <summary>
		/// Keep handle for a video output window
		/// </summary>
		private int _windowHandle;

		/// <summary>
		/// List of startup player commandline options
		/// </summary>
		public StringCollection Options { get; protected set; }

		/// <summary>
		/// Mplayer process
		/// </summary>
		private Process _mplayer;

		/// <summary>
		/// Video mode keeper
		/// </summary>
		private readonly MPlayerVideoMode videoMode;

		/// <summary>
		/// Audio mode keeper
		/// </summary>
		private readonly MPlayerAudioMode audioMode;

		/// <summary>
		/// Video mode
		/// </summary>
		public MPlayerVideoModeName VideoMode
		{
			get { return videoMode.Mode; }
			set { videoMode.Mode = value; }
		}

		/// <summary>
		/// Audio mode
		/// </summary>
		public MPlayerAudioModeName AudioMode
		{
			get { return audioMode.Mode; }
			set { audioMode.Mode = value; }
		}

		/// <summary>
		/// Expression for media details detection
		/// </summary>
		private readonly Regex ExpressionData;

		/// <summary>
		/// Expression for pregress detection
		/// </summary>
		private readonly Regex ExpressionProgress;

		/// <summary>
		/// Whether we processing header output
		/// </summary>
		private bool _isParsingHeader;

		/// <summary>
		/// Occurs when output data received.
		/// </summary>
		public event DataReceivedEventHandler OutputDataReceived
		{
			add { outputDataReceived += value; }
			remove { outputDataReceived -= value; }
		}

		/// <summary>
		/// Initializes static members of the MplayerContext class
		/// </summary>
		public MPlayerContext()
		{
			IsInitialized = false;
			ExecutablePath = string.Empty;
			ExecutableName = "mplayer.exe";
			Options = new StringCollection();
			audioMode = new MPlayerAudioMode();
			videoMode = new MPlayerVideoMode();
			FileName = string.Empty;

			ExpressionData = new Regex(
				@"^([a-z_]+)=([a-z0-9_\.\,]+)",
				RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			ExpressionProgress = new Regex(
				@"v:\s+(\d+[\.\,]\d+)\s+([\d/]+)\s+([\d]*)\s+([\d\?]+)%\s+([\d\?]+)%\s+([\d\?]+[\.\,]?[\d\?]*)%",
				RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		public void CloseAll()
		{
			try
			{
				if (_mplayer != null && !_mplayer.HasExited)
				{
					_mplayer.Kill();
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Add startup option for player
		/// </summary>
		/// <param name="option">MPlayer commandline option</param>
		public void AddOption(string option)
		{
			if (string.IsNullOrEmpty(option))
				return;

			if (option.Length > 0 && option.StartsWith("-"))
				option = option.Substring(1);

			Options.Add(option);
		}

		/// <summary>
		/// Remove all options
		/// </summary>
		public void ClearOptions()
		{
			Options.Clear();
		}

		/// <summary>
		/// Initialize mplayer instance
		/// </summary>
		/// <param name="windowHandle">Window handle for video output</param>
		public void Initialize(int windowHandle)
		{
			if (windowHandle == 0)
				return;
			_windowHandle = windowHandle;

			if (ExecutablePath == string.Empty)
				ExecutablePath = Directory.GetCurrentDirectory();

			if (ExecutableName == string.Empty)
				ExecutableName = "mplayer.exe";

			var fileName = ExecutablePath + "\\" + ExecutableName;
			if (!File.Exists(fileName))
				return;

			AddOption("nofs");          // disable fullscreen
			AddOption("noquiet");       // print out information
			AddOption("identify");      // print out detailed information
			AddOption("idle");          // wait insead of quit
			AddOption("slave");         // shitch on slave mode for frontend
			AddOption("nomouseinput");  // disable mouse input events

			IsInitialized = true;
		}

		/// <summary>
		/// Open media file
		/// </summary>
		/// <param name="fileName">Media file name</param>
		/// <returns>Result</returns>
		public bool OpenFile(string fileName)
		{
			if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
				return false;

			if (_mplayer != null)
			{
				_mplayer.OutputDataReceived -= OnOutputDataReceived;
				try
				{
					_mplayer.Kill();
				}
				catch
				{
				}
			}

			var commandArguments = string.Format(
				                       " -vo {0} -ao {1}",
				                       videoMode.ModeCommand,
				                       audioMode.ModeCommand);
			commandArguments = Options.Cast<string>().Aggregate(
				commandArguments,
				(current, option) => current + string.Format(" -{0}", option));
			commandArguments += string.Format(" -wid {0}", _windowHandle);

			_mplayer = new Process
			{
				StartInfo =
				{
					FileName = ExecutablePath + "\\" + ExecutableName,
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					Arguments = commandArguments + string.Format(" \"{0}\"", fileName)
				}
			};

			try
			{
				_mplayer.Start();
			}
			catch (Exception)
			{
				
				return false;
			}

			_mplayer.OutputDataReceived += (sender, e) =>
			{
				this.OnOutputDataReceived(sender, e);
			};
			_mplayer.ErrorDataReceived += ErrorDataReceived;
			_mplayer.BeginOutputReadLine();
			_mplayer.BeginErrorReadLine();
           
			FileName = fileName;
			_isParsingHeader = true;

			return _mplayer != null ? true : false;
		}

		/// <summary>
		/// Error data handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			Debug.WriteLine(e.Data);
		}

		/// <summary>
		/// Output data handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e == null || e.Data == null)
				return;

			if (this.outputDataReceived != null)
				outputDataReceived(sender, e);

			Match match;
			Debug.WriteLine(e.Data);

			if (_isParsingHeader)
			{
				match = ExpressionData.Match(e.Data);
				if (match.Groups.Count == 3)
				{
					ProcessDetails(match.Groups[1].Value, match.Groups[2].Value);
					return;
				}
			}

			match = ExpressionProgress.Match(e.Data);
			if (match.Groups.Count == 7)
			{
				ProcessProgress(match.Groups[1].Value);
				_isParsingHeader = true;
				return;
			}
		}

		public void ProcessProgress(string time)
		{
            
		}

		/// <summary>
		/// Process details
		/// </summary>
		/// <param name="key">Key string</param>
		/// <param name="value">Value string</param>
		public void ProcessDetails(string key, string value)
		{
			switch (key)
			{
				case "ID_VIDEO_FORMAT":
					break;
				case "ID_VIDEO_BITRATE":
					break;
				case "ID_VIDEO_WIDTH":
					break;
				case "ID_VIDEO_HEIGHT":
					break;
				case "ID_VIDEO_FPS":
					break;
				case "ID_VIDEO_ASPECT":
					break;
				case "ID_AUDIO_FORMAT":
					break;
				case "ID_AUDIO_BITRATE":
					break;
				case "ID_AUDIO_RATE":
					break;
				case "ID_AUDIO_NCH":
					break;
				case "ID_LENGTH":
					break;
				case "ID_SEEKABLE":
					break;
			}
		}

		/// <summary>
		/// Send command to player
		/// </summary>
		/// <param name="command">Command</param>
		/// <returns>Result</returns>
		public bool SendCommand(string command)
		{
			if (IsInitialized && _mplayer != null && !_mplayer.HasExited)
			{
				try
				{
					_mplayer.StandardInput.Write(command + "\n");
					return true;
				}
				catch
				{
					return false;
				}
			}
			return false;
		}
	}
}

