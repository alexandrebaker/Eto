using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Eto.Forms;

namespace Eto.Wpf.Forms
{
	public class MPlayerContext : MPlayerContextBase
	{
		/// <summary>
		/// Initializes static members of the MplayerContext class
		/// </summary>
		public MPlayerContext() :base("mplayer", "mplayer.exe")
		{
		}
	}
}

