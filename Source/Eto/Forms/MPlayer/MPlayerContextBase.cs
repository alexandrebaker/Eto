using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Eto.Forms
{
	/// <summary>
	/// Class for managing relations with mplayer application
	/// </summary>
	public abstract class MPlayerContextBase
	{
		/// <summary>
		/// Gets or sets a location of mplayer
		/// </summary>
		public virtual string ExecutablePath { get; set; }

		/// <summary>
		/// Gets or sets a filename of mplayer
		/// </summary>
		public virtual string ExecutableName { get; set; }

		/// <summary>
		/// Gets a value indicating whether context initialized
		/// </summary>
		public virtual bool IsInitialized { get; protected set; }

		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public virtual string FileName { get; protected set; }

		/// <summary>
		/// List of startup player commandline options
		/// </summary>
		public virtual StringCollection Options { get; protected set; }

		/// <summary>
		/// Keep handle for a video output window
		/// </summary>
		private int _windowHandle;

		/// <summary>
		/// Video mode keeper
		/// </summary>
		private readonly MPlayerVideoMode _videoMode;

		/// <summary>
		/// Audio mode keeper
		/// </summary>
		private readonly MPlayerAudioMode _audioMode;
	
		/// <summary>
		/// Mplayer process
		/// </summary>
		private Process _mplayer;

		/// <summary>
		/// Video mode
		/// </summary>
		public virtual MPlayerVideoModeName VideoMode 
		{ 
			get { return _videoMode.Mode; }
		}

		/// <summary>
		/// Audio mode
		/// </summary>
		public virtual MPlayerAudioModeName AudioMode
		{ 
			get { return _audioMode.Mode; }
		}

		/// <summary>
		/// Expression for media details detection
		/// </summary>
		protected readonly Regex ExpressionData;

		/// <summary>
		/// Expression for pregress detection
		/// </summary>
		protected readonly Regex ExpressionProgress;

		/// <summary>
		/// Whether we processing header output
		/// </summary>
		private bool _isParsingHeader;

		/// <summary>
		/// The output data received event.
		/// </summary>
		private DataReceivedEventHandler _outputDataReceived;

		/// <summary>
		/// Occurs when output data received.
		/// </summary>
		public event DataReceivedEventHandler OutputDataReceived
		{
			add { _outputDataReceived += value; }
			remove { _outputDataReceived -= value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MPlayerContextBase"/> class.
		/// </summary>
		/// <param name="mPlayerDirectory">Mplayer directory.</param>
		/// <param name="mPlayerExe">Mplayer executable.</param>
		protected MPlayerContextBase(string mPlayerDirectory, string mPlayerExe){
			IsInitialized = false;
			ExecutablePath = mPlayerDirectory;
			ExecutableName = mPlayerExe;
			Options = new StringCollection();
			_audioMode = new MPlayerAudioMode();
			_videoMode = new MPlayerVideoMode();
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
		public virtual void CloseAll()
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
		public virtual void AddOption(string option){
			if (string.IsNullOrEmpty(option))
				return;

			if (option.Length > 0 && option.StartsWith("-"))
				option = option.Substring(1);

			Options.Add(option);
		}

		/// <summary>
		/// Remove all options
		/// </summary>
		public virtual void ClearOptions()
		{
			Options.Clear();
		}
		/// <summary>
		/// Initialize mplayer instance
		/// </summary>
		/// <param name="windowHandle">Window handle for video output</param>
		public virtual void Initialize(int windowHandle){

			if (windowHandle == 0)
				return;
			_windowHandle = windowHandle;

			if (ExecutablePath == null || ExecutablePath == string.Empty)
				throw new NullReferenceException("ExecutablePath cannot be null.");

			if (ExecutableName == null || ExecutableName == string.Empty)
				throw new NullReferenceException("ExecutableName cannot be null.");

			var fileName = Path.Combine(ExecutablePath, ExecutableName);

			if (!File.Exists(fileName))
				throw new FileNotFoundException(String.Format("MPlayer is not found at '{0}'",fileName));

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
		public virtual bool OpenFile(string fileName)
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
				_videoMode.ModeCommand,
				_audioMode.ModeCommand);
			commandArguments = Options.Cast<string>().Aggregate(
				commandArguments,
				(current, option) => current + string.Format(" -{0}", option));
			commandArguments += string.Format(" -wid {0}", _windowHandle);

			_mplayer = new Process
			{
				StartInfo =
				{
					FileName = Path.Combine(ExecutablePath, ExecutableName),
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
		public virtual void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			Debug.WriteLine(e.Data);
		}

		/// <summary>
		/// Output data handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e == null || e.Data == null)
				return;

			if (this._outputDataReceived != null)
				_outputDataReceived(sender, e);

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

		/// <summary>
		/// Processes the progress.
		/// </summary>
		/// <param name="time">Time.</param>
		public virtual void ProcessProgress(string time)
		{

		}

		/// <summary>
		/// Process details
		/// </summary>
		/// <param name="key">Key string</param>
		/// <param name="value">Value string</param>
		public virtual void ProcessDetails(string key, string value)
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
		public virtual bool SendCommand(string command)
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

