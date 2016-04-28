using System;

namespace test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			bool stopped = false; string word = ""; 

			Console.WriteLine("Enter alphanumeric characters one by one, or \"stop\" to end the word");

			while (!stopped) //read the vertically inputted word
			{
				string letter = Console.ReadLine();
				if (letter == "stop")
				{
					stopped = true;
					continue;
				}
				else if (letter.Length < 2)
				{
					word = word + letter;
					continue;
				}
				else
				{
					Console.WriteLine("Enter one by one");
				}
			}

			Console.WriteLine (word);
			Console.ReadLine();

		}
	}
}
