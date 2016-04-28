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
    class Bullet
    {
        //properties
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D sprite;
        private int damage;
        private Rectangle hitbox;
        private bool enabled;

        //constructor
        public Bullet(Vector2 myPosition, Vector2 myVelocity, Texture2D mySprite)
        {
            position = myPosition;
            velocity = myVelocity;
            sprite = mySprite;
            
            enabled = true;
        }

        //getters
        public int Damage() { return damage; }
        public Vector2 Position() { return position; }
        public Vector2 Velocity() { return velocity; }
        public Texture2D Sprite() { return sprite; }
        public Rectangle Hitbox() { return hitbox; }
        public bool Enabled() { return enabled; }

        //setters
        public void SetEnabled(bool myTruth) { enabled = myTruth; }

        //other methods
        public void ChangePositionX(float dx) { position.X += dx; }

        public void Update()
        {
            if (enabled)
            {
                position += velocity;
                hitbox = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), sprite.Width, sprite.Height);
            }
            //it would be a mistake to put an "else" here.  suiciding objects always cause problems!
        }
            //it's better to put this kind of stuff in a class.
            //the less you have to manage from the main game class, the better.
        public void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, Color.White);
            }
        }
    }

}
