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

        private Text ScoreText;
        private Player player;

        private double LastRowRender = 0.0f;
        private double LastDebrisSpawn = 0.0f;

        private double LastEnemySpawn = 0;
        private float EnemySpawnDebounce = 4f;

        private double LastPickupSpawn = 0;
        private float PickupSpawnDebounce = 4f;

        // Config
        private int TileSize = 34;
        private int DebrisSize = 80;
        private int WarnSize = 100;

        private int WarnDuration = 500;

        private int DebrisSpawnAmountMin = 4;
        private int DebrisSpawnAmountMax = 10;
        private float DebrisSpawnInterval = 1.5f;
        private float DebrisRandomYMax = 200;
        private float DebrisStaticYOffset = 200;

        private Vector2 PLAYER_SIZE = new Vector2(150, 150);
        private float PLAYER_Y_STATIC = 0.9f; // A ratio of how far down the screen the vehicle is, 0 being top, 1 being down
        private int DEFAULT_PLR_HP = 3;
        private int PLR_DEPTH = 1;


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
        public void LoadMenu(string sceneString) // Either start or end screen
        {

            // Setup
            Game1._texts.Clear();
            activeSprites.Clear();
            CurrentScene = sceneString;


            // Calculate position
            Vector2 ScreenSize = new Vector2(Game1.WINDOW_SIZE.X, Game1.WINDOW_SIZE.Y);
            Vector2 Pos = new Vector2((Game1.WINDOW_SIZE.X / 2) - (ScreenSize.X / 2), (Game1.WINDOW_SIZE.Y / 2) - (ScreenSize.Y / 2)); // Center the sprite

            // Load menu
            Sprite Menu = new Sprite(Content.Load<Texture2D>(sceneString), Pos, ScreenSize, 1);


            // If it's the end screen, display score
            if (sceneString == "EndScreen") 
            {
                // Create text instance
                ScoreText = new Text(" ", new Vector2(0, 0), Color.MediumPurple, Content.Load<SpriteFont>("Font1"));

                // Calculate text and offset
                string finalText = "Score this run: " + Math.Round(Game1.CurrentGameSpeed - Game1.DefaultGameSpeed);
                Vector2 textSizeOffset = ScoreText.font.MeasureString(finalText);

                // Assign data to text
                ScoreText.str = finalText;
                ScoreText.position = new Vector2((Game1.WINDOW_SIZE.X / 2) - textSizeOffset.X/2, Game1.WINDOW_SIZE.Y/2 + Game1.WINDOW_SIZE.Y / 4);
                
                // Add text instance to renderer
                Game1._texts.Add(ScoreText);
            }




        }


        public void LoadLevel() // Initiate core game loop
        {


            // Setup
            Game1._texts.Clear();
            activeSprites.Clear();
            CurrentScene = "Playing";
            Game1.CurrentGameSpeed = Game1.DefaultGameSpeed;
            


            //  Calculate player position, create player object
            Vector2 PLAYER_DEFAULT_POS = new Vector2((Game1.WINDOW_SIZE.X / 2) - (PLAYER_SIZE.X / 2), (Game1.WINDOW_SIZE.Y * PLAYER_Y_STATIC) - (PLAYER_SIZE.Y / 2)); // Divide by 2 to get center
            Player Vehicle = new Player(Content.Load<Texture2D>("Vehicle"), PLAYER_DEFAULT_POS, PLAYER_SIZE, PLR_DEPTH, DEFAULT_PLR_HP);


            // Update empty var
            player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();


            // Create score text
            ScoreText = new Text("Score: ", new Vector2(PLAYER_DEFAULT_POS.X, 0), Color.Black, Content.Load<SpriteFont>("Font1")); // Align text with center of screen using plr pos var
            Game1._texts.Add(ScoreText);


            // Load initial tiles for whole map
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

            // Sort sprite list based on depth, (z axis)
            var sortedSprites = activeSprites.OrderBy(s => s.depth).ToList();

            // Call render methods on sprites
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (var sprite in sortedSprites)
            {
                sprite.Render(SpriteBatch);
            }
            SpriteBatch.End();

        }


        public async void UpdateScene(GameTime gameTime)
        {

            // No scene to update unless the level is loaded
            if (CurrentScene != "Playing")
            {
                return;
            }


            // Vars
            double timeNow = gameTime.TotalGameTime.TotalSeconds;
            float timeStep = TileSize / Game1.CurrentGameSpeed;


            // Increase game speed gradually
            Game1.CurrentGameSpeed += Game1.SpeedUpAmount;


            // Update score & health text
            ScoreText.str = "Score: " + Math.Round(Game1.CurrentGameSpeed - Game1.DefaultGameSpeed) + "\n Lives: " + player.Health;


            // Update sprites
            var sortedSprites = activeSprites.OrderBy(s => s.depth).ToList();
            foreach (var sprite in sortedSprites)
            {
                sprite.Update(gameTime);

            }


            // Draw next row of terrain just abovee the screen
            // We use the (time = distance (TileSize) / speed (CurrentGameSpeed) ) formula to calculate when to render next row
            if ((timeNow - LastRowRender) >= timeStep) // Check debounce
            {

                // Update var
                LastRowRender = timeNow;

                // Iterate based on screen and tile size
                for (int i = 0; i < Game1.WINDOW_SIZE.X /TileSize; i++)
                {
                    // Slightly oversize to avoid gaps in renderer...
                    // Last paramater is renderer depth which should always be 0 (Background)
                    BackgroundObject tile = new BackgroundObject(GetRandomGroundTexture(), new Vector2(TileSize * i, -TileSize), new Vector2(TileSize*1.2f, TileSize*1.2f), 0); // 1.2 tile size mult to fix render issues

                    // Random rotation of tile
                    Random random = new Random();
                    float newRot = random.Next(1, 4) * (MathF.PI/2); // Rotate tile 90 degrees up to 4 times

                    tile.rotation = newRot;

                }

            }


            // Generate random debris
            if ((timeNow - LastDebrisSpawn) > DebrisSpawnInterval) // Check debounce
            {


                // Vars
                LastDebrisSpawn = timeNow;
                Random random = new Random();
                int randomAmount = random.Next(DebrisSpawnAmountMin, DebrisSpawnAmountMax);

                // Iterate random amount
                for (int i = 0; i < randomAmount; i++)
                {
                    
                    // Calculate random point
                    int randomX = random.Next(-(int)Game1.WINDOW_SIZE.X, (int)Game1.WINDOW_SIZE.X); // Random point along the whole x axis on screen
                    int randomY = random.Next(0, (int)DebrisRandomYMax) + (int)DebrisStaticYOffset; // Random y point


                    // Create new debris class
                    Debris Rock = new Debris(GetRandomRockTexture(), new Vector2(randomX, -randomY), new Vector2(DebrisSize, DebrisSize), 1);  // Last paramater is render depth which should always be 1 (Debris)


                    // Create warning
                    Sprite WarnSprite = new Sprite(Content.Load<Texture2D>("Warning"), new Vector2(randomX, 0), new Vector2(WarnSize, WarnSize), 3); // Last paramater is render depth which should always be 3 (UI)
                    WarnSprite.pos = new Vector2(randomX - 50, 0); // Magic number.. don't have time to figure out why it works


                    //await Task.Delay( ((int)MathF.Abs(randomY / (int)Game1.CurrentGameSpeed)) * 1000 );
                    await Task.Delay( WarnDuration );
                    activeSprites.Remove(WarnSprite);

                }


            }


            // Generate random pickups
            if ((timeNow - LastPickupSpawn) > PickupSpawnDebounce) // Check debounce
            {


                // Vars
                LastPickupSpawn = timeNow;
                Random random = new Random();
         
                // Calculate random point
                int randomX = random.Next(-(int)Game1.WINDOW_SIZE.X, (int)Game1.WINDOW_SIZE.X); // Random point along the whole x axis on screen
                int randomY = random.Next(0, (int)DebrisRandomYMax) + (int)DebrisStaticYOffset; // Random y point


                // Create new debris class
                if (random.Next(0,100) > 50)
                {
                    // Health
                    Pickup Health = new Pickup(Content.Load<Texture2D>("Heart"), new Vector2(randomX, -randomY), new Vector2(DebrisSize, DebrisSize), 1, "Health");  // Penultimate paramater is render depth which should always be 1 (Debris)
                }
                else
                {
                    // Whiskey
                    Pickup Whiskey = new Pickup(Content.Load<Texture2D>("Whiskey"), new Vector2(randomX, -randomY), new Vector2(DebrisSize, DebrisSize), 1, "Whiskey");  // Penultimate paramater is render depth which should always be 1 (Debris)
                }
                


            }


            // Generate random enemies
            // Check debounce
            if ((timeNow - LastEnemySpawn) > EnemySpawnDebounce)
            {

                // Vars
                LastEnemySpawn = timeNow;
                Random random = new Random();
                int randomAmount = random.Next(1, 2);

                // Iterate random amount
                for (int i = 0; i < randomAmount; i++)
                {

                    // Calculate random position based on player x axis
                    int randomX = (int)player.pos.X + random.Next(-(int)Game1.WINDOW_SIZE.X / 3, (int)Game1.WINDOW_SIZE.X / 3);
                    int randomY = random.Next(-500, -300);


                    // Decide which enemy type to use
                    Enemy Enemy;
                    if (Game1.CurrentGameSpeed >= Game1.Level3Threshold + (Game1.DefaultGameSpeed))
                    {
                        // Level 3
                        Enemy = new Enemy(Content.Load<Texture2D>("EnemyHeavy"), new Vector2(randomX, randomY), new Vector2(PLAYER_SIZE.X, PLAYER_SIZE.Y), 1, 3, true);
                    }
                    else if (Game1.CurrentGameSpeed >= Game1.Level2Threshold + (Game1.DefaultGameSpeed))
                    {
                        // Level 2
                        Enemy = new Enemy(Content.Load<Texture2D>("EnemyLevel2"), new Vector2(randomX, randomY), new Vector2(PLAYER_SIZE.X, PLAYER_SIZE.Y), 1, 2, true);
                    }
                    else
                    {
                        // Level 1
                        Enemy = new Enemy(Content.Load<Texture2D>("EnemyStandard"), new Vector2(randomX, randomY), new Vector2(PLAYER_SIZE.X, PLAYER_SIZE.Y), 1, 1, false);
                    }



                    // Create warning
                    Sprite WarnSprite = new Sprite(Content.Load<Texture2D>("warningRed"), new Vector2(randomX, 0), new Vector2(WarnSize, WarnSize), 3);
                    WarnSprite.pos = new Vector2(randomX - 50, 0);

                    //await Task.Delay( ((int)MathF.Abs(randomY / (int)Game1.CurrentGameSpeed)) * 1000 );
                    await Task.Delay(WarnDuration);
                    activeSprites.Remove(WarnSprite);
                }


            }





        }

    }
}
