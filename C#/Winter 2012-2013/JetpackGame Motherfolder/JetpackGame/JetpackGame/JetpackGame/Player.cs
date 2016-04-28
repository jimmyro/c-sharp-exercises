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
    class Player : Entity
    {
        //fields
        private int maxHealth, currentHealth, oldHealth;
        private int maxFuel, currentFuel;
        private Vector2 velocity;
        private List<Vector2>[] activeEffects; //activeEffects[stat][effect] X = multiplier, Y = timer
        private Repulsor repulsor;

        //constructor
        public Player(int myHealth, int myFuel, Vector2 myPosition, Vector2 myVelocity, Texture2D mySprite)
        {
            maxHealth = myHealth; currentHealth = myHealth; oldHealth = myHealth;
            maxFuel = myFuel; currentFuel = myFuel;
            position = myPosition;
            velocity = myVelocity;
            sprite = mySprite;

            activeEffects = new List<Vector2>[7];
            for (int i = 0; i < 7; i++)
            {
                activeEffects[i] = new List<Vector2>();
            }

            color = Color.LightBlue;
            repulsor = null;
            enabled = true;
        }

        //getters AND setters
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        public float HealthPercentage() { return (float)currentHealth / (float)maxHealth; }

        public int MaxFuel { get { return maxFuel; } }
        public int CurrentFuel { get { return currentFuel; } set { currentFuel = value; } }
        public float FuelPercentage() { return (float)currentFuel / (float)maxFuel; }

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public List<Vector2>[] ActiveEffects { get { return activeEffects; } set { activeEffects = value; } }
        public Repulsor Repulsor { get { return repulsor; } set { repulsor = value; } }

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

        public void Update(GameTime gameTime) // just for collision detection
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);

            //color change
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
            
            //update timers on stat multipliers
            for (int stat = 0; stat < activeEffects.Length; stat++)
            {
                for (int effect = 0; effect < activeEffects[stat].Count; effect++)
                {
                    //decrement each effect's timer (the Y coordinate) as needed
                    activeEffects[stat][effect] = new Vector2(activeEffects[stat][effect].X,
                        activeEffects[stat][effect].Y - (float)gameTime.ElapsedGameTime.TotalSeconds);
                    //remove the effects that have expired
                    if (activeEffects[stat][effect].Y <= 0)
                    {
                        activeEffects[stat].RemoveAt(effect);
                        effect--;
                    }
                }
            }

            //check repulsor
            if (repulsor != null)
            {
                if (repulsor.Enabled)
                    repulsor.Update();
                else
                    repulsor = null;
            }
        }

        public float GetAggregateMultiplier(Helper.StatIndex stat)
        {
            float aggregateMultiplier = 1; //default: stat multiplier remains unchanged

            for (int effect = 0; effect < activeEffects[(int)stat].Count; effect++)
            {
                aggregateMultiplier *= activeEffects[(int)stat][effect].X;
            }

            return aggregateMultiplier;
        }

        public override void Draw(SpriteBatch myBatch)
        {
            if (repulsor != null)
                repulsor.Draw(myBatch);

            base.Draw(myBatch);
        }
    }
}
