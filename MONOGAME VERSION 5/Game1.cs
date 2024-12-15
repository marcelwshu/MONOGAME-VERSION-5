using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MONOGAME_VERSION_5.Content;
using System;
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


        // CONFIG
        private bool DEBUG_ENABLED = false;
        public static Vector2 WINDOW_SIZE = new Vector2(1000, 1000);
        public static float GameSpeed = 100;




        
        public Game1() // Runs first
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set window size
            _graphics.PreferredBackBufferWidth = (int)WINDOW_SIZE.X;
            _graphics.PreferredBackBufferHeight = (int)WINDOW_SIZE.Y;
            _graphics.ApplyChanges();


        }





        protected override void Initialize()
        {

            // Init services
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneManager = new SceneManager(GraphicsDevice, Content);


            // Load start screen
            _sceneManager.LoadMenu("StartScreen");

            
            //
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

            // Clear screen
            GraphicsDevice.Clear(new Color(153, 185, 0));

            // Draw scene
            _sceneManager.DrawScene();

            base.Draw(gameTime);

        }
    }
}
