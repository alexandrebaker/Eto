using System;
using Eto.Forms;
using Eto.Drawing;
using System.IO;
using System.Diagnostics;

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
		Button openFromBlobButton;
		Button closeButton;
		Button visibleButton;
		Button hiddenButton;


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
			LogEvents(player);
			return player;
		}

		Control Buttons()
		{
			var layout = new DynamicLayout { Spacing = Size.Empty };

			layout.BeginHorizontal();
			layout.Add(null);
			layout.Add(OpenButton());
			layout.Add(OpenFromBlobButton());
			layout.Add(PlayButton());
			layout.Add(PauseButton());
			layout.Add(StopButton());
			layout.Add(CloseButton());
			layout.Add(ShowButton());
			layout.Add(HideButton());
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
				var ofd = new OpenFileDialog() { MultiSelect = false, Filters = new [] { new FileDialogFilter{ Name = "Media", Extensions = new[] { ".mp3", ".wav", ".mp4", ".flv", ".avi", ".wmv", ".mov", ".swf" } } } };
				if (ofd.ShowDialog(this) == DialogResult.Ok)
				{
					if (File.Exists(ofd.FileName))
						player.SourceFile = ofd.FileName;
				}

			};
			return control;
		}

		Control OpenFromBlobButton()
		{
			var control = openFromBlobButton = new Button
			{
				Text = "Open From Blob"
			};
			control.Click += delegate
			{
				var ofd = new OpenFileDialog() { MultiSelect = false, Filters = new [] { new FileDialogFilter{ Name = "Media", Extensions = new[] { ".mp3", ".wav", ".mp4", ".flv", ".avi", ".wmv", ".mov", ".swf" } } } };
				if (ofd.ShowDialog(this) == DialogResult.Ok)
				{
					if (File.Exists(ofd.FileName))
						player.SourceBlob = File.ReadAllBytes(ofd.FileName);
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
				if (player.SourceFile != null)
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
				if (player.SourceFile != null)
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
				if (player.SourceFile != null)
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
				if (player.SourceFile != null)
					player.Close();
			};
			return control;
		}

		Control ShowButton()
		{
			var control = visibleButton = new Button
			{
				Text = "Show",
				Enabled = false
			};
			control.Click += delegate
			{
				player.Visible = true;
				visibleButton.Enabled = false;
				hiddenButton.Enabled = true;
			};
			return control;
		}

		Control HideButton()
		{
			var control = hiddenButton = new Button
			{
				Text = "Hide",
				Enabled = true
			};
			control.Click += delegate
			{
				player.Visible = false;
				visibleButton.Enabled = true;
				hiddenButton.Enabled = false;
			};
			return control;
		}

		void LogEvents(MediaPlayer control)
		{
			control.OutputDataReceived += (sender, e) =>
			{
				Log.Write(control, e.Data);
			};
		}
	}
}

