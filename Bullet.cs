using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace shooter2playergame
{
    public class Bullet
    {
        private Vector2 position;
        private Texture2D texture2D;
        Vector2 direction;
        public Bullet(Texture2D texture2D, Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.texture2D = texture2D;
            this.direction = direction;
        }

        public void MoveBullet()
        {
            position += direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle bulletRect = new Rectangle((int)position.X, (int)position.Y, (int)(texture2D.Width * 0.25f), (int)(texture2D.Height * 0.25f));
            spriteBatch.Draw(texture2D, bulletRect, Color.White);
        }
    }
}
