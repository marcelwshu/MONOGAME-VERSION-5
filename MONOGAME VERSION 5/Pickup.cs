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
    internal class Pickup : BackgroundObject
    {

        // Vars

        // Constructor
        public Pickup(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, depth)
        {

        }


        // Methods
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);


            // Vars
            Player Player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();
            float Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;



 

            // Check for collisions with the player
            CheckCollisions();
        }

        public void CheckCollisions()
        {
            //base.CheckCollisions();

            // Vars
            float paddingX = 20; // Padding is used to decrease hitbox size
            float paddingY = 30;

            Vector2 paddedPos = new Vector2(this.pos.X + paddingX, this.pos.Y + paddingY);
            Vector2 paddedSize = new Vector2(this.size.X - 2 * paddingX, this.size.Y - 2 * paddingY);

            // Loop through all objects which inherit or are debris class
            foreach (var debris in Game1._sceneManager.activeSprites.OfType<Debris>())
            {
                if (debris.GetType() == typeof(Debris)) // Filter for ONLY debris class
                {
                    if (paddedPos.X < debris.pos.X + debris.size.X &&
                        paddedPos.X + paddedSize.X > debris.pos.X &&
                        paddedPos.Y < debris.pos.Y + debris.size.Y &&
                        paddedPos.Y + paddedSize.Y > debris.pos.Y)
                    {

                        // Collision detected, delete debri
                        Game1._sceneManager.activeSprites.Remove(debris);

                        // Remove health, if less than 1 then enemy gets deleted
                        this.Health -= 1;
                        if (this.Health <= 0)
                        {
                            Game1._sceneManager.activeSprites.Remove(this);
                        }

                        // Instance deleted, return from loop
                        break;

                    }
                }
            }


        }

    }
}

