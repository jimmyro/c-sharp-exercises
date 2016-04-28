using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Lab
{
    /* GOALS:
     * :D   1. display options in a boxed, properly spaced list
     * :D   2. scroll up and down list of options w/ highlighting
     * :D   3. detect key presses that increment or decrement options
     * :D   4. clean up and simplify box drawing and inc/dec functions
     * 5. fix SliderOption number display (weird spacing issues?)
     * 6. finish quitting sequence
     * 7. allow menu contents and changes to be read
     * 
     * 
     */

    class Menu
    {
        //fields & properties & variables
        private IMenuOption[] options;
        private string title;
        private int pointer, nameLen, valueLen;
        private bool init; //not totally necessary...

        private char[][] borderStyle;
        private ConsoleColor borderColor, textColor, pointerColor;
        private Vector2 pos;

        //constructor
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

            nameLen = myIndent;
            valueLen = 0;

            init = false;
        }

        //public methods
        public void Initialize()
        {
            if (!init)
            {
                nameLen += options.OrderByDescending(o => o.Name.Length).First().Name.Length;
                valueLen += options.OrderByDescending(o => o.TextValue.Length).First().TextValue.Length;

                ConsoleTools.DrawBox(pos, new Vector2(nameLen + valueLen, options.Length + 2), borderStyle, borderColor);

                Console.SetCursorPosition(pos.X + 1, pos.Y + 1);
                Console.ForegroundColor = textColor;

                for (int i = -2; i < options.Length; i++)
                {
                    if (i == -2)
                        Console.WriteLine(title.ToUpper());
                    else if (i == -1)
                        Console.WriteLine("");
                    else
                    {
                        if (pointer == i)
                            Console.BackgroundColor = pointerColor;

                        Console.WriteLine(options[i].Name.PadRight(nameLen) + options[i].TextValue.PadRight(valueLen));

                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.CursorLeft += pos.X + 1;
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                init = true;
            }
        }

        public void Run()
        {
            bool quit = false;
            Console.CursorVisible = false;

            if (init)
            {
                ConsoleKey oldKey = ConsoleKey.NoName, newKey = ConsoleKey.NoName;

                while (!quit)
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
                            case ConsoleKey.Enter:
                                quit = true;
                                Console.CursorVisible = true;

                                //TODO: quitting

                                break;
                        }

                        oldKey = newKey;
                    }
                }
            }
        }

        //private methods
        private void Increment()
        {
            if (init)
            {
                options[pointer].Increment();
                Console.SetCursorPosition(pos.X + nameLen + 1, pos.Y + pointer + 3);
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = pointerColor;

                Console.Write(options[pointer].TextValue.PadRight(valueLen));

                Console.ResetColor();
            }
        }

        private void Decrement()
        {
            if (init)
            {
                options[pointer].Decrement();
                Console.SetCursorPosition(pos.X + nameLen + 1, pos.Y + pointer + 3);
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = pointerColor;

                Console.Write(options[pointer].TextValue.PadRight(valueLen));

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
                Console.SetCursorPosition(pos.X + 1, pos.Y + pointer + 3);
                Console.Write(options[pointer].Name.PadRight(nameLen) + options[pointer].TextValue.PadRight(valueLen));

                if (pointer < options.Length - 1)
                    pointer++;
                else
                    pointer = 0;

                Console.BackgroundColor = pointerColor;
                Console.SetCursorPosition(pos.X + 1, pos.Y + pointer + 3);
                Console.Write(options[pointer].Name.PadRight(nameLen) + options[pointer].TextValue.PadRight(valueLen));
            }
        }

        private void PointerUp()
        {
            if (init)
            {
                //erase old highlighting
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = textColor;
                Console.SetCursorPosition(pos.X + 1, pos.Y + pointer + 3);
                Console.Write(options[pointer].Name.PadRight(nameLen) + options[pointer].TextValue.PadRight(valueLen));

                if (pointer > 0)
                    pointer--;
                else
                    pointer = options.Length - 1;

                Console.BackgroundColor = pointerColor;
                Console.SetCursorPosition(pos.X + 1, pos.Y + pointer + 3);
                Console.Write(options[pointer].Name.PadRight(nameLen) + options[pointer].TextValue.PadRight(valueLen));
            }
        }

        private static string Capitalize(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Remove(0, 1);
        }
    }
}
