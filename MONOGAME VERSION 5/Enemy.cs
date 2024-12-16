using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace MONOGAME_VERSION_5
{
    internal class Enemy : Debris
    {

        private float DefaultSteerSpeed = 40f;

        public float Speed = 50;
        public float SteerSpeed;
        public int health;
        public bool steer;

        public Enemy(Texture2D texture, Vector2 pos, Vector2 size, int depth, int health, bool steer) : base(texture, pos, size, depth)
        {
            this.steer = steer;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            // Vars
            Player player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Update speed
            this.SteerSpeed = DefaultSteerSpeed + (Game1.CurrentGameSpeed / 4);



            // Move the enemy
            pos.Y += (Speed + (Game1.CurrentGameSpeed / 4)) * deltaTime;


            // Steer towards player 
            if (player != null && steer)
            {
       
                // Rotation logic
                int signedDir = MathF.Sign(player.pos.X - pos.X);

                this.rotation = MathHelper.Lerp(rotation, signedDir * -MathF.PI / 4 , 10 * deltaTime);


                pos.X += (signedDir * SteerSpeed) * deltaTime;





            }

            // Check for collisions with the player
            CheckCollisions();
        }

        public override void CheckCollisions()
        {
            base.CheckCollisions();

            float paddingX = 20;
            float paddingY = 30;

  
            Vector2 paddedPos = new Vector2(this.pos.X + paddingX, this.pos.Y + paddingY);
            Vector2 paddedSize = new Vector2(this.size.X - 2 * paddingX, this.size.Y - 2 * paddingY);

            foreach (var debris in Game1._sceneManager.activeSprites.OfType<Debris>())
            {
                if (debris.GetType() == typeof(Debris))
                {
                    if (paddedPos.X < debris.pos.X + debris.size.X &&
                        paddedPos.X + paddedSize.X > debris.pos.X &&
                        paddedPos.Y < debris.pos.Y + debris.size.Y &&
                        paddedPos.Y + paddedSize.Y > debris.pos.Y)
                    {
                        Console.WriteLine(debris);
                        // Collision detected, end the game
                        Game1._sceneManager.activeSprites.Remove(debris);
                        this.health -= 1;
                        if (this.health <= 0)
                        {
                            Game1._sceneManager.activeSprites.Remove(this);
                        }
                        break;
                    }
                }
            }


        }

    }
}

