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
        private float MoveSpeed = 300.0f;



        public Player(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, depth)
        {
            Console.WriteLine("player class initiated");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                pos.X -= (MoveSpeed * deltaTime);
                rotation = -(float)Math.PI / 4.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                pos.X += (MoveSpeed * deltaTime);
                rotation = (float)Math.PI / 4.0f;
            }


            pos.X = Math.Clamp(pos.X, 0, Game1.WINDOW_SIZE.X - size.X); // Adjusted to ensure the vehicle


        }

    }
}

