using System;
using System.IO;

namespace Eto.Wpf.Forms
{
	public static class MPlayerContextExtensions
	{
		/// <summary>
		/// Pause the specified context and state.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="state">State.</param>
		public static void Pause(this MPlayerContext context, MediaPlayerHandler handler)
		{
			if (handler.State == MediaState.Playing)
			{
				context.SendCommand("pause 1");
				handler.State = MediaState.Paused;
			}
		}

		/// <summary>
		/// Play the specified context and handler.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="handler">Handler.</param>
		public static void Play(this MPlayerContext context, MediaPlayerHandler handler)
		{
			if (handler.State == MediaState.Paused || handler.State == MediaState.Stopped)
			{
				context.SendCommand("pause 0");
				handler.State = MediaState.Playing;
			}
		}

		/// <summary>
		/// Stop the specified context and handler.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="handler">Handler.</param>
		public static void Stop(this MPlayerContext context, MediaPlayerHandler handler)
		{
			if (handler.State == MediaState.Playing || handler.State == MediaState.Paused)
			{
				context.SendCommand("stop");
				handler.State = MediaState.Stopped;
			}
		}

		/// <summary>
		/// Close the specified context and handler.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="handler">Handler.</param>
		public static void Close(this MPlayerContext context, MediaPlayerHandler handler)
		{
			context.Stop(handler);
			context.CloseAll();

			// Delete the temporary file
			if (handler.MediaFile != null && File.Exists(handler.MediaFile))
			{
				try
				{
					File.Delete(handler.MediaFile);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}

