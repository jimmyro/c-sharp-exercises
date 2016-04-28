using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Lab
{
    class Form
    {
        private string title;
        private Dictionary<string, string> entries;
        private string[] items;
        private int fieldLength, longest, indent, pointer;
        private Vector2 pos;

        public Form(string myTitle, int myFieldLength, int myIndent, Vector2 myPos, params string[] myItems)
        {
            title = myTitle;
            fieldLength = myFieldLength;
            items = myItems;
            pos = myPos;
            indent = myIndent + 1; //account for the colons
            pointer = 0;
            longest = items.OrderByDescending(s => s.Length).First().Length;
        }

        public void Initialize()
        {
            int width = longest + indent + fieldLength + 4;
            int height = (3 * items.Length) + 1;

            ConsoleTools.DrawBox(pos, new Vector2(width, height), ConsoleTools.style12, ConsoleColor.White);

            //start writing
            Console.SetCursorPosition(pos.X + 1, pos.Y + 1);
            Console.WriteLine(title.ToUpper());
            Console.CursorLeft = pos.X + 1;

            for (int i = 1; i <= items.Length; i++)
            {
                ConsoleTools.DrawBox(new Vector2(pos.X + longest + indent, pos.Y + (3 * i) - 1), new Vector2(fieldLength + 2, 1),
                    ConsoleTools.style11, ConsoleColor.White);

                Console.SetCursorPosition(pos.X + 1, pos.Y + (3 * i));
                Console.Write(items[i - 1] + ':');
            }

            //Console.SetCursorPosition(pos.X + longest + indent + 2, pos.Y + (i * 3));
                //where i = the item number
        }

        public void Run()
        {
            bool quit = false;

            while (!quit) //to-do: quitting
            {
                for (int i = 1; i <= items.Length; i++)
                {
                    //move to beginning of field and highlight
                    Console.SetCursorPosition(pos.X + longest + indent + 2, pos.Y + (i * 3));
                    
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("".PadRight(fieldLength));

                    //move back to beginning and take input
                    Console.SetCursorPosition(pos.X + longest + indent + 2, pos.Y + (i * 3));

                    while (true)
                    {

                    }

                    //carriage return = 13
                }
            }
        }
    }
}
