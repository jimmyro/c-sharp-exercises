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
    class Turret : Entity
    {
        //fields
        private Player target;

        private double rotation; // Math.Atan() doesn't take floats...
        private Vector2 origin;
        
        private int SCREEN_WIDTH, SCREEN_HEIGHT; //the getters I put in the Game1 class aren't working,
        //so I didn't know what else to do other than passing these values in through the constructor

        //constructor
        public Turret(Player myTarget, Texture2D mySprite, Vector2 myPosition, int screenWidth, int screenHeight)
        {
            target = myTarget;
            sprite = mySprite;
            position = myPosition;
            rotation = 0;
            SCREEN_WIDTH = screenWidth; //now, this is annoying
            SCREEN_HEIGHT = screenHeight;

            origin = new Vector2((sprite.Width/2f), sprite.Height);

            enabled = true;
        }

        //getters AND setters
        public Player Target { get { return target; } set { target = value; } }
        public double Rotation { get { return rotation; } set { rotation = value; } }

        //methods
        private float targetDistX, targetDistY, bulletPosX, bulletPosY; //not fields
        public override void Update()
        {
            //update rotation
            targetDistY = SCREEN_HEIGHT - (target.Position.Y + (target.Sprite.Height / 2));

            if (position.X <= (SCREEN_WIDTH / 2)) //left side
            {
                targetDistX = target.Position.X + (target.Sprite.Width / 2);
                rotation = Math.Atan(Convert.ToDouble(targetDistX / targetDistY));
            }
            else //right side
            {
                targetDistX = SCREEN_WIDTH - (target.Position.X + (target.Sprite.Width / 2));
                rotation = -Math.Atan(Convert.ToDouble(targetDistX / targetDistY));
            }
        }

        public Vector2 GetBulletPosition() //returns position the bullet would be fired from in the turret's current state
        {
            bulletPosY = SCREEN_HEIGHT - (sprite.Height * targetDistY) / Vector2.Distance(position, target.Position); //similar triangles 
            //bulletPosY = SCREEN_HEIGHT - (Convert.ToSingle(Math.Sin(MathHelper.PiOver4 - rotation)) * sprite.Height); //trig

            if (position.X <= (SCREEN_WIDTH / 2))
                bulletPosX = (sprite.Height * targetDistX) / Vector2.Distance(position, target.Position) - (sprite.Width / 2);
                //bulletPosX = Convert.ToSingle(Math.Cos(MathHelper.PiOver4 - rotation)) * sprite.Height;
            else
                bulletPosX = SCREEN_WIDTH - (sprite.Height * targetDistX) / Vector2.Distance(position, target.Position) - (sprite.Width / 2);
                //bulletPosX = Convert.ToSingle(Math.Cos(MathHelper.PiOver4 - rotation)) * sprite.Height;

            return new Vector2(bulletPosX, bulletPosY);
        }

        public Vector2 GetBulletVelocity(float speedFactor) //returns the direction in which to fire the bullet
        {
            if (position.X <= (SCREEN_WIDTH / 2)) //left side
                return Vector2.Normalize(new Vector2(targetDistX, -targetDistY)) * speedFactor;
            else //right side
                return Vector2.Normalize(new Vector2(-targetDistX, -targetDistY)) * speedFactor;
        }

        public override void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, null, Color.White, (float)rotation, origin, 1f, SpriteEffects.None, 1);
            }
        }
    }
}
