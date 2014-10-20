using Eto.Forms;
using Eto.Drawing;
using System.Collections.Generic;

namespace Eto.Test.Sections.Controls
{
	[Section("Controls", typeof(ListBox))]
	public class ListBoxSection : Scrollable
	{
		public ListBoxSection()
		{
			var layout = new DynamicLayout();
			
			layout.AddRow(new Label { Text = "Default" }, Default());
						
			layout.AddRow(new Label { Text = "Virtual list, with Icons" }, WithIcons());

			if (Platform.Supports<ContextMenu>())
				layout.AddRow(new Label { Text = "Context Menu" }, WithContextMenu());

			layout.Add(null);

			Content = layout;
		}

		Control Default()
		{
			var control = new ListBox
			{
				Size = new Size(100, 150)
			};
			LogEvents(control);

			for (int i = 0; i < 10; i++)
			{
				control.Items.Add(new ListItem { Text = "Item " + i });
			}
			
			var layout = new DynamicLayout();
			layout.Add(control);
			layout.BeginVertical();
			layout.AddRow(null, AddRowsButton(control), RemoveRowsButton(control), ClearButton(control), EditButton(control));
			layout.EndVertical();
			
			return layout;
		}

		Control AddRowsButton(ListBox list)
		{
			var control = new Button { Text = "Add Rows" };
			control.Click += delegate
			{
				for (int i = 0; i < 10; i++)
					list.Items.Add(new ListItem { Text = "Item " + list.Items.Count });
			};
			return control;
		}

		Control RemoveRowsButton(ListBox list)
		{
			var control = new Button { Text = "Remove Rows" };
			control.Click += delegate
			{
				if (list.SelectedIndex >= 0)
					list.Items.RemoveAt(list.SelectedIndex);
			};
			return control;
		}

		Control ClearButton(ListBox list)
		{
			var control = new Button { Text = "Clear" };
			control.Click += delegate
			{
				list.Items.Clear();
			};
			return control;
		}

		Control EditButton(ListBox list)
		{
			var control = new Button { Text = "Edit" };
			control.Click += delegate
			{
				list.Items[0].Text = "edit1";
				list.RefreshItems();
			};
			return control;
		}

		class VirtualList : IListStore, IEnumerable<IListItem>
		{
			Icon image = TestIcons.TestIcon;

			public int Count
			{
				get { return 1000; }
			}

			public IListItem this [int index]
			{
				get
				{
					return new ImageListItem { Text = "Item " + index, Image = image };
				}
			}

			public IEnumerator<IListItem> GetEnumerator()
			{
				for (int i = 0; i < Count; i++)
					yield return this[i];
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		Control WithIcons()
		{
			var control = new ListBox
			{
				Size = new Size(100, 150)
			};
			LogEvents(control);
			
			control.DataStore = new VirtualList();
			return control;
		}

		Control WithContextMenu()
		{
			var control = new ListBox
			{
				Size = new Size(100, 150)
			};
			LogEvents(control);

			for (int i = 0; i < 10; i++)
			{
				control.Items.Add(new ListItem { Text = "Item " + i });
			}
			
			var menu = new ContextMenu();
			var item = new ButtonMenuItem { Text = "Click Me!" };
			item.Click += delegate
			{
				if (control.SelectedValue != null)
					Log.Write(item, "Click, Item: {0}", ((ListItem)control.SelectedValue).Text);
				else
					Log.Write(item, "Click, no item selected");
			};
			menu.Items.Add(item);
			
			control.ContextMenu = menu;
			return control;
		}

		void LogEvents(ListBox control)
		{
			control.SelectedIndexChanged += delegate
			{
				Log.Write(control, "SelectedIndexChanged, Index: {0}", control.SelectedIndex);
			};
			control.Activated += delegate
			{
				Log.Write(control, "Activated, Index: {0}", control.SelectedIndex);
			};
		}
	}
}

