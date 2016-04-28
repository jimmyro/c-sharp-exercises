using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Lab
{
    class Program
    {
        #region menu border styles
        public static char[][] style12 = new char[3][]
        {
            new char[3] { '┌', '─', '╖' },
            new char[3] { '│', ' ', '║' }, 
            new char[3] { '╘', '═', '╝' }
        };

        public static char[][] style11 = new char[3][]
        {
            new char[3] { '┌', '─', '┐' },
            new char[3] { '│', ' ', '│' }, 
            new char[3] { '└', '─', '┘' }
        };
        #endregion

        static void Main(string[] args)
        {
            #region code page 437
            /* Set the window size and title
            Console.Title = "Code Page 437: MS-DOS ASCII Characters";

            for (byte b = 0; b < byte.MaxValue; b++)
            {
                char c = Encoding.GetEncoding(437).GetChars(new byte[] { b })[0];
                switch (b)
                {
                    case 8: // Backspace
                    case 9: // Tab
                    case 10: // Line feed
                    case 13: // Carriage return
                        c = '.';
                        break;
                }

                Console.Write("{0:000} {1}   ", b, c);

                // 7 is a beep -- Console.Beep() also works
                if (b == 7) Console.Write(" ");

                if ((b + 1) % 8 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();

            Console.ReadLine();
            */
            #endregion

            var o = new IMenuOption[3];

            o[0] = new SliderOption("Absorbance", myLower: 0, myUpper: 1, myIncrement: 0.1, myRounding: 1, myDefault: 0.0);
            o[1] = new OnOffOption("Timer", false);
            o[2] = new EnumOption("Light Color", 0, "red", "green", "blue", "uv", "gamma");

            var m = new Menu("beer's law", o, style12, ConsoleColor.White, ConsoleColor.White,
                ConsoleColor.Blue, new Vector2(1, 2), 3);

            m.Initialize();

            Console.ReadLine();
        }

        public static bool AboutEqual(double x, double y)
        {
            double epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * 1E-15;
            return Math.Abs(x - y) <= epsilon;
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

        public static Vector2 Zero()
        { 
            return new Vector2(0, 0);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", x, y);
        }

        public static Vector2 FromInt(int myInt)
        {
            Vector2 result = Vector2.Zero();

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
