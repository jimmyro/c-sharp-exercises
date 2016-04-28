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
    class StatCan
    {
        //fields
        private Vector2 position;
        private Texture2D sprite;
        private int svalue;
        private Rectangle hitbox;
        private Color color;

        //public enum StatIndex : int { health, fuel }; //health = 0, fuel = 1.  other stats coming soon?
        private int stat; //StatIndex stat;

        private bool enabled;

        //constructor
        public StatCan(Vector2 myPosition, Texture2D mySprite, int myValue, int myStat) //StatIndex myStat)
        {
            position = myPosition;
            sprite = mySprite;
            svalue = myValue;
            stat = myStat;

            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Width, (int)sprite.Height);

            enabled = true;
        }

        //getters AND setters
        public Vector2 Position { get { return position; } set { position = value; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public int Value { get { return svalue; } set { svalue = value; } }
        public int Stat { get { return stat; } }
        public Rectangle Hitbox { get { return hitbox; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        //methods
        public void Draw(SpriteBatch myBatch)
        {
            if (stat == 0)
                color = Color.Chartreuse;
            else
                color = Color.Gold;

            myBatch.Draw(sprite, position, color);
        }

    }
}
