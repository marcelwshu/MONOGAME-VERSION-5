using Microsoft.Xna.Framework;
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
        private int TileSize = 34;
        private int RockSize = 80;
        private int RockSpawnAmountMin = 4;
        private int RockSpawnAmountMax = 10;
        private float RockSpawnInterval = 1.5f;
        double lastEnemyRender = 0;

        private Text ScoreText;
        Vector2 PLAYER_SIZE = new Vector2(150, 150);

        //
        Player player;



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

            var weightedTexturesLevel1 = new List<(string textureName, int weight)>
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

            var weightedTexturesLevel2 = new List<(string textureName, int weight)>
            {
            ("Sand1", 10), // Big rock
            ("Sand2", 200), // Smallr ock
            ("Sand3", 150), // Med
            ("Sand4", 150), // Med
            ("Sand5", 10), // Big
            ("Sand6", 10), // Big
            ("Sand7", 10), // Big
            ("Sand8", 200), // Small
            ("Sand9", 800), // Smooth
            ("Sand10", 800), // Smooth
            };

            var weightedTexturesLevel3 = new List<(string textureName, int weight)>
            {
            ("Clay1", 150), // Med
            ("Clay2", 150), // Med
            ("Clay3", 10),  // Big
            ("Clay4", 200), // Small
            ("Clay5", 10),  // Big
            ("Clay6", 10), // Big
            ("Clay7", 800), // Smooth
            ("Clay8", 10), // Big
            ("Clay9", 800), // Smooth
            ("Clay10", 200), // Small
            };


            if (Game1.CurrentGameSpeed >= Game1.Level3Threshold + (Game1.DefaultGameSpeed))
            {
                return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel3));
            }


            if (Game1.CurrentGameSpeed >= Game1.Level2Threshold + (Game1.DefaultGameSpeed))
            {
                return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel2));
            }

            // Default 
            return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel1));


        }

        private Texture2D GetRandomRockTexture()
        {

            // Level 1
            var weightedTexturesLevel1 = new List<(string textureName, int weight)>
            {
            ("Rock1", 1), 
            ("Rock2", 1), 
            ("Rock3", 1), 
            ("Rock4", 1), 
            ("Rock5", 1), 
            ("Rock6", 1), 
            ("Rock7", 1), 
            ("Rock8", 1),
            ("Rock9", 1), 
            ("Rock10", 1),
            };

            // Level 2
            var weightedTexturesLevel2 = new List<(string textureName, int weight)>
            {
            ("Tree1", 3), 
            ("Tree2", 1), 
            ("Tree3", 5), 
            //("Tree4", 3), 
            ("Tree5", 1), 
            ("Tree6", 2), 
            ("Tree7", 3), 
            ("Tree8", 1),
            ("Tree9", 1),
            ("Tree10", 1),
            ("Tree11", 1),
            };

            // Level 3
            var weightedTexturesLevel3 = new List<(string textureName, int weight)>
            {
            ("Bones1", 3), // Skull
            ("Bones2", 1), // Mouth
            ("Bones3", 5), // Bone
            ("Bones4", 3), // Skull
            ("Bones5", 1), // Open mouth
            ("Bones6", 2), // broke bone
            ("Bones7", 3), // Skull
            ("Grave1", 1), 
            ("Grave2", 1), 
            ("Grave3", 1), 
            };


            if (Game1.CurrentGameSpeed >= Game1.Level3Threshold + (Game1.DefaultGameSpeed))
            {
                return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel3));
            }

            if (Game1.CurrentGameSpeed >= Game1.Level2Threshold + (Game1.DefaultGameSpeed))
            {
                return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel2));
            }

  
            // Default 
            return Content.Load<Texture2D>(WeightedProbability.GetRandomWeightedItem(weightedTexturesLevel1));
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
            float PLAYER_Y_STATIC = 0.9f; // A ratio of how far down the screen the vehicle is, 0 being top, 1 being down
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


            player =  Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();

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


        public async void UpdateScene(GameTime gameTime)
        {

            if (CurrentScene != "Playing")
            {
                return;
            }


            // Speed game up
            Game1.CurrentGameSpeed += Game1.SpeedUpAmount;


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
                    BackgroundObject tile = new BackgroundObject(GetRandomGroundTexture(), new Vector2(TileSize * i, -TileSize), new Vector2(TileSize*1.2f, TileSize*1.2f), 0); // Slightly oversize to avoid gaps in renderer...

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
                    int randomY = random.Next(-500, -300);

                    Debris Rock = new Debris(GetRandomRockTexture(), new Vector2(randomX, randomY), new Vector2(RockSize, RockSize), 1);  


                    // Warn
                    Sprite WarnSprite = new Sprite(Content.Load<Texture2D>("Warning"), new Vector2(randomX, 0), new Vector2(150, 150), 3);
                    WarnSprite.pos = new Vector2(randomX - 50, 0);

                    //await Task.Delay( ((int)MathF.Abs(randomY / (int)Game1.CurrentGameSpeed)) * 1000 );
                    await Task.Delay( 500 );
                    activeSprites.Remove(WarnSprite);

                }


            }



            // Generate random enemies

            if (Game1.CurrentGameSpeed >= Game1.Level2Threshold + (Game1.DefaultGameSpeed))
            {

                if ((timeNow - lastEnemyRender) > 1)
                {
                    lastEnemyRender = timeNow;

                    Random random = new Random();

                    int randomAmount = random.Next(1, 2);

                    for (int i = 0; i < randomAmount; i++)
                    {

                        //int randomX = random.Next(-(int)Game1.WINDOW_SIZE.X, (int)Game1.WINDOW_SIZE.X);
                        int randomX = (int)player.pos.X + random.Next(-(int)Game1.WINDOW_SIZE.X / 4, (int)Game1.WINDOW_SIZE.X / 4);

                        int randomY = random.Next(-500, -300);

                        Enemy Enemy;
                        if (Game1.CurrentGameSpeed >= Game1.Level3Threshold + (Game1.DefaultGameSpeed))
                        {
                            Enemy = new Enemy(Content.Load<Texture2D>("EnemyHeavy"), new Vector2(randomX, randomY), new Vector2(PLAYER_SIZE.X, PLAYER_SIZE.Y), 1, 3, true);
                        }
                        else
                        {
                            Enemy = new Enemy(Content.Load<Texture2D>("EnemyStandard"), new Vector2(randomX, randomY), new Vector2(PLAYER_SIZE.X, PLAYER_SIZE.Y), 1, 1, false);
                        }



                        // Warn
                        Sprite WarnSprite = new Sprite(Content.Load<Texture2D>("warningRed"), new Vector2(randomX, 0), new Vector2(150, 150), 3);
                        WarnSprite.pos = new Vector2(randomX - 50, 0);

                        //await Task.Delay( ((int)MathF.Abs(randomY / (int)Game1.CurrentGameSpeed)) * 1000 );
                        await Task.Delay(500);
                        activeSprites.Remove(WarnSprite);




                    }


                }

            }





        }

    }
}
