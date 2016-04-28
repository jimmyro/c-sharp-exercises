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
    //the Entity class is the template for Player, Bullet, Turret, and StatCan classes.

    abstract class Entity
    {
        //fields
        private Vector2 position;
        private Texture2D sprite;
        private Rectangle hitbox;
        private Color color;
        private bool enabled;

        //constructor
        public Entity()
        {
            //empty
        }

        //get-set
        public Vector2 Position { get { return position; } set { position = value; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        //methods
        public void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);

            //the rest really varies from entity to entity
        }

        public void Draw(SpriteBatch myBatch)
        {
            myBatch.Draw(sprite, position, color);
        }
    }
}
