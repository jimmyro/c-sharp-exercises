using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Lab
{
    /* GOALS:
     * 1. display options in a boxed, properly spaced list
     * 2. scroll up and down list of options w/ highlighting
     * 3. detect key presses that increment or decrement options
     * 4. 
     * 
     */

    class Menu
    {
        private IMenuOption[] options;
        private string title;
        private int pointer, indent, nameCol, valueCol, valueLength;
        private bool init; //not totally necessary...

        private char[][] borderStyle;
        private ConsoleColor borderColor, textColor, pointerColor;
        private Vector2 pos;

        public Menu(string myTitle, IMenuOption[] myOptions, char[][] myBorderStyle, ConsoleColor myBorderColor,
            ConsoleColor myTextColor, ConsoleColor myPointerColor, Vector2 myPos, int myIndent)
        {
            title = myTitle;
            options = myOptions;
            pointer = 0;
            borderStyle = myBorderStyle;
            borderColor = myBorderColor;
            textColor = myTextColor;
            pointerColor = myPointerColor;
            pos = myPos;
            indent = myIndent;

            nameCol = myPos.X + 1;
            valueCol = nameCol + (8 * indent);


            init = false;
        }

        //methods
        public void Initialize()
        {
            if (!init)
            {
                string[] content = new string[options.Length + 4];

                content[0] = title.ToUpper(); content[1] = "";

                for (int i = 0; i < options.Length; i++)
                    content[i + 2] = FillTabSpace(options[i].Name, indent) + options[i].TextValue;

                content[options.Length + 2] = ""; content[options.Length + 3] = "Press return to finish.";

                //old BoxText code
                string longest = content.OrderByDescending(s => s.Length).First();
                valueLength = longest.Length - (8 * indent);

                DrawBox(pos, new Vector2(longest.Length, content.Length), borderStyle, borderColor);

                Console.SetCursorPosition(pos.X + 1, pos.Y + 1);
                Console.ForegroundColor = textColor;

                for (int i = 0; i < content.Length; i++)
                {
                    if (pointer == i - 2)
                        Console.BackgroundColor = pointerColor;

                    Console.WriteLine(FillSpace(content[i], longest.Length));

                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.CursorLeft += pos.X + 1;
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                init = true;
            }
        }

        public void Run()
        {
            if (init)
            {
                ConsoleKey oldKey = ConsoleKey.NoName, newKey = ConsoleKey.NoName;

                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        newKey = Console.ReadKey(true).Key;

                        switch (newKey)
                        {
                            case ConsoleKey.UpArrow:
                                PointerUp();
                                break;
                            case ConsoleKey.DownArrow:
                                PointerDown();
                                break;
                            case ConsoleKey.LeftArrow:
                                Decrement();
                                break;
                            case ConsoleKey.RightArrow:
                                Increment();
                                break;
                        }

                        oldKey = newKey;
                    }
                }
            }
        }

        private void Increment()
        {
            if (init)
            {
                options[pointer].Increment();
                Console.SetCursorPosition(valueCol, pos.Y + pointer + 3);
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = pointerColor;

                Console.Write(FillSpace(options[pointer].TextValue, valueLength));

                Console.ResetColor();
            }
        }

        private void Decrement()
        {
            if (init)
            {
                options[pointer].Decrement();
                Console.SetCursorPosition(valueCol, pos.Y + pointer + 3);
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = pointerColor;

                Console.Write(FillSpace(options[pointer].TextValue, valueLength));

                Console.ResetColor();
            }
        }

        private void PointerDown()
        {
            if (init)
            {
                //erase old highlighting
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = textColor;
                Console.SetCursorPosition(nameCol, pos.Y + pointer + 3);
                Console.Write(FillTabSpace(options[pointer].Name, 3) + FillSpace(options[pointer].TextValue, valueLength));

                if (pointer < options.Length - 1)
                    pointer++;
                else
                    pointer = 0;

                Console.BackgroundColor = pointerColor;
                Console.SetCursorPosition(nameCol, pos.Y + pointer + 3);
                Console.Write(FillTabSpace(options[pointer].Name, 3) + FillSpace(options[pointer].TextValue, valueLength));
            }
        }

        private void PointerUp()
        {
            if (init)
            {
                //erase old highlighting
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = textColor;
                Console.SetCursorPosition(nameCol, pos.Y + pointer + 3);
                Console.Write(FillTabSpace(options[pointer].Name, 3) + FillSpace(options[pointer].TextValue, valueLength));

                if (pointer > 0)
                    pointer--;
                else
                    pointer = options.Length - 1;

                Console.BackgroundColor = pointerColor;
                Console.SetCursorPosition(nameCol, pos.Y + pointer + 3);
                Console.Write(FillTabSpace(options[pointer].Name, 3) + FillSpace(options[pointer].TextValue, valueLength));
            }
        }

        /// <summary>
        /// Draws a box w/ specified position, dimensions, and style information.
        /// </summary>
        /// <param name="pos">Position where box starts, including borders</param>
        /// <param name="dim">Dimensions of box, excluding borders</param>
        /// <param name="style">3x3 jagged char array</param>
        private static void DrawBox(Vector2 pos, Vector2 dim, char[][] style, ConsoleColor borderColor)
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

        private static string FillTabSpace(string s, int tabs)
        {
            string t = s;

            if (s.Length > tabs * 8)
                return s.Substring(0, tabs * 8 - 1);

            for (int i = 0; i < (8 * tabs - s.Length); i++)
                t += " ";

            return t;
        }

        private static string FillSpace(string s, int length)
        {
            if (s.Length > length)
                return s.Substring(0, length - 1);

            string result = s;

            for (int i = 0; i < length - s.Length; i++)
                result += " ";

            return result;
        }

        private static string Capitalize(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Remove(0, 1);
        }
    }
}
