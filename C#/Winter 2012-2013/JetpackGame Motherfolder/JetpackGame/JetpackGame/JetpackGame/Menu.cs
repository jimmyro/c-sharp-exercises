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
    class Menu
    {
        private SpriteFont font;
        private string[] choices;
        private int pointer, x, y, fontScale, vertSpacing;
        private Texture2D background;

        //constructor
        public Menu(SpriteFont myFont, string[] myChoices, int myX, int myY, int myFontScale, int myVertSpacing, Texture2D myBackground)
        {
            font = myFont;
            choices = myChoices;
            x = myX;
            y = myY;
            fontScale = myFontScale;
            vertSpacing = myVertSpacing;
            background = myBackground;

            pointer = 0;
        }

        //getters and setters
        public int Pointer { get { return pointer; } set { pointer = value; } }

        //methods
        public void IncPointer(int increment)
        {
            if (!(pointer + increment < 0 || pointer + increment > choices.Length))
            {
                pointer += increment;
            }
        }

        public void Draw(SpriteBatch myBatch)
        {
            myBatch.Draw(background, new Vector2(0, 0), Color.White);

            for (int i = 0; i < choices.Length; i++)
            {
                if (i != pointer)
                    myBatch.DrawString(font, choices[i], new Vector2(x, y + (i * ((12 * fontScale) + vertSpacing))), Color.White, 0f,
                        new Vector2(0, 0), fontScale, SpriteEffects.None, 0f);
                else
                    myBatch.DrawString(font, choices[i], new Vector2(x + 20, y + (i * ((12 * fontScale) + vertSpacing))), Color.IndianRed, 0f,
                        new Vector2(0, 0), fontScale, SpriteEffects.None, 0f);
            }
        }
    }
}
