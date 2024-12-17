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


        // Vars
        public int Health;

        private float DefaultSteerSpeed = 300.0f;
        private float SteerAnimationTime = 10;
        private float CurrentSteerSpeed;

    



        // Constructor
        public Player(Texture2D texture, Vector2 pos, Vector2 size, int depth, int health) : base(texture, pos, size, depth)
        {
            CurrentSteerSpeed = DefaultSteerSpeed;
            Health = health;
        }


        // Methods
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Vars
            float Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float TargetRot = 0;


            // Inputs
            if (Keyboard.GetState().IsKeyDown(Keys.A)) // Turn left
            {
                pos.X -= (CurrentSteerSpeed * Delta);
                TargetRot = -(float)Math.PI / 4.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D)) // Turn right
            {
                pos.X += (CurrentSteerSpeed * Delta);
                TargetRot = (float)Math.PI / 4.0f;
            }


            // Calculate rotation angle
            rotation = MathHelper.Lerp(rotation, TargetRot, SteerAnimationTime * Delta);


            // Set steer speed to increase with game speed
            this.CurrentSteerSpeed = DefaultSteerSpeed + (Game1.CurrentGameSpeed / 2);


            // Clamp the vehicle within window border
            pos.X = Math.Clamp(pos.X, 0, Game1.WINDOW_SIZE.X - size.X);

            
            // Check if dead
            if (this.Health < 1)
            {
                Game1._sceneManager.LoadMenu("EndScreen");
            } 

        }


        public override void Render(SpriteBatch spriteBatch)
        {   

            // Render sprite
            Vector2 spriteOrigin = new Vector2(texture.Width / 2f, texture.Height/2);
            Rectangle Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);

            spriteBatch.Draw(texture, pos, null, Color.White, rotation, spriteOrigin, scale, SpriteEffects.None, 1);

        }

    }
}

