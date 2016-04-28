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
    class Player
    {
        //properties of Player
        private int health;
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D sprite;

        //constructor
        public Player(int myHealth, Vector2 myPosition, Vector2 myVelocity)
        {
            health = myHealth;
            position = myPosition;
            velocity = myVelocity;
        }

        //getters
        public int Health() { return health; }
        public Vector2 Velocity() { return velocity;}
        public Vector2 Position() { return position; }
        public Texture2D Sprite() { return sprite; }

        //setters
        public void SetSprite(Texture2D mySprite) { sprite = mySprite; }
        public void SetPosition(Vector2 myPosition) {position = myPosition;}
        public void SetPositionX(float x) { position.X = x; }
        public void SetPositionY(float y) { position.Y = y; }
        public void SetVelocity(Vector2 myVelocity) { velocity = myVelocity; }
        public void SetVelocityX(float x) {velocity.X = x;}
        public void SetVelocityY(float y) { velocity.Y = y; }
        
        //other methods
        public void ChangePositionX(float dx)
        {
            position.X += dx;
        }
        public void ChangePositionY(float dy)
        {
            position.Y +=dy;
        }

    }
}
