using System;
using Eto.Forms;
using System.Collections.Generic;
using Eto.Mac.Forms.Controls;
using Eto.Drawing;
using Eto.Mac.Drawing;
using System.Collections;
using System.Linq;
using System.IO;
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
	public class EtoMediaPlayer : QTMovieView, IMacControl
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
		/// Media state.
		/// </summary>
		protected enum MediaState 
		{ 
			Closed,
			Stopped,
			Playing,
			Paused
		}

		/// <summary>
		/// The state.
		/// </summary>
		private MediaState _state;

		/// <summary>
		/// The video.
		/// </summary>
		private QTMovie video;

		/// <summary>
		/// The file path.
		/// </summary>
		private string FilePath;

		private NSObject objectControl = new NSObject();

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Mac.Forms.Controls.MediaPlayerHandler"/> class.
		/// </summary>
		public MediaPlayerHandler()
		{
			this._state = MediaState.Closed;
			Control = new EtoMediaPlayer() { Handler = this};
		}

		/// <summary>
		/// Play this media.
		/// </summary>
		public void Play()
		{
			if (this._state == MediaState.Closed)
				return;

			Control.Play(objectControl);
			this._state = MediaState.Playing;
		}

		/// <summary>
		/// Pause this media.
		/// </summary>
		public void Pause()
		{
			if (this._state == MediaState.Closed)
				return;

			Control.Pause(objectControl);
			this._state = MediaState.Paused;
		}

		/// <summary>
		/// Stop this media.
		/// </summary>
		public void Stop()
		{
			if (this._state == MediaState.Closed)
				return;
				
			Control.Pause(objectControl);
			Control.GotoBeginning(objectControl);
			this._state = MediaState.Stopped;
		}

		/// <summary>
		/// Close this media.
		/// </summary>
		public void Close()
		{
			if (this._state != MediaState.Closed)
				this.Stop();

			this._state = MediaState.Closed;
			Control.Movie = null;
		}

		/// <summary>
		/// Set the source of the media.
		/// </summary>
		/// <param name="path"></param>
		public void SetSource(string path)
		{

			NSError err = new NSError();

			FilePath = path;
			this.video = new QTMovie(path, out err);

			if (this.video != null){
				Control.Movie = this.video;
				Control.IsControllerVisible = false;
				this._state = MediaState.Stopped;
			}

		}

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public byte[] Source
		{
			get
			{
				if (this._state != MediaState.Closed)
					return File.ReadAllBytes(Uri.UnescapeDataString(this.FilePath));
				else
					return null;
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

