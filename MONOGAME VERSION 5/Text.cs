using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MONOGAME_VERSION_5
{
    internal class Text
    {

        // Vars
        public string str = "Default Text";
        public Vector2 position;
        public Color color;
        public SpriteFont font;


        // Constructor
        public Text(string str, Vector2 position, Color color, SpriteFont font)
        {
            this.str = str;
            this.position = position;
            this.color = color;
            this.font = font;
        }


        // Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, str, position, color);
        }

    }
}
