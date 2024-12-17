using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MONOGAME_VERSION_5
{
    internal class BackgroundObject : Sprite
    {


        // Constructor
        public BackgroundObject(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, (int)depth)
        {
           
        }


        // Functions
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


        public override void Render(SpriteBatch spriteBatch)
        {
            //base.Render(spriteBatch);   

            Vector2 spriteOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            Rectangle Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);
            spriteBatch.Draw(texture, pos, null, Color.White, rotation, spriteOrigin, scale, SpriteEffects.None, 1);

        }

    }
}
