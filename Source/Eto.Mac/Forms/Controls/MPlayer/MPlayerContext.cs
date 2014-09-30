using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Eto.Forms;

namespace Eto.Mac.Forms.Controls
{
	/// <summary>
	/// Class for managing relations with mplayer application
	/// </summary>
	public class MPlayerContext : MPlayerContextBase
	{
		/// <summary>
		/// Initializes static members of the MplayerContext class
		/// </summary>
		public MPlayerContext() :base("mplayer", "mplayer")
		{
		}
	}
}

