using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int TileSize = 40;



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
            ("Grass1", 70), // Small/med rock
            ("Grass2", 200), // Small rock
            ("Grass3", 10), // Big rock
            ("Grass4", 15), // Big/med rock
            ("Grass5", 10), // Big rock
            ("Grass6", 500), // Smooth
            ("Grass7", 10), // Big rock
            ("Grass8", 500), // Smooth
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
            activeSprites.Clear();
            CurrentScene = sceneString;


            // Load start/end screen
            Vector2 ScreenSize = new Vector2(Game1.WINDOW_SIZE.X, Game1.WINDOW_SIZE.Y);
            Vector2 Pos = new Vector2((Game1.WINDOW_SIZE.X / 2) - (ScreenSize.X / 2), (Game1.WINDOW_SIZE.Y / 2) - (ScreenSize.Y / 2)); // Center the sprite


            Sprite Menu = new Sprite(Content.Load<Texture2D>(sceneString), Pos, ScreenSize, 1);


        }

        // Note: maybe make sprite constructor automagically add to active sprites?

        public void LoadLevel()
        {

            Console.WriteLine("Loading level");

            // Setup
            activeSprites.Clear();
            CurrentScene = "Playing";



            // Create player object
            Vector2 PLAYER_SIZE = new Vector2(150, 150);
            float PLAYER_Y_STATIC = 0.7f; // A ratio of how far down the screen the vehicle is, 0 being top, 1 being down
            Vector2 PLAYER_DEFAULT_POS = new Vector2((Game1.WINDOW_SIZE.X / 2) - (PLAYER_SIZE.X / 2), (Game1.WINDOW_SIZE.Y * PLAYER_Y_STATIC) - (PLAYER_SIZE.Y / 2));

            Player Vehicle = new Player(Content.Load<Texture2D>("Vehicle"), PLAYER_DEFAULT_POS, PLAYER_SIZE, 2);


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

            // Update sprites

            var sortedSprites = activeSprites.OrderBy(s => s.depth).ToList();
            foreach (var sprite in sortedSprites)
            {
                sprite.Update(gameTime);

            }


            // Draw next row of terrain
            double timeNow = gameTime.TotalGameTime.TotalSeconds;
            float timeStep = TileSize / Game1.GameSpeed;
            if ((timeNow - LastRowRender) >= timeStep)
            {
                LastRowRender = timeNow;


                for (int i = 0; i < Game1.WINDOW_SIZE.X /TileSize; i++)
                {
                    BackgroundObject tile = new BackgroundObject(GetRandomGroundTexture(), new Vector2(TileSize * i, -TileSize), new Vector2(TileSize, TileSize), 0);
                }


            }


            // Generate random debris
            if ((timeNow - LastRockRender) > 3.0f)
            {
                LastRockRender = timeNow;

                Random random = new Random();

                int randomAmount = random.Next(4, 8);

                for (int i = 0; i < randomAmount; i++)
                {
                    
                    int randomX = random.Next(-1000, 1001);
                    int randomY = random.Next(-500, -150);

                    Debris Rock = new Debris(GetRandomRockTexture(), new Vector2(randomX, randomY), new Vector2(80, 80), 1);
                }


            }




        }

    }
}
