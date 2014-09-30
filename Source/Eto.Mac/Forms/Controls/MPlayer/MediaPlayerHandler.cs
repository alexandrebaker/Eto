using System;
using Eto.Forms;
using System.Collections.Generic;
using Eto.Mac.Forms.Controls;
using Eto.Drawing;
using Eto.Mac.Drawing;
using System.Collections;
using System.Linq;
using System.IO;
using System.Reflection;
#if XAMMAC2
using AppKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using CoreAnimation;
using nnint = System.Int32;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;
using MonoMac.CoreAnimation;
using MonoMac.QTKit;
#if Mac64
using CGSize = MonoMac.Foundation.NSSize;
using CGRect = MonoMac.Foundation.NSRect;
using CGPoint = MonoMac.Foundation.NSPoint;
using nfloat = System.Double;
using nint = System.Int64;
using nuint = System.UInt64;
using nnint = System.UInt64;
#else
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
using CGPoint = System.Drawing.PointF;
using nfloat = System.Single;
using nint = System.Int32;
using nuint = System.UInt32;
using nnint = System.Int32;
#endif
#endif

namespace Eto.Mac.Forms.Controls
{
	/// <summary>
	/// Eto media player.
	/// </summary>
	public class EtoMediaPlayer : NSView, IMacControl
	{
		/// <summary>
		/// Gets or sets the weak handler.
		/// </summary>
		/// <value>The weak handler.</value>
		public WeakReference WeakHandler { get; set; }

		/// <summary>
		/// Gets or sets the handler.
		/// </summary>
		/// <value>The handler.</value>
		public object Handler
		{ 
			get { return WeakHandler.Target; }
			set { WeakHandler = new WeakReference(value); } 
		}
	}

	/// <summary>
	/// Media player handler.
	/// </summary>
	public class MediaPlayerHandler : MacView<EtoMediaPlayer, MediaPlayer, MediaPlayer.ICallback>, MediaPlayer.IHandler
	{
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
		/// Initializes a new instance of the <see cref="Eto.Mac.Forms.Controls.MediaPlayerHandler"/> class.
		/// </summary>
		public MediaPlayerHandler()
		{
			this.State = MediaState.Closed;
			Control = new EtoMediaPlayer() { Handler = this};

			// Associate control with the context
			this.Context = new MPlayerContext();
			this.Context.ExecutablePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ,"mplayer");
			State = MediaState.Closed;

			this.Context.OutputDataReceived += (sender, e) => Callback.OutputDataReceived(Widget, e);
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
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Eto.Mac.Forms.Controls.MediaPlayerHandler"/> is reclaimed by garbage collection.
		/// </summary>
		~MediaPlayerHandler(){
			if (Control != null)
				Control.RemoveFromSuperview();
		}

		/// <summary>
		/// Gets the container control.
		/// </summary>
		/// <value>The container control.</value>
		public override NSView ContainerControl { get { return Control; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Eto.Mac.Forms.Controls.MediaPlayerHandler"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public override bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the font for the text of the control
		/// </summary>
		/// <value>The text font.</value>
		public Font Font { get; set; }
	}
}

