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
    class Bullet : Entity
    {
        //fields
        private Vector2 velocity;
        private int damage;
        private Helper.BulletType bulletType;
        private Color colorOverride;

        //constructor
        public Bullet(Vector2 myPosition, Vector2 myVelocity, BulletProperties myProperties)
        {
            bulletType = myProperties.bulletType;
            position = myPosition;
            velocity = myVelocity;
            sprite = myProperties.sprite;
            damage = myProperties.baseDamage;
            color = myProperties.color;
            colorOverride = Color.Transparent;

            enabled = true;
        }

        //getters AND setters
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public int Damage { get { return damage; } }
        public Color Color { get { return color; } set { color = value; } }
        public Helper.BulletType BulletType { get { return bulletType; } }
        public Color ColorOverride { set { colorOverride = value; } }

        //methods
        public override void Update()
        {
            if (enabled)
            {
                //essential bullet behavior (motion, hitbox, self-disable)
                position += velocity;

                hitbox = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), sprite.Width, sprite.Height);

                if (colorOverride != Color.Transparent)
                {
                    color = colorOverride;
                }

                if (position.X > Helper.SCREEN_WIDTH || position.X < 0 || position.Y > Helper.SCREEN_HEIGHT || position.Y < 0) //out of bounds
                {
                    enabled = false;
                    color = Color.Transparent;
                }
            }
        }

        //Draw() inherited from Entity class
    }
}
