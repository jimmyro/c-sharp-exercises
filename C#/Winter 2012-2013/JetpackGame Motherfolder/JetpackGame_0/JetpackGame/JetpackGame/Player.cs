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
    class Player
    {
        //fields
        private int maxHealth, currentHealth, oldHealth;
        private int maxFuel, currentFuel;
        private Vector2 position, velocity;
        private Texture2D sprite;
        private Color color;
        private Rectangle hitbox;
        private bool enabled;

        //TODO: player stats that other objects can recognize and look through?
            // currentHealth, startHealth, currentFuel, startFuel, power-ups and such
            // correspond to statCan types

        //constructor
        public Player(int myHealth, int myFuel, Vector2 myPosition, Vector2 myVelocity, Texture2D mySprite)
        {
            maxHealth = myHealth; currentHealth = myHealth; oldHealth = myHealth;
            maxFuel = myFuel; currentFuel = myFuel;
            position = myPosition;
            velocity = myVelocity;
            sprite = mySprite;

            color = Color.LightBlue;
            enabled = true;
        }

        //getters AND setters
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        public float HealthPercentage() { return (float)currentHealth / (float)maxHealth; }

        public int MaxFuel { get { return maxFuel; } }
        public int CurrentFuel { get { return currentFuel; } set { currentFuel = value; } }
        public float FuelPercentage() { return (float)currentFuel / (float)maxFuel; }

        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Rectangle Hitbox { get { return hitbox; } }

        public bool Enabled { get { return enabled; } set { enabled = value; } }

        //other setters
        public void SetPositionX(float x) { position.X = x; }
        public void SetPositionY(float y) { position.Y = y; }
        
        public void SetVelocityX(float x) { velocity.X = x; }
        public void SetVelocityY(float y) { velocity.Y = y; }

        //methods
        public void ChangePositionX(float dx)
        {
            position.X += dx;
        }

        public void ChangePositionY(float dy)
        {
            position.Y += dy;
        }

        public void Accelerate(float rate)
        {     
            velocity.Y *= (1f + rate);
        }

        public void Update() // just for collision detection
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);

            if (oldHealth > currentHealth)
            {
                color = Color.Red;
            }
            else if (currentHealth <= 0)
            {
                color = Color.DarkGray;

                //TODO: dying sequence
            }
            else
            {
                color = Color.LightBlue;
            }

            oldHealth = currentHealth;
        }

        public void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, color);
            }
        }
    }
}
