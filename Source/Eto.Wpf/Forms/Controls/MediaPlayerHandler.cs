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
	public class MediaPlayerHandler : WpfFrameworkElement<swc.MediaElement, MediaPlayer, MediaPlayer.ICallback>, MediaPlayer.IHandler
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
		/// Initializes a new instance of the <see cref="Eto.Wpf.Forms.MediaPlayerHandler"/> class.
		/// </summary>
		public MediaPlayerHandler()
		{
			Control = new swc.MediaElement()
			{
				LoadedBehavior = swc.MediaState.Manual,
				Height = 100,
				Width = 100
			};
			Control.MediaOpened += (sender, e) => Callback.OnMediaOpened(Widget, EventArgs.Empty);
			Control.MediaEnded += (sender, e) => Callback.OnMediaEnded(Widget, EventArgs.Empty);
		}

		/// <summary>
		/// Play this media.
		/// </summary>
		public void Play()
		{
			Control.Play();
		}

		/// <summary>
		/// Pause this media.
		/// </summary>
		public void Pause()
		{
			Control.Pause();
		}

		/// <summary>
		/// Stop this media.
		/// </summary>
		public void Stop()
		{
			Control.Stop();
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void Close()
		{
			Control.Close();
		}

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public byte[] Source
		{
			get
			{
				if (Control.Source.AbsolutePath != null && Control.Source.AbsolutePath.Trim() != String.Empty)
					return File.ReadAllBytes(Uri.UnescapeDataString(Control.Source.AbsolutePath));
				else
					return null;
			}
		}

		/// <summary>
		/// Set the source of the media.
		/// </summary>
		/// <param name="path"></param>
		public void SetSource(string path)
		{
			Control.Source = new Uri(path);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Eto.Wpf.Forms.MediaPlayerHandler"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible
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
		public override Color BackgroundColor
		{
			get { return new Color(); }
			set { }
		}

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
		public Font Font
		{
			get { return null; }
			set { }
		}
	}
}

