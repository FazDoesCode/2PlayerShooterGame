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

        Texture2D powerupTexture;

        public Rectangle powerupRect;

        public DashPowerup(Texture2D powerupTexture, Vector2 position)
        {
            this.position = position;
            this.powerupTexture = powerupTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            powerupRect = new Rectangle((int)position.X, (int)position.Y, (int)(powerupTexture.Width * 0.5f), (int)(powerupTexture.Height * 0.5f));
            spriteBatch.Draw(powerupTexture, powerupRect, Color.White);

        }
    }
}
