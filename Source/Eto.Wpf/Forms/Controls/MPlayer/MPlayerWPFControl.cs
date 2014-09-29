using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Interop;

namespace System.Windows.Controls
{
	/// <summary>
	/// Visualiztion control
	/// </summary>
	public class MPlayerWPFControl : HwndHost
	{
		/// <summary>
		/// Visible constant
		/// </summary>
		const int WS_VISIBLE = 0x10000000;

		/// <summary>
		/// Child constant
		/// </summary>
		const int WS_CHILD = 0x40000000;

		private HwndSource _source;

		/// <summary>
		/// Create handle
		/// </summary>
		/// <param name="hwndParent">Parent handle</param>
		/// <returns>Window handle</returns>
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var parameters = new HwndSourceParameters
			{
				ParentWindow = hwndParent.Handle,
				WindowStyle = WS_VISIBLE | WS_CHILD
			};
			parameters.SetSize(100, 100);

			_source = new HwndSource(parameters);
			//var border = new Border();
			var panel = new Grid();
			//border.Child = panel;
			_source.RootVisual = panel;

			return new HandleRef(null, _source.Handle);
		}

		/// <summary>
		//// Destroy handle
		/// </summary>
		/// <param name="hwnd">Parent Handle</param>
		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			_source.Dispose();
		}
	}
}

