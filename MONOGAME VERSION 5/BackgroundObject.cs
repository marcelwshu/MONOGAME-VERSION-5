using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MONOGAME_VERSION_5
{
    internal class BackgroundObject : Sprite
    {

        public BackgroundObject(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, (int)depth)
        {
           
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Make them "fall"
            pos.Y += (Game1.CurrentGameSpeed * deltaTime);

            // Reset position if it goes off screen
            if (pos.Y > Game1.WINDOW_SIZE.Y)
            {
                Game1._sceneManager.activeSprites.Remove(this);
            }
        }
    }
}
