﻿using Microsoft.Xna.Framework;
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
    internal class Debris : BackgroundObject
    {


        // CONFIG



        // Internal functions
        private void CheckCollisions()
        {
            Player player = Game1._sceneManager.activeSprites.OfType<Player>().FirstOrDefault();
            if (player != null)
            {
                // Define separate padding for the player
                float paddingX = 60.0f;
                float paddingY = 40.0f;

                // Adjust the player's hitbox with padding
                Vector2 paddedPlayerPos = new Vector2(player.pos.X + paddingX, player.pos.Y + paddingY);
                Vector2 paddedPlayerSize = new Vector2(player.size.X - 2 * paddingX, player.size.Y - 2 * paddingY);

                foreach (var debris in Game1._sceneManager.activeSprites.OfType<Debris>())
                {
                    if (paddedPlayerPos.X < debris.pos.X + debris.size.X &&
                        paddedPlayerPos.X + paddedPlayerSize.X > debris.pos.X &&
                        paddedPlayerPos.Y < debris.pos.Y + debris.size.Y &&
                        paddedPlayerPos.Y + paddedPlayerSize.Y > debris.pos.Y)
                    {
                        // Collision detected, end the game
                        Game1._sceneManager.LoadMenu("EndScreen");
                        break;
                    }
                }
            }
        }

        public Debris(Texture2D texture, Vector2 pos, Vector2 size, int depth) : base(texture, pos, size, depth)
        {
            //Console.WriteLine("debris class initiated");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CheckCollisions();

        }

    }
}

