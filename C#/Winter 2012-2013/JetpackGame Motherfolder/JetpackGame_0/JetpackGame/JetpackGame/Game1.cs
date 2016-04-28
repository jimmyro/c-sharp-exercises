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
    /*GOALS
     * :D     1. Movable player (L, R, jump) affected by gravity (accelerates down)
     * :D     2. Player has a "jetpack" (accelerates up against the force of gravity)
     * :D     3. Cannon object that can "look at" a point
     * :D     4. Cannon fires projectiles continuously at the player
     * :D     5. Player sustains damage from projectiles
     * :D     6. Display for health (and other stats)
     * :D     7. Jetpack will run out of fuel, fuel bottles respawn randomly to fill again
     * :D     8. Bullets become faster and more frequent as game goes on
     *      9. Player can sacrifice fuel for a momentary reflector shield (send bullets away)
     *      10. L/R sprites and jetpack on/off sprites for player
     */

    /*HOW TO MAKE A MENU
     * enum GameState
     * {
     * StartScreen,
     * LoadScreen,
     * Menu,
     * InGame
     * }
     */

    // three objects to deal with: player (P, green), bullets (B, red), and turrets (T, gray)
    // fourth object: fuel bottles (F, yellow)

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyboard, newKeyboard;
        int SCREEN_WIDTH, SCREEN_HEIGHT;
        SpriteFont font;

        Player player;
        Rectangle barRectangle;
        Vector2 spawnPosition;
        
        Turret leftTurret, rightTurret;
        
        List<Bullet> bullets;
        int bulletDamage;
        float bulletFrequency, bulletSpeed;

        List<StatCan> statCans;
        float spawnFrequency;
        Random random;
        
        Texture2D playerSprite, bulletSprite, turretSprite, turretCaseLeftSprite, 
            turretCaseRightSprite, barSprite, statCanSprite, header;

        float fireTimer;
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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            LoadContent();
           
            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;

            //player
            spawnPosition = new Vector2((SCREEN_WIDTH - playerSprite.Width)/2f, SCREEN_HEIGHT - playerSprite.Height);
            player = new Player(100, 1500, spawnPosition, new Vector2(2.5f, 0.1f), playerSprite);

            //turrets
            leftTurret = new Turret(player, turretSprite, new Vector2(0, SCREEN_HEIGHT), SCREEN_WIDTH, SCREEN_HEIGHT);
            rightTurret = new Turret(player, turretSprite, new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT), SCREEN_WIDTH, SCREEN_HEIGHT);

            //bullets
            bullets = new List<Bullet>();
            bulletDamage = 10;
            bulletFrequency = 1.5f;
            bulletSpeed = 5f;

            //stat cans
            statCans = new List<StatCan>();
            spawnFrequency = 4f;
            random = new Random();

            //timers
            fireTimer = 0; spawnTimer = 0; difficultyTimer = 0;
            difficultyFactor = 1;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerSprite = Content.Load<Texture2D>("player");
            turretSprite = Content.Load<Texture2D>("turret");
            bulletSprite = Content.Load<Texture2D>("bullet");
            statCanSprite = Content.Load<Texture2D>("stat_can");

            turretCaseLeftSprite = Content.Load<Texture2D>("turretcase_left");
            turretCaseRightSprite = Content.Load<Texture2D>("turretcase_right");

            barSprite = Content.Load<Texture2D>("horiz_bar");
            header = Content.Load<Texture2D>("headbar");

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

            //difficultyTimer: scale up difficulty at fixed intervals
            difficultyTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (difficultyTimer > 15) //bump up difficulty every 30 seconds
            {
                difficultyFactor += 0.15f;
                difficultyTimer = 0;
            }

            //update player (movement and input)
            playerInput();
            player.Update();

            //update turrets (movement)
            leftTurret.Update(); 
            rightTurret.Update();

            //fireTimer: fire turrets
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (fireTimer > (bulletFrequency / difficultyFactor)) //bulletFrequency = seconds between bullets
            {
                bullets.Add(new Bullet(rightTurret.GetBulletPosition(), rightTurret.GetBulletVelocity(bulletSpeed * difficultyFactor), 
                    bulletSprite, (int)(bulletDamage * difficultyFactor)));
                bullets.Add(new Bullet(leftTurret.GetBulletPosition(), leftTurret.GetBulletVelocity(bulletSpeed * difficultyFactor), 
                    bulletSprite, (int)(bulletDamage * difficultyFactor)));

                fireTimer = 0;
            }

            //spawnTimer: create stat cans
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            int randX, randY, randStatChance;
            if (spawnTimer > (spawnFrequency / difficultyFactor))
            {
                randX = random.Next(20, SCREEN_WIDTH - statCanSprite.Width - 20);
                randY = random.Next(20, 250);
                randStatChance = random.Next(1, 10);

                if (randStatChance > 6) //40% chance for health
                {
                    statCans.Add(new StatCan(new Vector2(randX, randY), statCanSprite, (int)(10 * difficultyFactor), 0));
                }
                else
                {
                    statCans.Add(new StatCan(new Vector2(randX, randY), statCanSprite, (int)(150 * difficultyFactor), 1));
                }

                spawnTimer = 0;
            }

            //update bullets (movement and collision)
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(graphics.GraphicsDevice);

                if (bullet.Enabled && bullet.Hitbox.Intersects(player.Hitbox))
                {
                    player.CurrentHealth -= bullet.Damage;
                    bullet.Enabled = false;
                }
            }

            //update stat cans (collision)
            foreach (StatCan statCan in statCans)
            {
                if (statCan.Enabled && statCan.Hitbox.Intersects(player.Hitbox))
                {
                    if ((int)statCan.Stat == 0) //health
                    {
                        player.CurrentHealth += statCan.Value;

                        if (player.CurrentHealth > player.MaxHealth) //check for overflow
                            player.CurrentHealth = player.MaxHealth;
                    }
                    else if ((int)statCan.Stat == 1) //fuel
                    {
                        player.CurrentFuel += statCan.Value;

                        if (player.CurrentFuel > player.MaxFuel) //check for overflow
                            player.CurrentFuel = player.MaxFuel;
                    }

                    statCan.Enabled = false;
                }
            }

            base.Update(gameTime);
        }

        public void playerInput()
        {
            newKeyboard = Keyboard.GetState();

            if (newKeyboard.IsKeyDown(Keys.Right)) // L-R movement
            {
                if (player.Position.X < SCREEN_WIDTH - player.Sprite.Width)
                    player.ChangePositionX(player.Velocity.X);
            }
            else if (newKeyboard.IsKeyDown(Keys.Left))
            {
                if (player.Position.X > 0)
                    player.ChangePositionX(-player.Velocity.X);
            }

            if (newKeyboard.IsKeyDown(Keys.Up) && player.CurrentFuel > 0) // jetpack is activated
            {
                if (oldKeyboard.IsKeyUp(Keys.Up)) // up was just pressed (wasn't down before)
                {
                    player.SetVelocityY(1.1f); // start a new upward acceleration
                }

                player.CurrentFuel -= 1;

                if (player.Position.Y > header.Height) // motion
                {
                    if (player.Velocity.Y < 4f) { player.Accelerate(0.05f); } // accelerate at 5%
                    player.ChangePositionY(-player.Velocity.Y);
                }
            }
            else if (player.Position.Y < SCREEN_HEIGHT - player.Sprite.Height)
            {
                if (oldKeyboard.IsKeyDown(Keys.Up)) // up was just released
                {
                    player.SetVelocityY(1.1f);
                }

                player.Accelerate(0.12f); // accelerate at 20%
                player.ChangePositionY(player.Velocity.Y);
            }

            // TODO: pause key

            // TODO: menu key

            oldKeyboard = newKeyboard;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
            
            // draw player with texture and position
            player.Draw(spriteBatch);

            // draw turret with texture, position, rotation, origin, and layerDepth
            leftTurret.Draw(spriteBatch);
            rightTurret.Draw(spriteBatch);

            // draw bullets with texture and position, or delete them
            for (int i = 1; i < bullets.Count; i++)
            {
                if (bullets[i].Enabled)
                {
                    bullets[i].Draw(spriteBatch);
                }
                else
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            // draw stat cans with texture and position, or delete them
            for (int i = 1; i < statCans.Count; i++)
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
            
            //slots: 541, 577, 613, 649, 685, 721, 757
            
            // eye candy for the turrets
            spriteBatch.Draw(turretCaseLeftSprite, new Vector2(0, SCREEN_HEIGHT - turretCaseLeftSprite.Height), Color.White);
            spriteBatch.Draw(turretCaseRightSprite, new Vector2(SCREEN_WIDTH - turretCaseRightSprite.Width, SCREEN_HEIGHT - turretCaseRightSprite.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
