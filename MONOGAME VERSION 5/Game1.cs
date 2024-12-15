using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MONOGAME_VERSION_5;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MONOGAME_VERSION_5
{
    public class Game1 : Game
    {
        // Vars
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal static SceneManager _sceneManager;

        // List to store Text objects
        internal static List<Text> _texts;

        // CONFIG
        private bool DEBUG_ENABLED = false;
        public static Vector2 WINDOW_SIZE;
        public static float DefaultGameSpeed = 120;
        public static float CurrentGameSpeed = DefaultGameSpeed;
        public static int Level2Threshold = 500;
        public static int Level3Threshold = 1000;


        public Game1() // Runs first
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set window size
            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            WINDOW_SIZE = new Vector2(displayMode.Width, displayMode.Height);

            _graphics.PreferredBackBufferWidth = (int)WINDOW_SIZE.X;
            _graphics.PreferredBackBufferHeight = (int)WINDOW_SIZE.Y;
            _graphics.IsFullScreen = true; // Add this line
            _graphics.ApplyChanges();

            // Initialize the list of Text objects
            _texts = new List<Text>();

            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(6.94);

        }

        protected override void Initialize()
        {
            // Init services
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneManager = new SceneManager(GraphicsDevice, Content);


  


            // Load start screen
            _sceneManager.LoadMenu("StartScreen");

            base.Initialize();
        }

        protected override void LoadContent() // Runs after initialize
        {

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Listen for user input to start game
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _sceneManager.CurrentScene == "StartScreen")
            {
                _sceneManager.LoadLevel();
            }

            // Listen for user input to restart game
            if (Keyboard.GetState().IsKeyDown(Keys.R) && _sceneManager.CurrentScene == "EndScreen")
            {
                _sceneManager.LoadLevel();
            }

            // Update scene
            _sceneManager.UpdateScene(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear screen, hack fix for gaps between tiles
            if (Game1.CurrentGameSpeed >= Game1.Level3Threshold + (Game1.DefaultGameSpeed))
            {
                GraphicsDevice.Clear(new Color(162, 93, 38)); // Brown
                
            }
            else if (Game1.CurrentGameSpeed >= Game1.Level2Threshold + (Game1.DefaultGameSpeed))
            {

                GraphicsDevice.Clear(new Color(250, 223, 165)); // Sand
            }
            else
            {
                GraphicsDevice.Clear(new Color(153, 185, 0)); // Green
            }





            // Draw scene
            _sceneManager.DrawScene();


            // Draw the text objects
            _spriteBatch.Begin();
            foreach (var text in _texts)
            {
                text.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}