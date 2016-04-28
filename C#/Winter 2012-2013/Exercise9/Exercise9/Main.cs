//REVIEWED 12-10-12

using System;
using System.Collections.Generic;

namespace Exercise9
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//address must have: only one '@', only one '.', "ng" at the end
			//address must not have: non-alphanumeric characters besides '@' and '.'

			while (true) 
			{
				Console.WriteLine ("Input email:");
				string email = Console.ReadLine ();


				if (!email.EndsWith(".ng") || checkHowMany (email, '@') != 1 || checkHowMany (email, '.') != 1)
				{
					Console.WriteLine ("NO");
				}
				else
				{
					Console.WriteLine ("YES");
				}
			}
		}

		//scans a string s for instances of a character c
		public static int checkHowMany (string s, char c)
		{
			int count = 0;
			
			for (int i = 0; i < s.Length; i++) 
			{
				if (s[i] == c)
				{
					count += 1;
				}
			}

			return count;
		}

	}
}
