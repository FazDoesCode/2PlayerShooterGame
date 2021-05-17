using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Texture2D blueguySprite;
        Texture2D bulletSprite;
        Texture2D backgroundSprite;
        Texture2D redguySpriteDodgeLarge;
        Texture2D blueguySpriteDodgeLarge;
        Texture2D redguySpriteDead;
        Texture2D blueguySpriteDead;
        Texture2D backgroundSprite2;
        Texture2D controlsScreen;
        Texture2D powerupSprite;

        // Declaring fonts
        SpriteFont font;
        SpriteFont fontBold;

        // Declaring sounds
        SoundEffect walkSound;
        SoundEffect pew;
        SoundEffect bang;
        SoundEffect woosh;
        SoundEffect deathSound;
        Song menuMusic;
        Song controlsScreenMusic;

        // Position & walking stuff
        Vector2 redguyPos = new Vector2(100, 175);
        Vector2 blueguyPos = new Vector2(600, 175); // Sets redguy and blueguy's default position
        double redTimeSinceLastWalked = 0;
        double blueTimeSinceLastWalked = 0;
        int walkSoundDelay = 400; // These are used to make walk sounds better

        // Dodging stuff
        int redDodgeDelay = 1200; // Red has 1.2 seconds of time between dodges, same as blue
        int redInvulnTime = 500; // Only 500ms of invincibility
        double redtimeSinceLastDodge = 0;
        double redInvulnTimer = 0; // These are used in the dodge timer
        
        int blueDodgeDelay = 1200; // All the same as red, however kept seperate so the dodge powerup can only effect the one who picked it up
        int blueInvulnTime = 500;
        double bluetimeSinceLastDodge = 0;
        double blueInvulnTimer = 0;

        // Firing Stuff
        bool redfireDelay = false; // booleans used to determine whether or not red or blue can fire
        bool redIsDodging = false; // This is used to stop dodge spam

        bool bluefireDelay = false;
        bool blueIsDodging = false;

        // Main menu stuff
        bool isInMainMenu = true;
        public bool gameHasStarted = false; // Also used to determine whether or not the players can move
        bool overControlsButton = false;
        bool overMap1Button = false; // Used for button selection
        bool overMap2Button = false; // Used for the button selection
        bool menuMusicCanPlay = true; // self explanitory

        // Background stuff
        bool isInDesert = false;
        bool isInForest = false;
        bool isInControlsMenu = false; // These are used to determine what background is displayed
        bool controlsMusicCanPlay = false;

        // Scoring stuff
        int redScore = 0;
        int blueScore = 0; // Default red and blue scores are 0, obviously
        string redScoreString;
        string blueScoreString; // Used to convert the ints into strings so that they can be displayed through text
        bool redHasScored = false;
        bool blueHasScored = false; // Used to stop the characters from moving after a point is scored

        int scoreDelay  = 2000; // How long the 'Red Scored!' or 'Blue Scored' screen stays up for
        double redTimeSinceLastScore = 0;
        double blueTimeSinceLastScore = 0;

        // Powerup Stuff
        double timeSinceLastSpawnRED = 0;
        double timeSinceLastSpawnBLUE = 0; // Used as timers

        int REDspawnDelay = new Random().Next(7000, 15000);
        int BLUEspawnDelay = new Random().Next(7000, 15000); // These set a random number between 7 second and 15 seconds, used to spawn the powerups
        int randomPositionREDX = new Random().Next(220, 380);
        int randomPositionREDY = new Random().Next(30, 460);
        int randomPositionBLUEX = new Random().Next(440, 560);
        int randomPositionBLUEY = new Random().Next(30, 460); // These are used to set a random spawn location

        double REDpowerupTimer = 0;
        double BLUEpowerupTimer = 0;
        int powerupTime = 5000; // The powerups are active for 5 seconds

        // Japanese mode stuff (Secret so don't tell anyone ok)
        Texture2D japaneseMenu;
        Song japaneseMusic;
        bool isInJapan = false;
        bool japanMusicCanPlay = false;

        // Listing bullets & powerups
        List<Bullet> bullets = new List<Bullet>();
        List<DashPowerup> powerups = new List<DashPowerup>(); // Using lists so there can be multiple on screen

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
        int blueguySpeed = 3; // Red and blueguy speed are seperate integers so they can be affected by powerups seperately
        int dodgeDistance = 60;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
           
            // Loading player sprites
            redguySprite = Content.Load<Texture2D>("Players/Redguy");
            blueguySprite = Content.Load<Texture2D>("Players/Blueguy");
            redguySpriteDodgeLarge = Content.Load<Texture2D>("Players/Redguydodgelarge");
            blueguySpriteDodgeLarge = Content.Load<Texture2D>("Players/Blueguydodgelarge"); // The sprites are 'Large' due to me fixing a bug with the old ones and never changing the new filenames
            redguySpriteDead = Content.Load<Texture2D>("Players/Redguydead");
            blueguySpriteDead = Content.Load<Texture2D>("Players/Blueguydead");

            // Loading miscellaneous sprites
            bulletSprite = Content.Load<Texture2D>("Items/Bullet");
            powerupSprite = Content.Load<Texture2D>("Items/Fastshoe");

            // Loading menu sprites
            MainMenuSprite = Content.Load<Texture2D>("Backgrounds/MainMenu");
            controlsScreen = Content.Load<Texture2D>("Backgrounds/Controlsscreen");
            japaneseMenu = Content.Load<Texture2D>("Backgrounds/Japanesemenu");

            // Loading gameplay backgrounds
            backgroundSprite = Content.Load<Texture2D>("Backgrounds/Desertbackground");
            backgroundSprite2 = Content.Load<Texture2D>("Backgrounds/Forestbackground");

            // Loading fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            fontBold = Content.Load<SpriteFont>("Fonts/FontBold");

            // Loading music
            menuMusic = Content.Load<Song>("music/menumusic");
            controlsScreenMusic = Content.Load<Song>("music/controlsscreenmusic");
            japaneseMusic = Content.Load<Song>("music/japanesemusic");

            // Loading sound effects
            pew = Content.Load<SoundEffect>("sound effects/pew");
            bang = Content.Load<SoundEffect>("sound effects/bang");
            walkSound = Content.Load<SoundEffect>("sound effects/walksound");
            deathSound = Content.Load<SoundEffect>("sound effects/deathsound");
            woosh = Content.Load<SoundEffect>("sound effects/woosh");
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            // Moving bullets forwards / backwards
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet(); // Calls the MoveBullet() function for every bullet in the list
            }

            // Debugging position code
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Debug.WriteLine("RedguyPos  = " + redguyPos.X + "," + redguyPos.Y);
                Debug.WriteLine("BlueguyPos = " + blueguyPos.X + "," + blueguyPos.Y); // Used for debugging player position
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                Debug.WriteLine(mouseState.X);
                Debug.WriteLine(mouseState.Y); // Used for debugging mouse position (for the menus)
            }

            // Main Menu Stuff
            if (menuMusicCanPlay == true)
            {
                MediaPlayer.Play(menuMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                menuMusicCanPlay = false; // Makes the menu music play once instead of constantly starting
            }
            if (mouseState.X > 591 && mouseState.Y < 80) 
            {
                overMap1Button = true;
            } else
            {
                overMap1Button = false;
            } // if the mouse is in the set box it's in the map 1 button
            if (mouseState.X > 591 && mouseState.Y < 161 && mouseState.Y > 85) 
            {
                overMap2Button = true;
            } else
            {
                overMap2Button = false;
            } // If the mouse is in the set box it's over the map2 button
            if (overMap1Button == true && mouseState.LeftButton == ButtonState.Pressed && isInMainMenu == true)
            {
                isInMainMenu = false;
                isInDesert = true;
                gameHasStarted = true;
                MediaPlayer.Stop(); // Sets the background to the desert and stops playing music
            }
            if (overMap2Button == true && mouseState.LeftButton == ButtonState.Pressed && isInMainMenu == true)
            {
                isInMainMenu = false;
                isInForest = true;
                gameHasStarted = true;
                MediaPlayer.Stop(); // Sets the background to the forest and stops playing music
            }
            
            // Controls Menu Stuff (same as backgrounds basically)
            if (controlsMusicCanPlay == true)
            {
                MediaPlayer.Play(controlsScreenMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                controlsMusicCanPlay = false;
            }
            if (mouseState.X < 200 && mouseState.Y > 405)
            {
                overControlsButton = true;
            }
            else
            {
                overControlsButton = false;
            }
            if (overControlsButton == true && mouseState.LeftButton == ButtonState.Pressed && isInMainMenu == true)
            {
                isInMainMenu = false;
                isInControlsMenu = true;
                MediaPlayer.Stop();
                controlsMusicCanPlay = true;
            }

            // Japanese mode stuff
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.N) && Keyboard.GetState().IsKeyDown(Keys.I) && Keyboard.GetState().IsKeyDown(Keys.M) && Keyboard.GetState().IsKeyDown(Keys.E) && isInMainMenu == true)
            {
                isInMainMenu = false;
                isInJapan = true;
                MediaPlayer.Stop();
                japanMusicCanPlay = true;
            }
            if (japanMusicCanPlay == true)
            {
                MediaPlayer.Play(japaneseMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                japanMusicCanPlay = false;
            }

            // Exiting the game / Returning to menu
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && gameHasStarted == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && isInControlsMenu == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && isInJapan == true)
            {
                MediaPlayer.Stop();
                bullets.Clear();
                isInMainMenu = true;
                isInDesert = false;
                isInForest = false;
                isInControlsMenu = false;
                isInJapan = false;
                gameHasStarted = false;
                menuMusicCanPlay = true;
                redScore = 0;
                blueScore = 0;
                ResetPos(); // resets everything and returns to main menu
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
                        if (Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 5) // I manually set bounds to where the players can and cannot go
                        {
                            redguyPos.Y -= redguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds; // This only allows the walk sound to play every 400ms so that it doesn't become painful to listen to
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 390)
                        {
                            redguyPos.Y += redguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 1)
                        {
                            redguyPos.X -= redguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 340)
                        {
                            redguyPos.X += redguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(redguyShoot))
                        {
                            if (redfireDelay == false)
                            {
                                bullets.Add(new Bullet(bulletSprite, redguyPos + new Vector2(50, 40), new Vector2(7, 0))); // Adds a new bullet at redguy's position with the horizontal speed of +7
                                pew.Play(0.2f,0,0);
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
                                woosh.Play(0.3f, 0, 0);
                                redguyPos.Y += dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 42)
                            {
                                redIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
                                redguyPos.Y -= dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 246)
                            {
                                redIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
                                redguyPos.X += dodgeDistance;
                                redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                redIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 70)
                            {
                                redIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
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
                            if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 390)
                        {
                            blueguyPos.Y += blueguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 395)
                        {
                            blueguyPos.X -= blueguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 753)
                        {
                            blueguyPos.X += blueguySpeed;
                            if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + walkSoundDelay)
                            {
                                walkSound.Play(0.2f, 0, 0);
                                blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(blueguyShoot))
                        {
                            if (bluefireDelay == false)
                            {
                                bullets.Add(new Bullet(bulletSprite, blueguyPos + new Vector2(-10, 40), new Vector2(-7, 0)));
                                bang.Play(0.2f, 0, 0);
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
                                woosh.Play(0.3f,0,0);
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 42)
                            {
                                blueIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
                                blueguyPos.Y -= dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 684)
                            {
                                blueIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
                                blueguyPos.X += dodgeDistance;
                                bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
                                blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
                                blueIsDodging = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 450)
                            {
                                blueIsDodging = true;
                                woosh.Play(0.3f, 0, 0);
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
                        bullets.Clear(); // Clears all bullets on screen
                        timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
                        timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds; // Sets powerup time spawned to the current time to stop them from spawning
                        powerups.Clear(); // Clears all powerups on screen
                        blueScore = blueScore + 1; // adds 1 to blue's score
                        deathSound.Play(0.5f,0,0); // plays a death sound
                        blueHasScored = true; // sets bluehasscored to true, so that it can display the 'Blue scored!' text
                        blueTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds; // sets blue time since last score to the current time, used for the amount of time 'Blue scored!' is on screen
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
                        timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
                        timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds;
                        powerups.Clear();
                        redScore = redScore + 1;
                        deathSound.Play(0.5f, 0, 0);
                        redHasScored = true;
                        redTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
            // Bullet & Player interactions end

            // Powerup spawning
            if (gameHasStarted == false)
            {
                timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
                timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds; // stops powerups from spawning before the game starts
            }

            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastSpawnRED + REDspawnDelay)
            {
                powerups.Add(new DashPowerup(powerupSprite, new Vector2(randomPositionREDX, randomPositionREDY))); // spawns a powerup at a random position on Redguy's side
                randomPositionREDX = new Random().Next(220, 380);
                randomPositionREDY = new Random().Next(30, 460); // makes a new random position
                REDspawnDelay = new Random().Next(15000, 25000); // sets the next powerup to spawn in 15 to 25 seconds
                timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastSpawnBLUE + BLUEspawnDelay)
            {
                powerups.Add(new DashPowerup(powerupSprite, new Vector2(randomPositionBLUEX, randomPositionBLUEY)));
                randomPositionBLUEX = new Random().Next(440, 560);
                randomPositionBLUEY = new Random().Next(30, 460);
                BLUEspawnDelay = new Random().Next(20000, 25000);
                timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds;
            }

            // Powerup & Player interactions
            // Redguy picks up powerup
            for (int i = 0; i < powerups.Count; i++)
            {
                if (redguyRect.Intersects(powerups[i].powerupRect))
                {
                    powerups.RemoveAt(i);
                    redguySpeed = 5;
                    redDodgeDelay = 600;
                    REDpowerupTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            // Blueguy picks up powerup
            for (int i = 0; i < powerups.Count; i++)
            {
                if (blueguyRect.Intersects(powerups[i].powerupRect))
                {
                    powerups.RemoveAt(i);
                    blueguySpeed = 5;
                    blueDodgeDelay = 600;
                    BLUEpowerupTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            // Powerup timer
            if (gameTime.TotalGameTime.TotalMilliseconds > REDpowerupTimer + powerupTime)
            {
                redguySpeed = 3;
                redDodgeDelay = 1200;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > BLUEpowerupTimer + powerupTime)
            {
                blueguySpeed = 3;
                blueDodgeDelay = 1200;
            }

            base.Update(gameTime);
        }

        void ResetPos()
        {
            redguyPos.X = 100;
            redguyPos.Y = 175;
            blueguyPos.X = 600;
            blueguyPos.Y = 175;
        }

        int scale = 3;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp
               );

            // Drawing background
            if (isInDesert == true)
            {
                _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            }
            if (isInForest == true)
            {
                _spriteBatch.Draw(backgroundSprite2, new Vector2(0, 0), Color.White);
            }

            // Making redguy and blueguy rectangles
            redguyRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, redguySprite.Width * scale, redguySprite.Height * scale);
            blueguyRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, blueguySprite.Width * scale, blueguySprite.Height * scale);
            Rectangle redguyDodgeRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, redguySpriteDodgeLarge.Width * scale, redguySpriteDodgeLarge.Height * scale);
            Rectangle blueguyDodgeRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, blueguySpriteDodgeLarge.Width * scale, blueguySpriteDodgeLarge.Height * scale);

            if (gameHasStarted == true)
            {
                // Drawing Redguy sprites
                if (blueHasScored == false)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > redInvulnTimer + redInvulnTime)
                    {
                        _spriteBatch.Draw(redguySprite, redguyRect, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(redguySpriteDodgeLarge, redguyDodgeRect, Color.White);
                    }
                }

                // Drawing Blueguy sprites
                if (redHasScored == false)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueInvulnTimer + blueInvulnTime)
                    {
                        _spriteBatch.Draw(blueguySprite, blueguyRect, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(blueguySpriteDodgeLarge, blueguyDodgeRect, Color.White);
                    }
                }
            }

            if (gameHasStarted == true)
            {
                // Drawing Blueguy & Redguy scores
                redScoreString = redScore.ToString();
                _spriteBatch.DrawString(font, "Red Score: " + redScoreString, new Vector2(10, 10), Color.Black);
                blueScoreString = blueScore.ToString();
                _spriteBatch.DrawString(font, "Blue Score: " + blueScoreString, new Vector2(680, 10), Color.Black);

                if (redHasScored == true)
                {
                    _spriteBatch.Draw(blueguySpriteDead, blueguyRect, Color.White);
                    _spriteBatch.DrawString(fontBold, "Red Scored!", new Vector2(330, 215), Color.Black);
                    if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastScore + scoreDelay)
                    {
                        redHasScored = false;
                        ResetPos();
                    }
                }
                if (blueHasScored == true)
                {
                    _spriteBatch.Draw(redguySpriteDead, redguyRect, Color.White);
                    _spriteBatch.DrawString(fontBold, "Blue Scored!", new Vector2(330, 215), Color.Black);
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastScore + scoreDelay)
                    {
                        blueHasScored = false;
                        ResetPos();
                    }
                }
            }

            // Drawing main menu
            if (isInMainMenu == true)
            {
                _spriteBatch.Draw(MainMenuSprite, new Vector2(0, 0), Color.White);
            }

            // Drawing bullets and powerups
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Draw(_spriteBatch);
            }

            // Drawing controls menu
            if (isInControlsMenu == true)
            {
                _spriteBatch.Draw(controlsScreen, new Vector2(0, 0), Color.White);
            }
            
            // Drawing japanese stuff
            if (isInJapan == true)
            {
                _spriteBatch.Draw(japaneseMenu, new Vector2(0, 0), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
