using System;
using Eto.Forms;
using MonoTouch.UIKit;
using NSCell = MonoTouch.UIKit.UITableViewCell;
using Eto.Drawing;

namespace Eto.iOS.Forms.Cells
{
	public class ImageTextCellHandler : CellHandler<NSCell, ImageTextCell>, ImageTextCell.IHandler
	{
		public override void Configure(object dataItem, NSCell cell)
		{
			if (Widget.TextBinding != null)
			{
				var val = Widget.TextBinding.GetValue(dataItem);
				cell.TextLabel.Text = Convert.ToString(val);
			}
		}

		public override string TitleForSection(object dataItem)
		{
			if (Widget.TextBinding != null)
			{
				var val = Widget.TextBinding.GetValue(dataItem);
				return Convert.ToString(val);
			}
			return null;
		}

		// TODO
		public ImageInterpolation ImageInterpolation { get; set; }
	}
}