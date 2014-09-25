using System;
using Eto.Forms;
using Eto.Drawing;
using System.IO;

namespace Eto.Test.Sections.Controls
{
	[Section("Controls", typeof(MediaPlayer))]
	public class MediaPlayerSection : Scrollable
	{
		MediaPlayer player;
		Button playButton;
		Button pauseButton;
		Button stopButton;
		Button openButton;
		Button closeButton;


		public MediaPlayerSection()
		{
			var layout = new DynamicLayout();

			var mediaPlayer = MediaPlayer();
			layout.Add(Buttons());
			layout.Add(mediaPlayer);
		

			Content = layout;
		}

		Control MediaPlayer()
		{
			player = new Eto.Forms.MediaPlayer();
			return player;
		}

		Control Buttons()
		{
			var layout = new DynamicLayout { Spacing = Size.Empty };

			layout.BeginHorizontal();
			layout.Add(null);
			layout.Add(OpenButton());
			layout.Add(PlayButton());
			layout.Add(PauseButton());
			layout.Add(StopButton());
			layout.Add(CloseButton());
			layout.Add(null);
			layout.EndHorizontal();

			return layout;
		}


		Control OpenButton()
		{
			var control = openButton = new Button
			{
				Text = "Open"
			};
			control.Click += delegate
			{
				var ofd = new OpenFileDialog() { MultiSelect = false, Filters = new [] { new FileDialogFilter{ Name = "Media", Extensions = new[] { ".mp3", ".wav", ".mp4", ".flv", ".avi", ".wmv" } } } };
				if (ofd.ShowDialog(this) == DialogResult.Ok)
				{
					if (File.Exists(ofd.FileName))
						player.SetSource(ofd.FileName);
				}

			};
			return control;
		}

		Control PlayButton()
		{
			var control = playButton = new Button
			{
				Text = "Play"
			};
			control.Click += delegate
			{
				if (player.Source != null)
					player.Play();
			};
			return control;
		}

		Control PauseButton()
		{
			var control = pauseButton = new Button
			{
				Text = "Pause"
			};
			control.Click += delegate
			{
				if (player.Source != null)
					player.Pause();
			};
			return control;
		}

		Control StopButton()
		{
			var control = stopButton = new Button
			{
				Text = "Stop"
			};
			control.Click += delegate
			{
				if (player.Source != null)
					player.Stop();
			};
			return control;
		}
		Control CloseButton()
		{
			var control = closeButton = new Button
			{
				Text = "Close"
			};
			control.Click += delegate
			{
				if (player.Source != null)
					player.Close();
			};
			return control;
		}
	}
}

