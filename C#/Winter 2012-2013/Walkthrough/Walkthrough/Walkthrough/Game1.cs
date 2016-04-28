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

namespace Walkthrough
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyboard, newKeyboard;
       
        Player player;
        
        List<Bullet> bullets; // contains all bullets the player shoots
        List<Enemy> enemies; // keeps track of all the enemies
        
        Texture2D bulletSprite, enemySprite;
       
        int SCREEN_WIDTH;
        int SCREEN_HEIGHT;
        const float EPSILON = 0.00001f;
        const int RESPAWN_TIME = 10;

        GameTime spawnTime = null;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // use it for things that have to be drawn once 
            player = new Player(100, new Vector2(0, 0), new Vector2(3, 3));
            LoadContent();

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            Enemy enemy0 = new Enemy(100, new Vector2(400f, 100f), new Vector2(0, 0));
            enemy0.SetSprite(enemySprite);
            enemies.Add(enemy0);
            
            SCREEN_WIDTH = graphics.GraphicsDevice.Viewport.Width;
            SCREEN_HEIGHT = graphics.GraphicsDevice.Viewport.Height;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bulletSprite = Content.Load<Texture2D>("bullet");
            enemySprite = Content.Load<Texture2D>("enemy");
            player.SetSprite(Content.Load<Texture2D>("cannonball"));
        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (spawnTime == null)
            {
                spawnTime = gameTime; // initialize
            }

            if (spawnTime.ElapsedGameTime.TotalSeconds % RESPAWN_TIME == 10)
            {
                Enemy spawn = new Enemy(100, new Vector2(400f, 250f), new Vector2(0, 0));
                enemies.Add(spawn);

                //spawnTime = gameTime;
            }

            playerInput();

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                CheckCollision(bullets[i]);
            }
           
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
            }

            base.Update(gameTime);
        }

        void CheckCollision(Bullet bullet)
        {
            Enemy collided = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                //returns true if bullet's rectangle is intersecting with an enemy's rectangle
                if (bullet.Hitbox().Intersects(enemies[i].Hitbox()))
                {
                    //if there is a collision, hurt enemy and remove bullet
                    collided = enemies[i];
                    break;
                }
            }

            if (collided != null)
            {
                enemies.Remove(collided);
                bullet.SetEnabled(false);
            }
        }

        public void playerInput()
        {
            newKeyboard = Keyboard.GetState();

            if (newKeyboard.IsKeyDown(Keys.Right))
            {
                player.ChangePositionX(player.Velocity().X);
            }
            else if (newKeyboard.IsKeyDown(Keys.Left))
            {
                player.ChangePositionX(-player.Velocity().X);
            }
            if (newKeyboard.IsKeyDown(Keys.Down))
            {
                player.ChangePositionY(player.Velocity().Y);
            }
            else if (newKeyboard.IsKeyDown(Keys.Up))
            {
                player.ChangePositionY(-player.Velocity().Y);
            }

            if (newKeyboard.IsKeyDown(Keys.Space))
            {
                if (oldKeyboard.IsKeyUp(Keys.Space))
                {
                    ShootBullet();
                }
            }

            /*for (int i = 0; i < bullets.Count; ++i)
            {
                bullets[i].ChangePositionX(bullets[i].Velocity().X); //add velocity to position for each bullet
            }*/

            oldKeyboard = newKeyboard;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
             
            spriteBatch.Begin();
            spriteBatch.Draw(player.Sprite(), player.Position(), Color.White);

            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }

            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void ShootBullet()
        {
            Bullet b = new Bullet(player.Position() + new Vector2(player.Sprite().Width, player.Sprite().Height / 2f), new Vector2 (10,0), bulletSprite);

            bullets.Add(b);
        }
    }
}
