using System;
using Eto.Drawing;

namespace Eto.Forms
{
	/// <summary>
	/// Base tool item class for a <see cref="ToolBar"/>.
	/// </summary>
	public abstract class ToolItem : Widget, ICommandItem
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		/// <summary>
		/// Occurs when the user clicks on the item.
		/// </summary>
		public event EventHandler<EventArgs> Click;

		/// <summary>
		/// Raises the <see cref="Click"/> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		public void OnClick(EventArgs e)
		{
			if (Click != null)
				Click(this, e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.ToolItem"/> class.
		/// </summary>
		protected ToolItem()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.ToolItem"/> class with the specified <paramref name="command"/>.
		/// </summary>
		/// <param name="command">Command to initialize the tool item with.</param>
		protected ToolItem(Command command)
		{
			ID = command.ID;
			Text = command.ToolBarText;
			ToolTip = command.ToolTip;
			Tag = command.Tag;
			Image = command.Image;
			Click += (sender, e) => command.Execute();
			Enabled = command.Enabled;
			command.EnabledChanged += (sender, e) => Enabled = command.Enabled;
			Order = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.ToolItem"/> class.
		/// </summary>
		/// <param name="command">Command.</param>
		/// <param name="generator">Generator.</param>
		/// <param name="type">Type.</param>
		/// <param name="initialize">If set to <c>true</c> initialize.</param>
		[Obsolete("Use ToolItem(Command) instead")]
		protected ToolItem(Command command, Generator generator, Type type, bool initialize = true)
			: base(generator, type, initialize)
		{
			ID = command.ID;
			Text = command.ToolBarText;
			ToolTip = command.ToolTip;
			Tag = command.Tag;
			Image = command.Image;
			Click += (sender, e) => command.Execute();
			Enabled = command.Enabled;
			command.EnabledChanged += (sender, e) => Enabled = command.Enabled;
			Order = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.ToolItem"/> class.
		/// </summary>
		/// <param name="g">The green component.</param>
		/// <param name="type">Type.</param>
		[Obsolete("Use default constructor and HandlerAttribute instead")]
		protected ToolItem(Generator g, Type type)
			: base(g, type)
		{
		}

		/// <summary>
		/// Gets or sets the order of the tool item when adding to the <see cref="ToolItemCollection"/>.
		/// </summary>
		/// <value>The order when adding the item.</value>
		public int Order { get; set; }

		/// <summary>
		/// Gets or sets the text of the item, with mnemonic.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return Handler.Text; }
			set { Handler.Text = value; }
		}

		/// <summary>
		/// Gets or sets the tool tip to show when hovering the mouse over the item.
		/// </summary>
		/// <value>The tool tip.</value>
		public string ToolTip
		{
			get { return Handler.ToolTip; }
			set { Handler.ToolTip = value; }
		}

		/// <summary>
		/// Gets or sets the image for the tool item.
		/// </summary>
		/// <value>The image.</value>
		public Image Image
		{
			get { return Handler.Image; }
			set { Handler.Image = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Eto.Forms.ToolItem"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool Enabled
		{
			get { return Handler.Enabled; }
			set { Handler.Enabled = value; }
		}

		/// <summary>
		/// Gets or sets a user-defined tag for the tool item.
		/// </summary>
		/// <value>The user-defined tag.</value>
		public object Tag { get; set; }

		/// <summary>
		/// Called when the tool item is loaded to be shown on the form.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		internal protected virtual void OnLoad(EventArgs e)
		{
		}

		/// <summary>
		/// Called when the tool item is removed from a form.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		internal protected virtual void OnUnLoad(EventArgs e)
		{
		}

		/// <summary>
		/// Handler interface for the <see cref="ToolItem"/>.
		/// </summary>
		public new interface IHandler : Widget.IHandler
		{
			/// <summary>
			/// Gets or sets the image for the tool item.
			/// </summary>
			/// <value>The image.</value>
			Image Image { get; set; }

			/// <summary>
			/// Creates the item from a command instance.
			/// </summary>
			/// <remarks>
			/// This is useful when using a platform-defined command. It allows you to create the item in a specific
			/// way based on the command it is created from.
			/// </remarks>
			/// <param name="command">Command the item is created from.</param>
			void CreateFromCommand(Command command);

			/// <summary>
			/// Gets or sets the text of the item, with mnemonic.
			/// </summary>
			/// <value>The text.</value>
			string Text { get; set; }

			/// <summary>
			/// Gets or sets the tool tip to show when hovering the mouse over the item.
			/// </summary>
			/// <value>The tool tip.</value>
			string ToolTip { get; set; }

			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="Eto.Forms.ToolItem"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			bool Enabled { get; set; }
		}
	}
}
