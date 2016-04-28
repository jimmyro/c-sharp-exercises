using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise7
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			int input; //bool stopped = false;
			
			while (true)
			{
			
				print ("Input your perfect number candidate:");
			
				//screen for positive even integers
				if (!int.TryParse (Console.ReadLine (), out input))
				{
					print ("Input must be an integer");
					continue;
				}

				if (input < 1)
				{
					print ("Input must be positive");
					continue;
				}
				else if (input % 2 != 0)
				{
					print ("No odd perfect numbers below 10^1500 have ever been found");
					continue;
				}

				//test perfection
				int sum = 0;
				for (int i = 1; i <= (input/2); i++)
				{
					if (input % i == 0)
					{
						sum += i;
					}
				}

				if (sum == input)
				{
					print ("Your number is perfect!");
				}
				else
				{
					print ("Your number is not perfect... but you are");
				}
			}

			//BONUS: Given a positive integer N, find the N-th perfect number.
		}

		public static void print (string s)
		{
			Console.WriteLine (s);
		}
	}
}
