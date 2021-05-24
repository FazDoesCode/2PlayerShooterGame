using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace shooter2playergame
{
    class Smiley
    {
        private Vector2 smileyPos;
        private Texture2D texture2D;
        Vector2 direction;

        public Rectangle smileyRect;

        public Smiley(Texture2D texture2D, Vector2 position, Vector2 direction)
        {
            this.smileyPos = position;
            this.texture2D = texture2D;
            this.direction = direction;
        }

        public void MoveSmiley()
        {
            smileyPos += direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            smileyRect = new Rectangle((int)smileyPos.X, (int)smileyPos.Y, (int)(texture2D.Width * 2), (int)(texture2D.Height * 2));
            spriteBatch.Draw(texture2D, smileyRect, Color.White);
        }
    }
}
