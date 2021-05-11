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

        // Declaring sprites
        Texture2D MainMenuSprite;
        Texture2D redguySprite;
        Texture2D redguySpriteDodge;
        Texture2D blueguySprite;
        Texture2D blueguySpriteDodge;
        Texture2D bulletSprite;
        Texture2D backgroundSprite;
        Vector2 redguyPos = new Vector2(100, 175);
        Vector2 blueguyPos = new Vector2(600, 175);
        SpriteFont font;
        SpriteFont fontBold;
        Texture2D redguySpriteDodgeLarge;
        Texture2D blueguySpriteDodgeLarge;

        // Dodging stuff
        int redDodgeDelay = 1200;
        int redInvulnTime = 500;
        double redtimeSinceLastDodge = 0;
        double redInvulnTimer = 0;
        
        int blueDodgeDelay = 1200;
        int blueInvulnTime = 500;
        double bluetimeSinceLastDodge = 0;
        double blueInvulnTimer = 0;

        // Firing Stuff
        bool redfireDelay = false;
        bool redIsDodging = false;

        bool bluefireDelay = false;
        bool blueIsDodging = false;

        // Main menu stuff
        bool isInMainMenu = true;
        bool gameHasStarted = false;
        bool overOptionsButton = false;

        // Scoring stuff
        int redScore = 0;
        int blueScore = 0;
        string redScoreString;
        string blueScoreString;
        bool redHasScored = false;
        bool blueHasScored = false;

        int scoreDelay  = 2000;
        double redTimeSinceLastScore = 0;
        double blueTimeSinceLastScore = 0;

        // Listing bullets
        List<Bullet> bullets = new List<Bullet>();

        // Declaring rectangle so I can use it in collisions
        Rectangle redguyRect;
        Rectangle blueguyRect;

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
        Keys blueguyShoot = Keys.OemPeriod;
        Keys blueguyDodge = Keys.OemComma;

        // Blue and Red speed
        int redguySpeed = 3;
        int blueguySpeed = 3;
        int dodgeDistance = 60;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            redguySpriteDodge = Content.Load<Texture2D>("Redguydodge");
            blueguySprite = Content.Load<Texture2D>("Blueguy");
            blueguySpriteDodge = Content.Load<Texture2D>("Blueguydodge");
            bulletSprite = Content.Load<Texture2D>("Bullet");
            MainMenuSprite = Content.Load<Texture2D>("MainMenu");
            font = Content.Load<SpriteFont>("Fonts/Font");
            fontBold = Content.Load<SpriteFont>("Fonts/FontBold");
            backgroundSprite = Content.Load<Texture2D>("Background");
            redguySpriteDodgeLarge = Content.Load<Texture2D>("Redguydodgelarge");
            blueguySpriteDodgeLarge = Content.Load<Texture2D>("Blueguydodgelarge");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet();
            }

            // Debugging position code
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Debug.WriteLine("RedguyPos  = " + redguyPos.X + "," + redguyPos.Y);
                Debug.WriteLine("BlueguyPos = " + blueguyPos.X + "," + blueguyPos.Y);
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

            // Exiting the game / Returning to menu
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && gameHasStarted == true)
            {
                isInMainMenu = true;
                gameHasStarted = false;
                redScore = 0;
                blueScore = 0;
                redguyPos.X = 100;
                redguyPos.Y = 175;
                blueguyPos.X = 600;
                blueguyPos.Y = 175;
            }

            if (gameHasStarted == true)
            {
                IsMouseVisible = false;
            } else
            {
                IsMouseVisible = true;
            }

            // Red and Blue movement start
            if (gameHasStarted == true) 
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastScore + scoreDelay && gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastScore + scoreDelay)
                {
                    // Red movement
                    if (redIsDodging == false)
                    {
                        if (Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 5)
                        {
                            redguyPos.Y -= redguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 390)
                        {
                            redguyPos.Y += redguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 1)
                        {
                            redguyPos.X -= redguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 340)
                        {
                            redguyPos.X += redguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyShoot))
                        {
                            if (redfireDelay == false)
                            {
                                bullets.Add(new Bullet(bulletSprite, redguyPos + new Vector2(50, 40), new Vector2(7, 0)));
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
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 42)
                            {
                                redIsDodging = true;
                                redguyPos.Y -= dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 246)
                            {
                                redIsDodging = true;
                                redguyPos.X += dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 70)
                            {
                                redIsDodging = true;
                                redguyPos.X -= dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                        }
                    }

                    // Blue movement
                    if (blueIsDodging == false)
                    {
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 5)
                        {
                            blueguyPos.Y -= blueguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 390)
                        {
                            blueguyPos.Y += blueguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 395)
                        {
                            blueguyPos.X -= blueguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 753)
                        {
                            blueguyPos.X += blueguySpeed;
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyShoot))
                        {
                            if (bluefireDelay == false)
                            {
                                bullets.Add(new Bullet(bulletSprite, blueguyPos + new Vector2(-10, 40), new Vector2(-7, 0)));
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
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 346)
                            {
                                blueIsDodging = true;
                                blueguyPos.Y += dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 42)
                            {
                                blueIsDodging = true;
                                blueguyPos.Y -= dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 684)
                            {
                                blueIsDodging = true;
                                blueguyPos.X += dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 450)
                            {
                                blueIsDodging = true;
                                blueguyPos.X -= dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                        }
                    }
                }
            }
            // Red and Blue movement end

            //Bullet & Player interactions
            //Redguy gets hit
            for (int i = 0; i < bullets.Count; i++)
            {
                if (redguyRect.Intersects(bullets[i].bulletRect)) {
                    if (gameTime.TotalGameTime.TotalMilliseconds > redInvulnTimer + redInvulnTime)
                    {
                        bullets.Clear();
                        blueScore = blueScore + 1;
                        blueHasScored = true;
                        redguyPos.X = 100;
                        redguyPos.Y = 175;
                        blueguyPos.X = 600;
                        blueguyPos.Y = 175;
                        blueTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds;
                        Debug.WriteLine("Red collision detected");
                    }
                }
            }

            //Blueguy gets hit
            for (int i = 0; i < bullets.Count; i++)
            {
                if (blueguyRect.Intersects(bullets[i].bulletRect))
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueInvulnTimer + blueInvulnTime)
                    {
                        bullets.Clear();
                        redScore = redScore + 1;
                        redHasScored = true;
                        blueguyPos.X = 600;
                        blueguyPos.Y = 175;
                        redguyPos.X = 100;
                        redguyPos.Y = 175;
                        redTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds;
                        Debug.WriteLine("Blue collision detected");
                    }
                }
            }
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

            // Drawing background
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);

            // Making redguy and blueguy rectangles
            redguyRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, redguySprite.Width * scale, redguySprite.Height * scale);
            blueguyRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, blueguySprite.Width * scale, blueguySprite.Height * scale);
            Rectangle redguyDodgeRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, redguySpriteDodgeLarge.Width * scale, redguySpriteDodgeLarge.Height * scale);
            Rectangle blueguyDodgeRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, blueguySpriteDodgeLarge.Width * scale, blueguySpriteDodgeLarge.Height * scale);

            // Drawing Redguy sprites
            if (gameTime.TotalGameTime.TotalMilliseconds > redInvulnTimer + redInvulnTime)
            {
                _spriteBatch.Draw(redguySprite, redguyRect, Color.White);
            }
            else
            {
                _spriteBatch.Draw(redguySpriteDodgeLarge, redguyDodgeRect, Color.White);
            }

            // Drawing Blueguy sprites
            if (gameTime.TotalGameTime.TotalMilliseconds > blueInvulnTimer + blueInvulnTime)
            {
                _spriteBatch.Draw(blueguySprite, blueguyRect, Color.White);
            }
            else
            {
                _spriteBatch.Draw(blueguySpriteDodgeLarge, blueguyDodgeRect, Color.White);
            }

            // Drawing Blueguy & Redguy scores
            redScoreString = redScore.ToString();
            _spriteBatch.DrawString(font, "Red Score: " + redScoreString, new Vector2(10,10), Color.Black);
            blueScoreString = blueScore.ToString();
            _spriteBatch.DrawString(font, "Blue Score: " + blueScoreString, new Vector2(680, 10), Color.Black);

            if (redHasScored == true)
            {
                _spriteBatch.DrawString(fontBold, "Red Scored!", new Vector2(330, 215), Color.Black);
                if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastScore + scoreDelay)
                {
                    redHasScored = false;
                }
            }
            if (blueHasScored == true)
            {
                _spriteBatch.DrawString(fontBold, "Blue Scored!", new Vector2(330, 215), Color.Black);
                if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastScore + scoreDelay)
                {
                    blueHasScored = false;
                }
            }

            // Drawing main menu
            if (isInMainMenu == true)
            {
                _spriteBatch.Draw(MainMenuSprite, new Vector2(0, 0), Color.White);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
