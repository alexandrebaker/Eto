using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Eto.Wpf.Drawing;
using swc = System.Windows.Controls;
using sw = System.Windows;
using swd = System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Eto.Wpf.Forms
{
	public class MediaPlayerHandler : WpfFrameworkElement<swc.MPlayerWPFControl, MediaPlayer, MediaPlayer.ICallback>, MediaPlayer.IHandler
	{
		/// <summary>
		/// The minimum size.
		/// </summary>
		public static Size MinimumSize = new Size(100, 100);

		/// <summary>
		/// Gets the default size.
		/// </summary>
		/// <value>The default size.</value>
		protected override Size DefaultSize { get { return MinimumSize; } }

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		private MPlayerContext Context { get; set; }

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>The state.</value>
		public MediaState State { get; set; }

		/// <summary>
		/// Gets or sets the media file.
		/// </summary>
		/// <value>The media file.</value>
		public string MediaFile { get; set; }

		private void SetMediaSize(object sender, sw.SizeChangedEventArgs e)
		{

		}

		/// <summary>
		/// Opens the and play.
		/// </summary>
		private void OpenAndPlay()
		{
			if (MediaFile == null || MediaFile.Trim() == string.Empty)
				return;

			this.Context.OpenFile(MediaFile);
			this.State = MediaState.Playing;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Wpf.Forms.MediaPlayerHandler"/> class.
		/// </summary>
		public MediaPlayerHandler()
		{
			Control = new swc.MPlayerWPFControl()
			{
				Height = 100,
				Width = 100
			};

			// Associate control with the context
			this.Context = new MPlayerContext();
			this.Context.ExecutablePath = "mplayer";
			State = MediaState.Closed;

			Control.SizeChanged += (object sender, sw.SizeChangedEventArgs e) => SetMediaSize(sender, e);
			this.Context.OutputDataReceived += (sender, e) => Callback.OutputDataReceived(Widget, e);
			/*Control.MediaOpened += (sender, e) => Callback.OnMediaOpened(Widget, EventArgs.Empty);
			Control.MediaEnded += (sender, e) => Callback.OnMediaEnded(Widget, EventArgs.Empty);*/
		}

		/// <summary>
		/// Play this media.
		/// </summary>
		public void Play()
		{
			if (this.State == MediaState.Stopped || State == MediaState.Closed)
				OpenAndPlay();
			else
				this.Context.Play(this);
		}

		/// <summary>
		/// Pause this media.
		/// </summary>
		public void Pause()
		{
			this.Context.Pause(this);
		}

		/// <summary>
		/// Stop this media.
		/// </summary>
		public void Stop()
		{
			this.Context.Stop(this);
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void Close()
		{
			this.Context.Close(this);
		}

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public byte[] SourceBlob
		{
			get
			{
				if (SourceFile != null && SourceFile.Trim() != String.Empty)
					return File.ReadAllBytes(SourceFile);
				else
					return null;
			}

			set
			{
				if (value != null && value.Length > 0)
				{
					var tmpFileName = Path.GetTempFileName();
					File.WriteAllBytes(tmpFileName, value);
					SourceFile = tmpFileName;
				}
			}
		}

		/// <summary>
		/// Set the source of the media.
		/// </summary>
		/// <param name="path"></param>
		public string SourceFile
		{ 
			get
			{
				return MediaFile;
			}
			set
			{
				if (value != null && value.Trim() != String.Empty)
				{
					MediaFile = value;
					this.Context.Initialize((int)Control.Handle);
					OpenAndPlay();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Eto.Wpf.Forms.MediaPlayerHandler"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public new bool Visible
		{
			get { return Control.Visibility == sw.Visibility.Visible; }
			set { Control.Visibility = value ? sw.Visibility.Visible : sw.Visibility.Hidden; }
		}

		/// <summary>
		/// Gets or sets the color for the background of the control
		/// </summary>
		/// <remarks>Note that on some platforms (e.g. Mac), setting the background color of a control can change the performance
		/// characteristics of the control and its children, since it must enable layers to do so.</remarks>
		/// <value>The color of the background.</value>
		public override Color BackgroundColor { get; set; }

		/// <summary>
		/// Sets the decorations.
		/// </summary>
		/// <param name="decorations">Decorations.</param>
		protected virtual void SetDecorations(sw.TextDecorationCollection decorations)
		{
		}

		/// <summary>
		/// Gets or sets the font for the text of the control
		/// </summary>
		/// <value>The text font.</value>
		public Font Font { get; set; }
	}
}

