using System;
using System.Diagnostics;

namespace Eto.Forms
{
	/// <summary>
	/// Control to show a media player
	/// </summary>
	[Handler(typeof(MediaPlayer.IHandler))]
	public class MediaPlayer : CommonControl
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MediaPlayer"/> class.
		/// </summary>
		public MediaPlayer()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MediaPlayer"/> class.
		/// </summary>
		/// <param name="generator">Generator.</param>
		[Obsolete("Use default constructor instead")]
		public MediaPlayer(Generator generator)
			: this(generator, typeof(IHandler))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.MediaPlayer"/> class.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="type">Type.</param>
		/// <param name="initialize">If set to <c>true</c> initialize.</param>
		[Obsolete("Use default constructor and HandlerAttribute instead")]
		protected MediaPlayer(Generator generator, Type type, bool initialize = true)
			: base(generator, type, initialize)
		{
		}

		/// <summary>
		/// Gets or sets the source blob.
		/// </summary>
		/// <value>The source.</value>
		public byte[] SourceBlob
		{
			get	{ return Handler.SourceBlob; }
			set	{ Handler.SourceBlob = value; }
		}

		/// <summary>
		/// Gets or sets the source file.
		/// </summary>
		/// <value>The source.</value>
		public string SourceFile
		{
			get	{ return Handler.SourceFile; }
			set	{ Handler.SourceFile = value; }
		}

		/// <summary>
		/// Play this media.
		/// </summary>
		public void Play()
		{
			Handler.Play();
		}

		/// <summary>
		/// Pause this media.
		/// </summary>
		public void Pause()
		{
			Handler.Pause();
		}

		/// <summary>
		/// Stop this media.
		/// </summary>
		public void Stop()
		{
			Handler.Stop();
		}

		/// <summary>
		/// Close this media.
		/// </summary>
		public void Close()
		{
			Handler.Close();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Eto.Forms.MediaPlayer"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public new bool Visible
		{
			get { return Handler.Visible; }

			set { Handler.Visible = value; }
		}

		EventHandler<EventArgs> mediaOpened;
		EventHandler<EventArgs> mediaEnded;
		DataReceivedEventHandler outputDataReceived;

		/// <summary>
		/// Event to handle when the media opened
		/// </summary>
		public event EventHandler<EventArgs> MediaOpened
		{
			add { mediaOpened += value; }
			remove { mediaOpened -= value; }
		}

		/// <summary>
		/// Event to handle when the media ended
		/// </summary>
		public event EventHandler<EventArgs> MediaEnded
		{
			add { mediaEnded += value; }
			remove { mediaEnded -= value; }
		}

		/// <summary>
		/// Occurs when output data received.
		/// </summary>
		public event DataReceivedEventHandler OutputDataReceived
		{
			add { outputDataReceived += value; }
			remove { outputDataReceived -= value; }
		}

		/// <summary>
		/// Raises the <see cref="MediaOpened"/> event
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnMediaOpened(EventArgs e)
		{
			if (mediaOpened != null)
				mediaOpened(this, e);
		}

		/// <summary>
		/// Raises the <see cref="MediaEnded"/> event
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnMediaEnded(EventArgs e)
		{
			if (mediaEnded != null)
				mediaEnded(this, e);
		}

		/// <summary>
		/// Raises the output data received event.
		/// </summary>
		/// <param name="e">E.</param>
		protected virtual void OnOutputDataReceived(DataReceivedEventArgs e)
		{
			if (outputDataReceived != null)
				outputDataReceived(this, e);
		}

		/// <summary>
		/// Handler interface for the <see cref="MediaPlayer"/> controls
		/// </summary>
		public new interface IHandler : CommonControl.IHandler
		{
			/// <summary>
			/// Gets or sets the source blob.
			/// </summary>
			/// <value>The source.</value>
			byte[] SourceBlob { get; set; }

			/// <summary>
			/// Gets or sets the source file.
			/// </summary>
			/// <value>The source.</value>
			string SourceFile { get; set; }

			/// <summary>
			/// Play this media.
			/// </summary>
			void Play();

			/// <summary>
			/// Pause this media.
			/// </summary>
			void Pause();

			/// <summary>
			/// Stop this media.
			/// </summary>
			void Stop();

			/// <summary>
			/// Close this media.
			/// </summary>
			void Close();

			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="Eto.Forms.Control"/> is visible to the user.
			/// </summary>
			/// <remarks>
			/// When the visibility of a control is set to false, it will still occupy space in the layout, but not be shown.
			/// The only exception is for controls like the <see cref="Splitter"/>, which will hide a pane if the visibility
			/// of one of the panels is changed.
			/// </remarks>
			/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
			new bool Visible { get; set; }
		}

		static readonly object callback = new Callback();

		/// <summary>
		/// Gets an instance of an object used to perform callbacks to the widget from handler implementations
		/// </summary>
		/// <returns>The callback instance to use for this widget</returns>
		protected override object GetCallback()
		{
			return callback;
		}

		/// <summary>
		/// Callback interface for <see cref="MediaPlayer"/>
		/// </summary>
		public new interface ICallback : CommonControl.ICallback
		{
			/// <summary>
			/// Raises the media opened event.
			/// </summary>
			/// <param name="widget">Widget.</param>
			/// <param name="e">E.</param>
			void OnMediaOpened(MediaPlayer widget, EventArgs e);

			/// <summary>
			/// Raises the media ended event.
			/// </summary>
			/// <param name="widget">Widget.</param>
			/// <param name="e">E.</param>
			void OnMediaEnded(MediaPlayer widget, EventArgs e);

			/// <summary>
			/// Outputs the data received.
			/// </summary>
			/// <param name="widget">Widget.</param>
			/// <param name="e">EventArgs.</param>
			void OutputDataReceived(MediaPlayer widget, DataReceivedEventArgs e);
		}

		/// <summary>
		/// Callback implementation for handlers of <see cref="Button"/>
		/// </summary>
		protected new class Callback : CommonControl.Callback, ICallback
		{
			/// <summary>
			/// Raises the media opened event.
			/// </summary>
			public void OnMediaOpened(MediaPlayer widget, EventArgs e)
			{
				widget.Platform.Invoke(() => widget.OnMediaOpened(e));
			}

			/// <summary>
			/// Raises the media ended event.
			/// </summary>
			public void OnMediaEnded(MediaPlayer widget, EventArgs e)
			{
				widget.Platform.Invoke(() => widget.OnMediaEnded(e));
			}

			/// <summary>
			/// Outputs the data received.
			/// </summary>
			/// <param name="widget">Widget.</param>
			/// <param name="e">EventArgs.</param>
			public void OutputDataReceived(MediaPlayer widget, DataReceivedEventArgs e)
			{
				widget.Platform.Invoke(() => widget.OnOutputDataReceived(e));
			}
		}
	}
}

