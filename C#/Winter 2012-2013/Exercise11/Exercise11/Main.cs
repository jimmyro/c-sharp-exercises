using System;
using System.Collections.Generic;

namespace Exercise11
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//input for number of items, then each item
			//output mode

			int count; int input;

			while (true)
			{

				//get inputs into an array
				Console.WriteLine ("Enter number of elements in the set");
				if (!int.TryParse(Console.ReadLine (), out count) || count < 0)
				{
					Console.WriteLine ("Please enter a positive integer.");
					continue;
				}

				var set = new int[count];

				for (int i = 0; i < count; i++)
				{
					if (!int.TryParse(Console.ReadLine (), out input) || count < 0)
					{
						Console.WriteLine ("Please enter a positive integer.");
						i -= 1; continue;
					}
					else
					{
						set[i] = input;
					}
				}

				//find mode

			}


			/* //LEIBNIZ PI (not relevant, but here it is!)
			Console.WriteLine ("How many iterations?");

			int N;
			string str = Console.ReadLine ();
			int.TryParse (str, out N);
		
			double sum = 0;
			for (int i = 0; i < N; i += 1)
			{
				double curValue = 1.0/(2*i+1);

				if (i % 2 == 1)
				{
					curValue = -1 * curValue;
				}

				Console.WriteLine (curValue + ", "); //or Console.Write ( ... )

				sum += curValue;
			}

			Console.WriteLine ("Your value of pi = " + (sum*4));

			Console.ReadLine (); //pause
			*/
		}
	}
}
