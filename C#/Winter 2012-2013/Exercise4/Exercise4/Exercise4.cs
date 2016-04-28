using System;
using System.Collections.Generic;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//in: positive integer n
			//out: n! (n factorial)
			//bonus: store previous calculations in a list
			
			int n;
			List<int> factorials = new List<int> ();
			bool stopped = false;
			
			while (!stopped) 
			{
				Console.WriteLine ("Factorial of?");

				//test positive integer
				if (!int.TryParse (Console.ReadLine (), out n) && n >= 0) 
				{
					Console.WriteLine ("Please enter a positive integer");
					continue;
				}
				int product = 1;

				//calculate
				if (factorials[n] == null && n != 0)
				{
					for (int i = 1; i < n; i += 1)
					{
						product = product * i;
						factorials[i] = product;
					}
				}

				string answer = factorials[n].ToString();
				Console.WriteLine ("factorial: " + answer);
				Console.ReadLine();

			}
		}
	}
}


