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
    class StatCan : Entity
    {
        //fields
        private float svalue, duration;

        private Helper.StatIndex stat;

        //constructor
        public StatCan(Vector2 myPosition, Texture2D mySprite, Helper.StatIndex myStat, float myValue, float myDuration = 0)
        {
            position = myPosition;
            sprite = mySprite;
            svalue = myValue;
            duration = myDuration;
            stat = myStat;

            switch (stat) //decide color
            {
                case Helper.StatIndex.health:
                    color = Color.Chartreuse;
                    break;
                case Helper.StatIndex.fuel:
                    color = Color.Gold;
                    break;
                case Helper.StatIndex.healthRegen:
                    color = Color.Chartreuse;
                    break;
                case Helper.StatIndex.fuelRegen:
                    color = Color.Gold;
                    break;
                case Helper.StatIndex.gravity:
                    color = Color.Thistle;
                    break;
                case Helper.StatIndex.speed:
                    color = Color.CornflowerBlue;
                    break;
                case Helper.StatIndex.repulsor:
                    color = Color.DarkViolet;
                    break;
                default:
                    color = Color.White;
                    break;
            }

            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Width, (int)sprite.Height);

            enabled = true;
        }

        //getters AND setters
        public float Value { get { return svalue; } set { svalue = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public Helper.StatIndex Stat { get { return stat; } }

        //Draw() from Entity class

    }
}
