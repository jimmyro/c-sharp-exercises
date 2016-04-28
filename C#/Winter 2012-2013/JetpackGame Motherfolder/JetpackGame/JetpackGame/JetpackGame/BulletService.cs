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
    class BulletService
    {
        //variables
        private Player player;
        private BulletProperties[] bulletCatalog;
        private List<Turret> turrets;

        private List<Bullet> bullets, waitingBullets;

        private float bulletFrequency, bulletSpeed, fireTimer;
        private Random random;

        //constructor
        public BulletService(Player myPlayer, BulletProperties[] myBulletCatalog, List<Turret> myTurrets)
        {
            player = myPlayer;
            bulletCatalog = myBulletCatalog;
            turrets = myTurrets;

            bullets = new List<Bullet>();
            waitingBullets = new List<Bullet>();

            bulletFrequency = 1.5f;
            bulletSpeed = 5f;

            fireTimer = 0f;
            random = new Random();
        }

        //getters and setters
        public List<Bullet> Bullets { get { return bullets; } }

        //methods
        public void Update(GameTime gameTime, float difficultyFactor)
        {
            //fireTimer: fire turrets and add bullets to waiting list
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (fireTimer > (bulletFrequency / difficultyFactor)) //bulletFrequency = seconds between bullets
            {
                for (int i = 0; i < turrets.Count; i++)
                {
                    waitingBullets.Add(new Bullet(turrets[i].GetBulletPosition(), turrets[i].GetBulletVelocity(bulletSpeed *
                        (1 + (1 - difficultyFactor) / 2)), GetBulletProperties(difficultyFactor)));
                }

                fireTimer = 0;
            }

            //update bullets (movement and collision)
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();

                //collision behaviors
                if (bullets[i].Enabled && bullets[i].Hitbox.Intersects(player.Hitbox))
                {
                    //universal
                    player.CurrentHealth -= bullets[i].Damage;
                    bullets[i].Enabled = false;

                    //special
                    switch (bullets[i].BulletType) //REMEMBER: x = multiplier, y = timer
                    {
                        case Helper.BulletType.slow:
                            //cuts speed in half for base duration of 5 seconds
                            player.ActiveEffects[(int)Helper.StatIndex.speed].Add(new Vector2(0.5f, 7.5f * difficultyFactor));
                            break;

                        case Helper.BulletType.freeze:
                            player.ActiveEffects[(int)Helper.StatIndex.speed].Add(new Vector2(0f, 5f * difficultyFactor));
                            break;

                        case Helper.BulletType.teleport:
                            player.Position = new Vector2(random.Next(0, Helper.SCREEN_WIDTH - player.Sprite.Width), random.Next(0, 250));
                            break;

                        default:
                            break;
                    }
                }

                //other behaviors (special)
                switch (bullets[i].BulletType)
                {
                    case Helper.BulletType.spread:
                        if (Vector2.Distance(bullets[i].Position, player.Position) < 100f)
                        {
                            bullets[i].Enabled = false;

                            //spawn three to six new 'normal' type bullets
                            for (int j = 1; j <= (int)random.Next(6, 12); j++)
                            {
                                waitingBullets.Add(new Bullet(bullets[i].Position, new Vector2((float)random.Next(-10, 10) / 10f * bulletSpeed,
                                    (float)random.Next(-10, 10) / 10f * bulletSpeed), bulletCatalog[0]));
                                waitingBullets.Last<Bullet>().ColorOverride = Color.HotPink;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            //now that all the *other* iterating is done, add newly created bullets to the master list
            for (int i = 0; i < waitingBullets.Count; i++)
            {
                bullets.Add(waitingBullets[i]);
            }

            waitingBullets.Clear();
        }

        private int randBulletChance;
        private BulletProperties GetBulletProperties(float difficultyFactor)
        {
            randBulletChance = random.Next(1, (int)(10 * difficultyFactor));

            if (randBulletChance <= 10)
                return bulletCatalog[(int)Helper.BulletType.normal];
            else if (randBulletChance == 11 || randBulletChance == 12) //11-12
                return bulletCatalog[(int)Helper.BulletType.slow];
            else if (randBulletChance >= 13 && randBulletChance <= 15) //13-15
                return bulletCatalog[(int)Helper.BulletType.teleport];
            else if (randBulletChance >= 16 && randBulletChance <= 18) //16-18
                return bulletCatalog[(int)Helper.BulletType.spread];
            else //19-29
                return bulletCatalog[(int)Helper.BulletType.freeze]; 
        }

        public void Draw(SpriteBatch myBatch)
        {
            for (int i = 1; i < bullets.Count; i++)
            {
                if (bullets[i].Enabled)
                {
                    bullets[i].Draw(myBatch);
                }
                else
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
