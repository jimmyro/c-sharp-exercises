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
    class Bullet //: Entity
    {
        //fields
        Vector2 position, velocity;
        Texture2D sprite;
        Rectangle hitbox;
        int damage;
        Color color;

        bool enabled;

        //constructor
        public Bullet(Vector2 myPosition, Vector2 myVelocity, Texture2D mySprite, int myDamage)
        {
            position = myPosition;
            velocity = myVelocity;
            sprite = mySprite;
            damage = myDamage;

            color = Color.Black;
            enabled = true;
        }

        //getters AND setters
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public int Damage { get { return damage; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        //methods
        public void Update(GraphicsDevice graphics)
        {
            if (enabled)
            {
                position += velocity;

                hitbox = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), sprite.Width, sprite.Height);

                if (position.X > graphics.Viewport.Width || position.X < 0 || position.Y > graphics.Viewport.Height || position.Y < 0) //out of bounds
                {
                    enabled = false;
                    color = Color.Transparent;
                }
            }
        }

        public void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, color);
                //chartreuse, orangered, navajowhite, hotpink, seashell
            }
        }
    }
}
