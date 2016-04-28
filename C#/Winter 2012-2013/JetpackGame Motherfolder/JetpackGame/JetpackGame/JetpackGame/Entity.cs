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

//inheritance experiment!

namespace JetpackGame
{
    //the Entity class is the template for Player, Bullet, Turret, and StatCan classes.

    abstract class Entity
    {
        //fields
        protected Vector2 position;
        protected Texture2D sprite;
        protected Rectangle hitbox;
        protected Color color;
        protected bool enabled;

            //protected = children can inherit, but it isn't public

        //constructor
        protected Entity()
        {
            //empty
        }

        //get-set
        public Vector2 Position { get { return position; } set { position = value; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Rectangle Hitbox { get { return hitbox; } set { hitbox = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        //methods
        public virtual void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);

            //the rest really varies from entity to entity
        }
            //why keep this?  everything that inherits from Entity will have this function... if I have various entities in the same list,
            //I can run update on all of them.

        public virtual void Draw(SpriteBatch myBatch)
        {
            if (enabled)
            {
                myBatch.Draw(sprite, position, color);
            }
        }
    }
}
