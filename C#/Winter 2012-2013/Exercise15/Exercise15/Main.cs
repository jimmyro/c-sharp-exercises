using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Exercise15
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			while (true) 
			{
			//don't worry about preserving punctuation for now.
				char[] punc = new char[] {',', '.', ':', ';', '!', '?', '(', ')', '$', '%', '-'};

			//prompt for input
				Console.WriteLine ("Input message (\"exit\" to close)");
				string message = Console.ReadLine ();
				if (message.ToLower () == "exit") {break;} //quickly check for exit command

				Console.WriteLine ("Input censor");
				string censor = Console.ReadLine (); 

				Console.WriteLine ("Input replacement");
				string replacement = Console.ReadLine ();

			//disassemble and reassemble
				string[] brokenMessage = message.Split (' ');

				for (int i = 0; i < brokenMessage.Length; i++) 
				{
					string noPunc = brokenMessage [i].TrimEnd (punc);
					if (noPunc.Equals(censor)) 
					{
						brokenMessage [i] = replacement;
					}
				}
				string censoredMessage = String.Join (" ", brokenMessage);

			//output
				Console.WriteLine (censoredMessage); Console.WriteLine ();
			}
		}
	}
}
