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
        public string Type;

        // Constructor
        public Pickup(Texture2D texture, Vector2 pos, Vector2 size, int depth, string PickupType) : base(texture, pos, size, depth)
        {
            Type = PickupType;
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

            Player player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();

            float paddingX = 20; // Padding is used to decrease hitbox size
            float paddingY = 30;

            Vector2 paddedPos = new Vector2(this.pos.X + paddingX, this.pos.Y + paddingY);
            Vector2 paddedSize = new Vector2(this.size.X - 2 * paddingX, this.size.Y - 2 * paddingY);

            // Loop through all objects which inherit or are debris class
            if (paddedPos.X < player.pos.X + player.size.X &&
                paddedPos.X + paddedSize.X > player.pos.X &&
                paddedPos.Y < player.pos.Y + player.size.Y &&
                paddedPos.Y + paddedSize.Y > player.pos.Y)
            {

                // Collision detected, delete debri
                Game1._sceneManager.activeSprites.Remove(this);

                if (Type == "Health")
                {
                    player.Health += 1;
                }

                
            }


        }

    }
}

