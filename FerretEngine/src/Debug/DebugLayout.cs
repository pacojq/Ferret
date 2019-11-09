using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;

namespace FerretEngine.Debug
{
    internal class DebugLayout
    {
#if DEBUG
        private Desktop _desktop;

        private bool _enabled;
#endif
        
        
        internal void LoadContent()
        {
#if DEBUG
            MyraEnvironment.Game = FeGame.Instance;
            
            //string data = File.ReadAllText(filePath);
            //Project project = Project.LoadFromXml(data);
            
            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            var helloWorld = new Label
            {
                Id = "label",
                Text = "Hello, World!"
            };
            grid.Widgets.Add(helloWorld);

            IRenderable bg = new ColoredRegion(new TextureRegion(FeGraphics.Pixel.Texture), new Color(1, 0, 1, 0));

            var obj = new Myra.Graphics2D.UI.TabControl()
            {
                Width = FeGraphics.Resolution.WindowWidth,
                Height = FeGraphics.Resolution.WindowHeight,
                Background = bg,
                OverBackground = bg,
                FocusedBackground = bg
                //Opacity = 0
            };
            
            //obj.Items.Add(new TabItem("Tab 1"));
            //obj.Items.Add(new TabItem("Tab 2"));
            
            
            
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
            //_desktop.Widgets.Add(grid);
            _desktop.Widgets.Add(obj);
#endif
        }



        internal void Update()
        {
#if DEBUG
            if (FeInput.IsKeyPressed(Keys.F1))
                _enabled = !_enabled;
#endif
        }
        
        

        internal void Draw()
        {
#if DEBUG
            if (_enabled)
                _desktop.Render();
#endif
        }
    }
}