using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
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
    internal class Player : Sprite
    {


        // CONFIG
        private float DefaultSteerSpeed = 300.0f;
        private float CurrentSteerSpeed;



        public Player(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, depth)
        {
            Console.WriteLine("player class initiated");
            CurrentSteerSpeed = DefaultSteerSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Vars
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float targetRotation = 0;


            // Inputs
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                pos.X -= (CurrentSteerSpeed * deltaTime);
                targetRotation = -(float)Math.PI / 4.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                pos.X += (CurrentSteerSpeed * deltaTime);
                targetRotation = (float)Math.PI / 4.0f;
            }


            // Rotate
            rotation = MathHelper.Lerp(rotation, targetRotation, 10 * deltaTime);


            // Steer speed
            this.CurrentSteerSpeed = DefaultSteerSpeed + (Game1.CurrentGameSpeed / 4);


            pos.X = Math.Clamp(pos.X, 0, Game1.WINDOW_SIZE.X - size.X); // Adjusted to ensure the vehicle


        }


        public override void Render(SpriteBatch spriteBatch)
        {
            //base.Render(spriteBatch);   

            Vector2 spriteOrigin = new Vector2(texture.Width / 2f, texture.Height/2);

            Rectangle Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);
            spriteBatch.Draw(texture, pos, null, Color.White, rotation, spriteOrigin, scale, SpriteEffects.None, 1);

        }

    }
}

