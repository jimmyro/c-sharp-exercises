using System;
using System.Collections.Generic;

namespace Exercise5
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			int index; int index2; string message; var notes = new List<string>();

			Console.WriteLine ("Enter a note, an index to look up, or a special command.");
			Console.WriteLine ("special commands: display, delete");

			while (true) 
			{
				//receive a string --> create a new note
					//"display" --> display all saved with their indices
					//"delete" --> ask for index, try to delete note with that index
				//receive an int --> look for note with index 10
				//otherwise --> try again

				message = Console.ReadLine ();

				if (!int.TryParse (message, out index) || index < 0)
				{ //didn't find integer
					if (message.ToLower() == "display")
					{
						for (int i = 0; i < notes.Count; i++)
						{
							Console.WriteLine (i.ToString() + " | " + notes[i]);
						}
					}
					else if (message.ToLower() == "delete")
					{
						Console.WriteLine ("Input an index");
						string message2 = Console.ReadLine ();
						if (!int.TryParse (message2, out index2) && index2 >= 0 && index2 < notes.Count)
						{
							notes.Remove (notes[index2]);
						}
						else
						{
							Console.WriteLine ("No note at index " + message2 + ".");
						}
					}
					else
					{
						notes.Add (message);
					}
				}
				else
				{ //did find integer
					if (index < notes.Count && index >= 0 && notes[index] != null)
					{
						Console.WriteLine (notes[index]);
					}
					else
					{
						Console.WriteLine ("No note at index " + message + ".");
					}
				}
			}
		}

		/*
		public static bool askYN (string action)
		{
			//ask stuff
		}

		*/

	}
}
