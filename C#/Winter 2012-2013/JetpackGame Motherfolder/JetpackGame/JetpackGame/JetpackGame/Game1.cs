using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JetpackGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // declarations
        bool firstRun = true;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyboard, newKeyboard;
        SpriteFont font;

        Player player;
        Rectangle barRectangle;
        Vector2 spawnPosition;
        Color statColor;

        List<Turret> turrets;

        BulletService bulletService;
        BulletProperties[] bulletCatalog;

        List<StatCan> statCans;
        float spawnFrequency;
        Random random;
        
        Texture2D playerSprite, bulletSprite, bulletSpreadSprite, turretSprite, turretCaseLeftSprite, 
            turretCaseRightSprite, barSprite, plusSprite, plusSpriteInv, repulsorSprite, header, 
            startBackground, pauseBackground, gameOverBackground;
        Texture2D healthStat, fuelStat, healthRegenStat, fuelRegenStat, gravityStat,
            speedStat, repulsorStat;
        Texture2D[] statIcons;

        enum GameMode { StartMenu, InGame, PauseMenu, GameOver };
        GameMode gameMode;
        Menu startMenu, pauseMenu, gameOverMenu;

        float spawnTimer;
        float difficultyTimer, difficultyFactor;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // set window preferences
            graphics.IsFullScreen = false;
            // graphics.PreferMultiSampling = true;
            // graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = Helper.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Helper.SCREEN_HEIGHT;
        }

        protected override void Initialize()
        {
            LoadContent();

            //player
            spawnPosition = new Vector2((Helper.SCREEN_WIDTH - playerSprite.Width)/2f, Helper.SCREEN_HEIGHT - playerSprite.Height);
            player = new Player(100, 7500, spawnPosition, new Vector2(2.5f, 0.1f), playerSprite);

            //turrets
            turrets = new List<Turret>();
            turrets.Add(new Turret(player, turretSprite, new Vector2(0, Helper.SCREEN_HEIGHT), Helper.SCREEN_WIDTH, Helper.SCREEN_HEIGHT));
            turrets.Add(new Turret(player, turretSprite, new Vector2(Helper.SCREEN_WIDTH, Helper.SCREEN_HEIGHT), Helper.SCREEN_WIDTH, Helper.SCREEN_HEIGHT));

            //stat cans
            statCans = new List<StatCan>();
            spawnFrequency = 4f;
            random = new Random();
            statIcons = new Texture2D[7] { healthStat, fuelStat, healthRegenStat, fuelRegenStat, gravityStat, speedStat, repulsorStat };

            //timers
            spawnTimer = 0; 
            difficultyTimer = 0;
            difficultyFactor = 1;

            //bullet catalog
            bulletCatalog = new BulletProperties[]
            {
                new BulletProperties(Helper.BulletType.normal, Color.Black, bulletSprite, 10),
                new BulletProperties(Helper.BulletType.spread, Color.HotPink, bulletSpreadSprite, 5),
                new BulletProperties(Helper.BulletType.slow, Color.CornflowerBlue, bulletSpreadSprite, 5),
                new BulletProperties(Helper.BulletType.freeze, Color.LightBlue, bulletSpreadSprite, 0),
                new BulletProperties(Helper.BulletType.teleport, Color.DarkViolet, bulletSpreadSprite, 0)
            };

            bulletService = new BulletService(player, bulletCatalog, turrets);

            //menus
            startMenu = new Menu(font, new string[] { "Play", "Exit" }, 50, 50, 3, 12, startBackground);
            pauseMenu = new Menu(font, new string[] { "Continue", "Start Menu", "Exit" }, 50, 50, 3, 12, pauseBackground);
            gameOverMenu = new Menu(font, new string[] { "Retry", "Start Menu", "Exit" }, 50, 50, 3, 12, gameOverBackground);

            if (firstRun)
            {
                gameMode = GameMode.StartMenu;
                firstRun = false;
            }
            else
                gameMode = GameMode.InGame;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("player");
            turretSprite = Content.Load<Texture2D>("turret");
            bulletSprite = Content.Load<Texture2D>("bullet");
            bulletSpreadSprite = Content.Load<Texture2D>("bullet_spread");
            plusSprite = Content.Load<Texture2D>("stat_can");
            plusSpriteInv = Content.Load<Texture2D>("stat_can_regen");
            repulsorSprite = Content.Load<Texture2D>("repulsor_large");

            healthStat = Content.Load<Texture2D>("health");
            fuelStat = Content.Load<Texture2D>("fuel");
            healthRegenStat = Content.Load<Texture2D>("healthRegen");
            fuelRegenStat = Content.Load<Texture2D>("fuelRegen");
            gravityStat = Content.Load<Texture2D>("gravity");
            speedStat = Content.Load<Texture2D>("speed");
            repulsorStat = Content.Load<Texture2D>("repulsor");

            turretCaseLeftSprite = Content.Load<Texture2D>("turretcase_left");
            turretCaseRightSprite = Content.Load<Texture2D>("turretcase_right");

            barSprite = Content.Load<Texture2D>("horiz_bar");
            header = Content.Load<Texture2D>("headbar");

            startBackground = Content.Load<Texture2D>("title_card");
            pauseBackground = Content.Load<Texture2D>("pause");
            gameOverBackground = Content.Load<Texture2D>("game_over");

            font = Content.Load<SpriteFont>("myFont");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            playerInput(); //handles game mode (mostly)

            if (gameMode == GameMode.InGame)
            {
                //check for death
                if (player.CurrentHealth <= 0)
                {
                    gameMode = GameMode.GameOver;
                }

                //difficultyTimer: scale up difficulty at fixed intervals
                difficultyTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (difficultyTimer > 20) //bump up difficulty every 20 seconds
                {
                   difficultyFactor += 0.1f; //10%
                   difficultyTimer = 0;
                }

                //update player (movement and input)
                player.Update(gameTime);

                //update turrets (movement)
                for (int i = 0; i < turrets.Count; i++)
                {
                    turrets[i].Update();
                }

                bulletService.Update(gameTime, difficultyFactor);

                StatCanManager(gameTime);
            }

            base.Update(gameTime);
        }

        public void playerInput()
        {
            newKeyboard = Keyboard.GetState();

            switch (gameMode)
            {
                case GameMode.StartMenu:
                    if (newKeyboard.IsKeyDown(Keys.Down) && oldKeyboard.IsKeyUp(Keys.Down))
                        startMenu.IncPointer(1);
                    else if (newKeyboard.IsKeyDown(Keys.Up) && oldKeyboard.IsKeyUp(Keys.Down))
                        startMenu.IncPointer(-1);
                    else if (newKeyboard.IsKeyDown(Keys.Enter) && oldKeyboard.IsKeyUp(Keys.Enter))
                    {
                        switch (startMenu.Pointer)
                        {
                            case 0: //play
                                this.Initialize();
                                break;

                            //case 2: //high scores
                            //    
                            //    break;

                            case 1: //exit
                                this.Exit();
                                break;
                        }
                    }

                    break;

                case GameMode.InGame:
                    if (newKeyboard.IsKeyDown(Keys.Right)) // L-R movement
                    {
                        if (player.Position.X < Helper.SCREEN_WIDTH - player.Sprite.Width)
                            player.ChangePositionX(player.Velocity.X * player.GetAggregateMultiplier(Helper.StatIndex.speed));
                    }
                    else if (newKeyboard.IsKeyDown(Keys.Left))
                    {
                        if (player.Position.X > 0)
                            player.ChangePositionX(-player.Velocity.X * player.GetAggregateMultiplier(Helper.StatIndex.speed));
                    }

                    if (newKeyboard.IsKeyDown(Keys.Up) && player.CurrentFuel > 0) // jetpack is activated
                    {
                        if (oldKeyboard.IsKeyUp(Keys.Up)) // up was just pressed (wasn't down before)
                        {
                            player.SetVelocityY(1.1f * player.GetAggregateMultiplier(Helper.StatIndex.speed)); // start a new upward acceleration
                        }

                        player.CurrentFuel -= (int)(5 / player.GetAggregateMultiplier(Helper.StatIndex.fuelRegen));

                        if (player.Position.Y > header.Height) // motion
                        {
                            if (player.Velocity.Y < 4f)
                            {
                                player.Accelerate(0.05f * player.GetAggregateMultiplier(Helper.StatIndex.speed)
                                    / player.GetAggregateMultiplier(Helper.StatIndex.gravity));
                            } // accelerate at 5%
                            player.ChangePositionY(-player.Velocity.Y);
                        }
                    }
                    else if (player.Position.Y < Helper.SCREEN_HEIGHT - player.Sprite.Height &&
                        player.GetAggregateMultiplier(Helper.StatIndex.speed) > 0)
                    {
                        if (oldKeyboard.IsKeyDown(Keys.Up)) // up was just released
                        {
                            player.SetVelocityY(1.1f);
                        }

                        player.Accelerate(0.12f * player.GetAggregateMultiplier(Helper.StatIndex.gravity)); // accelerate at 12%
                        player.ChangePositionY(player.Velocity.Y);
                    }

                    if (newKeyboard.IsKeyDown(Keys.Z) && oldKeyboard.IsKeyUp(Keys.Z))
                    {
                        if (player.Repulsor == null && player.CurrentFuel > player.MaxFuel / 3) //must spend a third of the tank
                        {
                            player.CurrentFuel -= player.MaxFuel / 3; //subtact fuel cost

                            player.Repulsor = new Repulsor(player, bulletService.Bullets, repulsorSprite, 75, 5);
                        }
                    }

                    if (newKeyboard.IsKeyDown(Keys.Space)) //pause
                    {
                        gameMode = GameMode.PauseMenu;
                    }

                    break;
                
                case GameMode.PauseMenu:
                    if (newKeyboard.IsKeyDown(Keys.Down) && oldKeyboard.IsKeyUp(Keys.Down))
                        pauseMenu.IncPointer(1);
                    else if (newKeyboard.IsKeyDown(Keys.Up) && oldKeyboard.IsKeyUp(Keys.Up))
                        pauseMenu.IncPointer(-1);
                    else if (newKeyboard.IsKeyDown(Keys.Enter) && oldKeyboard.IsKeyUp(Keys.Enter))
                    {
                        switch (pauseMenu.Pointer)
                        {
                            case 0: //continue
                                gameMode = GameMode.InGame;
                                break;
                           
                            case 1: //main menu
                                gameMode = GameMode.StartMenu;
                                break;
                           
                            case 2: //exit
                                this.Exit();
                                break;
                        }
                    }

                    break;

                case GameMode.GameOver:
                    if (newKeyboard.IsKeyDown(Keys.Down) && oldKeyboard.IsKeyUp(Keys.Down))
                        gameOverMenu.IncPointer(1);
                    else if (newKeyboard.IsKeyDown(Keys.Up) && oldKeyboard.IsKeyUp(Keys.Up))
                        gameOverMenu.IncPointer(-1);
                    else if (newKeyboard.IsKeyDown(Keys.Enter) && oldKeyboard.IsKeyUp(Keys.Enter))
                    {
                        switch (gameOverMenu.Pointer)
                        {
                            case 0: //retry
                                this.Initialize();
                                gameMode = GameMode.InGame;
                                break;

                            case 1: //main menu
                                gameMode = GameMode.StartMenu;
                                break;

                            case 2: //exit
                                this.Exit();
                                break;
                        }
                    }

                    break;
            }

            oldKeyboard = newKeyboard;
        }

        public void StatCanManager(GameTime gameTime)
        {
            //spawnTimer: create stat cans
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            int randX, randY, randStatChance;
            if (spawnTimer > (spawnFrequency / difficultyFactor))
            {
                randX = random.Next(20, Helper.SCREEN_WIDTH - plusSprite.Width - 20);
                randY = random.Next(20, 250);
                randStatChance = random.Next(1, 20);

                if (randStatChance < 8) // 35% health
                {
                    statCans.Add(new StatCan(new Vector2(randX, randY), plusSprite, Helper.StatIndex.health, 10 * difficultyFactor));
                }
                else if (randStatChance >= 8 && randStatChance < 16) // 40% fuel
                {
                    statCans.Add(new StatCan(new Vector2(randX, randY), plusSprite, Helper.StatIndex.fuel, 750 * difficultyFactor));
                }
                else
                {
                    switch (randStatChance) // 5% for each of the multipliers
                    {
                        case 16:
                            statCans.Add(new StatCan(new Vector2(randX, randY), plusSpriteInv, Helper.StatIndex.healthRegen, 1.33f, 30));
                            break;
                        case 17:
                            statCans.Add(new StatCan(new Vector2(randX, randY), plusSpriteInv, Helper.StatIndex.fuelRegen, 1.33f, 45));
                            break;
                        case 18:
                            statCans.Add(new StatCan(new Vector2(randX, randY), plusSpriteInv, Helper.StatIndex.gravity, 0.33f, 15));
                            break;
                        case 19:
                            //speed
                            break;
                        case 20:
                            //repulsor
                            break;
                        default:
                            break;
                    }
                }

                spawnTimer = 0;
            }

            //update stat cans (collision)
            foreach (StatCan statCan in statCans)
            {
                if (statCan.Enabled && statCan.Hitbox.Intersects(player.Hitbox))
                {
                    if (statCan.Stat == Helper.StatIndex.health) //health
                    {
                        player.CurrentHealth += (int)(statCan.Value * player.GetAggregateMultiplier(Helper.StatIndex.healthRegen));

                        if (player.CurrentHealth > player.MaxHealth) //check for overflow
                            player.CurrentHealth = player.MaxHealth;
                    }
                    else if (statCan.Stat == Helper.StatIndex.fuel)
                    {
                        player.CurrentFuel += (int)(statCan.Value * player.GetAggregateMultiplier(Helper.StatIndex.fuelRegen));

                        if (player.CurrentFuel > player.MaxFuel) //check for overflow
                            player.CurrentFuel = player.MaxFuel;
                    }
                    else
                    {
                        player.ActiveEffects[(int)statCan.Stat].Add(new Vector2(statCan.Value, statCan.Duration));
                    }

                    statCan.Enabled = false;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
            
            switch (gameMode)
            {
                case GameMode.StartMenu:
                    startMenu.Draw(spriteBatch);
                    break;

                case GameMode.InGame:
                    // draw player with texture and position
                    player.Draw(spriteBatch);

                    // draw turret with texture, position, rotation, origin, and layerDepth
                    for (int i = 0; i < turrets.Count; i++)
                    {
                        turrets[i].Draw(spriteBatch);
                    }

                    // draw bullets with texture and position, or delete them
                    bulletService.Draw(spriteBatch);

                    // draw stat cans with texture and position, or delete them
                    for (int i = 0; i < statCans.Count; i++)
                    {
                        if (statCans[i].Enabled)
                        {
                            statCans[i].Draw(spriteBatch);
                        }
                        else
                        {
                            statCans.RemoveAt(i);
                            i--;
                        }
                    }

                    //header (health/fuel bars, score, and more?)
                    spriteBatch.Draw(header, new Vector2(0, 0), Color.White);

                    barRectangle = new Rectangle(25, 8, (int)(barSprite.Width * player.HealthPercentage()), barSprite.Height);
                    spriteBatch.Draw(barSprite, barRectangle, Color.Chartreuse); //25, 8

                    barRectangle = new Rectangle(25, 22, (int)(barSprite.Width * player.FuelPercentage()), barSprite.Height);
                    spriteBatch.Draw(barSprite, barRectangle, Color.Gold); //25, 22

                    spriteBatch.DrawString(font, Convert.ToString(Math.Round((decimal)(gameTime.TotalGameTime.TotalMilliseconds/100), 0)), 
                        new Vector2(440, 9), Color.White); //437, 8
            
                    //slots: 541, 577, 613, 649, 685, 721, 757 (y = 8 for all)

                    for (int i = 0; i < statIcons.Length; i++)
                    {
                        if (player.GetAggregateMultiplier((Helper.StatIndex)i) > 1) //you can actually cast an int to an enum!  phew...
                            statColor = Color.White;
                        else if (player.GetAggregateMultiplier((Helper.StatIndex)i) < 1)
                            statColor = Color.Red;
                        else
                            statColor = Color.LightGray;

                        spriteBatch.Draw(statIcons[i], new Vector2(541 + 36*i, 8), statColor);
                    }
            
                    // eye candy for the turrets
                    spriteBatch.Draw(turretCaseLeftSprite, new Vector2(0, Helper.SCREEN_HEIGHT - turretCaseLeftSprite.Height), Color.White);
                    spriteBatch.Draw(turretCaseRightSprite, new Vector2(Helper.SCREEN_WIDTH - turretCaseRightSprite.Width, Helper.SCREEN_HEIGHT - turretCaseRightSprite.Height), Color.White);

                    break;

                case GameMode.PauseMenu:
                    pauseMenu.Draw(spriteBatch);
                    break;

                case GameMode.GameOver:
                    gameOverMenu.Draw(spriteBatch);
                    break;

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
