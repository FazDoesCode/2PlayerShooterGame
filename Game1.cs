using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace shooter2playergame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D MainMenuSprite;
        Texture2D redguySprite;
        Texture2D blueguySprite;
        Texture2D bulletSprite;
        Vector2 redguyPos = new Vector2(100, 175);
        Vector2 blueguyPos = new Vector2(600, 175);

        int redDodgeDelay = 1000;
        double redtimeSinceLastDodge = 0;

        bool redfireDelay = false;
        bool redIsDodging = false;

        int blueDodgeDelay = 1000;
        double bluetimeSinceLastDodge = 0;

        bool bluefireDelay = false;
        bool blueIsDodging = false;

        bool isInMainMenu = true;
        bool gameHasStarted = false;

        List<Bullet> bullets = new List<Bullet>();

        // Red Guy Controls
        Keys redguyMoveUp = Keys.W;
        Keys redguyMoveDown = Keys.S;
        Keys redguyMoveLeft = Keys.A;
        Keys redguyMoveRight = Keys.D;
        Keys redguyShoot = Keys.F;
        Keys redguyDodge = Keys.G;

        // Blue Guy Controls
        Keys blueguyMoveUp = Keys.Up;
        Keys blueguyMoveDown = Keys.Down;
        Keys blueguyMoveLeft = Keys.Left;
        Keys blueguyMoveRight = Keys.Right;
        Keys blueguyShoot = Keys.M;
        Keys blueguyDodge = Keys.N;

        // Blue and Red speed
        int redguySpeed = 3;
        int blueguySpeed = 3;
        int dodgeDistance = 60;

        bool overOptionsButton = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Debug.WriteLine(GraphicsDevice.Viewport.Bounds);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            redguySprite = Content.Load<Texture2D>("Redguy");
            blueguySprite = Content.Load<Texture2D>("Blueguy");
            bulletSprite = Content.Load<Texture2D>("Bullet");
            MainMenuSprite = Content.Load<Texture2D>("MainMenu");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet();
            }

            // Menu Stuff
            MouseState mouseState = Mouse.GetState();
            if (mouseState.X < 270 && mouseState.Y > 370)
            {
                overOptionsButton = true;
            }
            else
            {
                overOptionsButton = false;
            }

            if (overOptionsButton == false)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isInMainMenu = false;
                    gameHasStarted = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Debug.WriteLine("RedguyPos  = " + redguyPos.X + "," + redguyPos.Y);
                Debug.WriteLine("BlueguyPos = " + blueguyPos.X + "," + blueguyPos.Y);
            }

            // Red and Blue movement start
            if (gameHasStarted == true) 
            {
                // Red movement
                if (redIsDodging == false)
                {
                    if (Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 1)
                    {
                        redguyPos.Y -= redguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 384)
                    {
                        redguyPos.Y += redguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= -2)
                    {
                        redguyPos.X -= redguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 292)
                    {
                        redguyPos.X += redguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyShoot))
                    {
                        if (redfireDelay == false)
                        {
                            bullets.Add(new Bullet(bulletSprite, redguyPos + new Vector2(100, 35), new Vector2(7, 0)));
                            redfireDelay = true;
                        }
                    }
                    if (Keyboard.GetState().IsKeyUp(redguyShoot))
                    {
                        redfireDelay = false;
                    }

                    // Red dodging
                    if (gameTime.TotalGameTime.TotalMilliseconds > redtimeSinceLastDodge + redDodgeDelay)
                    {
                        if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 346)
                        {
                            redIsDodging = true;
                            redguyPos.Y += dodgeDistance;
                            redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            redIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 42)
                        {
                            redIsDodging = true;
                            redguyPos.Y -= dodgeDistance;
                            redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            redIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 246)
                        {
                            redIsDodging = true;
                            redguyPos.X += dodgeDistance;
                            redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            redIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 59)
                        {
                            redIsDodging = true;
                            redguyPos.X -= dodgeDistance;
                            redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            redIsDodging = false;
                        }
                    }
                }
                
                // Blue movement
                if (blueIsDodging == false)
                {
                    if (Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 1)
                    {
                        blueguyPos.Y -= blueguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 384)
                    {
                        blueguyPos.Y += blueguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 385)
                    {
                        blueguyPos.X -= blueguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 700)
                    {
                        blueguyPos.X += blueguySpeed;
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyShoot))
                    {
                        if (bluefireDelay == false)
                        {
                            bullets.Add(new Bullet(bulletSprite, blueguyPos + new Vector2(-10, 35), new Vector2(-7, 0)));
                            bluefireDelay = true;
                        }
                    }
                    if (Keyboard.GetState().IsKeyUp(blueguyShoot))
                    {
                        bluefireDelay = false;
                    }

                    // Blue dodging
                    if (gameTime.TotalGameTime.TotalMilliseconds > bluetimeSinceLastDodge + blueDodgeDelay)
                    {
                        if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveDown))
                        {
                            blueIsDodging = true;
                            blueguyPos.Y += dodgeDistance;
                            bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            blueIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveUp))
                        {
                            blueIsDodging = true;
                            blueguyPos.Y -= dodgeDistance;
                            bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            blueIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveRight))
                        {
                            blueIsDodging = true;
                            blueguyPos.X += dodgeDistance;
                            bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            blueIsDodging = false;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft))
                        {
                            blueIsDodging = true;
                            blueguyPos.X -= dodgeDistance;
                            bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                            blueIsDodging = false;
                        }
                    }
                }
            }
            // Red and Blue movement end

            base.Update(gameTime);
        }

        int scale = 3;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp
               );

            Rectangle redguyRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, redguySprite.Width * scale, redguySprite.Height * scale);
            Rectangle blueguyRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, blueguySprite.Width * scale, blueguySprite.Height * scale);
            _spriteBatch.Draw(redguySprite, redguyRect, Color.White);
            _spriteBatch.Draw(blueguySprite, blueguyRect, Color.White);

            if (isInMainMenu == true)
            {
                _spriteBatch.Draw(MainMenuSprite, new Vector2(0, 0), Color.White);
            }

            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
