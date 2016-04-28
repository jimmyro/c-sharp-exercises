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
    public static class Helper //stuff that all my other classes need to access, like bullet types and powerup types
    {
        public enum BulletType { normal, spread, slow, freeze, teleport }
        public enum StatIndex { health, fuel, healthRegen, fuelRegen, gravity, speed, repulsor }

        public static int SCREEN_WIDTH = 800;
        public static int SCREEN_HEIGHT = 480;
    }

    public struct BulletProperties
    {
        public Helper.BulletType bulletType;
        public Color color;
        public Texture2D sprite;
        public int baseDamage;

        public BulletProperties(Helper.BulletType myBulletType, Color myColor, Texture2D mySprite, int myBaseDamage)
        {
            bulletType = myBulletType;
            color = myColor;
            sprite = mySprite;
            baseDamage = myBaseDamage;
        }
    }
}
