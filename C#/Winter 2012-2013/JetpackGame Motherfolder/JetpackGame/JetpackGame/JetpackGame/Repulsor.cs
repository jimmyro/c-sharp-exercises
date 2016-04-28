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
    class Repulsor : Entity
    {
        //properties
        private Player player;
        private List<Bullet> bullets;
        private int radius, maxRadius, repulsorForce;

        private Vector2 origin;
        //+ position, sprite, hitbox, enabled, color

        //constructor
        public Repulsor(Player myPlayer, List<Bullet> myBullets, Texture2D mySprite, int myMaxRadius, int myRepulsorForce)
        {
            //passed in
            player = myPlayer;
            bullets = myBullets;
            maxRadius = myMaxRadius; 
            repulsorForce = myRepulsorForce;

            //class-specific
            radius = 0;
            //origin = new Vector2((int)(mySprite.Width / 2), (int)(mySprite.Height / 2));

            //inherited
            position = new Vector2(player.Position.X + (player.Sprite.Width / 2), player.Position.Y + (player.Sprite.Width / 2));
            sprite = mySprite;
            enabled = true;
            color = Color.DarkViolet;
        }

        //getters and setters

        //methods
        public override void Update()
        {
            position = new Vector2(player.Position.X + (player.Sprite.Width / 2), player.Position.Y + (player.Sprite.Width / 2));

            if (radius < maxRadius)
            {
                radius += 1;
            }
            else
            {
                enabled = false;
            }
        }

        public override void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, null, Color.DarkViolet, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);
            }
        }
    }
}
