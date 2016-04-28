using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management;
using System.Management.Instrumentation;

namespace Lab
{
    public static class ConsoleTools
    {
        #region static style sheets
        public static char[][] style11 = new char[3][]
        {
            new char[3] { '┌', '─', '┐' },
            new char[3] { '│', ' ', '│' }, 
            new char[3] { '└', '─', '┘' }
        };

        public static char[][] style12 = new char[3][]
        {
            new char[3] { '┌', '─', '╖' },
            new char[3] { '│', ' ', '║' }, 
            new char[3] { '╘', '═', '╝' }
        };

        public static char[][] style22 = new char[3][]
        {
            new char[3] { '╔', '═', '╗' },
            new char[3] { '║', ' ', '║' }, 
            new char[3] { '╚', '═', '╝' }
        };

        public static char[][] styleSS = new char[3][]
        {
            new char[3] { '⌠', '∙', '│' },
            new char[3] { '│', ' ', '│' }, 
            new char[3] { '│', '∙', '⌡' }
        };
        #endregion

        /// <summary>
        /// Draws a box w/ specified position, dimensions, and style information.
        /// </summary>
        /// <param name="pos">Position where box starts, including borders</param>
        /// <param name="dim">Dimensions of box, excluding borders</param>
        /// <param name="style">3x3 jagged char array</param>
        public static void DrawBox(Vector2 pos, Vector2 dim, char[][] style, ConsoleColor borderColor)
        {
            int left = pos.X, right = pos.X + dim.X + 2, top = pos.Y,
                bottom = pos.Y + dim.Y + 2;

            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = borderColor;

            //draw the box from a style array (with a little math to keep things concise)
            for (int y = 0; y < bottom - top; y++)
            {
                for (int x = 0; x < right - left; x++)
                {
                    int yChar = (int)Math.Ceiling((decimal)y / (bottom - top - 2)),
                        xChar = (int)Math.Ceiling((decimal)x / (right - left - 2));
                    // the "- 2" pushes the fraction slightly over so the last value comes out as 2

                    Console.Write(style[yChar][xChar]);
                }

                Console.WriteLine();
                Console.CursorLeft = left;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public struct Vector2
    {
        private int x, y;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public Vector2(int myX, int myY)
        {
            x = myX;
            y = myY;
        }

        public static Vector2 Zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", x, y);
        }

        public static Vector2 FromInt(int myInt)
        {
            Vector2 result = Vector2.Zero;

            if (!int.TryParse(myInt.ToString()[0].ToString(), out result.y))
                throw new Exception("That int sucked");

            if (myInt.ToString().Length < 2)
                result.x = 0;
            else if (!int.TryParse(myInt.ToString()[1].ToString(), out result.x))
                throw new Exception("That int sucked");

            return result;
        }
    }
}
