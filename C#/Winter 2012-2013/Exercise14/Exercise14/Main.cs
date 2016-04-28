using System;
using System.Collections.Generic;

namespace Exercise14
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//GOAL: discern several inputs separated by commas
			//BONUS: ...or by spaces

			while (true) 
			{
				Console.WriteLine ("Enter your separator character:");
				char separator;

				if (!char.TryParse (Console.ReadLine (), out separator)) 
				{
					Console.WriteLine ("Couldn't find a character, try again!");
					continue;
				}

				Console.WriteLine ("Enter your inputs:");
				string message = Console.ReadLine ();

				string[] input = message.Split (separator);

			//this is just to show the parsed inputs
				for (int i = 0; i < input.Length; i++)
				{
					Console.WriteLine (i + " | " + input[i]);
				}
			}
		}
	}
}
