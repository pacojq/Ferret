using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using FerretEngine.Sandbox;
using SDL2;
using Color = Microsoft.Xna.Framework.Color;

namespace FerretEngine.Editor
{
    public partial class FeEditorForm : Form
    {
        
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(
            IntPtr handle,
            IntPtr handleAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint flags
        );
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr handle, int command);

        
        
        
        
        private static SandboxGame game;
        private Panel gamePanel;
        private bool windowAttached = false;
        
        private Random random = new Random();
        
        
        public FeEditorForm()
        {
            // This is what we're going to attach the SDL2 window to
            gamePanel = new Panel();
            gamePanel.Size = new Size(640, 480);
            gamePanel.Location = new Point(80, 10);

            // Make the WinForms window
            Size = new Size(800, 600);
            FormClosing += new FormClosingEventHandler(WindowClosing);
            Button button = new Button();
            button.Text = "Whatever";
            button.Location = new Point(
                (Size.Width / 2) - (button.Size.Width / 2),
                gamePanel.Location.Y + gamePanel.Size.Height + 10
            );
            button.Click += new EventHandler(ClickedButton);
            Controls.Add(button);
            Controls.Add(gamePanel);
            
            //InitializeComponent();
        }
        
        
        private void ClickedButton(object sender, EventArgs e)
        {
            if (!windowAttached)
            {
                // Make the Game, give it time to start up
                new Thread(GameThread).Start();
                Thread.Sleep(1000);

                // Get the Win32 HWND from the FNA window
                SDL.SDL_SysWMinfo info = new SDL.SDL_SysWMinfo();
                SDL.SDL_GetWindowWMInfo(game.Window.Handle, ref info);
                IntPtr winHandle = info.info.win.window;

                // Move the SDL2 window to 0, 0
                game.Window.IsBorderlessEXT = true;
                SetWindowPos(
                    winHandle,
                    Handle,
                    0,
                    0,
                    0,
                    0,
                    0x0401 // NOSIZE | SHOWWINDOW
                );

                // Attach the SDL2 window to the panel
                SetParent(winHandle, gamePanel.Handle);
                ShowWindow(winHandle, 1); // SHOWNORMAL

                // We out.
                windowAttached = true;
            }

            if (game.Scene != null)
            {
                game.Scene.BackgroundColor = new Color(
                    (float) random.NextDouble(),
                    (float) random.NextDouble(),
                    (float) random.NextDouble(),
                    1.0f
                );
            }
        }
        
        
        private void WindowClosing(object sender, FormClosingEventArgs e)
        {
            game.Exit();
        }
        
        private static void GameThread()
        {
            using (game = new SandboxGame())
            {
                game.Run();
            }
        }
        
        
    }
}