using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace FerretEngine.Graphics
{
	public class DrawGUI
	{

		private FerretGame _game;
		private Desktop _desktop;

		public DrawGUI(FerretGame game)
		{
			_game = game;
		}
		

		public void LoadContent()
		{
			MyraEnvironment.Game = _game;

			var grid = new Grid
			{
				RowSpacing = 8,
				ColumnSpacing = 8
			};

			grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
			grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

// TextBlock
			var helloWorld = new TextBlock
			{
				Id = "label",
				Text = "Hello, World!"
			};
			grid.Widgets.Add(helloWorld);

// ComboBox
			var combo = new ComboBox
			{
				GridColumn = 1,
				GridRow = 0
			};

			combo.Items.Add(new ListItem("Red", Color.Red));
			combo.Items.Add(new ListItem("Green", Color.Green));
			combo.Items.Add(new ListItem("Blue", Color.Blue));
			grid.Widgets.Add(combo);

// Button
			var button = new TextButton
			{
				GridColumn = 0,
				GridRow = 1,
				Text = "Show"
			};

			button.Click += (s, a) =>
			{
				var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
				messageBox.ShowModal(_desktop);
			};

			grid.Widgets.Add(button);

// Spin button
			var spinButton = new SpinButton
			{
				GridColumn = 1,
				GridRow = 1,
				Width = 100,
				Nullable = true
			};
			grid.Widgets.Add(spinButton);

// Add it to the desktop
			_desktop = new Desktop();
			_desktop.Widgets.Add(grid);
		}



		public void Render()
		{
			_desktop.Render();
		}
	}
}