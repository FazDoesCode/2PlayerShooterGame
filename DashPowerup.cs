using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter2playergame
{
    class DashPowerup
    {
        Vector2 position;

        Texture2D powerupSprite;

        public Rectangle powerupRect;

        public DashPowerup(Texture2D powerupSprite, Vector2 position)
        {
            this.position = position;
            this.powerupSprite = powerupSprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            powerupRect = new Rectangle((int)position.X, (int)position.Y, (int)(powerupSprite.Width * 2f), (int)(powerupSprite.Height * 2f));
            spriteBatch.Draw(powerupSprite, powerupRect, Color.White);
        }
    }
}
