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
        Texture2D smileySprite;
        Texture2D dojoBackground;
        Texture2D finalBoss;
        Texture2D YellowguyARMS;
        Texture2D blueguyCOOP;
        Texture2D blueguyDodgeCOOP;
        Texture2D graveStone;
        Texture2D yellowguyBG;
        Texture2D LoseScreen;
        Texture2D YoureWinner;
        Texture2D controlsScreenAlt;

        // Declaring fonts
        SpriteFont font;
        SpriteFont fontBold;

        // Declaring sounds
        SoundEffect walkSound;
        SoundEffect pew;
        SoundEffect bang;
        SoundEffect woosh;
        SoundEffect deathSound;
        SoundEffect popSound;
        SoundEffect blingSound;
        SoundEffect BoomSound;
        SoundEffect AghSound;
        SoundEffect GruntSound;
        SoundEffect UghSound;
        SoundEffect JustDieAlready;
        SoundEffect FuckinStupid;
        Song finalBossSpeech;
        Song menuMusic;
        Song controlsScreenMusic;
        Song BossBattleMusic;
        Song GameOverMusic;
        Song EndCreditsSong;

        // Position & walking stuff
        Vector2 redguyPos = new Vector2(100, 175);
        Vector2 blueguyPos = new Vector2(600, 175); // Sets redguy and blueguy's default position
        double redTimeSinceLastWalked = 0;
        double blueTimeSinceLastWalked = 0;
        int redWalkSoundDelay = 400; // These are used to make walk sounds better
        int blueWalkSoundDelay = 400; // They are kept seperate so they can speed up if the speed powerup is picked up
        int scale = 3; // used to change sprite scales and I didn't find a better place to put it

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
        bool overAltControlsButton = false;
        bool inAltControlsMenu = false;
        bool overMap1Button = false;
        bool overMap2Button = false; // Used for the button selection
        bool menuMusicCanPlay = true; 
        bool controlsMusicCanPlay = false; // self explanatory
        bool escapeKeyWasPressed = false; // Used to make escape presses a toggle

        // Background stuff
        bool isInDesert = false;
        bool isInForest = false;
        bool isInControlsMenu = false; // These are used to determine what background is displayed

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
        int BLUEspawnDelay = new Random().Next(7000, 15000); // These set a random number between 7 second and 15 seconds, used to spawn the powerups when the game starts
        int randomPositionREDX = new Random().Next(180, 320);
        int randomPositionBLUEX = new Random().Next(430, 520); // These are used to set a random spawn location for the powerups, on red and blue's side respectivley
        int randomPositionY = new Random().Next(50, 300);
        double REDpowerupTimer = 0;
        double BLUEpowerupTimer = 0;
        int powerupTime = 5000; // The powerups are active for 5 seconds

        // Japanese mode stuff (Secret so don't tell anyone ok)
        Texture2D japaneseMenu;
        Song japaneseMusic;
        bool isInJapan = false;
        bool isInDojo = false; // Used for backgrounds
        bool japanMusicCanPlay = false; // Used to play the music
        bool overPlayJapButton = false; 
        bool overExitButton = false; // Used for button management
        bool JapGameHasStarted = false;
        bool blueIsAliveCOOP = true;
        bool redIsAliveCOOP = true;
        bool COOPFailure = false; // Used for the game over screen
        double cutsceneStartTime = 0; // Set to the current GameTime when the cutscene starts
        int cutsceneLength = 19500; // The cutscene is 19 and a half seconds long
        bool bossBattleSongCanPlay = false;
        bool finalCreditsSongCanPlay = false;
        bool gameOverMusicCanPlay = false;  // Used for music
        bool gameIsWon = false; // Used to draw the win screen

        // Japanese mode cutscene stuff (basically a bunch of timers)
        double cutscenepart1start = 0;
        int cutscenepart1time = 19000;
        double transformationstart = 99999999999999999;
        int transformationtime = 3000;
        bool transformpartCanStart = true;
        bool bossBattleMusicCheck = true;

        // Enemy Stuff (lol, 1 enemy being the boss)
        Vector2 enemyPos = new Vector2(570, 255);  // Starting position
        int enemyScale = 2; // Used to scale the final boss
        int randomNumberToFour = new Random().Next(1, 5); // Gets a random number between 1 and four
        double MoveStart = 0;
        float MoveDelay = 3000f; // Used for enemy movement
        Rectangle enemyRect; // Collision rectangle
        bool enemyCanDoAction = true; 
        double enemyTimeSinceLastAction = 0; // Doesn't allow enemy to spam moves
        int fireDelay = 300; // I would've implemented something like this for the players if I knew how to when starting this project.
        double timeSinceLastFired = 0;
        int enemyHealth = 500; // It used to be 1000 lol.
        string enemyHealthString; // Used to display enemy health
        int OneToThree = new Random().Next(1, 4); // Selects a random number between 1 and 3
        int OneOrTwo = new Random().Next(1, 3); // 1 or 2
        int OneInAMillion = new Random().Next(1, 1000001); //  one in a million (used to say secret line)
        bool hasSaidHalfHealthLine = false; // yeah
        double timeSinceLastPain = 0; 
        int painSoundDelay = 1500; // makes pain sounds less intrusive

        // Listing stuff
        List<Bullet> bullets = new List<Bullet>();
        List<DashPowerup> powerups = new List<DashPowerup>();
        List<Smiley> smileys = new List<Smiley>(); // Using lists so there can be multiple on screen

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
        Keys blueguyShoot2 = Keys.NumPad1;
        Keys blueguyDodge = Keys.OemComma;
        Keys blueguyDodge2 = Keys.NumPad2;

        // Blue and Red speed
        float redguySpeed = 3;
        float blueguySpeed = 3; // Red and blueguy speed are seperate integers so they can be affected by powerups seperately
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
            blueguyCOOP = Content.Load<Texture2D>("Players/BlueguyCOOP");
            blueguyDodgeCOOP = Content.Load<Texture2D>("Players/BlueguydodgelargeCOOP");

            // Loading miscellaneous sprites
            bulletSprite = Content.Load<Texture2D>("Items/Bullet");
            powerupSprite = Content.Load<Texture2D>("Items/Fastshoe");
            graveStone = Content.Load<Texture2D>("Players/Gravestone");

            // Loading enemies
            smileySprite = Content.Load<Texture2D>("Enemies/Smiley");
            finalBoss = Content.Load<Texture2D>("Enemies/finalboss1");
            YellowguyARMS = Content.Load<Texture2D>("Enemies/YellowguyARMS");

            // Loading menu sprites
            MainMenuSprite = Content.Load<Texture2D>("Backgrounds/MainMenu");
            controlsScreen = Content.Load<Texture2D>("Backgrounds/Controlsscreen");
            controlsScreenAlt = Content.Load<Texture2D>("Backgrounds/ControlsscreenALT");
            japaneseMenu = Content.Load<Texture2D>("Backgrounds/Japanesemenu");
            LoseScreen = Content.Load<Texture2D>("Backgrounds/YOULOSE");
            YoureWinner = Content.Load<Texture2D>("Backgrounds/YoureWinner");

            // Loading gameplay backgrounds
            backgroundSprite = Content.Load<Texture2D>("Backgrounds/Desertbackground");
            backgroundSprite2 = Content.Load<Texture2D>("Backgrounds/Forestbackground");
            dojoBackground = Content.Load<Texture2D>("Backgrounds/dojo");
            yellowguyBG = Content.Load<Texture2D>("Backgrounds/YellowguyBGBars");

            // Loading fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            fontBold = Content.Load<SpriteFont>("Fonts/FontBold");

            // Loading music
            menuMusic = Content.Load<Song>("music/menumusic");
            controlsScreenMusic = Content.Load<Song>("music/controlsscreenmusic");
            japaneseMusic = Content.Load<Song>("music/japanesemusic");
            finalBossSpeech = Content.Load<Song>("music/Finalbossspeech");
            BossBattleMusic = Content.Load<Song>("music/BossBattleMusic");
            GameOverMusic = Content.Load<Song>("music/GameOverMusic");
            EndCreditsSong = Content.Load<Song>("music/EndCreditsSong");

            // Loading sound effects
            pew = Content.Load<SoundEffect>("sound effects/pew");
            bang = Content.Load<SoundEffect>("sound effects/bang");
            walkSound = Content.Load<SoundEffect>("sound effects/walksound");
            deathSound = Content.Load<SoundEffect>("sound effects/deathsound");
            woosh = Content.Load<SoundEffect>("sound effects/woosh");
            popSound = Content.Load<SoundEffect>("sound effects/pop");
            blingSound = Content.Load<SoundEffect>("sound effects/bling");
            BoomSound = Content.Load<SoundEffect>("sound effects/Boom");
            AghSound = Content.Load<SoundEffect>("sound effects/Agh");
            GruntSound = Content.Load<SoundEffect>("sound effects/Grunt");
            UghSound = Content.Load<SoundEffect>("sound effects/Ugh");
            JustDieAlready = Content.Load<SoundEffect>("sound effects/JustDieAlready");
            FuckinStupid = Content.Load<SoundEffect>("sound effects/FuckinStupid");
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            // Moving bullets forwards / backwards
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].MoveBullet(); // Calls the MoveBullet() function for every bullet in the list
            }
            for (int i = 0; i < smileys.Count; i++)
            {
                smileys[i].MoveSmiley(); // Smileys are basically enemy bullets lol
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
                controlsMusicCanPlay = false; // Only plays it once so it isn't spammed
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
            if (isInControlsMenu == true)
            {
                if (mouseState.X > 285 && mouseState.X < 485 && mouseState.Y > 10 && mouseState.Y < 85)
                {
                    overAltControlsButton = true;
                } else
                {
                    overAltControlsButton = false;
                }
                if (overAltControlsButton == true && mouseState.LeftButton == ButtonState.Pressed)
                {
                    inAltControlsMenu = true;
                }
            }

            // Exiting the game / Returning to menu
            if (escapeKeyWasPressed == false)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && gameHasStarted == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && isInControlsMenu == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && inAltControlsMenu == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && isInJapan == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && gameIsWon == true)
                {
                    ReturnToMenu();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && JapGameHasStarted == true || Keyboard.GetState().IsKeyDown(Keys.Escape) && COOPFailure == true)
            {
                MediaPlayer.Stop();
                enemyHealth = 500;
                isInJapan = true;
                JapGameHasStarted = false;
                isInDojo = false;
                COOPFailure = false;
                bullets.Clear();
                smileys.Clear();
                japanMusicCanPlay = true;
                escapeKeyWasPressed = true;
                bossBattleMusicCheck = true;
                hasSaidHalfHealthLine = false; // Like return to menu but for japan
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                escapeKeyWasPressed = false; // Used so that it isn't detected twice and you go back to main menu
            }

            if (gameHasStarted == true || JapGameHasStarted == true)
            {
                IsMouseVisible = false; // Makes the mouse invisible during gameplay
            } else
            {
               IsMouseVisible = true;
            }

            // Red and Blue movement start
            if (gameHasStarted == true) 
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastScore + scoreDelay && gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastScore + scoreDelay)
                {
                    RedguyMove(gameTime);
                    BlueguyMove(gameTime); // Put redguy and blueguy's movement in seperate functions
                }
            }
            // Red and Blue movement end

            //Bullet & Player interactions
            if (gameHasStarted == true)
            {
                //Redguy gets hit
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (redguyRect.Intersects(bullets[i].bulletRect))
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds > redInvulnTimer + redInvulnTime)
                        {
                            bullets.Clear(); // Clears all bullets on screen
                            timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
                            timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds; // Sets powerup time spawned to the current time to stop them from spawning
                            powerups.Clear(); // Clears all powerups on screen
                            blueScore = blueScore + 1; // adds 1 to blue's score
                            deathSound.Play(0.5f, 0, 0); // plays a death sound
                            blueHasScored = true; // sets bluehasscored to true, so that it can display the 'Blue scored!' text
                            redguySpeed = 3;
                            redDodgeDelay = 1200;
                            redWalkSoundDelay = 400;
                            blueguySpeed = 3;
                            blueDodgeDelay = 1200;
                            blueWalkSoundDelay = 400; // Resets speed and sounds in case someone still had the powerup right before they died
                            blueTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds; // sets blue time since last score to the current time, used for the amount of time 'Blue scored!' is on screen
                        }
                    }
                }
                //Blueguy gets hit (same as red, except red scores)
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
                            redguySpeed = 3;
                            redDodgeDelay = 1200;
                            redWalkSoundDelay = 400;
                            blueguySpeed = 3;
                            blueDodgeDelay = 1200;
                            blueWalkSoundDelay = 400;
                            redTimeSinceLastScore = gameTime.TotalGameTime.TotalMilliseconds;
                        }
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
                powerups.Add(new DashPowerup(powerupSprite, new Vector2(randomPositionREDX, randomPositionY))); // spawns a powerup at a random position on Redguy's side
                popSound.Play(0.6f, 0, 0); // Plays a popping sound effect
                randomPositionREDX = new Random().Next(180, 320);
                randomPositionY = new Random().Next(50, 300); // makes a new random position
                REDspawnDelay = new Random().Next(17000, 22000); // sets the next powerup to spawn in 17 to 22 seconds
                timeSinceLastSpawnRED = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastSpawnBLUE + BLUEspawnDelay) // Same as red, except affecting blue.
            {
                powerups.Add(new DashPowerup(powerupSprite, new Vector2(randomPositionBLUEX, randomPositionY)));
                popSound.Play(0.6f, 0, 0);
                randomPositionBLUEX = new Random().Next(430, 520);
                randomPositionY = new Random().Next(50, 300);
                BLUEspawnDelay = new Random().Next(17000, 22000);
                timeSinceLastSpawnBLUE = gameTime.TotalGameTime.TotalMilliseconds;
            }
            // Powerup spawning end

            // Powerup & Player interactions
            // Redguy picks up powerup
            for (int i = 0; i < powerups.Count; i++)
            {
                if (redguyRect.Intersects(powerups[i].powerupRect))
                {
                    powerups.RemoveAt(i); // Removes the picked up powerup
                    blingSound.Play(0.6f, 0, 0); // Plays a bling sound
                    redguySpeed = redguySpeed * 1.5f; // Makes redguy faster
                    redDodgeDelay = 850; // Makes his dodge delay less, allowing him to dodge faster
                    redWalkSoundDelay = 200; // Halves the walk sound delay, so it plays more freqently
                    REDpowerupTimer = gameTime.TotalGameTime.TotalMilliseconds; // Sets the timer to the current time
                }
            }
            // Blueguy picks up powerup (same as red but blue)
            for (int i = 0; i < powerups.Count; i++)
            {
                if (blueguyRect.Intersects(powerups[i].powerupRect))
                {
                    powerups.RemoveAt(i);
                    blingSound.Play(0.6f, 0, 0);
                    blueguySpeed = blueguySpeed * 1.5f;
                    blueDodgeDelay = 850;
                    blueWalkSoundDelay = 200;
                    BLUEpowerupTimer = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            // Powerup timer
            if (gameTime.TotalGameTime.TotalMilliseconds > REDpowerupTimer + powerupTime)
            {
                redguySpeed = 3;
                redDodgeDelay = 1200;
                redWalkSoundDelay = 400; // Changes redguy's speed and dodge delay to default when the timer runs out
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > BLUEpowerupTimer + powerupTime)
            {
                blueguySpeed = 3;
                blueDodgeDelay = 1200;
                blueWalkSoundDelay = 400;
            }

            // Japanese mode stuff
            if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.N) && Keyboard.GetState().IsKeyDown(Keys.I) && Keyboard.GetState().IsKeyDown(Keys.M) && Keyboard.GetState().IsKeyDown(Keys.E) && isInMainMenu == true)
            { // If you hold down 'ANIME' you go to the seceret japanese mode
                isInMainMenu = false;
                isInJapan = true;
                MediaPlayer.Stop();
                japanMusicCanPlay = true;
            }
            if (japanMusicCanPlay == true) // The following is just for music playing
            {
                MediaPlayer.Play(japaneseMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                japanMusicCanPlay = false;
            }
            if (bossBattleSongCanPlay == true)
            {
                MediaPlayer.Play(BossBattleMusic);
                MediaPlayer.Volume = 0.5f;
                MediaPlayer.IsRepeating = true;
                bossBattleSongCanPlay = false;
            }
            if (gameOverMusicCanPlay == true)
            {
                MediaPlayer.Play(GameOverMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                gameOverMusicCanPlay = false;
            }
            if (finalCreditsSongCanPlay == true)
            {
                MediaPlayer.Play(EndCreditsSong);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                finalCreditsSongCanPlay = false;
            } // Music playing end
            if (isInJapan == true)
            {
                if (mouseState.X > 80 && mouseState.X < 325 && mouseState.Y < 235 && mouseState.Y > 115)
                {
                    overPlayJapButton = true;
                }
                else
                {
                    overPlayJapButton = false;
                }
                if (mouseState.X > 80 && mouseState.X < 325 && mouseState.Y < 390 && mouseState.Y > 270)
                {
                    overExitButton = true;
                }
                else
                {
                    overExitButton = false;
                }
                if (overExitButton == true && mouseState.LeftButton == ButtonState.Pressed)
                {
                    ReturnToMenu(); // Same menu stuff but as a japanese mode
                }
                if (overPlayJapButton == true && mouseState.LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    MediaPlayer.Stop();
                    ResetPosJapan();
                    isInJapan = false;
                    isInDojo = true;
                    JapGameHasStarted = true;
                    transformpartCanStart = true;
                    cutsceneStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                    MediaPlayer.Play(finalBossSpeech);
                    MediaPlayer.Volume = 0.2f;
                    MediaPlayer.IsRepeating = false;
                    cutscenepart1start = gameTime.TotalGameTime.TotalMilliseconds;
                    enemyHealth = 500;
                    hasSaidHalfHealthLine = false;

                    cutscenepart1start = gameTime.TotalGameTime.TotalMilliseconds;
                    cutsceneLength = 19500;
                    cutscenepart1time = 19000;
                    transformationstart = 99999999999999999;
                    transformationtime = 3000;
                    transformpartCanStart = true; // Sets the cutscene stuff to default when the game starts.
                }
            }
            if (JapGameHasStarted == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    MediaPlayer.Stop();
                    cutsceneLength = 0;
                    transformationtime = 0;
                    transformpartCanStart = false;
                    cutscenepart1time = 0;
                    transformationstart = gameTime.TotalGameTime.TotalMilliseconds; // On an enter press it skips the cutscene
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > cutsceneStartTime + cutsceneLength)
                {
                    if (redIsAliveCOOP == true)
                    {
                        RedguyMoveJAPAN(gameTime); // Seperate japan movement so that they stop in different places
                    }
                    if (blueIsAliveCOOP)
                    {
                        BlueguyMoveJAPAN(gameTime);
                    }
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > cutscenepart1start + 16000 && transformpartCanStart == true)
                {
                    transformationstart = gameTime.TotalGameTime.TotalMilliseconds;
                    BoomSound.Play(0.5f, 0, 0); // Plays a boom sound when he is revealed to be yellowguy
                    transformpartCanStart = false;
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > transformationstart + transformationtime)
                {                    
                    EnemyAction(gameTime); // After the tranformation the boss performs his actions
                    if (enemyCanDoAction == true)
                    {
                        enemyTimeSinceLastAction = gameTime.TotalGameTime.TotalMilliseconds;
                        enemyCanDoAction = false;
                    }
                    if (gameTime.TotalGameTime.TotalMilliseconds > enemyTimeSinceLastAction + 1000) // Used so that he doesn't spam actions (it looks weird)
                    {
                        randomNumberToFour = new Random().Next(1, 5);
                        enemyCanDoAction = true;
                    }
                }
                // Redguy and Blueguy smiley collisions (they die)
                for (int i = 0; i < smileys.Count; i++)
                {
                    if (redguyRect.Intersects(smileys[i].smileyRect))
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds > redInvulnTimer + redInvulnTime)
                        {
                            if (redIsAliveCOOP == true)
                            {
                                deathSound.Play(0.5f, 0, 0);
                            }
                            redIsAliveCOOP = false;
                        }
                    }
                    if (blueguyRect.Intersects(smileys[i].smileyRect))
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds > blueInvulnTimer + blueInvulnTime)
                        {
                            if (blueIsAliveCOOP == true)
                            {
                                deathSound.Play(0.5f, 0, 0);
                            }
                            blueIsAliveCOOP = false;
                        }
                    }
                }
                // Final boss bullet collisions (he loses health)
                for (int i = 0; i < bullets.Count; i++) // for every bullet in the list
                {
                    if (enemyRect.Intersects(bullets[i].bulletRect)) // if enemy intersects with bullet i (the one that hit him)
                    {
                        bullets.RemoveAt(i); // remove bullet i
                        enemyHealth--; // remove 1 health
                        if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastPain + painSoundDelay) // if the gametime is higher than the timesincelast pain and the paindelay
                        {
                            OneToThree = new Random().Next(1, 4); // get a new random number from 1 to 3
                            OneOrTwo = new Random().Next(1, 3); // get 1 or 2
                            switch (OneToThree)
                            {
                                case 1:
                                    AghSound.Play(0.3f, 0, 0);
                                    break; // if it's 1, go 'Agh'
                                case 2:
                                    GruntSound.Play(0.3f, 0, 0);
                                    break; // if it's 2, grunt
                                case 3:
                                    UghSound.Play(0.3f, 0, 0);
                                    break; // if it's 3, go 'Ugh'
                                default:
                                    AghSound.Play(0.3f, 0, 0);
                                    break; // if none, go 'Agh' again (just in case)
                            }
                            switch (OneOrTwo)
                            {
                                case 1:
                                    painSoundDelay = 500;
                                    break;
                                case 2:
                                    painSoundDelay = 1500;
                                    break;
                                default:
                                    painSoundDelay = 1000;
                                    break;
                            } // Either he waits 500ms to make a pain sound again, or he waits 1500ms.
                            timeSinceLastPain = gameTime.TotalGameTime.TotalMilliseconds;
                        }
                    }
                }
                if (enemyHealth <= 250) // If the enemy's health is under or equal to half
                {
                    OneInAMillion = new Random().Next(1, 1000001); // get a number between 1 in a million
                    if (hasSaidHalfHealthLine == false) // if he hasn't said the line yet
                    {
                        if (OneInAMillion == 1337) // if the number is 1337
                        {
                            FuckinStupid.Play(0.5f, 0, 0); // play the secret voiceline
                        }
                        else
                        {
                            JustDieAlready.Play(0.5f, 0, 0); // otherwise play the normal one
                        }
                    }
                    hasSaidHalfHealthLine = true; // he can't say the half health line until the boss battle is started again.
                }
                if (blueIsAliveCOOP == false && redIsAliveCOOP == false) // if they both die
                {
                    smileys.Clear();
                    bullets.Clear(); // clear bullets and smileys
                    COOPFailure = true; // they have failed
                    isInDojo = false;
                    JapGameHasStarted = false;
                    gameOverMusicCanPlay = true; // play game over music
                }
                if (enemyHealth <= 0) // when the enemy health reaches zero
                {
                    JapGameHasStarted = false; // game has ended
                    MediaPlayer.Stop(); // stops playing the boss music
                    bullets.Clear(); 
                    smileys.Clear(); // clears bullets and smileys
                    gameIsWon = true; // the game is won
                    isInDojo = false;
                    finalCreditsSongCanPlay = true; // plays the final credits song
                }
            }

            base.Update(gameTime);
        }

        void ResetPos() // Self explanatory
        {
            redguyPos.X = 100;
            redguyPos.Y = 175;
            blueguyPos.X = 600;
            blueguyPos.Y = 175;
        }

        void ResetPosJapan()
        {
            redguyPos.X = 162;
            redguyPos.Y = 258;
            blueguyPos.X = 60;
            blueguyPos.Y = 330;
            redIsAliveCOOP = true;
            blueIsAliveCOOP = true;
        } // same as ResetPos but uses the default japanese mode positions

        void RedguyMove(GameTime gameTime)
        {
            // Red Movement
            if (redIsDodging == false)
            {
                if (Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 5) // I manually set bounds to where the players can and cannot go
                {
                    redguyPos.Y -= redguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + redWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds; // This only allows the walk sound to play every 400ms so that it doesn't become painful to listen to
                    }
                }
                if (Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 390)
                {
                    redguyPos.Y += redguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + redWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 1)
                {
                    redguyPos.X -= redguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + redWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 340)
                {
                    redguyPos.X += redguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > redTimeSinceLastWalked + redWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        redTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(redguyShoot))
                {
                    if (redfireDelay == false)
                    {
                        bullets.Add(new Bullet(bulletSprite, redguyPos + new Vector2(50, 40), new Vector2(7, 0))); // Adds a new bullet at redguy's position with the horizontal speed of +7
                        pew.Play(0.2f, 0, 0);
                        redfireDelay = true; // Red cannot fire if the shoot button is held down
                    }
                }
                if (Keyboard.GetState().IsKeyUp(redguyShoot))
                {
                    redfireDelay = false; // Red has to lift the shoot button to fire again
                }

                // Red dodging
                if (gameTime.TotalGameTime.TotalMilliseconds > redtimeSinceLastDodge + redDodgeDelay)
                {
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 346)
                    {
                        redguyPos.Y += dodgeDistance; // Change redguy's Y position by the dodge distance (Y position because he's moving down)
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 42)
                    {
                        redguyPos.Y -= dodgeDistance;
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 246)
                    {
                        redguyPos.X += dodgeDistance;
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 70)
                    {
                        redguyPos.X -= dodgeDistance;
                        RedDodge(gameTime);
                    }
                }
            }
        } // THIS SAME BASE IS USED TO REDGUYJAPAN WITH A FEW DIFFERENCES, SAME GOES FOR BLUE
        void RedguyMoveJAPAN(GameTime gameTime)
        {
            if (redIsDodging == false)
            {
                if (Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 225)
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
                        pew.Play(0.1f, 0, 0);
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
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveDown) && redguyPos.Y <= 345)
                    {
                        redguyPos.Y += dodgeDistance;
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveUp) && redguyPos.Y >= 250)
                    {
                        redguyPos.Y -= dodgeDistance;
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveRight) && redguyPos.X <= 245)
                    {
                        redguyPos.X += dodgeDistance;
                        RedDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(redguyDodge) && Keyboard.GetState().IsKeyDown(redguyMoveLeft) && redguyPos.X >= 70)
                    {
                        redguyPos.X -= dodgeDistance;
                        RedDodge(gameTime);
                    }
                }
            }
        }

        void BlueguyMove(GameTime gameTime)
        {
            // Blue movement (basically the same as red)
            if (blueIsDodging == false)
            {
                if (Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 5)
                {
                    blueguyPos.Y -= blueguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + blueWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 390)
                {
                    blueguyPos.Y += blueguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + blueWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 395)
                {
                    blueguyPos.X -= blueguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + blueWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 753)
                {
                    blueguyPos.X += blueguySpeed;
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueTimeSinceLastWalked + blueWalkSoundDelay)
                    {
                        walkSound.Play(0.1f, 0, 0);
                        blueTimeSinceLastWalked = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(blueguyShoot) || Keyboard.GetState().IsKeyDown(blueguyShoot2))
                {
                    if (bluefireDelay == false)
                    {
                        bullets.Add(new Bullet(bulletSprite, blueguyPos + new Vector2(-10, 40), new Vector2(-7, 0)));
                        bang.Play(0.2f, 0, 0);
                        bluefireDelay = true;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(blueguyShoot) && Keyboard.GetState().IsKeyUp(blueguyShoot2))
                {
                    bluefireDelay = false;
                }

                // Blue dodging
                if (gameTime.TotalGameTime.TotalMilliseconds > bluetimeSinceLastDodge + blueDodgeDelay)
                {
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 346  || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 346)
                    {
                        blueguyPos.Y += dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 42  || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 42)
                    {
                        blueguyPos.Y -= dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 684 || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 684)
                    {
                        blueguyPos.X += dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 450 || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 450)
                    {
                        blueguyPos.X -= dodgeDistance;
                        BlueDodge(gameTime);
                    }
                }
            }
        }
        void BlueguyMoveJAPAN(GameTime gameTime)
        {
            // Blue movement (basically the same as red)
            if (blueIsDodging == false)
            {
                if (Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 225)
                {
                    blueguyPos.Y -= blueguySpeed;
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 390)
                {
                    blueguyPos.Y += blueguySpeed;
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 1)
                {
                    blueguyPos.X -= blueguySpeed;
                }
                if (Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 340)
                {
                    blueguyPos.X += blueguySpeed;
                }
                if (Keyboard.GetState().IsKeyDown(blueguyShoot) || Keyboard.GetState().IsKeyDown(blueguyShoot2))
                {
                    if (bluefireDelay == false)
                    {
                        bullets.Add(new Bullet(bulletSprite, blueguyPos + new Vector2(50, 40), new Vector2(7, 0)));
                        bang.Play(0.1f, 0, 0);
                        bluefireDelay = true;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(blueguyShoot) && Keyboard.GetState().IsKeyUp(blueguyShoot2))
                {
                    bluefireDelay = false;
                }

                // Blue dodging
                if (gameTime.TotalGameTime.TotalMilliseconds > bluetimeSinceLastDodge + blueDodgeDelay)
                {
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 345  || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveDown) && blueguyPos.Y <= 345)
                    {
                        blueguyPos.Y += dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 250 || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveUp) && blueguyPos.Y >= 250)
                    {
                        blueguyPos.Y -= dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 245  || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveRight) && blueguyPos.X <= 245)
                    {
                        blueguyPos.X += dodgeDistance;
                        BlueDodge(gameTime);
                    }
                    if (Keyboard.GetState().IsKeyDown(blueguyDodge) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 70  || Keyboard.GetState().IsKeyDown(blueguyDodge2) && Keyboard.GetState().IsKeyDown(blueguyMoveLeft) && blueguyPos.X >= 70)
                    {
                        blueguyPos.X -= dodgeDistance;
                        BlueDodge(gameTime);
                    }
                }
            }
        }

        void RedDodge(GameTime gameTime)
        {
            redtimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds; // Set the time since his last dodge to the current time
            redInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds; // Set his invulnerability timer to the current time
            redIsDodging = true; // Red is dodging
            woosh.Play(0.3f, 0, 0); // play a whoosh sound effect
            redIsDodging = false; // Red is no longer dodging
            // I think redIsDodging / blueIsDodging is actually useless but I don't know if something will break if I remove it.
        }
        void BlueDodge(GameTime gameTime)
        {
            bluetimeSinceLastDodge = gameTime.TotalGameTime.TotalMilliseconds;
            blueInvulnTimer = gameTime.TotalGameTime.TotalMilliseconds;
            blueIsDodging = true;
            woosh.Play(0.3f, 0, 0);
            blueIsDodging = false;
        }

        void ReturnToMenu()
        {
            MediaPlayer.Stop();
            bullets.Clear();
            powerups.Clear();
            isInMainMenu = true;
            gameIsWon = false;
            isInDesert = false;
            isInForest = false;
            isInControlsMenu = false;
            inAltControlsMenu = false;
            isInJapan = false;
            isInDojo = false;
            gameHasStarted = false;
            menuMusicCanPlay = true;
            redHasScored = false;
            blueHasScored = false;
            redScore = 0;
            blueScore = 0;
            ResetPos(); // on pressing escape it resets everything and returns to main menu
        } // self explanatory

        public void EnemyAction(GameTime gameTime)
        {
            if (bossBattleMusicCheck == true) // if the bossbattle music is not playing, play it
            {
                bossBattleSongCanPlay = true;
                bossBattleMusicCheck = false;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds > MoveStart + MoveDelay) // if he hasn't done a move for however long the move delay is, do a move
            {
                if (randomNumberToFour == 1 && enemyPos.Y > 180) // if the random number to four is one and he's lower than 180
                {
                    MoveEnemyUp(); // move him up
                }
                else if (randomNumberToFour == 1 && enemyPos.Y <= 180) // if his y position is at 180 or higher
                {
                    MoveEnemyDown(); // move him down
                    randomNumberToFour = 2; // set the random number to 2
                }
                if (randomNumberToFour == 2 && enemyPos.Y < 370) // if his position is more than 370 and the random number is 2
                {
                    MoveEnemyDown(); // move him down
                }
                else if (randomNumberToFour == 2 && enemyPos.Y >= 370) // otherwise if he's at 370
                {
                    MoveEnemyUp(); // move him up
                    randomNumberToFour = 1; // set the number to one
                } else
                {
                    EnemyFire(gameTime); // otherwise shoot if the number is anything else
                }
            }
        }
        void MoveEnemyUp()
        {
            enemyPos.Y -= 3; // moves the enemy up by 3 every frame
        }
        void MoveEnemyDown()
        {
            enemyPos.Y += 3; // moves the enemy down by 3 every frame
        }
        void EnemyFire(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastFired + fireDelay) // Same as how blueguy and redguy fire but with an added firedelay
            {
                smileys.Add(new Smiley(smileySprite, enemyPos + new Vector2(-20, 50), new Vector2(-7, 0)));
                timeSinceLastFired = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

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
            if (isInDojo == true)
            {
                _spriteBatch.Draw(dojoBackground, new Vector2(0, 0), Color.White);
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

                if (redScore + blueScore > 9 && redScore + blueScore < 13)
                {
                    _spriteBatch.DrawString(font, "You've been fighting for a while.", new Vector2(260, 440), Color.Black);
                    _spriteBatch.DrawString(font, "In the menu, hold down the keys that spell ANIME for your final fight.", new Vector2(150, 460), Color.Black);
                }
            }

            // Drawing main menu
            if (isInMainMenu == true)
            {
                _spriteBatch.Draw(MainMenuSprite, new Vector2(0, 0), Color.White);
            }

            // Drawing controls menu
            if (isInControlsMenu == true)
            {
                _spriteBatch.Draw(controlsScreen, new Vector2(0, 0), Color.White);
            }
            if (inAltControlsMenu == true)
            {
                _spriteBatch.Draw(controlsScreenAlt, new Vector2(0, 0), Color.White);
            }

            // Drawing japanese stuff
            Rectangle finalBossRect = new Rectangle((int)540, (int)225, finalBoss.Width * scale, finalBoss.Height * scale);
            Rectangle redGraveRect = new Rectangle((int)redguyPos.X, (int)redguyPos.Y, graveStone.Width * 2, graveStone.Height * 2);
            Rectangle blueGraveRect = new Rectangle((int)blueguyPos.X, (int)blueguyPos.Y, graveStone.Width * 2, graveStone.Height * 2);
            enemyRect = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, (int)YellowguyARMS.Width * enemyScale, (int)YellowguyARMS.Height * enemyScale);

            if (isInJapan == true)
            {
                _spriteBatch.Draw(japaneseMenu, new Vector2(0, 0), Color.White);
            }
            if (JapGameHasStarted == true)
            {
                if (redguyPos.Y <= blueguyPos.Y)
                {
                    if (redIsAliveCOOP == true)
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
                    else
                    {
                        _spriteBatch.Draw(graveStone, redGraveRect, Color.White);
                    }
                }
                if (blueIsAliveCOOP == true)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > blueInvulnTimer + blueInvulnTime)
                    {
                        _spriteBatch.Draw(blueguyCOOP, blueguyRect, Color.White);
                    } else
                    {
                        _spriteBatch.Draw(blueguyDodgeCOOP, blueguyDodgeRect, Color.White);
                    }
                } else
                {
                    _spriteBatch.Draw(graveStone, blueGraveRect, Color.White);
                }
                if (redguyPos.Y > blueguyPos.Y)
                {
                    if (redIsAliveCOOP == true)
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
                    else
                    {
                        _spriteBatch.Draw(graveStone, redGraveRect, Color.White);
                    }
                }
                if (gameTime.TotalGameTime.TotalMilliseconds < cutscenepart1start + cutscenepart1time)
                {
                    _spriteBatch.Draw(finalBoss, finalBossRect, Color.White);
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > transformationstart && gameTime.TotalGameTime.TotalMilliseconds < transformationstart + transformationtime)
                {
                    _spriteBatch.Draw(yellowguyBG, new Vector2(0, 0), Color.White);
                }
                if (gameTime.TotalGameTime.TotalMilliseconds > transformationstart + transformationtime)
                {
                    _spriteBatch.Draw(YellowguyARMS, enemyRect, Color.White);
                    enemyHealthString = enemyHealth.ToString();
                    _spriteBatch.DrawString(fontBold, "Boss Health: " + enemyHealthString, new Vector2(620, 10), Color.Black);
                }
            }
            if (COOPFailure == true)
            {
                _spriteBatch.Draw(LoseScreen, new Vector2(0, 0), Color.White);
            }
            if (gameIsWon == true)
            {
                _spriteBatch.Draw(YoureWinner, new Vector2(0, 0), Color.White);
            }

            // Drawing items (bullets, powerups)
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < smileys.Count; i++)
            {
                smileys[i].Draw(_spriteBatch);
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
