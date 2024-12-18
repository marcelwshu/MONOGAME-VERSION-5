using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        internal static Audio _audioManager;

        public static Vector2 WINDOW_SIZE;
        internal static List<Text> _texts;

        // Config 
        public static float SpeedUpAmount = 0.05f; // How fast the game speed increases

        public static float DefaultGameSpeed = 250; // Initial speed of game
        public static float CurrentGameSpeed = DefaultGameSpeed;

        public static int Level2Threshold = 250; // When level 2 loads
        public static int Level3Threshold = 500; // When level 3 loads



        public Game1() // Runs first
        {

            // Init vars
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            _texts = new List<Text>();


            // Update window size var
            WINDOW_SIZE = new Vector2(displayMode.Width, displayMode.Height);


            // Set resolution to monitor, full screen
            _graphics.PreferredBackBufferWidth = (int)WINDOW_SIZE.X;
            _graphics.PreferredBackBufferHeight = (int)WINDOW_SIZE.Y;
            _graphics.IsFullScreen = true; // Add this line
            _graphics.ApplyChanges();


            // Uncap framerate to 144
            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(6.94);


        }

        protected override void Initialize()
        {

            // Init services
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneManager = new SceneManager(GraphicsDevice, Content);
            _audioManager = new Audio(Content);

            // Load start screen
            _sceneManager.LoadMenu("StartScreen");

            base.Initialize();
        }

        protected override void LoadContent() // Runs after initialize
        {

            // Load sounds
            Content.Load<SoundEffect>("Audio/HealthPickup");
            Content.Load<SoundEffect>("Audio/WhiskeyPickup");

        }

        protected override void Update(GameTime gameTime)
        {

            // Exit game
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

            // Update the active scene
            _sceneManager.UpdateScene(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Dirty hack for hiding gaps in tile renderer (Sets background to tile color average)
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


            // Render the active scene
            _sceneManager.DrawScene();


            // Draw the text objects (Must be rendered last)
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