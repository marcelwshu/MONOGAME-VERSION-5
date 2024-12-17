using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace MONOGAME_VERSION_5
{
    internal class Sprite
    {

        // Vars
        public Texture2D texture;
        public Vector2 pos;
        public Vector2 size;
        public float depth;
        public float rotation;


        // Constructor
        public Sprite(Texture2D texture, Vector2 pos, Vector2 size, int depth)
        {
            this.texture = texture;
            this.pos = pos;
            this.size = size;
            this.depth = depth;
            this.rotation = 0;
            Game1._sceneManager.activeSprites.Add(this);
        }


        // Methods
        public virtual void Update(GameTime gameTime)
        {
  
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {

            Rectangle Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            spriteBatch.Draw(texture, Rect, color:Color.White);

        }

   
    }
}
