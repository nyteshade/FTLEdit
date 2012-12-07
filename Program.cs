using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.Window;
using System.Windows.Forms;

namespace FTLShipEdit
{


    static class Program
    {
        static Game game;
        public static RenderWindow app;
        [STAThread]
        static void Main(string[] args)
        {
            game = new Game();
            game.Start();
            StartupForm startup = new StartupForm();
            DialogResult startupResult;
            do
            {
                startupResult = startup.ShowDialog();
                if (startupResult == DialogResult.Abort)
                {
                    Application.Exit();
                    Environment.Exit(1);
                    return;
                }
            }
            while(startupResult == DialogResult.Cancel);

            app = new RenderWindow(new VideoMode(800, 600), "FTLEdit: Ship Editor", Styles.Default);
            app.Closed += new EventHandler(OnClosed);
            app.KeyPressed += new EventHandler<SFML.Window.KeyEventArgs>(OnKeyPressed); 
            
            app.SetFramerateLimit(30);
            
            while (app.IsOpen())
            {
                app.DispatchEvents();
                app.Clear();
                game.Draw(app);
                game.Update();
                app.Display();
                if(game.cursorMode != CursorMode.PlacedBGCursor)
                    Application.DoEvents();
            }
        }

        static void OnClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
        }
        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        static void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            Window window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();

            game.OnKeyPressed(sender, e);
        }
    }
}
