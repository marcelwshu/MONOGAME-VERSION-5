using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace MONOGAME_VERSION_5
{
    internal class Debris : BackgroundObject
    {


        // Vars
        float paddingX = 55.0f;
        float paddingY = 40.0f;



        // Constructor
        public Debris(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, depth)
        {
   
        }

        // Internal functions
        public virtual void CheckCollisions() // This might be inefficient because its looping over all debri classes.. in every instance?
        {

            // Loop over all debri classes 
            Player player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();
            if (player != null)
            {

                // Adjust invisible hitbox size
                Vector2 paddedPlayerPos = new Vector2(player.pos.X + paddingX, player.pos.Y + paddingY);
                Vector2 paddedPlayerSize = new Vector2(player.size.X - 2 * paddingX, player.size.Y - 2 * paddingY);


                if (paddedPlayerPos.X < this.pos.X + this.size.X &&
                   paddedPlayerPos.X + paddedPlayerSize.X > this.pos.X &&
                   paddedPlayerPos.Y < this.pos.Y + this.size.Y &&
                   paddedPlayerPos.Y + paddedPlayerSize.Y > this.pos.Y)
                {
                    // Collision detected

                    // Remove health from player
                    player.Health -= 1;

                    // Destroy debris instance
                    Game1._sceneManager.activeSprites.Remove(this);

   
                }

            }
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CheckCollisions();

        }

    }
}

