﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace MONOGAME_VERSION_5
{
    internal class SceneManager
    {

        // Private Vars
        private GraphicsDevice GraphicsDevice;
        private ContentManager Content;
        private SpriteBatch SpriteBatch;
        private double LastRowRender = 0.0f;
        private double LastRockRender = 0.0f;
        private int TileSize = 30;
        private int RockSize = 80;
        private int RockSpawnAmountMin = 4;
        private int RockSpawnAmountMax = 10;
        private float RockSpawnInterval = 1.5f;

        private Text ScoreText;



        // Public Vars
        public string CurrentScene; // StartMenu , EndMenu , Playing
        public List<Sprite> activeSprites;



        // Constructor
        public SceneManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            GraphicsDevice = graphicsDevice;
            Content = contentManager;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            CurrentScene = "GAME_SCENE_NIL";
            activeSprites = new List<Sprite>();

        }


        // Functions
        private Texture2D GetRandomGroundTexture()
        {
            var weightedTextures = new List<(string textureName, int weight)>
            {
            ("Grass1", 175), // Small/med rock
            ("Grass2", 200), // Small rock
            ("Grass3", 10), // Big rock
            ("Grass4", 50), // Big/med rock
            ("Grass5", 10), // Big rock
            ("Grass6", 800), // Smooth
            ("Grass7", 10), // Big rock
            ("Grass8", 800), // Smooth
            ("Grass9", 150), // Med rock
            ("Grass10", 200), // Small rock
            };

            return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTextures));
        }

        private Texture2D GetRandomRockTexture()
        {
            string[] textureNames = {
            "Rock1", "Rock2", "Rock3", "Rock4", "Rock5",
            "Rock6", "Rock7", "Rock8", "Rock9",
            };

            Random random = new Random();
            string randomTextureName = textureNames[random.Next(textureNames.Length)];
            return Content.Load<Texture2D>(randomTextureName);
        }


        // Methods
        public void LoadMenu(string sceneString)
        {

            // Setup
            Game1._texts.Clear();
            activeSprites.Clear();
            CurrentScene = sceneString;


            // Load start/end screen
            Vector2 ScreenSize = new Vector2(Game1.WINDOW_SIZE.X, Game1.WINDOW_SIZE.Y);
            Vector2 Pos = new Vector2((Game1.WINDOW_SIZE.X / 2) - (ScreenSize.X / 2), (Game1.WINDOW_SIZE.Y / 2) - (ScreenSize.Y / 2)); // Center the sprite

            Sprite Menu = new Sprite(Content.Load<Texture2D>(sceneString), Pos, ScreenSize, 1);


            // Game over screen
            if (sceneString == "EndScreen") 
            {
                // Score text
                ScoreText = new Text(" ", new Vector2(0, 0), Color.MediumPurple, Content.Load<SpriteFont>("Font1"));

                string finalText = "Score this run: " + Math.Round(Game1.CurrentGameSpeed - Game1.DefaultGameSpeed);
                Vector2 textSizeOffset = ScoreText.font.MeasureString(finalText);

                ScoreText.str = finalText;
                ScoreText.position = new Vector2((Game1.WINDOW_SIZE.X / 2) - textSizeOffset.X/2, Game1.WINDOW_SIZE.Y/2 + Game1.WINDOW_SIZE.Y / 4);
                
                Game1._texts.Add(ScoreText);
            }




        }

        // Note: maybe make sprite constructor automagically add to active sprites?

        public void LoadLevel()
        {


            Console.WriteLine("Loading level");



            // Setup
            Game1._texts.Clear();
            activeSprites.Clear();
            CurrentScene = "Playing";
            Game1.CurrentGameSpeed = Game1.DefaultGameSpeed;


   
            // Create player object
            Vector2 PLAYER_SIZE = new Vector2(150, 150);
            float PLAYER_Y_STATIC = 0.7f; // A ratio of how far down the screen the vehicle is, 0 being top, 1 being down
            Vector2 PLAYER_DEFAULT_POS = new Vector2((Game1.WINDOW_SIZE.X / 2) - (PLAYER_SIZE.X / 2), (Game1.WINDOW_SIZE.Y * PLAYER_Y_STATIC) - (PLAYER_SIZE.Y / 2));

            Player Vehicle = new Player(Content.Load<Texture2D>("Vehicle"), PLAYER_DEFAULT_POS, PLAYER_SIZE, 2);


            // Score text
            ScoreText = new Text("Score: ", new Vector2(PLAYER_DEFAULT_POS.X, 0), Color.Black, Content.Load<SpriteFont>("Font1"));
            Game1._texts.Add(ScoreText);


            // Load initial tiles for map
            for (int x = 0; x < Game1.WINDOW_SIZE.X / TileSize; x++)
            {
                for (int y = -1; y < (Game1.WINDOW_SIZE.X / TileSize) + 1; y++)
                {
                    BackgroundObject tile = new BackgroundObject(GetRandomGroundTexture(), new Vector2(TileSize * x, TileSize * y), new Vector2(TileSize, TileSize), 0);
                }
            }

        }


        public void DrawScene()
        {

            var sortedSprites = activeSprites.OrderBy(s => s.depth).ToList();
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (var sprite in sortedSprites)
            {
                sprite.Render(SpriteBatch);
            }
            SpriteBatch.End();

        }


        public void UpdateScene(GameTime gameTime)
        {

            if (CurrentScene != "Playing")
            {
                return;
            }


            // Speed game up
            Game1.CurrentGameSpeed += 0.1f;


            ScoreText.str = "Score: " + Math.Round(Game1.CurrentGameSpeed - Game1.DefaultGameSpeed);


            // Update sprites
            var sortedSprites = activeSprites.OrderBy(s => s.depth).ToList();
            foreach (var sprite in sortedSprites)
            {
                sprite.Update(gameTime);

            }


            // Draw next row of terrain
            double timeNow = gameTime.TotalGameTime.TotalSeconds;
            float timeStep = TileSize / Game1.CurrentGameSpeed;
            if ((timeNow - LastRowRender) >= timeStep)
            {
                LastRowRender = timeNow;


                for (int i = 0; i < Game1.WINDOW_SIZE.X /TileSize; i++)
                {
                    BackgroundObject tile = new BackgroundObject(GetRandomGroundTexture(), new Vector2(TileSize * i, -TileSize), new Vector2(TileSize, TileSize), 0);

                    // Random rotation of tile
                    Random random = new Random();
                    float newRot = random.Next(1, 4) * (MathF.PI/2);

                    tile.rotation = newRot;

                }


            }


            // Generate random debris
            if ((timeNow - LastRockRender) > RockSpawnInterval)
            {
                LastRockRender = timeNow;

                Random random = new Random();

                int randomAmount = random.Next(RockSpawnAmountMin, RockSpawnAmountMax);

                for (int i = 0; i < randomAmount; i++)
                {
                    
                    int randomX = random.Next(-(int)Game1.WINDOW_SIZE.X, (int)Game1.WINDOW_SIZE.X);
                    int randomY = random.Next(-500, -150);

                    Debris Rock = new Debris(GetRandomRockTexture(), new Vector2(randomX, randomY), new Vector2(RockSize, RockSize), 1);
 
                }


            }




        }

    }
}
