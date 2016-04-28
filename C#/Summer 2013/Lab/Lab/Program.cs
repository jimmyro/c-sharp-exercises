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
        static void Main(string[] args)
        {
            #region menu testing code
            /*var o = new IMenuOption[5]
            {
                new SliderOption("players", 2, 8, 1, 0, 4),
                new OnOffOption("auto-health", false),
                new EnumOption("gamemode", 0, "auto-all", "auto-turns", "manual-turns", "manual-all"),
                new OnOffOption("hotkeys", true),
                new SliderOption("graphics factor", 0, 5, 0.5, 1, 0)
            };

            var m = new Menu("D&D ENGINE: Settings 1", o, ConsoleTools.style12, ConsoleColor.DarkYellow, ConsoleColor.Yellow,
                ConsoleColor.Blue, new Vector2(4, 5), 5);

            m.Initialize();
            m.Run();*/
            #endregion

            var f = new Form("TEST_FORM", 20, 2, new Vector2(0, 0), "length7", "size5", "magnitude11", "expanse8");

            //f.Initialize();
            //f.Run();

            var o = new IMenuOption[5]
            {
                new SliderOption("TEST_SLIDER", 0, 10, 1, 0, 5),
                new OnOffOption("TEST_ON_OFF", false),
                new EnumOption("TEST_ENUM", 0, "TEST1", "TEST2", "TEST3", "TEST4"),
                new SliderOption("PLACING_TEST", 0.350, 1.650, 0.050, 3, 0.800),
                new EnumOption("ABCDEF", 3, "A", "B", "C", "D", "E", "F")
            };

            var m = new Menu("TEST_MENU", o, ConsoleTools.style12, ConsoleColor.DarkBlue, ConsoleColor.Blue,
                ConsoleColor.Yellow, new Vector2(5, 5), 3);

            m.Initialize();
            m.Run();
            

            /* 
             * 
             * GOALS:
             * 4. draw the form in any number of columns; pass in distance b/w columns
             * 5. allow highlight-scrolling (enforce order?)
             * 6. make the forms work, prohibit non-alphanumeric characters, add entered values to a dictionary
             * 7. enable border color, text color, highlight color, etc.
             */

            Console.ReadLine();
        }

        public static Vector2 GetWindowMidpt(Vector2 dim)
        {
            return new Vector2((Console.BufferWidth - dim.X) / 2, ((Console.BufferHeight - dim.Y) / 2));
        }

        public static bool AboutEqual(double x, double y)
        {
            double epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * 1E-15;
            return Math.Abs(x - y) <= epsilon;
        }
    }
}
